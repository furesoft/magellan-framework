@echo off
rem Welcome!
rem
rem This build script does the following
rem 
rem   1. Compiles all Magellan code
rem   2. Builds source ZIP file
rem   3. Builds binaries ZIP files
rem   4. Builds ruby gem
rem   5. Runs unit tests
rem 
rem  The parameters below control the sample version number that is used in the built files.
rem  Generally these would come from the CI server

color 0A
echo Building...
color

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" build\Local.proj /t:Package /verbosity:minimal /nologo

pause