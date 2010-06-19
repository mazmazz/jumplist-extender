;;;;;;;;;;;;;;;;;;;;;;
; Jumplist Extender
; AutoHotKey Script - For "{AppName}"
;
; Scripts run on their own. You must start the app from this script, if desired.
; Below are some lines to get you started. Refer to the help for more information.
; Jump to the VERY BOTTOM to start coding!
;;;;;;;;;;;;;;;;;;;;;;

#Include {Path_AppData}\AHK\JLE_Common.ahk

JLE_AppId := "{AppId}"
JLE_AppName := "{AppName}"
JLE_AppPath := "{AppPath}"
JLE_AppProcessName := "{AppProcessName}"
JLE_AppWindowClassName := "{AppWindowClassName}"
JLE_StartNewProcess := 0

;;;;;;;;;;;;;;;;;;;;;;

; Does process exist at all? Start a process.
ProcessExists := JLE_CheckProcessWindowExists(JLE_AppProcessName, JLE_AppPath, JLE_AppName, JLE_AppWindowClassName, JLE_StartNewProcess, JLE_KBDDelay)
if(ProcessExists <= 0) 
    exit

; Get most recent active window
RecentWindow := JLE_GetMostRecentWindow(JLE_AppProcessName, JLE_AppName, JLE_AppWindowClassName)
if(RecentWindow <= 0) 
    exit

; Activate window
WinActivate, ahk_id %RecentWindow%
WinWaitActive, ahk_id %RecentWindow%, , 5

;;;;;;;;;;;;;;;;;;;;;;
; Do things below!
; Refer to Help for available JLE-specific functions
;;;;;;;;;;;;;;;;;;;;;;
