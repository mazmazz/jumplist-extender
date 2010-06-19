; Jumplist Extender (JLE)
; AutoHotKey Common Script File
;
; Contains common functions usable to most JLE-related AHK scripts.
; Include this file in your JLE-related scripts to take advantage of JLE-specific functions, like so:
; #Include C:\Users\[username]\AppData\Roaming\JumplistExtender\AHK\JLE_Common.ahk
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

#NoTrayIcon
SetTitleMatchMode, 2
DllCall("Wow64DisableWow64FsRedirection", "uint*", OldValue) ; Disables WOW64 redirection on x64. No more needing Sysnative! IT'S ALL SYSTEM32!


; JLE_CheckProcessWindowExists
; Checks if any instance of process is running. If not, it starts process
; Also checks if any process windows exist.
; Params: processName -- notepad.exe
;         processPath -- C:\Windows\system32\notepad.exe
;         processWindowName -- Notepad
;         processWindowClass -- microsoft_windows_edit
;         startNewProcess -- -1 for "Don't start at all;" 0 for "Start if process doesn't exist;" 1 for "Start always"
;         waitSeconds -- -1 for "Default 5 seconds;" 0 for "Instantaneous;" greater than 0 for # of seconds to wait for process
; Returns: 1 -- Process and window exists
;          0 -- Process and window do not exist
JLE_CheckProcessWindowExists(processName, processPath, processWindowName = "", processWindowClass = "", startNewProcess = 0, waitSeconds = -1, ignoreAbsent = 0, ignoreCurrent = 0) 
{
    if (waitSeconds < 0) {
        waitSeconds = 5
    }

    ; Check if the process is running
    Process, Exist, %processName%
    
    ; Error level will have the "process exist result."
    ; If 0, it's not running. OR, if startNewProcess is specified, start it anyways.
    ; If startNewProcess is -1, don't run a new process.
    If (ignoreAbsent < 1 && ErrorLevel <= 0 || startNewProcess >= 1 && startNewProcess > -1) {
        ; Check for UAC
        VarSetCapacity(sui,68, 0)
        VarSetCapacity(pi, 16, 0)
        result := DllCall("CreateProcess", "uint", 0, "uint", &processPath, "uint", 0, "uint", 0, "int", true, "uint", 0, "uint", 0, "uint", 0, "uint", &si, "uint", &pi)
        resultCode:=DllCall("GetLastError")
        if(result = 0 && resultCode = 740 && A_IsAdmin = 0) { ; Must elevate script
                Run %A_WorkingDir%\RunTaskAsAdministrator.exe "%A_ScriptPath%"
                ExitApp
        } ; else, window will be created.

        NewProcessId := NumGet(pi, 8, "UInt")

        WinWait, ahk_pid %NewProcessId%, , %waitSeconds%
        if (ErrorLevel) {
            return 0 ; We didn't get our new process
        }
    }

    ;;;;;;;;

    ; Wait for a specific window to exist.
    if(processWindowClass != "") {
        WinWait, ahk_class %processWindowClass%, , %waitSeconds%
        if (ErrorLevel) {
            return 0 ; The window class doesn't exist.
        } else {
            if(ignoreCurrent <= 0 || NewProcessId > 0) {
                return 1 ; We received our window!
            } else {
                return 0 ; Window exists, but do nothing
            }
        }
    }
    
    if(processWindowName != "") {
        WinWait, %processWindowName%, , %waitSeconds%
        if (ErrorLevel) {
            return 0 ; The window name doesn't exist.
        } else {
            if(ignoreCurrent <= 0 || NewProcessId > 0) {
                return 1 ; We received our window!
            } else {
                return 0 ; Window exists, but do nothing
            }
        }
    }
    
    return 0
}

