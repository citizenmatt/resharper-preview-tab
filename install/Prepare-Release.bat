@echo off
setlocal enableextensions

set PLUGIN=PreviewTab

mkdir %PLUGIN%.7.1 2> NUL
copy /y ..\src\resharper-preview-tab\bin\Release\*.* %PLUGIN%.7.1\
