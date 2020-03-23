@echo off
setlocal

cd /d %~dp0

start /B /WAIT powershell -ExecutionPolicy ByPass -File ./Build.ps1

endlocal
echo Build Finished
pause
exit /B