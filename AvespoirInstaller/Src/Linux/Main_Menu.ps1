#include
. "./Global/Get_Culture_Dictionary.ps1"
. "./Global/Get_Version.ps1"
. "./Global/Check_Update.ps1"
. "./Global/ClientConfig_Setting.ps1"
. "./Global/DBConfig_Setting.ps1"
. "./Global_After/Download_Avespoir.ps1"
. "./Global/Bot_Run.ps1"
#endinclude

function MainMenu.Linux {
	$LanguageObject = GetCultureDictionary
	Write-Host $LanguageObject.MainMenu.Welcome

	[bool]$LoopCheck = $true
	while ($LoopCheck) {
		$Version = GetVersion
		if (!([string]::IsNullOrWhiteSpace($Version))) {
			Write-Host ($LanguageObject.MainMenu.CurrentVersion + ": " + $Version)

			$UpdateCheck = CheckUpdate($Version)
			if ($UpdateCheck) {
				Write-Host $LanguageObject.MainMenu.Update
			}
		}

		Write-Host (
			"[1]: " + $LanguageObject.MainMenu.1 + "`n" +
			"[2]: " + $LanguageObject.MainMenu.2 + "`n" +
			"[3]: " + $LanguageObject.MainMenu.4 + "`n" +
			"[4]: " + $LanguageObject.MainMenu.5 + "`n" +
			"[5]: " + $LanguageObject.MainMenu.6
		)
		$SelectNumber = Read-Host -Prompt $LanguageObject.MainMenu.ReadLineLinux

		# Download Avespoir
		if ($SelectNumber -eq 1) {
			DownloadAvespoir
			Write-Host $LanguageObject.ReturnMainMenu
			Start-Sleep -Seconds 3
		}
		# Run Avespoir
		elseif ($SelectNumber -eq 2) {
			BotRun
			Write-Host $LanguageObject.ReturnMainMenu
			Start-Sleep -Seconds 3
		}
		# ClientConfig,json setup
		elseif ($SelectNumber -eq 3) {
			ClientConfigSetting($LanguageObject)
			Write-Host $LanguageObject.ReturnMainMenu
			Start-Sleep -Seconds 3
		}
		# MongoDBConfig.json setup
		elseif ($SelectNumber -eq 4) {
			DBConfigSetting($LanguageObject)
			Write-Host $LanguageObject.ReturnMainMenu
			Start-Sleep -Seconds 3
		}
		# Exit
		elseif ($SelectNumber -eq 5) {
			Write-Host $LanguageObject.Exit
			return
		}
		else {
			Clear-Host
		}
	}
}
