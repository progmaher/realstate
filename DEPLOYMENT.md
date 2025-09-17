# Real Estate Application - Self-Contained Deployment Guide

## Overview

This application supports self-contained deployment with x64 target, which means it can run on target servers without requiring .NET runtime to be pre-installed. All necessary runtime dependencies are included in the deployment package.

## Quick Start

### Windows Users

1. **Single Platform (Windows x64):**
   ```cmd
   publish-selfcontained.bat
   ```

2. **Multiple Platforms:**
   ```powershell
   .\publish-selfcontained.ps1 -MultiPlatform
   ```

3. **Specific Platform:**
   ```powershell
   .\publish-selfcontained.ps1 -Runtime linux-x64
   ```

### Linux/macOS Users

1. **Single Platform (Windows x64):**
   ```bash
   ./publish-selfcontained.sh
   ```

2. **Multiple Platforms:**
   ```bash
   ./publish-multiplatform.sh
   ```

## Manual Deployment Commands

### Windows x64 (Self-Contained)
```bash
dotnet publish --configuration Release --runtime win-x64 --self-contained true --output ./publish/win-x64
```

### Linux x64 (Self-Contained)
```bash
dotnet publish --configuration Release --runtime linux-x64 --self-contained true --output ./publish/linux-x64
```

### macOS x64 (Self-Contained)
```bash
dotnet publish --configuration Release --runtime osx-x64 --self-contained true --output ./publish/osx-x64
```

## Supported Runtime Identifiers

- **win-x64**: Windows 64-bit
- **win-x86**: Windows 32-bit
- **win-arm64**: Windows ARM 64-bit
- **linux-x64**: Linux 64-bit
- **linux-arm64**: Linux ARM 64-bit
- **osx-x64**: macOS 64-bit (Intel)
- **osx-arm64**: macOS ARM 64-bit (Apple Silicon)

## Project Configuration

The following properties in `Home.csproj` enable self-contained deployment:

```xml
<PropertyGroup>
  <!-- Enable self-contained deployment -->
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  
  <!-- Create a single executable file -->
  <PublishSingleFile>true</PublishSingleFile>
  
  <!-- Enable ReadyToRun for better performance -->
  <PublishReadyToRun>true</PublishReadyToRun>
  
  <!-- Disable trimming to avoid runtime issues -->
  <PublishTrimmed>false</PublishTrimmed>
</PropertyGroup>
```

## Deployment Output

After successful publishing, you'll find:

### Windows Deployment (`./publish/win-x64/`)
- `Home.exe` - Main executable (~251MB)
- `appsettings.json` - Application configuration
- `appsettings.Development.json` - Development configuration
- `web.config` - IIS configuration
- `wwwroot/` - Static files and assets

### Linux Deployment (`./publish/linux-x64/`)
- `Home` - Main executable (~249MB)
- `appsettings.json` - Application configuration
- `appsettings.Development.json` - Development configuration
- `wwwroot/` - Static files and assets

## Server Deployment

### Windows Server

1. Copy the entire `./publish/win-x64/` folder to your server
2. Update `appsettings.json` with production database connection strings
3. Run directly: `Home.exe`
4. Or configure as Windows Service or IIS application

### Linux Server

1. Copy the entire `./publish/linux-x64/` folder to your server
2. Make the executable file executable: `chmod +x Home`
3. Update `appsettings.json` with production configuration
4. Run directly: `./Home`
5. Or configure as systemd service

### macOS Server

1. Copy the entire `./publish/osx-x64/` folder to your server
2. Make the executable file executable: `chmod +x Home`
3. Update `appsettings.json` with production configuration
4. Run directly: `./Home`

## Performance Considerations

### Advantages
- **No Runtime Dependency**: Target server doesn't need .NET runtime installed
- **Better Performance**: ReadyToRun compilation improves startup time
- **Single File**: Easy to deploy and distribute
- **Version Isolation**: Each application includes its own runtime version

### Considerations
- **Larger File Size**: ~250MB per deployment (includes .NET runtime)
- **Platform Specific**: Each platform requires its own deployment package
- **Memory Usage**: May use slightly more memory due to bundled runtime

## Troubleshooting

### Common Issues

1. **File Size Too Large**
   - Consider using framework-dependent deployment for smaller size
   - Enable trimming with caution: `<PublishTrimmed>true</PublishTrimmed>`

2. **Runtime Errors**
   - Ensure correct Runtime Identifier for target platform
   - Disable trimming if encountering reflection-related issues

3. **Configuration Issues**
   - Update connection strings in `appsettings.json`
   - Set appropriate environment variables
   - Configure logging paths and permissions

### Logs and Debugging

- Application logs are written to the `Logs/` directory
- Use `--environment Production` argument to set production environment
- Check `appsettings.json` for database and service configurations

## Security Notes

- Update default connection strings before deployment
- Use secure passwords for database connections
- Configure HTTPS certificates for production use
- Review and update JWT settings in production

## Additional Resources

- [Microsoft .NET Deployment Guide](https://docs.microsoft.com/en-us/dotnet/core/deploying/)
- [Runtime Identifier Catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)
- [Self-Contained Deployment](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli#self-contained-deployment)