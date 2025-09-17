# Real Estate API Documentation

## Base URL
`https://your-domain.com/Api`

## Authentication
The API supports two authentication methods:
1. **API Key Authentication**: Add header `X-Api-Key: Home@@3040`
2. **JWT Bearer Authentication**: Add header `Authorization: Bearer {token}`

## Language Support
- **English (default)**: Add header `Accept-Language: en`
- **Arabic**: Add header `Accept-Language: ar`

---

## 🏠 Property Management

### Search Properties
**GET** `/Property/Search`

Search and filter properties with comprehensive criteria.

**Query Parameters:**
```
SearchText: string          # Search in title and description
MinPrice: decimal           # Minimum price filter
MaxPrice: decimal           # Maximum price filter
CountryId: int             # Filter by country
CityId: int                # Filter by city
DistrictId: int            # Filter by district
RealStateTypeId: int       # Filter by property type
RealStatePurposeId: int    # Filter by purpose (sale/rent)
MinBedrooms: int           # Minimum bedrooms
MaxBedrooms: int           # Maximum bedrooms
MinArea: decimal           # Minimum area in sqm
MaxArea: decimal           # Maximum area in sqm
HasParking: bool           # Filter by parking availability
HasElevator: bool          # Filter by elevator availability
IsFeatured: bool           # Filter featured properties
SortBy: string             # price_asc, price_desc, area_asc, area_desc, date_asc, date_desc
PageNumber: int            # Page number (default: 1)
PageSize: int              # Items per page (default: 20, max: 100)
```

**Example Request:**
```bash
GET /Api/Property/Search?MinPrice=100000&MaxPrice=500000&Bedrooms=3&SortBy=price_asc&PageSize=10
```

**Response:**
```json
[
  {
    "id": 1,
    "title": "Luxury Villa in Downtown",
    "description": "Beautiful 3-bedroom villa...",
    "price": 350000.00,
    "isNegotiable": true,
    "area": 250.5,
    "bedrooms": 3,
    "bathrooms": 2,
    "floorNumber": null,
    "hasParking": true,
    "hasElevator": false,
    "isFeatured": true,
    "countryId": 1,
    "cityId": 1,
    "districtId": 1,
    "realStateTypeId": 1,
    "realStatePurposeId": 1,
    "contactPhone": "+1234567890",
    "contactEmail": "agent@example.com",
    "insertedDate": "2024-01-15T10:30:00Z",
    "mainImageUrl": "/uploads/properties/1/main.jpg",
    "imageCount": 5
  }
]
```

### Get Property Details
**GET** `/Property/Details/{id}`

Get complete property information including location names, agent details, and all images.

**Example Request:**
```bash
GET /Api/Property/Details/1
```

**Response:**
```json
{
  "id": 1,
  "title": "Luxury Villa in Downtown",
  "description": "Beautiful 3-bedroom villa with modern amenities...",
  "price": 350000.00,
  "isNegotiable": true,
  "area": 250.5,
  "bedrooms": 3,
  "bathrooms": 2,
  "livingRooms": 1,
  "kitchens": 1,
  "floorNumber": null,
  "totalFloors": null,
  "apartmentNumber": null,
  "hasElevator": false,
  "hasParking": true,
  "parkingSpaces": 2,
  "buildYear": 2020,
  "address": "123 Main Street",
  "addressDescription": "Near city center",
  "locationDescription": "Prime location with easy access to amenities",
  "contactPhone": "+1234567890",
  "contactEmail": "agent@example.com",
  "videoUrl": "https://youtube.com/watch?v=...",
  "threeDTour": "https://virtual-tour.com/...",
  "amenities": "Swimming pool, Gym, Garden",
  "isFeatured": true,
  "isActive": true,
  "isAvailable": true,
  "insertedDate": "2024-01-15T10:30:00Z",
  "country": { "id": 1, "name": "United States" },
  "city": { "id": 1, "name": "New York" },
  "district": { "id": 1, "name": "Manhattan" },
  "realStateType": { "id": 1, "name": "Villa" },
  "realStatePurpose": { "id": 1, "name": "For Sale" },
  "rentType": null,
  "agent": {
    "id": 1,
    "name": "John Smith Real Estate",
    "phone": "+1234567890",
    "email": "john@realestate.com",
    "cr": "CR123456",
    "fal": "FAL789"
  },
  "images": [
    {
      "id": 1,
      "imagePath": "/uploads/properties/1/main.jpg",
      "isMain": true,
      "displayOrder": 1
    },
    {
      "id": 2,
      "imagePath": "/uploads/properties/1/bedroom.jpg",
      "isMain": false,
      "displayOrder": 2
    }
  ]
}
```

