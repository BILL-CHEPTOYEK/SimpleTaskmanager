# SimpleTaskManager

A simple task management web API built with ASP.NET Core and PostgreSQL, designed as a beginner-friendly project to learn modern C# web development.

---

## Features

- Create, Read, Update, and Delete (CRUD) tasks  
- RESTful API endpoints  
- PostgreSQL database integration using Entity Framework Core  

---

## Technologies Used

- [.NET 8.0](https://dotnet.microsoft.com/en-us/)  
- ASP.NET Core Web API  
- Entity Framework Core with Npgsql provider (PostgreSQL)  
- PostgreSQL Database  

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)  
- [PostgreSQL](https://www.postgresql.org/download/)  
- A PostgreSQL user and database configured  

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/BILL-CHEPTOYEK/SimpleTaskmanager.git
cd SimpleTaskManager
```

### 2. Configure your PostgreSQL database
Create a database and user in PostgreSQL matching your connection string in appsettings.json:

```sql
-- Connect to PostgreSQL and run the following commands
-- Replace 'TaskDb' and 'Taskuser' with your desired database and user names
CREATE DATABASE TaskDb;
CREATE USER Taskuser WITH PASSWORD 'task@123';
GRANT ALL PRIVILEGES ON DATABASE TaskDb TO Taskuser;
```

### 3. Apply migrations and update the database
Make sure you have the Entity Framework CLI tool installed:
```bash
dotnet tool install --global dotnet-ef
```
Create and apply migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the application
```bash
dotnet run
```
The API will be available at:
```bash
http://localhost:5158
```
### 5. Usage
You can use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/) to interact with the API endpoints:
```bash
Sample API Endpoints
GET /tasks - Retrieve all tasks

GET /tasks/{id} - Retrieve a task by ID

POST /tasks - Create a new task

PUT /tasks/{id} - Update an existing task

DELETE /tasks/{id} - Delete a task
```

### Contributing
Feel free to fork the repo, make improvements, and submit pull requests!

### License
MIT License. See [LICENSE](LICENSE) for details.
---
This project is a great starting point for anyone looking to learn ASP.NET Core and PostgreSQL.
Happy coding! 😊
### Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [ChatGPT](https://openai.com/chatgpt)
