@echo off
@setlocal
set ERROR_CODE=0
src\Fssec.CLI\bin\Debug\net6.0\Fssec.CLI.exe %*

:end
exit /B %ERROR_CODE%