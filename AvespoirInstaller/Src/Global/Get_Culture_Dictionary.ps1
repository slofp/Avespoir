function GetCultureDictionary {
	try {
		$DefaultLang = "en-US"
		$GetCulture = (Get-Culture).Name
		
		$LangData = Get-Variable -Name $GetCulture -ValueOnly -ea SilentlyContinue
		if ($LangData -eq $null) {
			Write-Host "Warning: " -NoNewline -ForegroundColor Yellow
			Write-Host "Not Exist Language Json"
			$LangData = Get-Variable -Name $DefaultLang -ea SilentlyContinue
		}

		$LangPSObject = ConvertFrom-Json $LangData
	
		return $LangPSObject
	}
	catch {
		Write-Host "Could not get culture dictionary" -ForegroundColor Red
		return $null
	}
}