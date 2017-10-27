CREATE TABLE [dbo].[SurveyAnswers] (
    [SurveyAnsId]    INT              IDENTITY (1, 1) NOT NULL,
    [SurveyId]       UNIQUEIDENTIFIER NOT NULL,
    [QueId]          INT              NULL,
    [Selectedoption] INT              NULL,
    [EnteredText]    VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_SurveyAnswers] PRIMARY KEY CLUSTERED ([SurveyAnsId] ASC),
    CONSTRAINT [FK_SurveyAnswers_QuestionBank] FOREIGN KEY ([QueId]) REFERENCES [dbo].[QuestionBank] ([QueId]),
    CONSTRAINT [FK_SurveyAnswers_SurveyMaster] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[SurveyMaster] ([SurveyId])
);

