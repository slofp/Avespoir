#include
. "./Windows/Install/Git_Install.ps1"
. "./Windows/Install/Dotnet_Install.ps1"
. "./Windows/Install/Mongo_Install.ps1"
#endinclude

function PrerequisitesInstall.Windows {
	Write-Host "Start install prerequisites..." -ForegroundColor Magenta

	GitInstall.Windows
	DotnetInstall.Windows
	MongoInstall.Windows

	Write-Host "End install prerequisites." -ForegroundColor Green

	return
}