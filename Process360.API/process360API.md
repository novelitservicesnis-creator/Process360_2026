# Process360 API Documentation

## Overview
This document outlines the API structure, endpoints, security requirements, and standards for the Process360 project.

---

## Table of Contents
1. [API Standards](#api-standards)
2. [Base Controller](#base-controller)
3. [Security & Authorization](#security--authorization)
4. [Response Format](#response-format)
5. [Endpoints](#endpoints)

---

## API Standards

### Base URL
```
https://api.process360.com/api/v1
```

### API Version
- **Current Version**: v1
- **Content Type**: `application/json`

### Request/Response Standards
- **Timestamp Format**: ISO 8601 (UTC)
- **Date Format**: YYYY-MM-DD
- **Decimal Format**: Up to 2 decimal places for currency/time
- **HTTP Methods**: GET, POST, PUT, DELETE

---

## Base Controller

### Base Controller Features

#### 1. **Response Wrapper**
All endpoints return a standardized response object:

```csharp
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { /* entity data */ },
  "errors": [],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

#### 2. **Error Handling**
- All exceptions are caught and logged
- Error responses include:
  - HTTP Status Code
  - Error Message
  - Error Details (in development only)
  - Timestamp

#### 3. **Standard HTTP Status Codes**
| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful GET, PUT |
| 201 | Created | Successful POST |
| 204 | No Content | Successful DELETE |
| 400 | Bad Request | Invalid input validation |
| 401 | Unauthorized | Missing/invalid authentication |
| 403 | Forbidden | Insufficient permissions |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Unhandled exception |

#### 4. **Pagination**
Applicable to list endpoints (GET all):
```json
{
  "success": true,
  "data": {
    "items": [],
    "pageNumber": 1,
    "pageSize": 10,
    "totalItems": 50,
    "totalPages": 5
  }
}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10, max: 100)
- `searchTerm` (optional)
- `sortBy` (optional)
- `sortDirection` (asc/desc, default: asc)

---

## Security & Authorization

### Authentication
- **Type**: JWT (JSON Web Tokens)
- **Header**: `Authorization: Bearer {token}`
- **Token Expiration**: 24 hours
- **Refresh Token**: Supported (7 days)

### Authorization
- **Roles**: Admin, User
- **Attributes**: `[Authorize]`, `[Authorize(Roles = "Admin")]`

### Default Security Rules
1. **All endpoints require authentication** (unless marked as `[AllowAnonymous]`)
2. **Admin-only endpoints**:
   - Create/Edit/Delete operations on core entities
   - User management
   - System configuration

3. **User endpoints**:
   - View own resources
   - Submit comments and attachments
   - View assigned tasks

### Security Headers
All responses include:
```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

---

## Response Format

### Success Response (200/201)
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    "id": 1,
    "name": "Sample"
  },
  "errors": [],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Error Response (4xx/5xx)
```json
{
  "success": false,
  "message": "An error occurred",
  "data": null,
  "errors": [
    {
      "field": "name",
      "message": "Name is required"
    }
  ],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Validation Error Response (400)
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    {
      "field": "email",
      "message": "Email format is invalid"
    },
    {
      "field": "password",
      "message": "Password must be at least 8 characters"
    }
  ],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

---

## Endpoints

### 1. Customer Controller
**Base Route**: `/api/v1/customers`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all customers | ✓ | Admin |
| GET | `/{id}` | Get customer by ID | ✓ | Admin |
| GET | `/login/{login}` | Get customer by login | ✓ | Admin |
| GET | `/email/{email}` | Get customer by email | ✓ | Admin |
| GET | `/active` | Get active customers | ✓ | Admin |
| POST | `/` | Create customer | ✓ | Admin |
| PUT | `/{id}` | Update customer | ✓ | Admin |
| DELETE | `/{id}` | Delete customer | ✓ | Admin |

### 2. Project Controller
**Base Route**: `/api/v1/projects`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all projects | ✓ | User |
| GET | `/{id}` | Get project by ID | ✓ | User |
| GET | `/code/{code}` | Get project by code | ✓ | User |
| GET | `/customer/{customerId}` | Get projects by customer | ✓ | User |
| GET | `/active` | Get active projects | ✓ | User |
| POST | `/` | Create project | ✓ | Admin |
| PUT | `/{id}` | Update project | ✓ | Admin |
| DELETE | `/{id}` | Delete project | ✓ | Admin |

### 3. Resources Controller
**Base Route**: `/api/v1/resources`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all resources | ✓ | User |
| GET | `/{id}` | Get resource by ID | ✓ | User |
| GET | `/email/{email}` | Get resource by email | ✓ | User |
| GET | `/active` | Get active resources | ✓ | User |
| GET | `/role/{role}` | Get resources by role | ✓ | Admin |
| POST | `/` | Create resource | ✓ | Admin |
| PUT | `/{id}` | Update resource | ✓ | Admin |
| DELETE | `/{id}` | Delete resource | ✓ | Admin |

### 4. Project Task Type Controller
**Base Route**: `/api/v1/project-task-types`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all task types | ✓ | User |
| GET | `/{id}` | Get task type by ID | ✓ | User |
| GET | `/name/{name}` | Get task type by name | ✓ | User |
| POST | `/` | Create task type | ✓ | Admin |
| PUT | `/{id}` | Update task type | ✓ | Admin |
| DELETE | `/{id}` | Delete task type | ✓ | Admin |

### 5. Project Task Controller
**Base Route**: `/api/v1/project-tasks`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all tasks | ✓ | User |
| GET | `/{id}` | Get task by ID | ✓ | User |
| GET | `/project/{projectId}` | Get tasks by project | ✓ | User |
| GET | `/assignee/{resourceId}` | Get tasks by assignee | ✓ | User |
| GET | `/type/{typeId}` | Get tasks by type | ✓ | User |
| GET | `/overdue` | Get overdue tasks | ✓ | User |
| GET | `/sprint/{sprintId}` | Get tasks by sprint | ✓ | User |
| POST | `/` | Create task | ✓ | User |
| PUT | `/{id}` | Update task | ✓ | User |
| DELETE | `/{id}` | Delete task | ✓ | User |

### 6. Project Task Attachments Controller
**Base Route**: `/api/v1/project-task-attachments`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all attachments | ✓ | User |
| GET | `/{id}` | Get attachment by ID | ✓ | User |
| GET | `/task/{taskId}` | Get attachments by task | ✓ | User |
| POST | `/` | Create attachment | ✓ | User |
| PUT | `/{id}` | Update attachment | ✓ | User |
| DELETE | `/{id}` | Delete attachment | ✓ | User |

### 7. Technology Controller
**Base Route**: `/api/v1/technologies`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all technologies | ✓ | User |
| GET | `/{id}` | Get technology by ID | ✓ | User |
| GET | `/type/{type}` | Get technologies by type | ✓ | User |
| GET | `/name/{name}` | Get technology by name | ✓ | User |
| GET | `/active` | Get active technologies | ✓ | User |
| POST | `/` | Create technology | ✓ | Admin |
| PUT | `/{id}` | Update technology | ✓ | Admin |
| DELETE | `/{id}` | Delete technology | ✓ | Admin |

### 8. Project Planning Controller
**Base Route**: `/api/v1/project-plannings`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all plannings | ✓ | User |
| GET | `/{id}` | Get planning by ID | ✓ | User |
| GET | `/date-range` | Get plannings by date range | ✓ | User |
| GET | `/current` | Get current plannings | ✓ | User |
| POST | `/` | Create planning | ✓ | Admin |
| PUT | `/{id}` | Update planning | ✓ | Admin |
| DELETE | `/{id}` | Delete planning | ✓ | Admin |

### 9. Project Planning Tasks Controller
**Base Route**: `/api/v1/project-planning-tasks`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all planning tasks | ✓ | User |
| GET | `/{id}` | Get planning task by ID | ✓ | User |
| GET | `/planning/{planningId}` | Get tasks by planning | ✓ | User |
| GET | `/project/{projectId}` | Get tasks by project | ✓ | User |
| GET | `/completed` | Get completed tasks | ✓ | User |
| POST | `/` | Create planning task | ✓ | User |
| PUT | `/{id}` | Update planning task | ✓ | User |
| DELETE | `/{id}` | Delete planning task | ✓ | User |

### 10. Project Resources Controller
**Base Route**: `/api/v1/project-resources`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all project resources | ✓ | User |
| GET | `/{id}` | Get project resource by ID | ✓ | User |
| GET | `/project/{projectId}` | Get resources by project | ✓ | User |
| GET | `/resource/{resourceId}` | Get projects by resource | ✓ | User |
| GET | `/role/{role}` | Get resources by role | ✓ | Admin |
| POST | `/` | Create project resource | ✓ | Admin |
| PUT | `/{id}` | Update project resource | ✓ | Admin |
| DELETE | `/{id}` | Delete project resource | ✓ | Admin |

### 11. Task Comments Controller
**Base Route**: `/api/v1/task-comments`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all comments | ✓ | User |
| GET | `/{id}` | Get comment by ID | ✓ | User |
| GET | `/task/{taskId}` | Get comments by task | ✓ | User |
| GET | `/user/{userId}` | Get comments by user | ✓ | User |
| POST | `/` | Create comment | ✓ | User |
| PUT | `/{id}` | Update comment | ✓ | User |
| DELETE | `/{id}` | Delete comment | ✓ | User |

### 12. Project Task Linked Controller
**Base Route**: `/api/v1/project-task-linked`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all linked tasks | ✓ | User |
| GET | `/{id}` | Get linked task by ID | ✓ | User |
| GET | `/task/{taskId}` | Get linked tasks by task | ✓ | User |
| GET | `/relation/{relationType}` | Get linked tasks by relation | ✓ | User |
| POST | `/` | Create linked task | ✓ | User |
| PUT | `/{id}` | Update linked task | ✓ | User |
| DELETE | `/{id}` | Delete linked task | ✓ | User |

### 13. Project Task Status History Controller
**Base Route**: `/api/v1/project-task-status-histories`

| Method | Endpoint | Description | Auth | Role |
|--------|----------|-------------|------|------|
| GET | `/` | Get all status histories | ✓ | User |
| GET | `/{id}` | Get status history by ID | ✓ | User |
| GET | `/task/{taskId}` | Get status history by task | ✓ | User |
| POST | `/` | Create status history | ✓ | User |
| DELETE | `/{id}` | Delete status history | ✓ | Admin |

---

## Query Parameters

### Common Filters
```
GET /api/v1/resource?pageNumber=1&pageSize=10&searchTerm=test&sortBy=name&sortDirection=asc
```

### Supported Parameters
- `pageNumber`: Page number (default: 1)
- `pageSize`: Items per page (default: 10, max: 100)
- `searchTerm`: Search keyword
- `sortBy`: Field to sort by
- `sortDirection`: asc or desc

---

## Error Codes

### Authentication Errors
| Code | Message |
|------|---------|
| 401-001 | Token expired |
| 401-002 | Invalid token |
| 401-003 | Token not provided |

### Validation Errors
| Code | Message |
|------|---------|
| 400-001 | Validation failed |
| 400-002 | Required field missing |
| 400-003 | Invalid format |

### Authorization Errors
| Code | Message |
|------|---------|
| 403-001 | Insufficient permissions |
| 403-002 | Resource access denied |

### Not Found Errors
| Code | Message |
|------|---------|
| 404-001 | Resource not found |
| 404-002 | Entity not found |

### Server Errors
| Code | Message |
|------|---------|
| 500-001 | Internal server error |
| 500-002 | Database error |

---

## Rate Limiting
- **Limit**: 1000 requests per hour per IP
- **Header**: `X-RateLimit-Remaining`
- **Reset Time**: `X-RateLimit-Reset`

---

## CORS Policy
- **Allowed Origins**: https://process360.com, https://*.process360.com
- **Allowed Methods**: GET, POST, PUT, DELETE, OPTIONS
- **Allowed Headers**: Content-Type, Authorization
- **Credentials**: Allowed

---

## Best Practices

### Client Guidelines
1. **Always include Authorization header**
2. **Handle pagination for list endpoints**
3. **Implement retry logic with exponential backoff**
4. **Cache responses when applicable**
5. **Validate input before sending requests**
6. **Handle all error codes appropriately**

### API Guidelines
1. **All endpoints are case-insensitive**
2. **Timestamps are in UTC**
3. **Empty results return 200 with empty array**
4. **Null values are omitted in responses**
5. **IDs are always integers**

---

## Example Requests

### Create Customer
```
POST /api/v1/customers
Authorization: Bearer {token}
Content-Type: application/json

{
  "login": "customer001",
  "name": "John Doe",
  "email": "john@example.com",
  "company": "ABC Corp",
  "website": "https://abc.com"
}
```

### Get Project Tasks
```
GET /api/v1/project-tasks?projectId=1&pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

### Update Task
```
PUT /api/v1/project-tasks/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Updated Task",
  "description": "Updated description",
  "endDate": "2024-12-31"
}
```

---

## Changelog

### v1.0 (Current)
- Initial API release
- 13 resource endpoints
- JWT authentication
- Role-based authorization
- Pagination support
- Comprehensive error handling

---

**Last Updated**: January 2024
**API Version**: v1
**Status**: Production Ready
