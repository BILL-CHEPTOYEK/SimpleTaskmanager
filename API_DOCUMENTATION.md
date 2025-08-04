# SimpleTaskManager - Complete API Documentation

## Overview

SimpleTaskManager is a comprehensive task management REST API built with ASP.NET Core and PostgreSQL. It provides full CRUD operations with enhanced features for managing tasks efficiently.

## Features Implemented

### ✅ Core CRUD Operations
- **Create** new tasks with validation
- **Read** all tasks or specific tasks by ID
- **Update** existing tasks with proper validation
- **Delete** tasks with error handling

### ✅ Enhanced Task Model
- **Title** (required, 1-100 characters)
- **Description** (optional, up to 500 characters)
- **Priority** (1-5: Low, Below Normal, Normal, High, Critical)
- **Due Date** (optional)
- **Completion Status** with automatic completion timestamp
- **Creation Date** (automatic)
- **Completion Date** (automatic when marked complete)

### ✅ Advanced Filtering & Querying
- Get tasks by completion status (`/api/tasks/status/{true|false}`)
- Get tasks by priority level (`/api/tasks/priority/{1-5}`)
- Get overdue tasks (`/api/tasks/overdue`)
- Get task statistics (`/api/tasks/statistics`)

### ✅ Data Transfer Objects (DTOs)
- **CreateTaskDto** - For creating new tasks
- **UpdateTaskDto** - For updating existing tasks  
- **TaskResponseDto** - Enhanced response with computed properties (priority text, overdue status)

### ✅ Professional API Design
- Comprehensive error handling and logging
- Input validation with detailed error messages
- Proper HTTP status codes
- Swagger/OpenAPI documentation
- CORS support for frontend integration

### ✅ Additional Endpoints
- **PATCH** `/api/tasks/{id}/complete` - Mark task as complete
- **GET** `/api/tasks/statistics` - Get task statistics summary
- **GET** `/api/tasks/overdue` - Get all overdue tasks

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks/{id}` | Get specific task by ID |
| POST | `/api/tasks` | Create new task |
| PUT | `/api/tasks/{id}` | Update existing task |
| DELETE | `/api/tasks/{id}` | Delete task |
| PATCH | `/api/tasks/{id}/complete` | Mark task as complete |
| GET | `/api/tasks/status/{completed}` | Get tasks by completion status |
| GET | `/api/tasks/priority/{priority}` | Get tasks by priority (1-5) |
| GET | `/api/tasks/overdue` | Get overdue tasks |
| GET | `/api/tasks/statistics` | Get task statistics |

## Request/Response Examples

### Create Task
```json
POST /api/tasks
{
  "title": "Complete project documentation",
  "description": "Write comprehensive documentation",
  "priority": 4,
  "dueDate": "2025-08-10T10:00:00Z"
}
```

### Response
```json
{
  "id": 1,
  "title": "Complete project documentation",
  "description": "Write comprehensive documentation",
  "isComplete": false,
  "createdDate": "2025-08-04T10:00:00Z",
  "dueDate": "2025-08-10T10:00:00Z",
  "completedDate": null,
  "priority": 4,
  "priorityText": "High",
  "isOverdue": false
}
```

### Task Statistics Response
```json
{
  "totalTasks": 10,
  "completedTasks": 6,
  "pendingTasks": 4,
  "overdueTasks": 2,
  "highPriorityTasks": 3,
  "completionRate": 60.0
}
```

## Priority Levels

| Value | Description |
|-------|-------------|
| 1 | Low |
| 2 | Below Normal |
| 3 | Normal (default) |
| 4 | High |
| 5 | Critical |

## Validation Rules

- **Title**: Required, 1-100 characters
- **Description**: Optional, max 500 characters
- **Priority**: Must be between 1-5
- **DueDate**: Optional ISO 8601 datetime

## Error Handling

The API provides comprehensive error handling:
- **400 Bad Request**: Invalid input data or validation errors
- **404 Not Found**: Resource not found
- **409 Conflict**: Concurrency conflicts during updates
- **500 Internal Server Error**: Server-side errors with logging

## Testing

The project includes a comprehensive HTTP test file (`SimpleTaskManager.http`) with examples for:
- All CRUD operations
- Filtering and querying
- Error scenarios
- Validation testing

## Technology Stack

- **.NET 8.0** - Latest LTS framework
- **ASP.NET Core Web API** - REST API framework
- **Entity Framework Core** - ORM with PostgreSQL provider
- **PostgreSQL** - Reliable relational database
- **Swagger/OpenAPI** - API documentation
- **Data Annotations** - Input validation

## Getting Started

1. **Prerequisites**: .NET 8.0 SDK, PostgreSQL
2. **Database Setup**: Create database and user as described in README.md
3. **Run Migrations**: `dotnet ef database update`
4. **Start Application**: `dotnet run`
5. **Access API**: Navigate to `http://localhost:5158` for Swagger UI

## Development Features

- **Comprehensive Logging** - Structured logging for monitoring
- **Auto-generated Timestamps** - Creation and completion dates
- **Concurrency Handling** - Proper handling of concurrent updates
- **Input Sanitization** - Validation and error handling
- **Professional Architecture** - DTOs, services, and clean separation

This implementation provides a production-ready foundation for a task management system with all essential features and professional development practices.
