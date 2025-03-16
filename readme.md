# Cinema Booking API
- 
- .NET 8 SDK
- SQLite

## Getting Started

### 1. Clone the Repository
```sh
git clone https://github.com/alixizadi/cinema-booking-api.git
cd cinema-booking-api
```

### 2. Install Dependencies
```sh
dotnet restore
```

### 3. Apply Migrations
```sh
dotnet ef database update
```

### 4. Run the Application
```sh
dotnet run
```


## Authentication
Before using any endpoints, you need to create a user and authenticate.

### 1. Register a User
```
POST /api/auth/register
```
**Set IsAdmin to true** in registeration to access admin endpoints

### 2. Login to Get a Token
```
POST /api/auth/login
```


### 3. Use Token for Authorization
Copy the token and use it in Swagger's **Authorize** section or include it in your requests as:
```
Authorization: Bearer your-jwt-token
```


## Notes
- Only **admin users** can access admin endpoints. Set `IsAdmin : true` during registration.
- The database is **not included** in this repository. Run migrations to set up a fresh SQLite database.
- Uploaded images are stored in the `/uploads` folder, which is publicly accessible.

