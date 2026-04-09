CREATE TABLE [dbo].[ProjectPlanningTasks] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [ProjectId]     INT      NULL,
    [ProjectTaskId] INT      NULL,
    [IsCompleted]   BIT      DEFAULT ((0)) NULL,
    [CreatedBy]     INT      NULL,
    [CreatedDate]   DATETIME DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    FOREIGN KEY ([ProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id])
);

