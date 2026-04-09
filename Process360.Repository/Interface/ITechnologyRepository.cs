using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface ITechnologyRepository : IRepository<Technology>
{
    Task<IEnumerable<Technology>> GetTechnologiesByTypeAsync(string type);
    Task<IEnumerable<Technology>> GetActiveTechnologiesAsync();
    Task<Technology?> GetTechnologyByNameAsync(string name);
}
