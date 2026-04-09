using Microsoft.EntityFrameworkCore;
using Process360.Core.Models;

namespace Process360.Core;

public class ProcessDbContext : DbContext
{
    private readonly string _connectionString;

    public ProcessDbContext() { }
    public ProcessDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public ProcessDbContext(DbContextOptions options) : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Resources> Resources { get; set; } = null!;
    public DbSet<ProjectTaskType> ProjectTaskTypes { get; set; } = null!;
    public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;
    public DbSet<ProjectTaskAttachments> ProjectTaskAttachments { get; set; } = null!;
    public DbSet<Technology> Technologies { get; set; } = null!;
    public DbSet<ProjectPlanning> ProjectPlannings { get; set; } = null!;
    public DbSet<ProjectPlanningTasks> ProjectPlanningTasks { get; set; } = null!;
    public DbSet<ProjectResources> ProjectResources { get; set; } = null!;
    public DbSet<TaskComments> TaskComments { get; set; } = null!;
    public DbSet<ProjectTaskLinked> ProjectTasksLinked { get; set; } = null!;
    public DbSet<ProjectTaskStatusHistory> ProjectTaskStatusHistories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships and constraints
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerID);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(pt => pt.ProjectTaskType)
            .WithMany(ptt => ptt.ProjectTasks)
            .HasForeignKey(pt => pt.ProjectTaskTypeId);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(pt => pt.AssignedResource)
            .WithMany(r => r.AssignedTasks)
            .HasForeignKey(pt => pt.AssignTo)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(pt => pt.ReportedByResource)
            .WithMany(r => r.ReportedTasks)
            .HasForeignKey(pt => pt.ReportedBy)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProjectResources>()
            .HasOne(pr => pr.Resource)
            .WithMany(r => r.ProjectResources)
            .HasForeignKey(pr => pr.ResourceId);

        modelBuilder.Entity<ProjectResources>()
            .HasOne(pr => pr.Project)
            .WithMany(p => p.ProjectResources)
            .HasForeignKey(pr => pr.ProjectId);

        modelBuilder.Entity<ProjectTaskAttachments>()
            .HasOne(pta => pta.ProjectTask)
            .WithMany(pt => pt.ProjectTaskAttachments)
            .HasForeignKey(pta => pta.ProjectTaskId);

        modelBuilder.Entity<TaskComments>()
            .HasOne(tc => tc.ProjectTask)
            .WithMany(pt => pt.TaskComments)
            .HasForeignKey(tc => tc.ProjectTaskId);

        modelBuilder.Entity<ProjectTaskLinked>()
            .HasOne(ptl => ptl.ProjectTask)
            .WithMany(pt => pt.LinkedTasksFrom)
            .HasForeignKey(ptl => ptl.ProjectTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProjectTaskLinked>()
            .HasOne(ptl => ptl.RelatedProjectTask)
            .WithMany(pt => pt.LinkedTasksTo)
            .HasForeignKey(ptl => ptl.RelatedProjectTaskId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProjectTaskStatusHistory>()
            .HasOne(ptsh => ptsh.ProjectTask)
            .WithMany(pt => pt.StatusHistories)
            .HasForeignKey(ptsh => ptsh.ProjectTaskId);

        modelBuilder.Entity<ProjectPlanningTasks>()
            .HasOne(ppt => ppt.Project)
            .WithMany(p => p.ProjectPlanningTasks)
            .HasForeignKey(ppt => ppt.ProjectId);

        modelBuilder.Entity<ProjectPlanningTasks>()
            .HasOne(ppt => ppt.ProjectTask)
            .WithMany(pt => pt.ProjectPlanningTasks)
            .HasForeignKey(ppt => ppt.ProjectTaskId);
    }
}
