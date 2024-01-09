#include "pch.h"
#include "WinUser.h"
#include <Windows.h>
#include <TlHelp32.h>

#define SE_DEBUG_PRIVILEGE 20

// define missing needed functions from ntdll (kernel library)
typedef LONG(__stdcall* _RtlAdjustPrivilege)(ULONG Privilege, BOOLEAN Enable, BOOLEAN CurrentThread, PBOOLEAN Enabled);
typedef LONG(__stdcall* _SuspendOrResumeProcess)(HANDLE hProcess);

static HMODULE User32Instance;
static HHOOK hHookUser32;

static _RtlAdjustPrivilege RtlAdjustPrivilege;
static _SuspendOrResumeProcess NtSuspendProcess;
static _SuspendOrResumeProcess NtResumeProcess;

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

    BOOLEAN dummy = 0;
    RtlAdjustPrivilege(SE_DEBUG_PRIVILEGE, true, false, &dummy);

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

    BOOLEAN dummy = 0;
    RtlAdjustPrivilege(SE_DEBUG_PRIVILEGE, true, false, &dummy);

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
        RtlAdjustPrivilege = (_RtlAdjustPrivilege)GetProcAddress(ntdllInstance, "RtlAdjustPrivilege");
        if (RtlAdjustPrivilege == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load RtlAdjustPrivilege! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

        // get the address of the NtSuspendProcess function from ntdll
        NtSuspendProcess = (_SuspendOrResumeProcess)GetProcAddress(ntdllInstance, "NtSuspendProcess");
        if (NtSuspendProcess == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load NtSuspendProcess! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

        // get the address of the NtResumeProcess function from ntdll
        NtResumeProcess = (_SuspendOrResumeProcess)GetProcAddress(ntdllInstance, "NtResumeProcess");
        if (NtSuspendProcess == INVALID_HANDLE_VALUE) {
            MessageBox(NULL, "Failed to load NtResumeProcess! Some functionality may be missings!", "Error", MB_OK | MB_ICONERROR);
            return 1;
        }

    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

