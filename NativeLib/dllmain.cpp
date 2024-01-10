/*

Due to anti-virus issues we decided to not use this anymore, however, we keep it here for reference.


*/





#include "pch.h"
#include "WinUser.h"
#include <Windows.h>
#include <TlHelp32.h>
#include <cstdlib>

#define SE_DEBUG_PRIVILEGE 20

typedef LONG NTSTATUS;

// define missing needed functions from ntdll (kernel library)
//typedef LONG(__stdcall* _RtlAdjustPrivilege)(ULONG Privilege, BOOLEAN Enable, BOOLEAN CurrentThread, PBOOLEAN Enabled);
//typedef NTSTATUS(__stdcall* _SuspendOrResumeProcess)(HANDLE hProcess);

typedef NTSTATUS(__stdcall* _ZwOpenProcessToken)(LONG ProcessHandle, ACCESS_MASK DesiredAccess, PHANDLE TokenHandle);
typedef NTSTATUS(__stdcall* _ZwAdjustPrivilegesToken)(HANDLE TokenHandle, BOOLEAN DisableAllPrivileges, PTOKEN_PRIVILEGES NewState, ULONG BufferLength, PTOKEN_PRIVILEGES PreviousState, PULONG ReturnLength);

typedef NTSTATUS(__stdcall* _NtClose)(HANDLE Handle);

static HMODULE User32Instance;
static HHOOK hHookUser32;

//static _RtlAdjustPrivilege RtlAdjustPrivilege;
//static _SuspendOrResumeProcess NtSuspendProcess;
//static _SuspendOrResumeProcess NtResumeProcess;
//static _ZwOpenProcessToken ZwOpenProcessToken;
//static _ZwAdjustPrivilegesToken ZwAdjustPrivilegesToken;
static _NtClose NtClose;

extern "C" NTSTATUS ZwAdjustPrivilegesToken(HANDLE TokenHandle, BOOLEAN DisableAllPrivileges, PTOKEN_PRIVILEGES NewState, ULONG BufferLength, PTOKEN_PRIVILEGES PreviousState, PULONG ReturnLength);
extern "C" NTSTATUS ZwOpenProcessToken(LONG ProcessHandle, ACCESS_MASK DesiredAccess, PHANDLE TokenHandle);
extern "C" NTSTATUS NtSuspendProcess(HANDLE hProcess);
extern "C" NTSTATUS NtResumeProcess(HANDLE hProcess);

NTSTATUS AdjustPrivilege(ULONG Privilege, BOOLEAN Enable) {
    HANDLE handle;
    NTSTATUS result = ZwOpenProcessToken(-1, 40, &handle);
    if (result >= 0) {
        TOKEN_PRIVILEGES privileges;
        privileges.PrivilegeCount = 1;
        privileges.Privileges[0].Luid.LowPart = Privilege;
        privileges.Privileges[0].Luid.HighPart = 0;
        privileges.Privileges[0].Attributes = Enable ? 2 : 0;

        TOKEN_PRIVILEGES previousState;
        ULONG returnLength;

        result = ZwAdjustPrivilegesToken(handle, FALSE, &privileges, sizeof(TOKEN_PRIVILEGES), &previousState, &returnLength);
        NtClose(handle);

        if (result < 0) {
            MessageBox(NULL, "Failed to adjust privileges!", "Error", MB_OK | MB_ICONERROR);
        }
    }
    else {
        MessageBox(NULL, "Failed to open process token!", "Error", MB_OK | MB_ICONERROR);
        char* buffer;
        FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, NULL, result, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPSTR)&buffer, 0, NULL);
        MessageBox(NULL, buffer, "Error", MB_OK | MB_ICONERROR);
    }

    return result;
}

// get the PID of the process respomsible for the CTRL+ALT+SUPR screen
DWORD GetWinlogonPID() {
    HANDLE snap;
    PROCESSENTRY32 pEntry;
    BOOL success;

    snap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    pEntry.dwSize = sizeof(pEntry);
    success = Process32First(snap, &pEntry);

    while (success) {
        if (!strcmp(pEntry.szExeFile, "winlogon.exe")) {
            CloseHandle(snap);
            return pEntry.th32ProcessID;
        }

        memset(pEntry.szExeFile, 0, 260);
        success = Process32Next(snap, &pEntry);
    }

    CloseHandle(snap);
    return 0;
}

