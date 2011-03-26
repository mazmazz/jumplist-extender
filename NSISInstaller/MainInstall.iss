[Setup]
OutputBaseFilename=JumplistExtender_v0.3
VersionInfoVersion=0.3.0
VersionInfoProductVersion=0.3.0
AppVerName=Version 0.3
AppVersion=0.3
VersionInfoCompany=Marco Zafra
VersionInfoDescription=A custom jumplist creator for any program on Windows 7
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
Source: IssProc.dll; DestDir: {tmp}; Flags: dontcopy
;------ add IssProc extra language file (you don't need to add this file if you are using english only)
Source: IssProcLanguage.ini; DestDir: {tmp}; Flags: dontcopy
Source: Files\*; DestDir: {app}; Excludes: Files\Defaults\Icons\*; Flags: ignoreversion recursesubdirs overwritereadonly uninsremovereadonly
Source: Files\Defaults\Icons\*; DestDir: {app}\Defaults\Icons; Flags: recursesubdirs onlyifdoesntexist

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

[Code]
// IssFindModule called on install
function IssFindModule(hWnd: Integer; Modulename: PAnsiChar; Language: PAnsiChar; Silent: Boolean; CanIgnore: Boolean ): Integer;
external 'IssFindModule@files:IssProc.dll stdcall setuponly';

//********************************************************************************************************************************************
// IssFindModule function returns: 0 if no module found; 1 if cancel pressed; 2 if ignore pressed; -1 if an error occured
//
//  hWnd        = main wizard window handle.
//
//  Modulename  = module name(s) to check. You can use a full path to a DLL/EXE/OCX or wildcard file name/path. Separate multiple modules with semicolon.
//                 Example1 : Modulename='*mymodule.dll';     -  will search in any path for mymodule.dll
//                 Example2 : Modulename=ExpandConstant('{app}\mymodule.dll');     -  will search for mymodule.dll only in {app} folder (the application directory)
//                 Example3 : Modulename=ExpandConstant('{app}\mymodule.dll;*myApp.exe');   - just like Example2 + search for myApp.exe regardless of his path.
//
//  Language    = files in use language dialog. Set this value to empty '' and default english will be used
//                ( see and include IssProcLanguage.ini if you need custom text or other language)
//
//  Silent      = silent mode : set this var to true if you don't want to display the files in use dialog.
//                When Silent is true IssFindModule will return 1 if it founds the Modulename or 0 if nothing found
//
//  CanIgnore   = set this var to false to Disable the Ignore button forcing the user to close those applications before continuing
//                set this var to true to Enable the Ignore button allowing the user to continue without closing those applications
//******************************************************************************************************************************************


function NextButtonClick(CurPage: Integer): Boolean;
var
  hWnd: Integer;
  sModuleName: String;
  nCode: Integer;  {IssFindModule returns: 0 if no module found; 1 if cancel pressed; 2 if ignore pressed; -1 if an error occured }
begin
  Result := true;

 if CurPage = wpReady then
   begin
      Result := false;
      ExtractTemporaryFile('IssProcLanguage.ini');                          { extract extra language file - you don't need to add this line if you are using english only }
      hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));                     { get main wizard handle }
      sModuleName :=ExpandConstant('{app}\T7EBackground.exe;{app}\T7EPreferences.exe')                        { searched modules. Tip: separate multiple modules with semicolon Ex: '*mymodule.dll;*mymodule2.dll;*myapp.exe'}

     nCode:=IssFindModule(hWnd,sModuleName,'en',false,true);                { search for module and display files-in-use window if found  }
     //sModuleName:=IntToStr(nCode);
    // MsgBox ( sModuleName, mbConfirmation, MB_YESNO or MB_DEFBUTTON2);

     if nCode=1 then  begin                                                 { cancel pressed or files-in-use window closed }
          PostMessage (WizardForm.Handle, $0010, 0, 0);                     { quit setup, $0010=WM_CLOSE }
     end else if (nCode=0) or (nCode=2) then begin                          { no module found or ignored pressed}
          Result := true;                                                   { continue setup  }
     end;

  end;

end;


