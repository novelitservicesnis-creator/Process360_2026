using AutoMapper;
using Process360.Core.Models;
using Process360.Repository.ViewModel;

namespace Process360.API.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain models and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
        CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();

        // Project
        CreateMap<Project, ProjectDTO>().ReverseMap();
        CreateMap<Project, CreateProjectDTO>().ReverseMap();
        CreateMap<Project, UpdateProjectDTO>().ReverseMap();

        // ProjectTask
        CreateMap<ProjectTask, ProjectTaskDTO>().ReverseMap();
        CreateMap<ProjectTask, CreateProjectTaskDTO>().ReverseMap();
        CreateMap<ProjectTask, UpdateProjectTaskDTO>().ReverseMap();

        // ProjectPlanning
        CreateMap<ProjectPlanning, ProjectPlanningDTO>().ReverseMap();
        CreateMap<ProjectPlanning, CreateProjectPlanningDTO>().ReverseMap();
        CreateMap<ProjectPlanning, UpdateProjectPlanningDTO>().ReverseMap();

        // ProjectPlanningTasks
        CreateMap<ProjectPlanningTasks, ProjectPlanningTasksDTO>().ReverseMap();
        CreateMap<ProjectPlanningTasks, CreateProjectPlanningTasksDTO>().ReverseMap();
        CreateMap<ProjectPlanningTasks, UpdateProjectPlanningTasksDTO>().ReverseMap();

        // Resources
        CreateMap<Resources, ResourcesDTO>().ReverseMap();
        CreateMap<Resources, CreateResourcesDTO>().ReverseMap();
        CreateMap<Resources, UpdateResourcesDTO>().ReverseMap();

        // Technology
        CreateMap<Technology, TechnologyDTO>().ReverseMap();
        CreateMap<Technology, CreateTechnologyDTO>().ReverseMap();
        CreateMap<Technology, UpdateTechnologyDTO>().ReverseMap();

        // ProjectTaskType
        CreateMap<ProjectTaskType, ProjectTaskTypeDTO>().ReverseMap();
        CreateMap<ProjectTaskType, CreateProjectTaskTypeDTO>().ReverseMap();
        CreateMap<ProjectTaskType, UpdateProjectTaskTypeDTO>().ReverseMap();

        // ProjectResources
        CreateMap<ProjectResources, ProjectResourcesDTO>().ReverseMap();
        CreateMap<ProjectResources, CreateProjectResourcesDTO>().ReverseMap();
        CreateMap<ProjectResources, UpdateProjectResourcesDTO>().ReverseMap();

        // TaskComments
        CreateMap<TaskComments, TaskCommentsDTO>().ReverseMap();
        CreateMap<TaskComments, CreateTaskCommentsDTO>().ReverseMap();
        CreateMap<TaskComments, UpdateTaskCommentsDTO>().ReverseMap();

        // ProjectTaskStatusHistory
        CreateMap<ProjectTaskStatusHistory, ProjectTaskStatusHistoryDTO>().ReverseMap();
        CreateMap<ProjectTaskStatusHistory, CreateProjectTaskStatusHistoryDTO>().ReverseMap();
        CreateMap<ProjectTaskStatusHistory, UpdateProjectTaskStatusHistoryDTO>().ReverseMap();

        // ProjectTaskAttachments
        CreateMap<ProjectTaskAttachments, ProjectTaskAttachmentsDTO>().ReverseMap();
        CreateMap<ProjectTaskAttachments, CreateProjectTaskAttachmentsDTO>().ReverseMap();
        CreateMap<ProjectTaskAttachments, UpdateProjectTaskAttachmentsDTO>().ReverseMap();

        // ProjectTaskLinked
        CreateMap<ProjectTaskLinked, ProjectTaskLinkedDTO>().ReverseMap();
        CreateMap<ProjectTaskLinked, CreateProjectTaskLinkedDTO>().ReverseMap();
        CreateMap<ProjectTaskLinked, UpdateProjectTaskLinkedDTO>().ReverseMap();
    }
}