; JLE_GetMostRecentWindow
; Gets window handle of most recent window for windowName and processName
; Checks windowName if window _truly_ belongs to processName
; Params: processName -- notepad.exe
;                 windowName -- Notepad
; Returns: windowHandle -- Recent window successfully gotten
;                    0 -- Recent window not successfully gotten
JLE_GetMostRecentWindow(processName, processWindowName = "", processWindowClass = "")
{
    if(processWindowClass != "") {
        WinGet, windowList, List, ahk_class %processWindowClass%
    } else if (processWindowName != "") {
        WinGet, windowList, List, %processWindowName%
    }
    
    Loop { ; Loop through each entry of windowList
        if windowList%A_Index% <= 0 ; Does array entry not exist?
            break
        
        ; Get windowHandle of array entry. Check its process name for a match
        windowHandle := windowList%A_Index%

        WinGet, windowProcessId, PID, ahk_id %windowHandle%
        WinGet, windowProcessName, ProcessName, ahk_id %windowHandle%
        if (windowProcessName = processName) {
            ; Check if UAC is needed
            windowIsUac := JLE_GetWindowUac(windowProcessId, windowHandle)
            if(windowIsUac = 1 && A_IsAdmin = 0) { ; Must elevate script
                Run RunTaskAsAdministrator.exe "%A_ScriptFullPath%"
                ExitApp
            }
            return %windowHandle% ; Got a match!
        }
    }
    
    return 0
}

; JLE_SendKeystrokeToWindow
; Activates window, and then sends keystrokes to it
; Note: Window must be activated to receive keystrokes.
; Params: windowHandle -- Window to receive keystrokes
;                 keystroke -- Keystrokes in "Send" format, e.g. ^o ==> Ctrl+O; ^!{Del} ==> Ctrl+Alt+Del
; Returns: 1 -- Keystroke successfully sent to window
;                    0 -- Window could not be activated
JLE_SendKeystrokeToWindow(windowHandle, keystroke)
{
    ; Activate most recent window
    WinActivate, ahk_id %windowHandle%

    ; Wait for most recent window to become active. Send keystroke.
    ; After 5 seconds, stop trying.
    WinWaitActive, ahk_id %windowHandle%, , 5
    if(ErrorLevel) {
        return 0
    } else {
        Send %keystroke%
        return 1
    }
    
    return 0
}

; Thanks to "Guest!"
; http://www.autohotkey.com/forum/topic55257.html

JLE_GetWindowUac(windowProcessId, windowHandle) {
TOKEN_QUERY:=0x0008
TokenElevationTypeDefault:=1
TokenElevationTypeFull:=2
TokenElevationTypeLimited:=3
TokenType:=8
TokenElevationType:=18

TokenInformationClass:=TokenElevationType

; Open process token first
windowProcessHandle := DllCall("OpenProcess", "UInt", 0x1000, "Int", false, "UInt", windowProcessId)
; Open process
ret:=DllCall("Advapi32.dll\OpenProcessToken", "UInt", windowProcessHandle, "UInt", TOKEN_QUERY, "UIntP", hToken)

; GetTokenInformation 1, for size
DllCall("Advapi32.dll\GetTokenInformation"
   , "UInt", hToken               
   , "UInt", TokenInformationClass
   , "Int", 0                     
   , "Int", 0                     
   , "UIntP", ReturnLength)       

sizeof_elevationType:=VarSetCapacity(elevationType, ReturnLength, 0)
; Get token information 2, for elevation
DllCall("Advapi32.dll\GetTokenInformation"
   , "UInt", hToken               
   , "UInt", TokenInformationClass
   , "UIntP", elevationType       
   , "Int", sizeof_elevationType  
   , "UIntP", ReturnLength)       

; Detect elevation type
result := 0
if (elevationType=TokenElevationTypeDefault) {
   ; Detect if user is really USER, or the native ADMINISTRATOR
   ; HACK alert: I'd rather inject the target process and call GetUsername, but I haven't done injecting yet.
   ; For now, change the window title, and check if change was successful
   WinGetTitle, CurWinTitle, ahk_id %windowHandle%
   WinSetTitle, ahk_id %windowHandle%, , %CurWinTitle%." "
   WinGetTitle, NewWinTitle, ahk_id %windowHandle%
   if(CurWinTitle=NewWinTitle)
      result := 1 ; Change was not successful, probably elevated.
   else
      result := 0 ; Change was successful, meaning that window is not elevated.
   WinSetTitle, ahk_id %windowHandle%, , %CurWinTitle% ; set it back
} else if (elevationType=TokenElevationTypeFull) {
   ; Split user, full privs
   result := 1
} else if (elevationType=TokenElevationTypeLimited) {
   ; Split user, limited privs
   result := 0
} else {
   ; Error retrieving info
   result := 0
}

; Close token handle
if (hToken)
   DllCall("CloseHandle", "UInt", hToken)

if (windowProcessHandle)
   DllCall("CloesHandle", "UInt", windowProcessHandle)

return result
}