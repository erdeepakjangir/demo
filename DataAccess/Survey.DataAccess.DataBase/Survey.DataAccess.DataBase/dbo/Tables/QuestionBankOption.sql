CREATE TABLE [dbo].[QuestionBankOption] (
    [AnswerId]      INT           IDENTITY (1, 1) NOT NULL,
    [QueId]         INT           NULL,
    [Answers]       VARCHAR (255) NULL,
    [CorrectAnswer] BIT           NULL,
    [CreatedDate]   DATETIME      NULL,
    [ModifiedDate]  DATETIME      NULL,
    CONSTRAINT [PK_QuestionBankOption] PRIMARY KEY CLUSTERED ([AnswerId] ASC),
    CONSTRAINT [FK_QuestionBankOption_QuestionBank] FOREIGN KEY ([QueId]) REFERENCES [dbo].[QuestionBank] ([QueId])
);

