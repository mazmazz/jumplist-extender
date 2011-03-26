Const CSIDL_COMMON_PROGRAMS = &H17
Const CSIDL_PROGRAMS = &H2

Set args = WScript.Arguments
verbCommand = args.Item(0)
lnkFolder = args.Item(1)
lnkFilename = args.Item(2)

verbPinStart = 

' Default
' shell32.dll
' 5381 - Pin to Start Men&u
' 5382 - Unpin from Start Men&u
' 5386 - Pin to Tas&kbar
' 5387 - Unpin from Tas&kbar

verbName = "Pin to Taskbar"
If verbCommand = "/pinStart" Then verbName = "Pin to Start Menu"
If verbCommand = "/pinTaskbar" Then verbName = "Pin to Taskbar"
If verbCommand = "/unpin" AND InStr(lnkFolder, "StartMenu") > 0 Then verbName = "Unpin from Start Menu"
If verbCommand = "/unpin" AND InStr(lnkFolder, "TaskBar") > 0 Then verbName = "Unpin from Taskbar"

WScript.Echo lnkFolder & " | " & lnkFilename & " | " & verbName & " || "

Set objShell = CreateObject("Shell.Application")
Set objFolder = objShell.Namespace(lnkFolder)
Set objFolderItem = objFolder.ParseName(lnkFilename)
Set colVerbs = objFolderItem.Verbs
For Each objVerb in colVerbs
    If Replace(objVerb.name, "&", "") = verbName Then objVerb.DoIt
Next