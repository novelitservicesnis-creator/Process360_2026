CREATE TABLE [dbo].[ProjectPlanning] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [StartDate]   DATETIME       NULL,
    [EndDate]     DATETIME       NULL,
    [Name]        NVARCHAR (150) NULL,
    [Goal]        NVARCHAR (MAX) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

