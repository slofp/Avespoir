#include
. "./Windows/Main_Menu.ps1"
#endinclude

if ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)) {
	MainMenu.Windows
}
elseif ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Linux)) {
	Write-Host "This OS is not Windows and cannot be run" -ForegroundColor Red
}

Start-Sleep -Seconds 3
exit