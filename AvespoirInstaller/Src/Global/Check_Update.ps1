function CheckUpdate($Version) {
	$Latest = (ConvertFrom-Json (Invoke-WebRequest "https://gitlab.com/api/v4/projects/Avespoir_Project%2FAvespoir/repository/tags").Content)[0].name

	if ($Version -ne $Latest) {
		return $true
	}
	else {
		return $false
	}
}