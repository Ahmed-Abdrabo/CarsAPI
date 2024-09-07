
# Cars API

## Description
The Cars API is a robust and scalable ASP.NET Core 8 project designed for efficient car data management. It provides a comprehensive set of CRUD operations, allowing users to create, read, update, and delete car information seamlessly. The API is secured using JWT authentication, ensuring that sensitive data is protected from unauthorized access.

Built with a structured N-tier architecture, the project promotes a clean separation of concerns, making it easier to maintain and extend. The use of Entity Framework Core simplifies database interactions, providing a high level of abstraction while maintaining performance. This API is ideal for integrating into car dealership systems, rental services, or any application requiring reliable car data management.
## Features
- **CRUD Operations**: Perform Create, Read, Update, and Delete operations on car data.
- **JWT Authentication**: Secure the API with JSON Web Tokens (JWT) to protect sensitive information.
- **Entity Framework Core**: Handle database interactions using a modern ORM, allowing for efficient data management.
- **N-tier Architecture**: Structured architecture that promotes separation of concerns and scalability.

## Technologies Used
- **.NET 8**
- **ASP.NET Core**
- **Entity Framework Core**
- **JWT Authentication**
- **C#**
- **SQL Server**

## Installation
To run this project locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Ahmed-Abdrabo/CarsAPI.git
   ```

2. **Navigate to the project directory:**
   ```bash
   cd CarsAPI
   ```

3. **Restore the packages:**
   ```bash
   dotnet restore
   ```

4. **Update the database:**
   Ensure you have SQL Server installed and update the connection string in `appsettings.json`.
   Then, run:
   ```bash
   dotnet ef database update
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

## Usage
Once the application is running, you can use tools like Postman or Swagger to interact with the API.

### API Endpoints

#### CarAPI
- **GET** `/api/v2/CarAPI` - Retrieve a list of all cars.
- **POST** `/api/v2/CarAPI` - Add a new car.
- **GET** `/api/v2/CarAPI/{id}` - Retrieve a specific car by its ID.
- **PUT** `/api/v2/CarAPI/{id}` - Update an existing car's information.
- **DELETE** `/api/v2/CarAPI/{id}` - Delete a car by its ID.
![image](https://github.com/user-attachments/assets/9a67f216-2cf6-410d-aedd-8d390a621dc8)

#### CarDetailsAPI
- **GET** `/api/v2/CarDetailsAPI` - Retrieve a list of all car details.
- **POST** `/api/v2/CarDetailsAPI` - Add new car details.
- **GET** `/api/v2/CarDetailsAPI/{id}` - Retrieve specific car details by ID.
- **PUT** `/api/v2/CarDetailsAPI/{id}` - Update existing car details by ID.
- **DELETE** `/api/v2/CarDetailsAPI/{id}` - Delete car details by ID.
![image](https://github.com/user-attachments/assets/c3761d39-9dd8-47d3-afb0-555f034211ea)

#### UsersAuth
- **GET** `/api/v2/UsersAuth/Error` - Retrieve error information.
- **GET** `/api/v2/UsersAuth/ImageError` - Retrieve image error information.
- **POST** `/api/v2/UsersAuth/login` - Authenticate and log in a user.
- **POST** `/api/v2/UsersAuth/register` - Register a new user.
- **POST** `/api/v2/UsersAuth/refresh` - Refresh the JWT token.
- **POST** `/api/v2/UsersAuth/revoke` - Revoke a JWT token.
![image](https://github.com/user-attachments/assets/b1ad6610-81d1-412f-8ec4-7ed57575f8af)

### JWT Authentication
To access secure endpoints, you must include a valid JWT token in the request header:
```http
Authorization: Bearer <token>
```

