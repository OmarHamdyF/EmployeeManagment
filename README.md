# Employee Management System API

This is a backend API for an Employee Management System, built with .NET 6 and following Clean Architecture principles. It provides functionalities for managing employees, departments, and roles, with robust authentication using JWT.

## Architecture Overview

This project adheres to the principles of **Clean Architecture**, dividing the application into distinct, testable, and maintainable layers.

### 1. `EmployeeManagement.Domain`
* **Core Business Logic:** Contains the core business entities (e.g., `Employee`, `Department`, `Role`).
* **Abstractions:** Defines interfaces and base classes that represent the core domain concepts and business rules.
* **Independent:** Has no dependencies on other layers.

### 2. `EmployeeManagement.Application`
* **Application-Specific Business Rules:** Orchestrates the domain entities to perform application-specific tasks.
* **Use Cases/CQRS:** Implements MediatR for a Command Query Responsibility Segregation (CQRS) pattern, with `IRequest`, `IRequestHandler`, `ICommand`, `IQuery` to handle distinct operations.
* **DTOs:** Defines Data Transfer Objects (DTOs) for input (Commands/Queries) and output (Responses).
* **Interfaces:** Contains interfaces for services and repositories defined in the `Infrastructure` layer.
* **Validation:** Utilizes FluentValidation for defining and applying business rule validations.
* **Dependencies:** Depends on `Domain`.

### 3. `EmployeeManagement.Infrastructure`
* **External Concerns:** Implements the interfaces defined in the `Application` layer.
* **Data Access:** Uses Entity Framework Core (EF Core) as the Object-Relational Mapper (ORM) to interact with the database (SQL Server in this case).
* **Unit of Work:** Implements the Unit of Work pattern to ensure atomic transactions across multiple repository operations.
* **Persistence:** Contains `ApplicationDbContext` and EF Core Migrations.
* **Dependencies:** Depends on `Application` and `Domain`.

### 4. `EmployeeManagment.API`
* **Presentation Layer:** The entry point of the application, exposing RESTful API endpoints.
* **Controllers:** Handles HTTP requests and dispatches them to MediatR handlers in the `Application` layer.
* **Configuration:** Configures Dependency Injection (DI) for all layers, JWT authentication, and other cross-cutting concerns (e.g., CORS, Swagger).
* **Middleware:** Sets up HTTP request processing pipeline, including authentication and authorization.
* **Dependencies:** Depends on `Application` (and implicitly `Infrastructure` via DI).

## Key Decisions

* **Clean Architecture:** Chosen for clear separation of concerns, testability, and maintainability, especially for growing applications.
* **MediatR for CQRS:** Facilitates clear segregation of commands (mutations) and queries (reads), leading to more organized and focused handlers. Reduces direct dependencies between components.
* **Entity Framework Core with Fluent API:** Provides a powerful ORM for database interactions. Fluent API is preferred over Data Annotations to keep domain entities clean and to centralize mapping configurations.
* **Unit of Work Pattern:** Ensures data consistency by wrapping multiple repository operations within a single transaction, committed or rolled back together.
* **JWT Bearer Authentication:** A standard, stateless mechanism for securing RESTful APIs, providing a scalable and efficient way to manage user sessions.
* **`IPasswordHasher<TUser>` for Password Security:** Utilizes ASP.NET Core Identity's built-in password hashing for strong, salted password storage and verification, avoiding custom or insecure hashing implementations.
* **Centralized Exception Handling:** Custom exceptions (e.g., `UnauthorizedException`, `ValidationException`) are thrown by application handlers and caught by global API middleware to return consistent, user-friendly error responses.
* **AutoMapper:** Used for effortless mapping between entities, DTOs, and commands, reducing boilerplate code.

## Getting Started

### Prerequisites

* .NET SDK (6.0 , depending on your target framework)
* SQL Server (or LocalDB)

### Setup

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/OmarHamdyF/EmployeeManagment.git](https://github.com/OmarHamdyF/EmployeeManagment.git)
    cd EmployeeManagment
    ```

2.  **Configure Database:**
    * Open `EmployeeManagment.API/appsettings.json` and `appsettings.Development.json`.
    * Update the `ConnectionStrings:DefaultConnection` to point to your SQL Server instance. If using LocalDB, `Server=(localdb)\mssqllocaldb;Database=EmployeeManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true` should work.

3.  **Apply Migrations:**
    * Open the **Package Manager Console** in Visual Studio.
    * Set the **Default project** to `EmployeeManagement.Infrastructure`.
    * Run the following commands:
        ```powershell
        Add-Migration InitialCreate
        Update-Database
        ```
    * Alternatively, the `context.Database.Migrate()` call in `Program.cs` will apply pending migrations on startup.

4.  **Configure JWT Secret Key:**
    * In `appsettings.json`, ensure `JwtSettings:Key` is a long, complex string (at least 32 characters for HMACSHA256). **For production, use environment variables or a secrets management service.**

### Running the Application

1.  **Build the Solution:**
    ```bash
    dotnet build
    ```
    or use Visual Studio's Build command.

2.  **Run the API Project:**
    Set `EmployeeManagment.API` as the startup project in Visual Studio and run it, or use:
    ```bash
    cd EmployeeManagment.API
    dotnet run
    ```
    The API will typically run on `https://localhost:7068` (or another port assigned by VS).

## API Endpoints

Once the API is running, you can access the Swagger UI at `https://localhost:7068/swagger` (replace port if different) to explore and test the endpoints.

### Sample JWT Token for Testing

To get a sample JWT token, you need to use the login endpoint.

**Endpoint:** `POST /api/Auth/login`

**Request Body (JSON):**

```json
{
  "email": "test@example.com",
  "password": "Pa$$w0rd"
}
The response jwt needs to be attached in every request using postman in BearerToken field
