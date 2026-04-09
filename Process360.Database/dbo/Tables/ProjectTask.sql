CREATE TABLE [dbo].[ProjectTask] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ProjectTaskTypeId] INT             NULL,
    [SprintId]          INT             NULL,
    [Title]             NVARCHAR (255)  NOT NULL,
    [Description]       NVARCHAR (MAX)  NULL,
    [PriorityId]        INT             NULL,
    [StartDate]         DATETIME        NULL,
    [EndDate]           DATETIME        NULL,
    [TotalTimeLogged]   DECIMAL (10, 2) DEFAULT ((0)) NULL,
    [AssignTo]          INT             NULL,
    [ReportedBy]        INT             NULL,
    [CreatedBy]         INT             NULL,
    [CreatedDate]       DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([AssignTo]) REFERENCES [dbo].[Resources] ([Id]),
    FOREIGN KEY ([ProjectTaskTypeId]) REFERENCES [dbo].[ProjectTaskType] ([Id]),
    FOREIGN KEY ([ReportedBy]) REFERENCES [dbo].[Resources] ([Id])
);

