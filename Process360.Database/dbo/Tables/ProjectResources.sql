CREATE TABLE [dbo].[ProjectResources] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ResourceId] INT           NOT NULL,
    [ProjectId]  INT           NOT NULL,
    [Role]       NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Role]='Other' OR [Role]='QA' OR [Role]='Developer' OR [Role]='Manager'),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[Resources] ([Id])
);

