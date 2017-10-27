CREATE TABLE [dbo].[SurveyType] (
    [TypeId]     INT          IDENTITY (1, 1) NOT NULL,
    [SurveyType] VARCHAR (50) NULL,
    CONSTRAINT [PK_SurveyMaster] PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

