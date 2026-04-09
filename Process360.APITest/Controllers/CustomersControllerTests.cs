using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Process360.API.Controllers;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;
using Xunit;

namespace Process360.APITest.Controllers;

/// <summary>
/// Unit tests for CustomersController
/// </summary>
public class CustomersControllerTests : Base.ControllerTestBase
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        var mockLogger = CreateMockLogger<CustomersController>();
        _controller = new CustomersController(_mockRepository.Object, MockMapper.Object, mockLogger.Object);
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenCustomersExist_ReturnsOkResultWithDTOs()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Id = 1, Login = "customer1", Name = "Customer 1", Email = "customer1@example.com", IsActive = true },
            new() { Id = 2, Login = "customer2", Name = "Customer 2", Email = "customer2@example.com", IsActive = true }
        };
        var customerDTOs = new List<CustomerDTO>
        {
            new() { Id = 1, Login = "customer1", Name = "Customer 1" },
            new() { Id = 2, Login = "customer2", Name = "Customer 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);
        MockMapper.Setup(m => m.Map<List<CustomerDTO>>(customers)).Returns(customerDTOs);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<CustomerDTO>>(okResult.Value);
        returnedDTOs.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAll_WhenNoCustomersExist_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>());
        MockMapper.Setup(m => m.Map<List<CustomerDTO>>(It.IsAny<IEnumerable<Customer>>())).Returns(new List<CustomerDTO>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<CustomerDTO>>(okResult.Value);
        returnedDTOs.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_WhenCustomerExists_ReturnsOkResultWithDTO()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Login = "customer1",
            Name = "Customer 1",
            Email = "customer1@example.com",
            IsActive = true
        };
        var customerDTO = new CustomerDTO { Id = 1, Login = "customer1", Name = "Customer 1" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync(customer);
        MockMapper.Setup(m => m.Map<CustomerDTO>(customer)).Returns(customerDTO);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTO = ExtractDataFromResponse<CustomerDTO>(okResult.Value);
        returnedDTO.Id.Should().Be(1);
        returnedDTO.Login.Should().Be("customer1");
    }

    [Fact]
    public async Task GetById_WhenCustomerDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync((Customer)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WithValidID_ReturnsNoContentResult()
    {
        // Arrange
        int customerId = 1;
        var customer = new Customer { Id = customerId, Login = "customer1" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(customerId)).ReturnsAsync(customer);
        _mockRepository.Setup(r => r.DeleteAsync(customerId)).Returns(Task.FromResult(true));
        _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockRepository.Verify(r => r.DeleteAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenCustomerDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        int customerId = 999;
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(customerId)).ReturnsAsync((Customer)null);

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    #endregion
}