### Add Property
**POST** `/Agent/Property/Add`

Add a new property listing (requires JWT authentication).

**Request Body:**
```json
{
  "title": "Luxury Villa",
  "titleAr": "فيلا فاخرة",
  "description": "Beautiful villa with modern amenities",
  "descriptionAr": "فيلا جميلة مع وسائل الراحة الحديثة",
  "price": 350000.00,
  "isNegotiable": true,
  "area": 250.5,
  "bedrooms": 3,
  "bathrooms": 2,
  "livingRooms": 1,
  "kitchens": 1,
  "hasParking": true,
  "parkingSpaces": 2,
  "hasElevator": false,
  "buildYear": 2020,
  "address": "123 Main Street",
  "countryId": 1,
  "cityId": 1,
  "districtId": 1,
  "realStateTypeId": 1,
  "realStatePurposeId": 1,
  "contactPhone": "+1234567890",
  "contactEmail": "contact@example.com"
}
```

---

## 🏢 Agent Management

### Register Agent
**POST** `/Agent/Register`

Register a new real estate agent.

### Get Agent Profile
**GET** `/Agent/Profile`

Get agent profile information (requires JWT authentication).

---

## 👤 Account Management

### User Registration
**POST** `/Account/Register`

Register a new user account.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

### User Login
**POST** `/Account/Login`

Authenticate user and receive JWT token.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "status": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiration": "2024-01-16T10:30:00Z",
    "user": {
      "id": "user-id",
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe"
    }
  }
}
```

### Check Login Status
**GET** `/Account/CheckLogin`

Verify if user is logged in (requires JWT authentication).

### Update Profile
**PUT** `/Account/UpdateProfile`

Update user profile information (requires JWT authentication).

### Change Password
**POST** `/Account/ChangePassword`

Change user password (requires JWT authentication).

---

## 📍 Location Management

### Get Countries
**GET** `/Locations/Countries`

Get list of all countries.

**Response:**
```json
[
  { "id": 1, "name": "United States" },
  { "id": 2, "name": "Canada" }
]
```

### Get Cities by Country
**GET** `/Locations/Cities?id={countryId}`

Get cities for a specific country.

### Get Districts by City
**GET** `/Locations/Districts?id={cityId}`

Get districts for a specific city.

### Get Nationalities
**GET** `/Locations/Nationalities`

Get list of all nationalities.

---

## 🏷️ Classification & Configuration

### Get Document Types
**GET** `/General/DocumentTypes`

Get list of document types.

### Get Real Estate Types
**GET** `/General/RealStateTypes`

Get property types (Villa, Apartment, Office, etc.).

### Get Property Purposes
**GET** `/General/PurposeTypes`

Get property purposes (For Sale, For Rent, etc.).

### Get Rent Types
**GET** `/General/RentTypes`

Get rental period types (Monthly, Yearly, etc.).

---

## 🖼️ Image Management

### Add Property Image
**POST** `/Property/AddPropertyImage`

Upload image for a property.

### Get Property Images
**GET** `/Property/GetPropertyImages/{propertyId}`

Get all images for a property.

### Delete Property Image
**DELETE** `/Property/DeletePropertyImage/{imageId}`

Delete a specific property image.

---

## Error Handling

All endpoints return standardized error responses:

```json
{
  "status": false,
  "message": "Error description",
  "errors": [
    {
      "field": "Email",
      "message": "Email is required"
    }
  ]
}
```

## HTTP Status Codes

- **200 OK**: Request successful
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required
- **403 Forbidden**: Access denied
- **404 Not Found**: Resource not found
- **422 Unprocessable Entity**: Validation failed
- **500 Internal Server Error**: Server error

## Rate Limiting

API requests are limited to:
- **Public endpoints**: 100 requests per minute
- **Authenticated endpoints**: 1000 requests per minute

## Pagination

For endpoints that return lists, use pagination parameters:
- `PageNumber`: Page number (starts from 1)
- `PageSize`: Items per page (max 100)

Response includes pagination metadata:
```json
{
  "data": [...],
  "pagination": {
    "currentPage": 1,
    "totalPages": 5,
    "totalItems": 100,
    "pageSize": 20,
    "hasNext": true,
    "hasPrevious": false
  }
}
```