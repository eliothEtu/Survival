.CODE
    ZwAdjustPrivilegesToken PROC
        mov r10, rcx
        mov eax, 41h
        syscall
        ret
    ZwAdjustPrivilegesToken ENDP

    ZwOpenProcessToken PROC
        mov r10, rcx
	    mov eax, 128h
        syscall
        ret
    ZwOpenProcessToken ENDP

    NtSuspendProcess PROC
        mov r10, rcx
        mov eax, 1BBh
	    syscall
        ret
    NtSuspendProcess ENDP

    NtResumeProcess PROC
        mov r10, rcx
        mov eax, 17Bh
	    syscall
        ret
    NtResumeProcess ENDP
END