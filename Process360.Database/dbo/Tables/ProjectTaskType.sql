CREATE TABLE [dbo].[ProjectTaskType] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [CreatedBy]   INT           NULL,
    [CreatedDate] DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Name]='Feature' OR [Name]='Bug')
);

