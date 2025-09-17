# Real Estate API Testing Examples

This file contains examples of how to test the Real Estate Management System APIs.

## Prerequisites
- Application running locally (use `./dev-scripts.sh run`)
- API accessible at http://localhost:5000
- API Key: `Home@@3040`

## 1. General Data Endpoints (No JWT required, only API key)

### Get Document Types
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/General/DocumentTypes
```

### Get Real Estate Types
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/General/RealStateTypes
```

### Get Rent Types
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/General/RentTypes
```

### Get Property Purpose Types
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/General/PropertyPurposeTypes
```

## 2. Location Services

### Get Countries
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/Locations/Countries
```

### Get Cities
```bash
curl -H "apikey: Home@@3040" \
     -H "Accept-Language: en" \
     http://localhost:5000/Api/Locations/Cities
```

### Get Districts
```bash
curl -H "apikey: Home@@3040" \
     -H "Accept-Language: ar" \
     http://localhost:5000/Api/Locations/Districts
```

### Get Nationalities
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/Locations/Nationalities
```

## 3. User Authentication

### Register New User
```bash
curl -X POST \
     -H "Content-Type: application/json" \
     -H "apikey: Home@@3040" \
     -d '{
       "email": "test@example.com",
       "password": "Password123!",
       "confirmPassword": "Password123!"
     }' \
     http://localhost:5000/Api/Account/Register
```

### User Login
```bash
curl -X POST \
     -H "Content-Type: application/json" \
     -H "apikey: Home@@3040" \
     -d '{
       "email": "test@example.com",
       "password": "Password123!"
     }' \
     http://localhost:5000/Api/Account/Login
```

### Check Login Status (requires JWT token)
```bash
# Replace YOUR_JWT_TOKEN with actual token from login response
curl -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     http://localhost:5000/Api/Account/CheckLogin
```

## 4. Agent Registration

### Register as Agent
```bash
curl -X POST \
     -H "Content-Type: application/json" \
     -H "apikey: Home@@3040" \
     -d '{
       "email": "agent@example.com",
       "password": "Password123!",
       "nameAr": "وكيل عقاري",
       "nameEn": "Real Estate Agent",
       "cr": "1234567890",
       "fal": "FAL123456",
       "falExpiryDate": "2025-12-31T00:00:00",
       "mobileNumber": "+966501234567",
       "countryId": 1,
       "cityId": 1
     }' \
     http://localhost:5000/Api/Agent/Register
```

## 5. Property Management (requires JWT token)

### Add Property Image
```bash
# Replace YOUR_JWT_TOKEN with actual token
curl -X POST \
     -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     -F "PropertyId=1" \
     -F "Image=@/path/to/image.jpg" \
     http://localhost:5000/Api/Property/AddPropertyImage
```

### Get Property Images
```bash
curl -H "apikey: Home@@3040" \
     http://localhost:5000/Api/Property/GetPropertyImages/1
```

### Delete Property Image
```bash
# Replace YOUR_JWT_TOKEN with actual token
curl -X DELETE \
     -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     http://localhost:5000/Api/Property/DeletePropertyImage/1
```

## 6. Language Support Examples

### Arabic Content
```bash
curl -H "apikey: Home@@3040" \
     -H "Accept-Language: ar" \
     http://localhost:5000/Api/General/DocumentTypes
```

### English Content
```bash
curl -H "apikey: Home@@3040" \
     -H "Accept-Language: en" \
     http://localhost:5000/Api/General/DocumentTypes
```

## 7. Error Handling Examples

### Invalid API Key
```bash
curl -H "apikey: invalid_key" \
     http://localhost:5000/Api/General/DocumentTypes
# Expected: 401 Unauthorized
```

### Missing Authentication
```bash
curl http://localhost:5000/Api/Account/CheckLogin
# Expected: 401 Unauthorized
```

### Invalid Data
```bash
curl -X POST \
     -H "Content-Type: application/json" \
     -H "apikey: Home@@3040" \
     -d '{
       "email": "invalid_email",
       "password": "123"
     }' \
     http://localhost:5000/Api/Account/Register
# Expected: 400 Bad Request with validation errors
```

## 8. Using Postman

Import these endpoints into Postman:

1. **Environment Variables**:
   - `base_url`: http://localhost:5000
   - `api_key`: Home@@3040
   - `jwt_token`: (set after login)

2. **Headers for all requests**:
   - `apikey`: {{api_key}}

3. **Headers for authenticated requests**:
   - `Authorization`: Bearer {{jwt_token}}

## 9. Swagger UI

Access the interactive API documentation at:
http://localhost:5000/swagger

The Swagger UI provides:
- Complete API documentation
- Interactive testing interface
- Request/response examples
- Schema definitions

## 10. Common Response Formats

### Success Response
```json
{
  "status": true,
  "message": "Operation successful",
  "data": { ... }
}
```

### Error Response
```json
{
  "status": false,
  "message": "Error description",
  "errors": [ ... ]
}
```

### List Response
```json
[
  {
    "id": 1,
    "name": "Item Name"
  },
  ...
]
```

## Notes

- Replace placeholder values (YOUR_JWT_TOKEN, file paths, etc.) with actual values
- Ensure the application is running before testing
- Check application logs for detailed error information
- Use appropriate Content-Type headers for different request types
- JWT tokens expire after 60 minutes (configurable in appsettings)