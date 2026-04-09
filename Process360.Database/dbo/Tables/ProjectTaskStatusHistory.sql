CREATE TABLE [dbo].[ProjectTaskStatusHistory] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [ProjectTaskId] INT      NOT NULL,
    [OldStatusId]   INT      NULL,
    [NewStatusId]   INT      NULL,
    [CreatedBy]     INT      NULL,
    [CreatedDate]   DATETIME DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id])
);

