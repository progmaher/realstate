# Real Estate Management System

A comprehensive real estate management platform built with ASP.NET Core 8.0, featuring bilingual support (Arabic/English), property management, and agent operations.

## 🏗️ Architecture Overview

### Technology Stack
- **Backend**: ASP.NET Core 8.0 with FastEndpoints
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: JWT Bearer + API Key authentication
- **API Documentation**: Swagger/OpenAPI
- **Image Processing**: SixLabors.ImageSharp
- **Localization**: Arabic/English bilingual support

### Core Components
- **Property Management System**: Complete property lifecycle management
- **Agent Management**: Agent registration, profiles, and property assignments
- **Location Services**: Geographic hierarchy (Countries → Cities → Districts)
- **User Authentication**: Secure JWT-based authentication with role management
- **Image Processing**: Background image processing for property photos
- **Multi-language Support**: Full Arabic/English localization

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/progmaher/realstate.git
   cd realstate
   ```

2. **Update connection string**
   Edit `appsettings.json` and update the database connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your_SQL_Server_Connection_String"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the application**
   - API: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

## 📋 Available Features

### 🏠 Property Management
- **Property CRUD Operations**: Complete property lifecycle management
- **Image Management**: Upload, process, and manage property images
- **Property Types**: Categorization by purpose (sale/rent) and property types
- **Multi-language Descriptions**: Arabic and English property descriptions
- **Location Assignment**: Link properties to specific districts and cities

### 👥 User & Agent Management
- **User Registration/Login**: Secure authentication system
- **Agent Registration**: Specialized agent registration with business details
- **Profile Management**: Update user profiles and change passwords
- **Role-based Access**: Different permissions for users vs agents

### 📍 Location Services
- **Geographic Hierarchy**: Countries → Cities → Districts
- **Nationality Management**: Track user nationalities
- **Location-based Search**: Filter properties by location

### 🔧 Administrative Features
- **Document Types**: Manage various document categories
- **Real Estate Types**: Configure property and rent types
- **Background Processing**: Automated image processing
- **API Key Management**: Secure API access

## 🔌 API Endpoints

### Authentication Endpoints
```
POST /Api/Account/Register - User registration
POST /Api/Account/Login - User authentication
GET /Api/Account/CheckLogin - Verify authentication status
PUT /Api/Account/ChangePassword - Update user password
PUT /Api/Account/UpdateProfile - Update user profile
```

### Agent Endpoints
```
POST /Api/Agent/Register - Agent registration
GET /Api/Agent/Profile - Get agent profile
POST /Api/Agent/AddProperty - Add new property
```

### Property Endpoints
```
GET /Api/Property/GetPropertyImages/{propertyId} - Get property images
POST /Api/Property/AddPropertyImage - Upload property image
DELETE /Api/Property/DeletePropertyImage/{imageId} - Delete property image
```

### Location Endpoints
```
GET /Api/Locations/Countries - Get all countries
GET /Api/Locations/Cities - Get cities by country
GET /Api/Locations/Districts - Get districts by city
GET /Api/Locations/Nationalities - Get all nationalities
```

### General Data Endpoints
```
GET /Api/General/DocumentTypes - Get document types
GET /Api/General/RealStateTypes - Get real estate types
GET /Api/General/RentTypes - Get rent types
GET /Api/General/PropertyPurposeTypes - Get property purposes
```

## 🔒 Authentication

The system supports dual authentication:

1. **JWT Bearer Authentication** for user/agent sessions
2. **API Key Authentication** for programmatic access

### API Key Usage
Include the API key in request headers:
```
apikey: Your_API_Key_Here
```

### JWT Token Usage
Include the JWT token in request headers:
```
Authorization: Bearer Your_JWT_Token_Here
```

## 🌐 Localization

The system supports bilingual content:

- **Languages**: Arabic (ar) and English (en)
- **Dynamic Content**: Property descriptions, names, and system messages
- **API Language Support**: Pass language preference via `Accept-Language` header

## 📊 Database Schema

### Core Entities
- **Properties**: Main property records with details and location
- **PropertyImages**: Property photo management
- **Agents**: Real estate agent information
- **Users**: System user accounts
- **Countries/Cities/Districts**: Geographic hierarchy
- **DocumentTypes**: Various document categories

### Key Relationships
- Properties belong to Agents
- Properties have multiple PropertyImages
- Properties are located in Districts
- Districts belong to Cities and Countries
- Agents have multiple Properties

## 🛠️ Development & Deployment

### Development Environment
```bash
# Run in development mode
dotnet run --environment Development

# Watch for changes
dotnet watch run
```

### Production Deployment
- Configure production connection strings
- Set up SSL certificates
- Configure logging and monitoring
- Set up image storage (file system or cloud)

### Docker Support
The project can be containerized for easier deployment:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# ... (see DeploymentGuide.md for complete Dockerfile)
```

## 🧪 Testing

### API Testing
Use the included Swagger UI or tools like Postman:
1. Navigate to `/swagger` for interactive API documentation
2. Test endpoints with proper authentication
3. Validate data models and responses

### Sample API Calls
```bash
# Get document types
curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/DocumentTypes

# Register new user
curl -X POST -H "Content-Type: application/json" \
     -H "apikey: Home@@3040" \
     -d '{"email":"user@example.com","password":"Password123!"}' \
     http://localhost:5000/Api/Account/Register
```

## 📚 Next Steps & Enhancements

### Immediate Improvements
- [ ] Add comprehensive unit tests
- [ ] Implement API rate limiting
- [ ] Add request/response logging
- [ ] Enhance error handling and validation

### Feature Enhancements
- [ ] Advanced property search and filtering
- [ ] Property comparison functionality
- [ ] Email notifications system
- [ ] Mobile app API optimization
- [ ] Real-time notifications
- [ ] Property analytics and reporting

### Infrastructure Improvements
- [ ] Redis caching implementation
- [ ] CDN integration for images
- [ ] Background job processing with Hangfire
- [ ] Health check endpoints
- [ ] Performance monitoring

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -am 'Add new feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Submit a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For questions and support:
- Create an issue in the GitHub repository
- Contact the development team
- Check the documentation and API reference

---

**Built with ❤️ for the Real Estate Industry**