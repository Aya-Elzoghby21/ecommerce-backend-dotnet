E-Commerce Backend (.NET 8 – 3-Tier Architecture)

This is the Backend API for the E-Commerce web application built using ASP.NET Core 8 with a 3-Tier Architecture (Data Access Layer, Business Logic Layer, and API Layer).
The API handles user authentication (JWT) and product management with full CRUD operations.

🧠 Architecture Overview

The project follows a clean 3-tier architecture:

ECommerce.sln
┣ ECommerce.DAL/           → Data Access Layer (Entities + DbContext + Repository)
┃ ┣ Models/
┃ ┣ Repositories/
┃ ┗ ECommerceDbContext.cs
┣ ECommerce.BLL/           → Business Logic Layer (Services + Interfaces)
┃ ┣ Services/
┃ ┗ Interfaces/
┣ ECommerce.API/           → Presentation Layer (Controllers + Dependency Injection)
┃ ┣ Controllers/
┃ ┣ appsettings.json
┃ ┗ Program.cs

🚀 Main Features

🔐 JWT Authentication (Login / Register)

🧾 Product CRUD operations

🧱 Clean separation of concerns (DAL, BLL, API)

🧭 Dependency Injection for Services & Repositories

⚡ Entity Framework Core Code First

🧩 Validation using Data Annotations

📦 Swagger UI for testing APIs

🧰 Tech Stack
Category	Technology
Framework	.NET 8 Web API
ORM	Entity Framework Core
Database	SQL Server
Authentication	JWT (JSON Web Token)
Architecture	3-Tier (DAL, BLL, API)
Documentation	Swagger
⚙️ Setup Instructions
1️⃣ Clone the repository
git clone https://github.com/Aya-Elzoghby21/ecommerce-backend-dotnet.git
cd ecommerce-backend-dotnet

2️⃣ Update your connection string

Edit appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "YourSecretKeyHere",
  "Issuer": "https://localhost:7188",
  "Audience": "https://localhost:7188"
}

3️⃣ Apply Migrations & Run
cd ECommerce.API
dotnet ef database update
dotnet run


The API will start at
👉 https://localhost:7188/swagger

🧩 API Endpoints
🔐 Authentication
Method	Endpoint	Description
POST	/api/Auth/register	Register a new user
POST	/api/Auth/login	Login and get JWT token

Example Request:

{
  "username": "aya",
  "email": "aya@gmail.com",
  "password": "P@ssw0rd"
}


Example Response:

{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}

🛒 Products
Method	Endpoint	Description
GET	/api/Products	Get all products
GET	/api/Products/{id}	Get product by ID
POST	/api/Products	Add new product
PUT	/api/Products/{id}	Update existing product
DELETE	/api/Products/{id}	Delete product

Example Product:

{
  "category": "Electronics",
  "productCode": "P1001",
  "name": "Wireless Headphones",
  "imagePath": "images/headphones.jpg",
  "price": 499.99,
  "minimumQuantity": 1,
  "discountRate": 10
}

🧠 Business Logic Layer

Handles:

Validation & data transformation

Product pricing logic

Delegating data operations to DAL

Returning DTOs to API layer

🧩 Data Access Layer

Contains:

Entity Models

DbContext (ECommerceDbContext)

Repositories (e.g., GenericRepository, ProductRepository)

🧱 API Layer

Handles:

Routing & request handling

Model validation

Authentication & authorization

Dependency injection for services

🧪 Testing

You can test endpoints easily using:

Swagger UI → https://localhost:7188/swagger

Postman Collection (available in repo)

💾 Database Backup

A ready .bak file is included under:

DatabaseBackup/ECommerceDB.bak

👩‍💻 Author

Aya El-Zoghby
Full Stack Developer (.NET & Angular)
📧 ayaelzoghby651@gmail.com
