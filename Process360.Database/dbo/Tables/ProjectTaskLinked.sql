CREATE TABLE [dbo].[ProjectTaskLinked] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [ProjectTaskId]        INT           NOT NULL,
    [RelatedProjectTaskId] INT           NOT NULL,
    [RelationType]         NVARCHAR (50) NULL,
    [CreatedBy]            INT           NULL,
    [CreatedDate]          DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([RelationType]='SubTask' OR [RelationType]='Linked' OR [RelationType]='Duplicate'),
    FOREIGN KEY ([ProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id]),
    FOREIGN KEY ([RelatedProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id])
);

