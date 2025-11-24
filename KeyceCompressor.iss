; Inno Setup script for Keyce Compressor
[Setup]
AppName=Keyce Compressor
AppVersion=1.0.0
DefaultDirName={pf}\Keyce Compressor
DefaultGroupName=Keyce Compressor
OutputBaseFilename=KeyceCompressor-Setup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
DisableProgramGroupPage=no
Uninstallable=yes
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Copy everything from dist\KeyceCompressor
Source: "dist\KeyceCompressor\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Keyce Compressor"; Filename: "{app}\KeyceCompressor.exe"; WorkingDir: "{app}"; IconFilename: "{app}\Assets\icon.ico"
Name: "{commondesktop}\Keyce Compressor"; Filename: "{app}\KeyceCompressor.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Registry]
; Associate .keyce file extension
Root: HKCR; Subkey: ".keyce"; ValueType: string; ValueName: ""; ValueData: "KeyceCompressor.File"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "KeyceCompressor.File\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\Assets\icon.ico"; Flags: uninsdeletekey
Root: HKCR; Subkey: "KeyceCompressor.File\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\KeyceCompressor.exe"" ""%1"""; Flags: uninsdeletekey

[Run]
; Optionally run the application after install
Filename: "{app}\KeyceCompressor.exe"; Description: "Launch Keyce Compressor"; Flags: nowait postinstall skipifsilent

[Code]
function IsDotNet472Installed(): Boolean;
var
  installRelease: Cardinal;
begin
  { .NET 4.7.2 has release DWORD 461808 (Windows 10 RS3+). }
  if RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', installRelease) then
  begin
    Result := installRelease >= 461808;
  end
  else
    Result := False;
end;

function InitializeSetup(): Boolean;
begin
  if not IsDotNet472Installed() then
  begin
    if MsgBox('.NET Framework 4.7.2 or later is required. Open download page?', mbConfirmation, MB_YESNO) = IDYES then
      ShellExec('', 'https://dotnet.microsoft.com/download/dotnet-framework/net472', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
    Result := False; // cancel installation until .NET installed
  end
  else
    Result := True;
end;