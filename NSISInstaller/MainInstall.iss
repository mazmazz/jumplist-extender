[Setup]
OutputBaseFilename=JumplistExtender_v0.4-B
VersionInfoVersion=0.4.1
VersionInfoProductVersion=0.4.1
AppVerName=Jumplist Extender v0.4-B
AppVersion=0.4-B
VersionInfoCompany=Marco Zafra
VersionInfoDescription=A custom jump list creator for any program.
VersionInfoCopyright=Released under GPLv3
Compression=lzma/ultra64
VersionInfoProductName=Jumplist Extender
MinVersion=0,6.1.7600
AppCopyright=Released under GPLv3
AppName=Jumplist Extender
InfoBeforeFile=CHANGELOG.txt
LicenseFile=COPYING.txt
ChangesAssociations=true
RestartIfNeededByRun=true
SetupIconFile=PrimaryIcon.ico
AppPublisher=Marco Zafra
AppPublisherURL=http://code.google.com/p/jumplist-extender
AppSupportURL=http://code.google.com/p/jumplist-extender
AppUpdatesURL=http://code.google.com/p/jumplist-extender
AppID={{2D5349D5-167D-4D27-BD8C-9117A6C63FED}
UninstallDisplayName=Jumplist Extender
DefaultDirName={pf}\JumplistExtender
DefaultGroupName=Jumplist Extender
AlwaysUsePersonalGroup=true

[InstallDelete]
;Name: {app}\T7ECommon.dll; Type: files
;Name: {app}\T7EPreferences.exe; Type: files
;Name: {app}\T7EBackground.exe; Type: files
;Name: {app}\NSISInstaller.exe; Type: files
Type: files; Name: "{commonstartup}\Jumplist Extender Applicator.lnk"
Type: files; Name: "{userstartup}\Jumplist Extender Applicator.lnk"

[Files]
;------ add IssProc (Files In Use Extension)
;Source: IssProc.dll; DestDir: {tmp}; Flags: dontcopy
;------ add IssProc extra language file (you don't need to add this file if you are using english only)
;Source: IssProcLanguage.ini; DestDir: {tmp}; Flags: dontcopy
Source: Files\*; DestDir: {app}; Excludes: Files\Defaults\Icons\*; Flags: ignoreversion recursesubdirs overwritereadonly uninsremovereadonly
;Source: Files\Defaults\Icons\*; DestDir: {app}\Defaults\Icons; Flags: recursesubdirs onlyifdoesntexist

[Dirs]
Name: {app}\Defaults\Icons; Flags: uninsalwaysuninstall

[Icons]
Name: {group}\Jumplist Extender; Filename: {app}\T7EPreferences.exe; WorkingDir: {app}; Comment: Create custom jumplists for any program on Windows 7.; IconIndex: 0
Name: {app}\Defaults\Icons\[00] shell32.dll; Filename: {sys}\shell32.dll

[Registry]
Root: HKCR; Subkey: .jlp; ValueType: string; ValueName: ; ValueData: JumplistPack; Flags: uninsdeletevalue
Root: HKCR; Subkey: JumplistPack; ValueType: string; ValueName: ; ValueData: Jumplist Pack; Flags: uninsdeletekey
Root: HKCR; Subkey: JumplistPack\DefaultIcon; ValueType: string; ValueName: ; ValueData: {app}\T7EPreferences.exe,0
Root: HKCR; Subkey: JumplistPack\shell\open\command; ValueType: string; ValueName: ; ValueData: """{app}\T7EPreferences.exe"" ""%1"""
Root: HKCU; Subkey: Software\Microsoft\Windows\CurrentVersion\Run; ValueType: string; ValueName: JumplistWatcher; ValueData: {app}\T7EBackground.exe; Flags: uninsdeletekey


[Run]
Filename: {app}\NSISInstaller.exe; Parameters: /inst:{{90fd0530-b663-4fe7-9291-189031b47993}; StatusMsg: Applying jump list settings; Flags: runascurrentuser runhidden
Filename: {app}\T7EBackground.exe; WorkingDir: {app}; Flags: runasoriginaluser nowait
Filename: {app}\T7EPreferences.exe; WorkingDir: {app}; Description: Start Jumplist Extender; Flags: postinstall runasoriginaluser nowait


[UninstallRun]
Filename: {app}\NSISInstaller.exe; Parameters: /uninst:{{f3ea12fa-c9a5-4c3d-990a-ba01be930e3e}; Flags: runminimized; RunOnceId: DelService
