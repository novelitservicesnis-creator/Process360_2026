CREATE TABLE [dbo].[TaskComments] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ProjectTaskId] INT            NOT NULL,
    [Comments]      NVARCHAR (MAX) NULL,
    [Attachment]    NVARCHAR (500) NULL,
    [CreatedBy]     INT            NULL,
    [CreatedDate]   DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id])
);

