set "BuildVersion=0.001"
:start
echo off
cls
echo ---===[Welcome to the Project Easy Builder]===---
echo.
echo Let me check your internet connection...
echo It will spend a few seconds.

ping 8.8.8.8 -n 4 > nul
if errorLevel 1 (
echo.
echo No connection to internet, please, check your internet connection and try again.
echo.
pause
exit
) else (
goto :creatingfolder
)

:creatingfolder
cls
echo ---===[Welcome to the Project Easy Builder]===---
echo.
echo That's good, access to DNS Google obtained!
echo.
echo Let's start Unity, build vervion is %BuildVersion%
"C:\Program Files\Unity\Hub\Editor\2019.2.16f1\Editor\Unity.exe" -username "thealexsmithcore@gmail.com" -password "WtSSPaSS098" -projectPath ""
pause
