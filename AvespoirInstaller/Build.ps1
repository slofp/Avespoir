Set-StrictMode -Version 3.0
$ExportPath = "./Avespoir_Installer.ps1"

function Init {
	if (Test-Path $ExportPath) {
		Remove-Item $ExportPath
	}

	$Linuxuse = "#! /usr/bin/pwsh `n"
	Add-Content -Path $ExportPath -Value $Linuxuse -Encoding Unicode

	return
}

function Lang {
	$LangPath = "./Src/Lang/"
	$LangList = (Get-ChildItem $LangPath | Where-Object {$_.Name -like "*.json"}).Name
	foreach ($LangFile in $LangList) {
		$LangData = Get-Content -Raw -Encoding UTF8 -Path ($LangPath + $LangFile)
		$LangName = ($LangFile.Split("."))[0]

		$LangDefinition = "`${$LangName} = `'$LangData`' `n"

		Add-Content -Path $ExportPath -Value $LangDefinition -Encoding Unicode
	}

	return
}

function Global {
	$GlobalPath = "./Src/Global/"
	$GlobalList = (Get-ChildItem $GlobalPath | Where-Object {$_.Name -like "*.ps1"}).Name
	foreach ($GlobalFile in $GlobalList) {
		$GlobalData = Get-Content -Raw -Encoding UTF8 -Path ($GlobalPath + $GlobalFile)

		$GlobalDefinition = "$GlobalData `n"

		Add-Content -Path $ExportPath -Value $GlobalDefinition -Encoding Unicode
	}

	return
}

function Global_After {
	$Global_AfterPath = "./Src/Global_After/"
	$Global_AfterList = (Get-ChildItem $Global_AfterPath | Where-Object {$_.Name -like "*.ps1"}).Name
	foreach ($Global_AfterFile in $Global_AfterList) {
		[System.IO.StreamReader] $Global_AfterData = New-Object System.IO.StreamReader(($Global_AfterPath + $Global_AfterFile), [System.Text.Encoding]::UTF8)
		$include = $false
		while (($Global_AfterDataLine = $Global_AfterData.ReadLine()) -ne $null) {
			if ($Global_AfterDataLine -eq "#include") {
				$include = $true
			}
			if ($Global_AfterDataLine -eq "#endinclude") {
				$include = $false
				continue
			}
			if ($include) {
				continue
			}

			Add-Content -Path $ExportPath -Value $Global_AfterDataLine -Encoding Unicode
		}
		$Global_AfterData.Close()
	}

	return
}

function Linux {
	$LinuxPath = "./Src/Linux/"
	$LinuxList = (Get-ChildItem $LinuxPath | Where-Object {$_.Name -like "*.ps1"}).Name
	foreach ($LinuxFile in $LinuxList) {
		[System.IO.StreamReader] $LinuxData = New-Object System.IO.StreamReader(($LinuxPath + $LinuxFile), [System.Text.Encoding]::UTF8)
		$include = $false
		while (($LinuxDataLine = $LinuxData.ReadLine()) -ne $null) {
			if ($LinuxDataLine -eq "#include") {
				$include = $true
			}
			if ($LinuxDataLine -eq "#endinclude") {
				$include = $false
				continue
			}
			if ($include) {
				continue
			}

			Add-Content -Path $ExportPath -Value $LinuxDataLine -Encoding Unicode
		}

		$LinuxData.Close()
	}

	return
}

function Windows {
	function Install {
		$InstallPath = "./Src/Windows/Install/"
		$InstallList = (Get-ChildItem $InstallPath | Where-Object {$_.Name -like "*.ps1"}).Name
		foreach ($InstallFile in $InstallList) {
			[System.IO.StreamReader] $InstallData = New-Object System.IO.StreamReader(($InstallPath + $InstallFile), [System.Text.Encoding]::UTF8)
			$include = $false
			while (($InstallDataLine = $InstallData.ReadLine()) -ne $null) {
				if ($InstallDataLine -eq "#include") {
					$include = $true
				}
				if ($InstallDataLine -eq "#endinclude") {
					$include = $false
					continue
				}
				if ($include) {
					continue
				}

				Add-Content -Path $ExportPath -Value $InstallDataLine -Encoding Unicode
			}
			$InstallData.Close()
		}

		return
	}
	Install

	$WindowsPath = "./Src/Windows/"
	$WindowsList = (Get-ChildItem $WindowsPath | Where-Object {$_.Name -like "*.ps1"}).Name
	foreach ($WindowsFile in $WindowsList) {
		[System.IO.StreamReader] $WindowsData = New-Object System.IO.StreamReader(($WindowsPath + $WindowsFile), [System.Text.Encoding]::UTF8)
		$include = $false
		while (($WindowsDataLine = $WindowsData.ReadLine()) -ne $null) {
			if ($WindowsDataLine -eq "#include") {
				$include = $true
			}
			if ($WindowsDataLine -eq "#endinclude") {
				$include = $false
				continue
			}
			if ($include) {
				continue
			}

			Add-Content -Path $ExportPath -Value $WindowsDataLine -Encoding Unicode
		}

		$WindowsData.Close()
	}

	return
}

function Index {
	$IndexPath = "./Src/"
	$IndexList = (Get-ChildItem $IndexPath | Where-Object {$_.Name -like "*.ps1"}).Name
	foreach ($IndexFile in $IndexList) {
		[System.IO.StreamReader] $IndexData = New-Object System.IO.StreamReader(($IndexPath + $IndexFile), [System.Text.Encoding]::UTF8)
		$include = $false
		while (($IndexDataLine = $IndexData.ReadLine()) -ne $null) {
			if ($IndexDataLine -eq "#include") {
				$include = $true
			}
			if ($IndexDataLine -eq "#endinclude") {
				$include = $false
				continue
			}
			if ($include) {
				continue
			}

			Add-Content -Path $ExportPath -Value $IndexDataLine -Encoding Unicode
		}

		$IndexData.Close()
	}

	return
}

function CombiningFiles {
	Init
	Lang
	Global
	Global_After
	Windows
	Index

	return
}

CombiningFiles