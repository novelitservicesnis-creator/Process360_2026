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
/// Unit tests for ProjectPlanningsController
/// </summary>
public class ProjectPlanningsControllerTests : Base.ControllerTestBase
{
    private readonly Mock<IProjectPlanningRepository> _mockRepository;
    private readonly ProjectPlanningsController _controller;

    public ProjectPlanningsControllerTests()
    {
        _mockRepository = new Mock<IProjectPlanningRepository>();
        var mockLogger = CreateMockLogger<ProjectPlanningsController>();
        _controller = new ProjectPlanningsController(_mockRepository.Object, MockMapper.Object, mockLogger.Object);
    }

    #region GetAll Tests
    
    [Fact]
    public async Task GetAll_WhenPlanningsExist_ReturnsOkResultWithDTOs()
    {
        // Arrange
        var plannings = new List<ProjectPlanning>
        {
            new() { Id = 1, Name = "Planning 1", Goal = "Goal 1", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) },
            new() { Id = 2, Name = "Planning 2", Goal = "Goal 2", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) }
        };
        var planningDTOs = new List<ProjectPlanningDTO>
        {
            new() { Id = 1, Name = "Planning 1", Goal = "Goal 1" },
            new() { Id = 2, Name = "Planning 2", Goal = "Goal 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(plannings);
        MockMapper.Setup(m => m.Map<List<ProjectPlanningDTO>>(plannings)).Returns(planningDTOs);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<ProjectPlanningDTO>>(okResult.Value);
        returnedDTOs.Should().HaveCount(2);
        returnedDTOs[0].Name.Should().Be("Planning 1");
        returnedDTOs[1].Name.Should().Be("Planning 2");
    }

    [Fact]
    public async Task GetAll_WhenNoPlannersExist_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProjectPlanning>());
        MockMapper.Setup(m => m.Map<List<ProjectPlanningDTO>>(It.IsAny<IEnumerable<ProjectPlanning>>())).Returns(new List<ProjectPlanningDTO>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<ProjectPlanningDTO>>(okResult.Value);
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
    public async Task GetById_WhenPlanningExists_ReturnsOkResultWithDTO()
    {
        // Arrange
        var planning = new ProjectPlanning 
        { 
            Id = 1, 
            Name = "Planning 1", 
            Goal = "Goal 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        var planningDTO = new ProjectPlanningDTO { Id = 1, Name = "Planning 1", Goal = "Goal 1" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync(planning);
        MockMapper.Setup(m => m.Map<ProjectPlanningDTO>(planning)).Returns(planningDTO);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTO = ExtractDataFromResponse<ProjectPlanningDTO>(okResult.Value);
        returnedDTO.Id.Should().Be(1);
        returnedDTO.Name.Should().Be("Planning 1");
    }

    [Fact]
    public async Task GetById_WhenPlanningDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync((ProjectPlanning)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetById_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(1)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region GetByDateRange Tests

    [Fact]
    public async Task GetByDateRange_WhenPlanningsExistInRange_ReturnsOkResultWithDTOs()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-10);
        var endDate = DateTime.UtcNow.AddDays(10);
        var plannings = new List<ProjectPlanning>
        {
            new() { Id = 1, Name = "Planning 1", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) }
        };
        var planningDTOs = new List<ProjectPlanningDTO>
        {
            new() { Id = 1, Name = "Planning 1" }
        };
        _mockRepository.Setup(r => r.GetPlanningByDateRangeAsync(startDate, endDate)).ReturnsAsync(plannings);
        MockMapper.Setup(m => m.Map<List<ProjectPlanningDTO>>(plannings)).Returns(planningDTOs);

        // Act
        var result = await _controller.GetByDateRange(startDate, endDate);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<ProjectPlanningDTO>>(okResult.Value);
        returnedDTOs.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByDateRange_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(10);
        _mockRepository.Setup(r => r.GetPlanningByDateRangeAsync(startDate, endDate))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetByDateRange(startDate, endDate);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region GetCurrent Tests

    [Fact]
    public async Task GetCurrent_WhenCurrentPlanningsExist_ReturnsOkResultWithDTOs()
    {
        // Arrange
        var plannings = new List<ProjectPlanning>
        {
            new() { Id = 1, Name = "Current Planning 1", StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(10) }
        };
        var planningDTOs = new List<ProjectPlanningDTO>
        {
            new() { Id = 1, Name = "Current Planning 1" }
        };
        _mockRepository.Setup(r => r.GetCurrentPlanningsAsync()).ReturnsAsync(plannings);
        MockMapper.Setup(m => m.Map<List<ProjectPlanningDTO>>(plannings)).Returns(planningDTOs);

        // Act
        var result = await _controller.GetCurrent();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTOs = ExtractDataFromResponse<List<ProjectPlanningDTO>>(okResult.Value);
        returnedDTOs.Should().HaveCount(1);
        returnedDTOs[0].Name.Should().Be("Current Planning 1");
    }

    [Fact]
    public async Task GetCurrent_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetCurrentPlanningsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetCurrent();

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WithValidDTO_ReturnsCreatedAtActionResultWithDTO()
    {
        // Arrange
        var createDTO = new CreateProjectPlanningDTO
        {
            Name = "New Planning",
            Goal = "New Goal",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        var createdPlanning = new ProjectPlanning
        {
            Id = 1,
            Name = createDTO.Name,
            Goal = createDTO.Goal,
            StartDate = createDTO.StartDate,
            EndDate = createDTO.EndDate,
            CreatedDate = DateTime.UtcNow
        };
        var createdPlanningDTO = new ProjectPlanningDTO
        {
            Id = 1,
            Name = "New Planning",
            Goal = "New Goal"
        };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<ProjectPlanning>())).ReturnsAsync(createdPlanning);
        _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.FromResult(1));
        MockMapper.Setup(m => m.Map<ProjectPlanning>(createDTO)).Returns(createdPlanning);
        MockMapper.Setup(m => m.Map<ProjectPlanningDTO>(createdPlanning)).Returns(createdPlanningDTO);

        // Act
        var result = await _controller.Create(createDTO);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        var returnedDTO = ExtractDataFromResponse<ProjectPlanningDTO>(createdResult.Value);
        returnedDTO.Name.Should().Be("New Planning");
        returnedDTO.Goal.Should().Be("New Goal");
    }

    [Fact]
    public async Task Create_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        var createDTO = new CreateProjectPlanningDTO
        {
            Name = "New Planning",
            Goal = "New Goal"
        };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<ProjectPlanning>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(createDTO);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_WithValidIDAndDTO_ReturnsOkResultWithUpdatedDTO()
    {
        // Arrange
        int planningId = 1;
        var updateDTO = new UpdateProjectPlanningDTO
        {
            Name = "Updated Planning",
            Goal = "Updated Goal",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(45)
        };
        var existingPlanning = new ProjectPlanning
        {
            Id = planningId,
            Name = "Old Planning",
            Goal = "Old Goal",
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow.AddDays(10)
        };
        var updatedPlanning = new ProjectPlanning
        {
            Id = planningId,
            Name = updateDTO.Name,
            Goal = updateDTO.Goal,
            StartDate = updateDTO.StartDate,
            EndDate = updateDTO.EndDate
        };
        var updatedPlanningDTO = new ProjectPlanningDTO
        {
            Id = planningId,
            Name = "Updated Planning",
            Goal = "Updated Goal"
        };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId)).ReturnsAsync(existingPlanning);
        _mockRepository.Setup(r => r.EditAsync(It.IsAny<ProjectPlanning>())).ReturnsAsync(updatedPlanning);
        _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.FromResult(1));
        MockMapper.Setup(m => m.Map<ProjectPlanningDTO>(updatedPlanning)).Returns(updatedPlanningDTO);

        // Act
        var result = await _controller.Update(planningId, updateDTO);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDTO = ExtractDataFromResponse<ProjectPlanningDTO>(okResult.Value);
        returnedDTO.Name.Should().Be("Updated Planning");
        returnedDTO.Goal.Should().Be("Updated Goal");
    }

    [Fact]
    public async Task Update_WhenPlanningDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        int planningId = 999;
        var updateDTO = new UpdateProjectPlanningDTO { Name = "Updated" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId)).ReturnsAsync((ProjectPlanning)null);

        // Act
        var result = await _controller.Update(planningId, updateDTO);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Update_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        int planningId = 1;
        var updateDTO = new UpdateProjectPlanningDTO { Name = "Updated" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Update(planningId, updateDTO);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WithValidID_ReturnsNoContentResult()
    {
        // Arrange
        int planningId = 1;
        var planning = new ProjectPlanning { Id = planningId, Name = "Planning to delete" };
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId)).ReturnsAsync(planning);
        _mockRepository.Setup(r => r.DeleteAsync(planningId)).Returns(Task.FromResult(true));
        _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _controller.Delete(planningId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockRepository.Verify(r => r.DeleteAsync(planningId), Times.Once);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenPlanningDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        int planningId = 999;
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId)).ReturnsAsync((ProjectPlanning)null);

        // Act
        var result = await _controller.Delete(planningId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_WhenExceptionOccurs_Returns500Error()
    {
        // Arrange
        int planningId = 1;
        _mockRepository.Setup(r => r.GetDetailsByIdAsync(planningId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Delete(planningId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
    }

    #endregion
}
