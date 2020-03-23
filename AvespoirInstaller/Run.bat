@echo off
setlocal

cd /d %~dp0

start powershell -ExecutionPolicy ByPass -File ./Avespoir_Installer.ps1

endlocal
exit /B