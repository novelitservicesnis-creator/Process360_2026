CREATE TABLE [dbo].[Project] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [CustomerID]     INT             NOT NULL,
    [Code]           NVARCHAR (50)   NOT NULL,
    [Name]           NVARCHAR (150)  NOT NULL,
    [DatabaseSchema] NVARCHAR (100)  NULL,
    [GitProvider]    NVARCHAR (50)   NULL,
    [GitRepoUrl]     NVARCHAR (500)  NULL,
    [GitAccessToken] VARBINARY (MAX) NULL,
    [IsActive]       BIT             DEFAULT ((1)) NULL,
    [CreatedBy]      INT             NULL,
    [CreatedDate]    DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([Id]),
    UNIQUE NONCLUSTERED ([Code] ASC)
);

