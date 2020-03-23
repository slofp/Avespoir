#include
. "./Global/Download_File.ps1"
#endinclude

function DotnetInstall.Windows {
	function InstallDotnet.Windows {
		if ([System.Environment]::Is64BitOperatingSystem -or [System.Environment]::Is64BitProcess) {
			$InstallFileUrl = "https://download.visualstudio.microsoft.com/download/pr/43660ad4-b4a5-449f-8275-a1a3fd51a8f7/a51eff00a30b77eae4e960242f10ed39/dotnet-sdk-3.1.200-win-x64.exe"
			$InstallFilePath = "$env:temp/dotnet-sdk-3.1.200-win-x64.exe"
	
			Write-Host "Download Dotnet Install File..."
			DownloadFile($InstallFileUrl, $InstallFilePath)
			if (!Test-Path $InstallFilePath) {
				Write-Host "Could not download Dotnet Install File"
	
				return
			}
			Write-Host "Successfully Downloaded Dotnet Install File."
	
			Write-Host "Excute Dotnet Install File..."
			Start-Process -FilePath $InstallFilePath -Wait
		}
		else {
			$InstallFileUrl = "https://download.visualstudio.microsoft.com/download/pr/05b7bc3e-69b2-4226-ad11-db472130e6e8/50e04d3ed87cde4a7aa2d591051bfafb/dotnet-sdk-3.1.200-win-x86.exe"
			$InstallFilePath = "$env:temp/dotnet-sdk-3.1.200-win-x86.exe"
	
			Write-Host "Download Dotnet Install File..."
			DownloadFile($InstallFileUrl, $InstallFilePath)
			if (!Test-Path $InstallFilePath) {
				Write-Host "Could not download Dotnet Install File"
	
				return
			}
			Write-Host "Successfully Downloaded Dotnet Install File."
	
			Write-Host "Excute Dotnet Install File..."
			Start-Process -FilePath $InstallFilePath -Wait
		}
		Write-Host "Excuted Dotnet Install File."
	
		Write-Host "Remove Dotnet Install File..."
		Remove-Item -Path $InstallFilePath
		Write-Host "Removed Dotnet Install File."
		
		return
	}
	
	if (Get-Command dotnet -ea SilentlyContinue) {
		$DotnetVersion = Invoke-Expression "dotnet --Version"
		if ($DotnetVersion -ge "3.1") {
			Write-Host "Dotnet is already installed"

			return
		}
		else {
			InstallDotnet.Windows

			return
		}
	}
	else {
		InstallDotnet.Windows

		return
	}
}