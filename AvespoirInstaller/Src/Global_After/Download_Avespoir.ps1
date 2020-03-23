#include
. "./Global/Get_Version.ps1"
. "./Global/Check_Update.ps1"
#endinclude

function DownloadAvespoir {
	Write-Host "Start Download Avespoir..." -ForegroundColor Magenta

	$Version = GetVersion
	$VersionNotExist = [string]::IsNullOrWhiteSpace($Version)
	if (!$VersionNotExist) {
		$UpdateCheck = CheckUpdate($Version)
		if (!$UpdateCheck) {
			Write-Host "This version is Latest"
			return
		}
	}

	if (Get-Command git -ea SilentlyContinue) {
		if (Get-Command dotnet -ea SilentlyContinue) {
			$OldPath = (Get-Location).Path
			$BinPath = $OldPath + "/Avespoir"
			Set-Location ($env:temp +"/")
			$RepoTemp = $env:temp + "/Avespoir"
			$Latest = (ConvertFrom-Json (Invoke-WebRequest "https://gitlab.com/api/v4/projects/Avespoir_Project%2FAvespoir/repository/tags").Content)[0].name

			if (Test-Path $RepoTemp) {
				Remove-Item -Recurse -Force $RepoTemp
			}

			Write-Host "Clone Avespoir..." -ForegroundColor Green
			Start-Process "git" -ArgumentList @("clone", "-b", $Latest, "https://gitlab.com/Avespoir_Project/Avespoir.git") -NoNewWindow -Wait
			Set-Location $RepoTemp

			Write-Host "Build Avespoir..." -ForegroundColor Green
			Start-Process "dotnet" -ArgumentList @("restore") -NoNewWindow -Wait
			Start-Process "dotnet" -ArgumentList @("build", "-c", "Release", "-o", $BinPath) -NoNewWindow -Wait

			Set-Location $BinPath
			$VersionPath = "./Version"
			Get-Variable "Latest" -ValueOnly | Out-File -Encoding UTF8 -NoNewline -FilePath $VersionPath

			Write-Host "End Download Avespoir." -ForegroundColor Green
			Set-Location $OldPath
			return
		}
		else {
			Write-Host "Dotnet Core SDK is not installed" -ForegroundColor Red
			return
		}
	}
	else {
		Write-Host "Git is not installed" -ForegroundColor Red
		return
	}
}