CREATE TABLE [dbo].[SurveyMaster] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [SurveyId]     UNIQUEIDENTIFIER NOT NULL,
    [SurveyTitle]  VARCHAR (255)    NULL,
    [SurveyType]   INT              NULL,
    [IsUnique]     BIT              CONSTRAINT [DF_SurveyMaster_IsUnique] DEFAULT ((1)) NOT NULL,
    [Status]       BIT              CONSTRAINT [DF_SurveyData_Status] DEFAULT ((0)) NOT NULL,
    [StartDate]    DATETIME         NULL,
    [EndDate]      DATETIME         NULL,
    [ModifiedDate] DATETIME         NULL,
    [CreatedDate]  DATETIME         NULL,
    [CraetedBy]    VARCHAR (50)     NULL,
    CONSTRAINT [PK_SurveyData] PRIMARY KEY CLUSTERED ([SurveyId] ASC),
    CONSTRAINT [FK_SurveyData_SurveyType] FOREIGN KEY ([SurveyType]) REFERENCES [dbo].[SurveyType] ([TypeId])
);

