@echo off
set address=%~dp0
cd %windir%\system32\inetsrv
appcmd add site /name:WebGameService /id:100 /physicalPath:%address% /bindings:http/*:8080:
