CREATE TABLE [dbo].[Customer] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Login]       NVARCHAR (100) NOT NULL,
    [Website]     NVARCHAR (255) NULL,
    [Email]       NVARCHAR (150) NULL,
    [Name]        NVARCHAR (150) NOT NULL,
    [Company]     NVARCHAR (150) NULL,
    [IsActive]    BIT            DEFAULT ((1)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