LRESULT WINAPI hookProc(int code, WPARAM wParam, LPARAM lParam) {
    if (code >= 0) {
        return 1;
    }

    return CallNextHookEx(hHookUser32, code, wParam, lParam);
}

extern "C" __declspec(dllexport) void EnableLock() {
    // Lock normal inputs
    hHookUser32 = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, User32Instance, 0);

    // lock CTRL+ALT+SUPR
    DWORD pid = GetWinlogonPID();
    if (pid == 0) {
        MessageBox(NULL, "Failed to get winlogon.exe's PID", "Error", MB_OK | MB_ICONERROR);
        return;
    }

    //BOOLEAN dummy = 0;
   // RtlAdjustPrivilege(SE_DEBUG_PRIVILEGE, true, false, &dummy);
    AdjustPrivilege(SE_DEBUG_PRIVILEGE, true);

    HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
    if (hProcess == INVALID_HANDLE_VALUE) {
        MessageBox(NULL, "Failed to open winlogon.exe!", "Error", MB_OK | MB_ICONERROR);
        return;
    }

    NtSuspendProcess(hProcess);
    CloseHandle(hProcess);
}

extern "C" __declspec(dllexport) void DisableLock() {
    UnhookWindowsHookEx(hHookUser32);

    // unlock CTRL+ALT+SUPR
    DWORD pid = GetWinlogonPID();
    if (pid == 0) {
        MessageBox(NULL, "Failed to get winlogon.exe's PID", "Error", MB_OK | MB_ICONERROR);
        return;
    }

    //BOOLEAN dummy = 0;
    //RtlAdjustPrivilege(SE_DEBUG_PRIVILEGE, true, false, &dummy);

    AdjustPrivilege(SE_DEBUG_PRIVILEGE, true);

    HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
    if (hProcess == INVALID_HANDLE_VALUE) {
        MessageBox(NULL, "Failed to open winlogon.exe!", "Error", MB_OK | MB_ICONERROR);
        return;
    }

    NtResumeProcess(hProcess);
    CloseHandle(hProcess);
}



BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    HMODULE ntdllInstance;

    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        // Initialize
        User32Instance = LoadLibrary("User32");

        ntdllInstance = LoadLibrary("ntdll"); // load kernel library
        if (ntdllInstance == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load ntdll! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

        // get the address of the RtlAdjustPrivilege function from ntdll
       /* RtlAdjustPrivilege = (_RtlAdjustPrivilege)GetProcAddress(ntdllInstance, "RtlAdjustPrivilege");
        if (RtlAdjustPrivilege == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load RtlAdjustPrivilege! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }*/

       /* ZwOpenProcessToken = (_ZwOpenProcessToken)GetProcAddress(ntdllInstance, "ZwOpenProcessToken");
        if (ZwOpenProcessToken == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load ZwOpenProcessToken! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

        ZwAdjustPrivilegesToken = (_ZwAdjustPrivilegesToken)GetProcAddress(ntdllInstance, "ZwAdjustPrivilegesToken");
        if (ZwOpenProcessToken == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load ZwAdjustPrivilegesToken! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }*/

        NtClose = (_NtClose)GetProcAddress(ntdllInstance, "NtClose");
        if (NtClose == INVALID_HANDLE_VALUE) {
			MessageBox(NULL, "Failed to load NtClose! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
			return 1;
		}

        // get the address of the NtSuspendProcess function from ntdll
      /*  NtSuspendProcess = (_SuspendOrResumeProcess)GetProcAddress(ntdllInstance, "NtSuspendProcess");
        if (NtSuspendProcess == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load NtSuspendProcess! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

        // get the address of the NtResumeProcess function from ntdll
        NtResumeProcess = (_SuspendOrResumeProcess)GetProcAddress(ntdllInstance, "NtResumeProcess");
        if (NtSuspendProcess == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load NtResumeProcess! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }*/

    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

