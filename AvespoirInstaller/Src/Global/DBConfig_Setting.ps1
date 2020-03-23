function DBConfigSetting($LanguageObject) {
	Write-Host "Start DBConfig.json setting..." -ForegroundColor Magenta

	Write-Host $LanguageObject.DCS.DBSetupped -ForegroundColor Yellow
	$DBSetupCheck = $true
	while ($DBSetupCheck) {
		if (($DBSetupped = Read-Host -Prompt ($LanguageObject.DCS.ConfirmDBSetup + "(y or n)"))) {
			if ($DBSetupped -eq "y") {
				$DBSetupCheck = $false
			}
			elseif ($DBSetupped -eq "n") {
				Write-Host $LanguageObject.Cancel
		
				return
			}
		}
	}

	Write-Host $LanguageObject.DCS.UseDatabase
	$UseDatabase = Read-Host -Prompt "UseDatabase(Default: admin)"
	if ([string]::IsNullOrWhiteSpace($UseDatabase)) {
		$UseDatabase = "admin"
	}

	Write-Host $LanguageObject.DCS.Url
	$Url = Read-Host -Prompt "Url(Default: localhost)"
	if ([string]::IsNullOrWhiteSpace($Url)) {
		$Url = "localhost"
	}

	Write-Host $LanguageObject.DCS.Port
	[int]$Port = Read-Host -Prompt "Port(Default: 27017)"
	if ($Port -eq 0) {
		$Port = 27017
	}

	$UsernameCheck = $true
	while ($UsernameCheck) {
		Write-Host $LanguageObject.DCS.Username
		if (($Username = Read-Host -Prompt "Username")) {
			$UsernameCheck = $false
		}
	}

	$PasswordCheck = $true
	while ($PasswordCheck) {
		Write-Host $LanguageObject.DCS.Password
		if (($Password = Read-Host -Prompt "Password")) {
			$PasswordCheck = $false
		}
	}

	Write-Host $LanguageObject.DCS.Mechanism
	$Mechanism = Read-Host -Prompt "Mechanism(Default: SCRAM-SHA-256)"
	if ([string]::IsNullOrWhiteSpace($Mechanism)) {
		$Mechanism = "SCRAM-SHA-256"
	}

	Write-Host $LanguageObject.DCS.MainDatabase
	$MainDatabase = Read-Host -Prompt "MainDatabase(Default: DiscordBot)"
	if ([string]::IsNullOrWhiteSpace($MainDatabase)) {
		$MainDatabase = "DiscordBot"
	}

	Write-Host (
		"UseDatabase: " + $UseDatabase + "`n" +
		"Url: " + $Url + "`n" +
		"Port: " + $Port + "`n" +
		"Username: " + $Username + "`n" +
		"Password: " + $Password + "`n" +
		"Mechanism: " + $Mechanism + "`n" +
		"MainDatabase: " + $MainDatabase
	)

	$ConfirmCheck = $true
	while ($ConfirmCheck) {
		if (($Confirm = Read-Host -Prompt ($LanguageObject.Confirm + "(y or n)"))) {
			if ($Confirm -eq "y") {
				$DBConfigPSObject = New-Object PSObject -Property @{
					UseDatabase  = $UseDatabase
					Url          = $Url
					Port         = $Port
					Username     = $Username
					Password     = $Password
					Mechanism    = $Mechanism
					MainDatabase = $MainDatabase
				}
			
				if (!(Test-Path "./Avespoir")) {
					New-Item "./Avespoir" -ItemType Directory | Out-Null
				}
				if (!(Test-Path "./Avespoir/Configs")) {
					New-Item "./Avespoir/Configs" -ItemType Directory | Out-Null
				}
				ConvertTo-Json $DBConfigPSObject | Out-File -Encoding UTF8 -FilePath "./Avespoir/Configs/DBConfig.json"
		
				$ConfirmCheck = $false
			}
			elseif ($Confirm -eq "n") {
				Write-Host $LanguageObject.Cancel
		
				return
			}
		}
	}

	Write-Host "End DBConfig.json setting." -ForegroundColor Green

	return
}