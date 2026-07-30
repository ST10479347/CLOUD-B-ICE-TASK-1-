@echo off
title Azurite Local Storage
echo Starting Azurite Local Storage...
echo.

npx azurite --silent --location "%TEMP%\azurite" --debug "%TEMP%\azurite\debug.log"

pause