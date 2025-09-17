#!/bin/bash

# Real Estate Management System - Development Scripts
# This file contains useful commands for development

echo "=== Real Estate Management System - Development Helper ==="
echo ""

# Function to display available commands
show_help() {
    echo "Available commands:"
    echo "  build      - Build the project"
    echo "  run        - Run the project in development mode"
    echo "  test       - Run tests (when available)"
    echo "  migrate    - Run database migrations"
    echo "  clean      - Clean build artifacts"
    echo "  restore    - Restore NuGet packages"
    echo "  watch      - Run with file watching for development"
    echo "  swagger    - Open Swagger documentation"
    echo "  endpoints  - List all available API endpoints"
    echo ""
}

# Build the project
if [ "$1" = "build" ]; then
    echo "Building the project..."
    dotnet build
    exit 0
fi

# Run in development mode
if [ "$1" = "run" ]; then
    echo "Starting the application in development mode..."
    echo "API will be available at: http://localhost:5000"
    echo "Swagger UI will be available at: http://localhost:5000/swagger"
    echo ""
    dotnet run --environment Development
    exit 0
fi

# Run with file watching
if [ "$1" = "watch" ]; then
    echo "Starting the application with file watching..."
    echo "Application will restart automatically when files change"
    echo ""
    dotnet watch run
    exit 0
fi

# Clean build artifacts
if [ "$1" = "clean" ]; then
    echo "Cleaning build artifacts..."
    dotnet clean
    rm -rf bin obj
    exit 0
fi

# Restore packages
if [ "$1" = "restore" ]; then
    echo "Restoring NuGet packages..."
    dotnet restore
    exit 0
fi

# Database migrations
if [ "$1" = "migrate" ]; then
    echo "Running database migrations..."
    if command -v dotnet-ef &> /dev/null; then
        dotnet ef database update
    else
        echo "Entity Framework Core tools not found. Installing..."
        dotnet tool install --global dotnet-ef
        dotnet ef database update
    fi
    exit 0
fi

# Open Swagger
if [ "$1" = "swagger" ]; then
    echo "Opening Swagger documentation..."
    if command -v xdg-open &> /dev/null; then
        xdg-open http://localhost:5000/swagger
    elif command -v open &> /dev/null; then
        open http://localhost:5000/swagger
    else
        echo "Please open http://localhost:5000/swagger in your browser"
    fi
    exit 0
fi

# List API endpoints
if [ "$1" = "endpoints" ]; then
    echo "Available API Endpoints:"
    echo ""
    echo "=== Authentication ==="
    echo "POST   /Api/Account/Register        - User registration"
    echo "POST   /Api/Account/Login           - User login"
    echo "GET    /Api/Account/CheckLogin      - Check login status"
    echo "PUT    /Api/Account/ChangePassword  - Change password"
    echo "PUT    /Api/Account/UpdateProfile   - Update profile"
    echo ""
    echo "=== Agent Management ==="
    echo "POST   /Api/Agent/Register          - Agent registration"
    echo "GET    /Api/Agent/Profile           - Get agent profile"
    echo "POST   /Api/Agent/AddProperty       - Add new property"
    echo ""
    echo "=== Property Management ==="
    echo "GET    /Api/Property/GetPropertyImages/{id}  - Get property images"
    echo "POST   /Api/Property/AddPropertyImage         - Upload property image"
    echo "DELETE /Api/Property/DeletePropertyImage/{id} - Delete property image"
    echo ""
    echo "=== Location Services ==="
    echo "GET    /Api/Locations/Countries     - Get all countries"
    echo "GET    /Api/Locations/Cities        - Get cities"
    echo "GET    /Api/Locations/Districts     - Get districts"
    echo "GET    /Api/Locations/Nationalities - Get nationalities"
    echo ""
    echo "=== Reference Data ==="
    echo "GET    /Api/General/DocumentTypes       - Get document types"
    echo "GET    /Api/General/RealStateTypes      - Get real estate types"
    echo "GET    /Api/General/RentTypes           - Get rent types"
    echo "GET    /Api/General/PropertyPurposeTypes - Get property purposes"
    echo ""
    echo "Note: Most endpoints require authentication (API key or JWT token)"
    echo "API Key: Add header 'apikey: Home@@3040'"
    echo ""
    exit 0
fi

# Run tests (placeholder for when tests are added)
if [ "$1" = "test" ]; then
    echo "Running tests..."
    if [ -d "Tests" ] || [ -d "tests" ] || find . -name "*.Test.csproj" -o -name "*.Tests.csproj" | grep -q .; then
        dotnet test
    else
        echo "No tests found. Consider adding unit tests to improve code quality."
        echo "You can create a test project with:"
        echo "  dotnet new xunit -n Home.Tests"
        echo "  dotnet add Home.Tests/Home.Tests.csproj reference Home.csproj"
    fi
    exit 0
fi

# Default: show help
show_help