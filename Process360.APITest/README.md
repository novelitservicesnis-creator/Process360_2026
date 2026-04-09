# Process360.APITest - Unit Testing Guide

## Overview
`Process360.APITest` is the comprehensive unit test project for all API endpoints in the Process360 system. It uses **xUnit**, **Moq**, and **FluentAssertions** for testing.

## Project Structure

```
Process360.APITest/
├── Base/
│   └── ControllerTestBase.cs       # Base class for all controller tests
├── Controllers/
│   ├── ProjectPlanningsControllerTests.cs
│   ├── CustomersControllerTests.cs
│   └── [Additional Controller Tests]
└── Process360.APITest.csproj
```

## Testing Framework & Libraries

- **xUnit**: Testing framework
- **Moq**: Mocking library for dependencies
- **FluentAssertions**: Fluent assertion API for readable tests
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing support
- **AutoMapper**: For DTO mappings in tests

## Testing Pattern

Each controller test class follows the **Arrange-Act-Assert (AAA)** pattern:

```csharp
[Fact]
public async Task MethodName_WhenCondition_ReturnsExpectedResult()
{
    // Arrange - Setup test data and mocks
    var mockRepository = new Mock<IRepository>();
    mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(testData);
    
    // Act - Execute the method
    var result = await controller.MethodName();
    
    // Assert - Verify the result
    result.Should().BeOfType<OkObjectResult>();
}
```

## Test Coverage Guidelines

For each API endpoint, test the following scenarios:

### GET Endpoints
- ✅ Returns 200 OK with correct data
- ✅ Returns 404 NotFound when resource doesn't exist
- ✅ Returns 500 InternalServerError on exception

### POST Endpoints
- ✅ Returns 201 Created with DTO response
- ✅ Returns validation error on invalid input
- ✅ Returns 500 InternalServerError on exception

### PUT Endpoints
- ✅ Returns 200 OK with updated DTO
- ✅ Returns 404 NotFound when resource doesn't exist
- ✅ Returns 500 InternalServerError on exception

### DELETE Endpoints
- ✅ Returns 204 NoContent on successful deletion
- ✅ Returns 404 NotFound when resource doesn't exist
- ✅ Returns 500 InternalServerError on exception

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run tests in specific project
```bash
dotnet test Process360.APITest.csproj
```

### Run specific test class
```bash
dotnet test --filter "FullyQualifiedName~ProjectPlanningsControllerTests"
```

### Run specific test method
```bash
dotnet test --filter "FullyQualifiedName~ProjectPlanningsControllerTests.GetAll_WhenPlanningsExist_ReturnsOkResultWithDTOs"
```

## Key Testing Principles

1. **Mock Dependencies**: Always mock repository and logger dependencies
2. **DTO Validation**: Verify DTOs are returned, not domain models
3. **Mapper Verification**: Ensure AutoMapper correctly converts between DTOs and Models
4. **Exception Handling**: Test that controllers properly handle exceptions
5. **HTTP Status Codes**: Verify correct HTTP status codes are returned
6. **Fluent Assertions**: Use fluent syntax for readable assertions

## Example: Adding a New Controller Test

```csharp
public class NewControllerTests : ControllerTestBase
{
    private readonly Mock<INewRepository> _mockRepository;
    private readonly NewController _controller;

    public NewControllerTests()
    {
        _mockRepository = new Mock<INewRepository>();
        var mockLogger = CreateMockLogger<NewController>();
        _controller = new NewController(_mockRepository.Object, Mapper, mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_WhenDataExists_ReturnsOkResultWithDTOs()
    {
        // Arrange
        var data = new List<Entity> { new Entity { Id = 1 } };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = (List<EntityDTO>)okResult.Value;
        returnedDTOs.Should().HaveCount(1);
    }
}
```

## Best Practices

1. **One assertion per test** (when possible) - Use fluent assertions to check multiple conditions
2. **Descriptive test names** - Follow the `MethodName_Condition_ExpectedResult` pattern
3. **Isolate tests** - Each test should be independent
4. **Use Mock.Verify()** - Ensure methods are called with expected parameters
5. **Test edge cases** - Null values, empty collections, invalid data
6. **DRY principle** - Use base class for common setup

## Continuous Integration

Tests will run automatically on:
- Pre-commit hooks
- Pull request validation
- CI/CD pipeline

Ensure all tests pass before merging to main branch.
