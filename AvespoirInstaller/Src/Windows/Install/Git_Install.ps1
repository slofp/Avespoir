#include
. "./Global/Download_File.ps1"
#endinclude

function GitInstall.Windows {
	function InstallGit.Windows {
		if ([System.Environment]::Is64BitOperatingSystem -or [System.Environment]::Is64BitProcess) {
			$InstallFileUrl = "https://github.com/git-for-windows/git/releases/download/v2.25.1.windows.1/Git-2.25.1-64-bit.exe"
			$InstallFilePath = "$env:temp/v2.25.1.windows.1/Git-2.25.1-64-bit.exe"
	
			Write-Host "Download Git Install File..."
			DownloadFile($InstallFileUrl, $InstallFilePath)
			if (!Test-Path $InstallFilePath) {
				Write-Host "Could not download Git Install File"
	
				return
			}
			Write-Host "Successfully Downloaded Git Install File."
	
			Write-Host "Excute Git Install File..."
			Start-Process -FilePath $InstallFilePath -Wait
		}
		else {
			$InstallFileUrl = "https://github.com/git-for-windows/git/releases/download/v2.25.1.windows.1/Git-2.25.1-32-bit.exe"
			$InstallFilePath = "$env:temp/v2.25.1.windows.1/Git-2.25.1-32-bit.exe"
	
			Write-Host "Download Git Install File..."
			DownloadFile($InstallFileUrl, $InstallFilePath)
			if (!Test-Path $InstallFilePath) {
				Write-Host "Could not download Git Install File"
	
				return
			}
			Write-Host "Successfully Downloaded Git Install File."
	
			Write-Host "Excute Git Install File..."
			Start-Process -FilePath $InstallFilePath -Wait
		}
		Write-Host "Excuted Git Install File."
	
		Write-Host "Remove Git Install File..."
		Remove-Item -Path $InstallFilePath
		Write-Host "Removed Git Install File."
		
		return
	}
	
	if (Get-Command git -ea SilentlyContinue) {
		Write-Host "Git is already installed"

		return
	}
	else {
		InstallGit.Windows

		return
	}
}