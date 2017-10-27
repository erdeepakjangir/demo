CREATE TABLE [dbo].[OptionMaster] (
    [OptionId]   INT          IDENTITY (1, 1) NOT NULL,
    [OptionType] VARCHAR (50) NULL,
    CONSTRAINT [PK_OptionMaster] PRIMARY KEY CLUSTERED ([OptionId] ASC)
);

