CREATE TABLE [dbo].[SurveyDetails] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [SurveyId]     UNIQUEIDENTIFIER NULL,
    [QueId]        INT              NULL,
    [QueWeightage] INT              NULL,
    CONSTRAINT [PK_SurveyDetailsMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyDetails_SurveyMaster] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[SurveyMaster] ([SurveyId]),
    CONSTRAINT [FK_SurveyDetailsMaster_QuestionBank] FOREIGN KEY ([QueId]) REFERENCES [dbo].[QuestionBank] ([QueId])
);

