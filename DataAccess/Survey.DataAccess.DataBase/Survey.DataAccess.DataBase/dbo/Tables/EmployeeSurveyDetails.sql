CREATE TABLE [dbo].[EmployeeSurveyDetails] (
    [EmpSurveyId]      INT              IDENTITY (1, 1) NOT NULL,
    [SurveyId]         UNIQUEIDENTIFIER NULL,
    [UserId]           VARCHAR (50)     NULL,
    [EmpSurveyLink]    VARCHAR (50)     NULL,
    [Status]           BIT              CONSTRAINT [DF_EmployeeSurveyDetails_Status] DEFAULT ((0)) NOT NULL,
    [SurveySubmitDate] DATETIME         NULL,
    CONSTRAINT [PK_EmployeeSurveyDetails] PRIMARY KEY CLUSTERED ([EmpSurveyId] ASC),
    CONSTRAINT [FK_EmployeeSurveyDetails_SurveyMaster] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[SurveyMaster] ([SurveyId])
);

