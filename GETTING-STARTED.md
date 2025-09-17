# Getting Started with Real Estate Management System

Welcome to the Real Estate Management System! This guide will help you get the project up and running quickly.

## 🎯 What This Project Does

This is a comprehensive **Real Estate Management Platform** that provides:

- **Property Management**: List, manage, and categorize real estate properties
- **Agent Operations**: Agent registration, property assignments, and management
- **Multi-language Support**: Full Arabic and English localization
- **Image Management**: Upload and process property images
- **Location Services**: Manage countries, cities, and districts
- **Authentication**: Secure JWT and API key authentication
- **RESTful APIs**: Complete set of APIs for all operations

## 🚀 Quick Start (5 Minutes)

### Step 1: Prerequisites
- .NET 8.0 SDK installed
- Git installed
- A code editor (VS Code, Visual Studio, etc.)

### Step 2: Clone and Setup
```bash
# Clone the repository
git clone https://github.com/progmaher/realstate.git
cd realstate

# Make development script executable
chmod +x dev-scripts.sh

# Restore packages and build
./dev-scripts.sh restore
./dev-scripts.sh build
```

### Step 3: Run the Application
```bash
# Start the application
./dev-scripts.sh run

# Or run with file watching for development
./dev-scripts.sh watch
```

### Step 4: Test the APIs
```bash
# In a new terminal, test an API endpoint
curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/DocumentTypes

# Or open Swagger UI in your browser
# http://localhost:5000/swagger
```

That's it! Your real estate management system is now running.

## 📋 What You Can Do Now

### 1. **Explore the APIs**
- **Swagger UI**: http://localhost:5000/swagger
- **API Testing Guide**: See `API-TESTING.md` for examples
- **Available Endpoints**: Run `./dev-scripts.sh endpoints`

### 2. **Test Core Features**
```bash
# Get all document types
curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/DocumentTypes

# Get real estate types
curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/RealStateTypes

# Test with Arabic language
curl -H "apikey: Home@@3040" -H "Accept-Language: ar" http://localhost:5000/Api/General/DocumentTypes
```

### 3. **Register Users and Agents**
```bash
# Register a new user
curl -X POST -H "Content-Type: application/json" -H "apikey: Home@@3040" \
  -d '{"email":"user@test.com","password":"Password123!","confirmPassword":"Password123!"}' \
  http://localhost:5000/Api/Account/Register

# Register a new agent
curl -X POST -H "Content-Type: application/json" -H "apikey: Home@@3040" \
  -d '{"email":"agent@test.com","password":"Password123!","nameAr":"وكيل","nameEn":"Agent","cr":"123","fal":"FAL123","falExpiryDate":"2025-12-31","mobileNumber":"+966501234567","countryId":1,"cityId":1}' \
  http://localhost:5000/Api/Agent/Register
```

## 🗄️ Database Setup

### Option 1: SQL Server LocalDB (Recommended for Windows)
The project is configured to use LocalDB by default. Just run:
```bash
./dev-scripts.sh migrate
```

### Option 2: SQL Server Express/Full
1. Install SQL Server Express
2. Update `appsettings.json` connection string
3. Run migrations: `./dev-scripts.sh migrate`

### Option 3: In-Memory Database (for testing)
Modify `Program.cs` to use in-memory database:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("RealEstateDB"));
```

## 🛠️ Development Workflow

### Daily Development
```bash
# Start development with file watching
./dev-scripts.sh watch

# In another terminal, test your changes
curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/DocumentTypes
```

### Adding New Features
1. **Add API Endpoint**: Create new endpoint in `Api/` folder
2. **Add Data Model**: Create model in `Data/` folder
3. **Update DbContext**: Add DbSet to `ApplicationDbContext`
4. **Create Migration**: `dotnet ef migrations add YourFeatureName`
5. **Update Database**: `./dev-scripts.sh migrate`

### Testing Your Changes
```bash
# Build and test
./dev-scripts.sh build
./dev-scripts.sh run

