#include
. "./Windows/Main_Menu.ps1"
. "./Linux/Main_Menu.ps1"
#endinclude

if ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)) {
	MainMenu.Windows
}
elseif ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Linux)) {
	MainMenu.Linux
}

Start-Sleep -Seconds 3
exit