#!/bin/bash

echo "======================================================"
echo "Real Estate Application - Multi-Platform Self-Contained Deployment"
echo "======================================================"

# Function to publish for a specific runtime
publish_for_runtime() {
    local runtime=$1
    local output_dir="./publish/$runtime"
    
    echo "Publishing for $runtime..."
    echo "Output directory: $output_dir"
    
    # Clean previous build for this runtime
    rm -rf "$output_dir"
    mkdir -p "$output_dir"
    
    # Publish with self-contained deployment
    dotnet publish --configuration Release \
        --runtime "$runtime" \
        --self-contained true \
        --output "$output_dir" \
        --verbosity normal
    
    if [ $? -eq 0 ]; then
        echo "✅ Successfully published for $runtime"
        echo "📁 Output: $output_dir"
        echo "📊 Size: $(du -sh "$output_dir" | cut -f1)"
        echo ""
    else
        echo "❌ Failed to publish for $runtime"
        echo ""
        return 1
    fi
}

# Create publish directory
mkdir -p publish

# Clean all previous builds
echo "Cleaning previous builds..."
dotnet clean --configuration Release

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Build the application
echo "Building application..."
dotnet build --configuration Release --no-restore

if [ $? -ne 0 ]; then
    echo "❌ Build failed. Exiting."
    exit 1
fi

echo ""
echo "Starting multi-platform publishing..."
echo ""

# Publish for Windows x64
publish_for_runtime "win-x64"

# Publish for Linux x64
publish_for_runtime "linux-x64"

# Publish for macOS x64
publish_for_runtime "osx-x64"

echo "======================================================"
echo "✅ Multi-platform publishing completed!"
echo "======================================================"
echo "Available deployments:"
ls -la publish/*/
echo ""
echo "Each deployment is self-contained and includes all required runtime dependencies."
echo "Copy the appropriate folder to your target server and run the executable."
echo ""