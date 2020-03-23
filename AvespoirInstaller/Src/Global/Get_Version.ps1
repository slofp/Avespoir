function GetVersion {
	try {
		$Version = [System.IO.File]::ReadAllText("./Avespoir/Version", [System.Text.Encoding]::UTF8)
		return $Version
	}
	catch {
		return $null
	}
}