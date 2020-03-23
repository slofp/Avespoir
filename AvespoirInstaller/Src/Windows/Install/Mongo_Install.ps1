#include
. "./Global/Download_File.ps1"
#endinclude

function MongoInstall.Windows {
	function InstallMongo.Windows {
		$InstallFileUrl = "https://fastdl.mongodb.org/win32/mongodb-win32-x86_64-2012plus-4.2.3-signed.msi"
		$InstallFilePath = "$env:temp/mongodb-win32-x86_64-2012plus-4.2.3-signed.msi"

		Write-Host "Download MongoDB Install File..."
		DownloadFile($InstallFileUrl, $InstallFilePath)
		if (!Test-Path $InstallFilePath) {
			Write-Host "Could not download MongoDB Install File"

			return
		}
		Write-Host "Successfully Downloaded MongoDB Install File."

		Write-Host "Excute MongoDB Install File..."
		$CommandArgs = @("/i", $InstallFilePath)
		Start-Process -FilePath "msiexec" -ArgumentList $CommandArgs -Wait
		Write-Host "Excuted MongoDB Install File."

		Write-Host "Remove MongoDB Install File..."
		Remove-Item -Path $InstallFilePath
		Write-Host "Removed MongoDB Install File."
		
		return
	}

	if (Get-Command mongo -ea SilentlyContinue) {
		$MongoVersion = ((mongo --version).Split(" ") | Select-Object -Skip 3 -First 1).SubString(1)
		if ($MongoVersion -ge "4.2") {
			Write-Host "MongoDB is already installed"

			return
		}
		else {
			InstallMongo.Windows

			return
		}
	}
	else {
		InstallMongo.Windows

		return
	}
}

