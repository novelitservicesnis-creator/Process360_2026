using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectPlanningRepository : IRepository<ProjectPlanning>
{
    Task<IEnumerable<ProjectPlanning>> GetPlanningByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ProjectPlanning>> GetCurrentPlanningsAsync();
}
