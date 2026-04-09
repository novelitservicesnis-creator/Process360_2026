CREATE TABLE [dbo].[Resources] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Password]        NVARCHAR (255) NOT NULL,
    [Role]            NVARCHAR (50)  NULL,
    [Email]           NVARCHAR (150) NULL,
    [FirstName]       NVARCHAR (100) NULL,
    [LastName]        NVARCHAR (100) NULL,
    [DOB]             DATE           NULL,
    [Address]         NVARCHAR (255) NULL,
    [CurrentLocation] NVARCHAR (150) NULL,
    [Experience]      INT            NULL,
    [IsActive]        BIT            DEFAULT ((1)) NULL,
    [CreatedDate]     DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Role]='User' OR [Role]='Admin'),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

