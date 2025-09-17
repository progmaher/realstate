@echo off
echo Publishing Real Estate Application with Self-Contained x64 Deployment...

rem Create publish directory if it doesn't exist
if not exist "publish" mkdir publish

rem Clean previous build
dotnet clean --configuration Release

rem Restore dependencies
echo Restoring dependencies...
dotnet restore

rem Build the application
echo Building application...
dotnet build --configuration Release --no-restore

rem Publish with self-contained x64 target
echo Publishing self-contained x64 deployment...
dotnet publish --configuration Release --runtime win-x64 --self-contained true --output ./publish/win-x64 --verbosity normal

echo.
echo ========================================
echo Publish completed successfully!
echo ========================================
echo Output directory: .\publish\win-x64
echo Executable: .\publish\win-x64\Home.exe
echo.
echo The application is ready for deployment with all runtime dependencies included.
echo.
pause