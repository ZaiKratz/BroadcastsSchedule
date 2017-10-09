; -- 64Bit.iss --
; Demonstrates installation of a program built for the x64 (a.k.a. AMD64)
; architecture.
; To successfully run this installation and the program it installs,
; you must have a "x64" edition of Windows.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

; Имя приложения
#define   Name       "YouTube Broadcasts Schedule"
; Версия приложения
#define   Version    "1.0.0"
; Фирма-разработчик
#define   Publisher  "3DMaya"
; Сафт фирмы разработчика
#define   URL        "3dmaya.com.ua"
; Имя исполняемого модуля
#define   ExeName    "BroadcastsSchedule.exe"

[Setup]
AppId = {{96816BBB-6463-41FA-AD88-9A72B6BA310F}}
AppName={#Name}
AppVersion={#Version}
DefaultDirName={pf}\{#Name}
DefaultGroupName={#Name}
;UninstallDisplayIcon={app}\MyProg.exe
Compression=lzma
SolidCompression=yes
OutputDir= D:\Sadovnichy\Installer
OutputBaseFileName=Setup
SetupIconFile=D:\Sadovnichy\BroadcastsSchedule\YouTube-icon-big.ico
; "ArchitecturesAllowed=x64" specifies that Setup cannot run on
; anything but x64.
;ArchitecturesAllowed=x64
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64


[Tasks]
; Создание иконки на рабочем столе
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Исполняемый файл
Source: "D:\Sadovnichy\BroadcastsSchedule\BroadcastsSchedule\bin\Release\BroadcastsSchedule.exe"; DestDir: "{app}"; Flags: ignoreversion
; Прилагающиеся ресурсы
Source: "D:\Sadovnichy\BroadcastsSchedule\BroadcastsSchedule\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
;Source: "D:\Sadovnichy\BroadcastsSchedule\packages"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; .NET Framework 4.0
Source: "D:\Sadovnichy\BroadcastsSchedule\dotNetFx40_Full_x86_x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; Check: not IsRequiredDotNetDetected

[Icons]
Name: "{group}\{#Name}"; Filename: "{app}\{#ExeName}"

Name: "{commondesktop}\{#Name}"; Filename: "{app}\{#ExeName}"; Tasks: desktopicon

[Code]
#include "dotnet.pas"

[Run]
;------------------------------------------------------------------------------
;   Секция запуска после инсталляции
;------------------------------------------------------------------------------
Filename: {tmp}\dotNetFx40_Full_x86_x64.exe; Parameters: "/q:a /c:""install /l /q"""; Check: not IsRequiredDotNetDetected; StatusMsg: Microsoft Framework 4.0 is installed. Please wait...