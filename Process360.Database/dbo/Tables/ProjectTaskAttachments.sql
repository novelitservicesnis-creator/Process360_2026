CREATE TABLE [dbo].[ProjectTaskAttachments] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ProjectTaskId] INT            NOT NULL,
    [FileName]      NVARCHAR (255) NULL,
    [FileUrl]       NVARCHAR (500) NULL,
    [FileType]      NVARCHAR (50)  NULL,
    [FileSize]      INT            NULL,
    [CreatedBy]     INT            NULL,
    [CreatedDate]   DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([FileType]='Other' OR [FileType]='Video' OR [FileType]='Doc' OR [FileType]='Image'),
    FOREIGN KEY ([ProjectTaskId]) REFERENCES [dbo].[ProjectTask] ([Id])
);

