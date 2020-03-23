function ClientConfigSetting($LanguageObject) {
	Write-Host "Start ClientConfig.json setting..." -ForegroundColor Magenta

	$TokenCheck = $true
	while ($TokenCheck) {
		Write-Host $LanguageObject.CCS.Token
		if (($Token = Read-Host -Prompt "Token")) {
			$TokenCheck = $false
		}
	}

	Write-Host $LanguageObject.CCS.BotownerId
	$BotownerId = Read-Host -Prompt "Botowner ID(Default: 0)"
	if ([string]::IsNullOrWhiteSpace($BotownerId)) {
		$BotownerId = "0"
	}

	Write-Host $LanguageObject.CCS.MainPrefix
	$MainPrefix = Read-Host -Prompt "Main prefix(Default: null)"

	Write-Host $LanguageObject.CCS.PublicPrefixTag
	$PublicPrefixTag = Read-Host -Prompt "Public prefix tag(Default: $)"
	if ([string]::IsNullOrWhiteSpace($PublicPrefixTag)) {
		$PublicPrefixTag = "$"
	}

	Write-Host $LanguageObject.CCS.ModeratorPrefixTag
	$ModeratorPrefixTag = Read-Host -Prompt "Moderator prefix tag(Default: @)"
	if ([string]::IsNullOrWhiteSpace($ModeratorPrefixTag)) {
		$ModeratorPrefixTag = "@"
	}

	Write-Host $LanguageObject.CCS.BotownerPrefixTag
	$BotownerPrefixTag = Read-Host -Prompt "Botowner prefix tag(Default: >)"
	if ([string]::IsNullOrWhiteSpace($BotownerPrefixTag)) {
		$BotownerPrefixTag = ">"
	}

	Write-Host (
		"Token: " + $Token + "`n" +
		"BotownerId: " + $BotownerId + "`n" +
		"MainPrefix: " + $MainPrefix + "`n" +
		"PublicPrefixTag: " + $PublicPrefixTag + "`n" +
		"ModeratorPrefixTag: " + $ModeratorPrefixTag + "`n" +
		"BotownerPrefixTag: " + $BotownerPrefixTag + "`n"
	)

	$ConfirmCheck = $true
	while ($ConfirmCheck) {
		if (($Confirm = Read-Host -Prompt ($LanguageObject.Confirm + "(y or n)"))) {
			if ($Confirm -eq "y") {
				$ClientConfigPSObject = New-Object PSObject -Property @{
					Token = $Token
					BotownerId = $BotownerId
					MainPrefix = $MainPrefix
					PublicPrefixTag = $PublicPrefixTag
					ModeratorPrefixTag = $ModeratorPrefixTag
					BotownerPrefixTag = $BotownerPrefixTag
				}
			
				if (!(Test-Path "./Avespoir")) {
					New-Item "./Avespoir" -ItemType Directory | Out-Null
				}
				if (!(Test-Path "./Avespoir/Configs")) {
					New-Item "./Avespoir/Configs" -ItemType Directory | Out-Null
				}
				ConvertTo-Json $ClientConfigPSObject | Out-File -Encoding UTF8 -FilePath "./Avespoir/Configs/ClientConfig.json"
		
				$ConfirmCheck = $false
			}
			elseif ($Confirm -eq "n") {
				Write-Host $LanguageObject.Cancel
		
				return
			}
		}
	}

	Write-Host "End ClientConfig.json setting." -ForegroundColor Green

	return
}