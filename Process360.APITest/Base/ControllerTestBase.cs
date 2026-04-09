using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Process360.Repository.ViewModel;

namespace Process360.APITest.Base;

/// <summary>
/// Base class for controller tests providing common setup and utilities
/// </summary>
public abstract class ControllerTestBase
{
    protected readonly Mock<IMapper> MockMapper;

    protected ControllerTestBase()
    {
        MockMapper = new Mock<IMapper>();
    }

    /// <summary>
    /// Helper method to setup mapper to return a DTO when mapping is called
    /// </summary>
    protected void SetupMapperMock<TSource, TDestination>(TSource source, TDestination destination)
        where TSource : class
        where TDestination : class
    {
        MockMapper
            .Setup(m => m.Map<TDestination>(It.Is<TSource>(s => s == source)))
            .Returns(destination);
    }

    /// <summary>
    /// Helper method to setup mapper to return DTOs when mapping collections
    /// </summary>
    protected void SetupMapperMockForList<TSource, TDestination>(IEnumerable<TSource> source, List<TDestination> destination)
        where TSource : class
        where TDestination : class
    {
        MockMapper
            .Setup(m => m.Map<List<TDestination>>(It.Is<IEnumerable<TSource>>(s => s == source)))
            .Returns(destination);
    }

    /// <summary>
    /// Helper method to setup mapper for any mapping call (more flexible)
    /// </summary>
    protected void SetupMapperMockAny<TSource, TDestination>()
        where TSource : class
        where TDestination : class, new()
    {
        MockMapper
            .Setup(m => m.Map<TDestination>(It.IsAny<TSource>()))
            .Returns((TSource source) => new TDestination());
    }

    /// <summary>
    /// Extract data from ApiResponse wrapper
    /// </summary>
    protected T ExtractDataFromResponse<T>(object? response) where T : class
    {
        if (response == null)
            throw new InvalidOperationException("Response is null");

        var responseType = response.GetType();
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(ApiResponse<>))
            throw new InvalidOperationException($"Response is not an ApiResponse<T>, it is {responseType.Name}");

        var dataProperty = responseType.GetProperty("Data");
        if (dataProperty == null)
            throw new InvalidOperationException("ApiResponse does not have a Data property");

        return (T)dataProperty.GetValue(response)!;
    }

    protected Mock<ILogger<T>> CreateMockLogger<T>() where T : class
    {
        return new Mock<ILogger<T>>();
    }
}