# Or use the development script with watching
./dev-scripts.sh watch
```

## 🎯 Common Use Cases

### 1. Property Management Company
- Register agents through `/Api/Agent/Register`
- Add properties through `/Api/Agent/AddProperty`
- Upload property images through `/Api/Property/AddPropertyImage`
- Manage location data through `/Api/Locations/*`

### 2. Real Estate Portal
- Use location APIs to build search filters
- Integrate property APIs for listings
- Use image APIs for property galleries
- Implement user registration for buyers/renters

### 3. Agent Management System
- Register and manage real estate agents
- Track agent properties and performance
- Manage agent documentation and compliance
- Location-based agent assignments

## 📖 Key Files and Folders

```
realstate/
├── Api/                    # API endpoints organized by feature
│   ├── Accounts/          # User authentication
│   ├── Agent/             # Agent management
│   ├── General/           # Reference data
│   ├── Locations/         # Geographic data
│   └── Property/          # Property management
├── Data/                  # Database models and context
├── Services/              # Background services
├── Pages/                 # Razor pages (if needed)
├── appsettings.json       # Configuration
├── Program.cs             # Application startup
├── README.md              # This file
├── API-TESTING.md         # API testing examples
└── dev-scripts.sh         # Development helper script
```

## 🔧 Configuration

### API Key
Default API key: `Home@@3040` (change in production!)

### JWT Settings
Configured in `appsettings.json`:
- Token expiry: 60 minutes
- Issuer: "Home"
- Audience: "Users"

### Localization
Supported languages: Arabic (ar), English (en)
Use `Accept-Language` header to specify language preference.

## 🎓 Learning Resources

### Understanding the Codebase
1. **Start with**: `Program.cs` - Application configuration
2. **API Structure**: Explore `Api/` folder organization
3. **Data Models**: Check `Data/` folder for entity definitions
4. **Authentication**: Review `ApiKeyAuthHandler.cs` and JWT setup

### FastEndpoints Framework
This project uses FastEndpoints instead of traditional controllers:
- **Documentation**: https://fast-endpoints.com/
- **Benefits**: Better performance, cleaner code, built-in validation
- **Examples**: Check any file in `Api/` folder

## 🚀 Next Steps

### Immediate Enhancements
- [ ] Add unit tests
- [ ] Implement property search functionality
- [ ] Add property listing APIs
- [ ] Create admin dashboard
- [ ] Add email notifications

### Advanced Features
- [ ] Property comparison
- [ ] Advanced search filters
- [ ] Real-time notifications
- [ ] Property analytics
- [ ] Mobile app APIs

### Production Deployment
- [ ] Configure production database
- [ ] Set up SSL certificates
- [ ] Configure logging and monitoring
- [ ] Set up CI/CD pipeline

## 🤝 Getting Help

### If Something Doesn't Work
1. **Check application logs** in the console
2. **Verify database connection** in `appsettings.json`
3. **Test basic API** with `curl` or Swagger UI
4. **Review error messages** for specific guidance

### Common Issues
- **Port conflicts**: Change port in `Properties/launchSettings.json`
- **Database issues**: Check connection string and SQL Server installation
- **Authentication errors**: Verify API key: `Home@@3040`

### Resources
- **Swagger UI**: http://localhost:5000/swagger
- **API Testing Guide**: `API-TESTING.md`
- **Development Commands**: Run `./dev-scripts.sh` for help

## 🎉 Success Indicators

You'll know everything is working when:
- ✅ Application starts without errors
- ✅ Swagger UI loads at http://localhost:5000/swagger
- ✅ API test returns data: `curl -H "apikey: Home@@3040" http://localhost:5000/Api/General/DocumentTypes`
- ✅ You can register users and agents
- ✅ Multi-language support works with `Accept-Language` header

---

**Happy Coding! 🏠✨**

This real estate management system is ready for customization and extension based on your specific business needs.