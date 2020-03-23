function BotRun {
	Write-Host "Start Bot Run..." -ForegroundColor Magenta

	$OldPath = (Get-Location).Path
	$BinPath = "./Avespoir"
	if (!(Test-Path $BinPath)) {
		Write-Host "Not exist Avespoir directory" -ForegroundColor Red
		return
	}
	Set-Location $BinPath

	if (Test-Path "./Avespoir.dll") {
		Start-Process "dotnet" -ArgumentList @("./Avespoir.dll") -NoNewWindow -Wait
	}
	elseif (Test-Path "./AvespoirTest.Test.dll") {
		Start-Process "dotnet" -ArgumentList @("./AvespoirTest.Test.dll") -NoNewWindow -Wait
	}
	else {
		Write-Host "Not exist Excute Avespoir files" -ForegroundColor Red
		return
	}

	Write-Host "End Bot Run." -ForegroundColor Green
	
}