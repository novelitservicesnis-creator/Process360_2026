CREATE TABLE [dbo].[Technology] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Type]        NVARCHAR (50)  NULL,
    [IsActive]    BIT            DEFAULT ((1)) NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Type]='ServerSide' OR [Type]='FrontEnd' OR [Type]='Database')
);

