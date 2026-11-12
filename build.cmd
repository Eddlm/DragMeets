@echo off
rem Build Drag Meets. Pass a GTA V root to also deploy the DLL.
setlocal
cd /d "%~dp0src\DragMeets2.2" || exit /b 1
dotnet build -nologo || exit /b 1
if "%~1"=="" (
  echo Built src\DragMeets2.2\bin\Debug\DragMeets.dll
  exit /b 0
)
if not exist "%~1\scripts" (
  echo No scripts folder under %~1
  exit /b 1
)
copy /y "bin\Debug\DragMeets.dll" "%~1\scripts\DragMeets.dll" >nul || exit /b 1
echo Deployed to %~1\scripts\DragMeets.dll
