# ViewModel Architecture Instructions

## Overview
This document outlines the ViewModel pattern used in the Process360 architecture. ViewModels serve as Data Transfer Objects (DTOs) that bridge data between the **Process360.Core** domain models and the API layer.

## Architecture Pattern

### Model Location S    tructure
```
Process360.Core/
  └── Models/
      ├── Customer.cs
      ├── Project.cs
      ├── ProjectTask.cs
      └── ... (other domain models)

Process360.Repository/
  └── ViewModel/
      ├── CustomerDTO.cs
      ├── ProjectDTO.cs
      ├── ProjectTaskDTO.cs
      └── ... (corresponding DTOs)
```

## Naming Convention

### Naming Rules
- **Core Models**: Located in `Process360.Core/Models/` with domain-specific names
  - Example: `Customer.cs`, `Project.cs`, `ProjectTask.cs`

- **ViewModels/DTOs**: Located in `Process360.Repository/ViewModel/` with `DTO` suffix
  - Example: `CustomerDTO.cs`, `ProjectDTO.cs`, `ProjectTaskDTO.cs`

### Naming Pattern
For every model `XyzModel.cs` in `Process360.Core/Models/`, create a corresponding `XyzDTO.cs` in `Process360.Repository/ViewModel/`.

## Purpose and Responsibilities

### Domain Models (Process360.Core/Models)
- **Purpose**: Represent core business entities with navigation properties and relationships
- **Scope**: Full domain logic, including collections and navigation properties
- **Responsibility**: Serve as the source of truth for entity structure in the database

Example:
```csharp
public class Customer
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Project> Projects { get; set; } // Navigation
}
```

### ViewModels/DTOs (Process360.Repository/ViewModel)
- **Purpose**: Transfer data between API and business logic layers
- **Scope**: Simplified versions of domain models with only necessary properties
- **Responsibility**: 
  - Control what data is exposed through APIs
  - Prevent circular references
  - Provide API-level validation
  - Map data from/to domain models

Example:
```csharp
public class CustomerDTO
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    // Note: No Projects navigation property to avoid circular references
}
```

## ViewModel Design Guidelines

### 1. **Include Essential Properties**
   - Include all properties that APIs need to expose
   - Exclude internal/sensitive properties
   - Include nullable properties where appropriate using `?`

### 2. **Avoid Navigation Properties**
   - Don't include `ICollection<>` or complex navigation properties in DTOs
   - This prevents circular reference issues and simplifies API responses
   - Use flattening or separate endpoints for related data

### 3. **Use Nullable Reference Types**
   - Follow the nullable reference type pattern: `string?` for optional properties
   - Use non-nullable `string` for required properties
   - Initialize strings with empty string: `string Name = string.Empty;`

### 4. **Apply Data Annotations**
   - Use `System.ComponentModel.DataAnnotations` for validation
   - Mark required fields appropriately
   - Example:
```csharp
using System.ComponentModel.DataAnnotations;

public class ProjectDTO
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
}
```

### 5. **Add Metadata Properties**
   - Include audit properties that APIs should expose:
     - `CreatedBy`
     - `CreatedDate`
     - `ModifiedBy`
     - `ModifiedDate`

## Standard ViewModel Classes

### Core Response DTOs (Always Included)

#### ApiError.cs
```csharp
public class ApiError
{
    public string? Field { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
}
```
**Usage**: Return validation errors or business logic errors

#### ApiResponse.cs
**Usage**: Standardized API response envelope

#### PaginatedResponse.cs
**Usage**: For endpoints returning paginated data

#### PaginationMetadata.cs
**Usage**: Contains pagination information (page size, count, etc.)

## Mapping Between Models and DTOs

### Using AutoMapper (Recommended)
Create mapping profiles in `Process360.Repository` to automatically map between domain models and DTOs:

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<Project, ProjectDTO>().ReverseMap();
        // ... other mappings
    }
}
```

### Manual Mapping
If AutoMapper is not used, implement mapping methods:

```csharp
public static class CustomerMapper
{
    public static CustomerDTO ToDTO(Customer model)
    {
        return new CustomerDTO
        {
            Id = model.Id,
            Login = model.Login,
            Name = model.Name,
            Email = model.Email,
            Company = model.Company
        };
    }

    public static Customer ToModel(CustomerDTO dto)
    {
        return new Customer
        {
            Id = dto.Id,
            Login = dto.Login,
            Name = dto.Name,
            Email = dto.Email,
            Company = dto.Company
        };
    }
}
```

## Best Practices

1. **Keep DTOs Simple**: DTOs should focus on data transfer, not business logic
2. **Avoid Circular References**: Don't include navigation properties that reference back
3. **Use Consistent Naming**: Follow the `{ModelName}DTO` convention
4. **Document API Contracts**: DTOs define your API contract - keep them well-organized
5. **Separate Concerns**: Create specific DTOs for different API operations (CreateDTO, UpdateDTO, ResponseDTO)
6. **Version DTOs if Needed**: As APIs evolve, consider versioning (e.g., `CustomerDTOV2`)

## Common DTO Patterns

### Create Request DTO
```csharp
public class CreateCustomerDTO
{
    [Required]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Email { get; set; }
}
```

### Update Request DTO
```csharp
public class UpdateCustomerDTO
{
    [Required]
    public int Id { get; set; }
    
    public string? Name { get; set; }
    public string? Email { get; set; }
}
```

### Response DTO
```csharp
public class CustomerDTO
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
}
```

## File Organization

```
Process360.Repository/
├── ViewModel/
│   ├── ViewModel.md                    (this file)
│   ├── ApiError.cs
│   ├── ApiResponse.cs
│   ├── PaginatedResponse.cs
│   ├── PaginationMetadata.cs
│   ├── CustomerDTO.cs
│   ├── ProjectDTO.cs
│   ├── ProjectTaskDTO.cs
│   └── ... (other DTOs)
├── Interface/                          (Repository interfaces)
├── Repository/                         (Repository implementations)
└── Process360.Repository.csproj
```

## Summary

The ViewModel pattern in Process360 ensures:
- **Clean Separation**: Domain models remain independent from API concerns
- **Data Control**: APIs expose only necessary data
- **Type Safety**: DTOs provide compile-time checking for API contracts
- **Maintainability**: Changes to domain models don't break APIs automatically
- **Consistency**: All DTOs follow the same naming and structural conventions

Follow these guidelines when creating new DTOs to maintain consistency across the codebase.

D:\Nis-git\Process360.us\
├── docs/
│   ├── README.md                          (Overview)
│   ├── Architecture/
│   │   └── ViewModel.md                   (ViewModel documentation)
│   │   └── Database.md
│   │   └── API-Design.md
│   ├── Guidelines/
│   │   ├── Coding-Standards.md
│   │   ├── Git-Workflow.md
│   │   └── Setup-Instructions.md
│   ├── Database/
│   │   └── Schema.md
│   │   └── Migrations.md
│   ├── API/
│   │   └── Endpoints.md
│   │   └── Authentication.md
│   └── Troubleshooting.md
├── Process360.Core/
├── Process360.Repository/
├── Process360.API/
├── Process360.Database/
└── README.md                              (Root level overview)
