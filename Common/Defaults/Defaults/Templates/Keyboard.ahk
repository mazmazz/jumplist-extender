; Jumplist Extender
; AutoHotkey Keyboard Stroke Script
; For "{AppName}", {AppProcessName}
;
; Sends keystrokes to the specified app
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

#Include {Path_AppData}\AHK\JLE_Common.ahk

JLE_AppId := "{AppId}"
JLE_AppName := "{AppName}"
JLE_AppPath := "{AppPath}"
JLE_AppProcessName := "{AppProcessName}"
JLE_AppWindowClassName := "{AppWindowClassName}"
JLE_StartNewProcess := {KBDStartNewProcess}
JLE_KBDIgnoreAbsent := {KBDIgnoreAbsent}
JLE_KBDIgnoreCurrent := {KBDIgnoreCurrent}

; Keystroke is below, on JLE_SendKeystrokeToWindow()

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

; Does process exist at all? Start a process.
ProcessExists := JLE_CheckProcessWindowExists(JLE_AppProcessName, JLE_AppPath, JLE_AppName, JLE_AppWindowClassName, JLE_StartNewProcess, -1, JLE_KBDIgnoreAbsent, JLE_KBDIgnoreCurrent)
if(ProcessExists <= 0) 
  exit

; Get most recent active window
RecentWindow := JLE_GetMostRecentWindow(JLE_AppProcessName, JLE_AppName, JLE_AppWindowClassName)
if(RecentWindow <= 0) 
  exit

; Send keystroke!
JLE_SendKeystrokeToWindow(RecentWindow, "{Keystroke}")