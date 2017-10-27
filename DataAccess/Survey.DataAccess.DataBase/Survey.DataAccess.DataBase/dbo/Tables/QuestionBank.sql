CREATE TABLE [dbo].[QuestionBank] (
    [QueId]        INT           IDENTITY (1, 1) NOT NULL,
    [QueText]      VARCHAR (MAX) NOT NULL,
    [OptionType]   INT           NULL,
    [CreatedDate]  DATETIME      NULL,
    [ModifiedDate] DATETIME      NULL,
    [CreatedBy]    DATETIME      NULL,
    CONSTRAINT [PK_QuestionBank] PRIMARY KEY CLUSTERED ([QueId] ASC),
    CONSTRAINT [FK_QuestionBank_OptionMaster] FOREIGN KEY ([OptionType]) REFERENCES [dbo].[OptionMaster] ([OptionId])
);

