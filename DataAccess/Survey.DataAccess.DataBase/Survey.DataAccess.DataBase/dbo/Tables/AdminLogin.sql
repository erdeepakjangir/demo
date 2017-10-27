CREATE TABLE [dbo].[AdminLogin] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [UserId] VARCHAR (50) NULL,
    CONSTRAINT [PK_AdminLogin] PRIMARY KEY CLUSTERED ([Id] ASC)
);

