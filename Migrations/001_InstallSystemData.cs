using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 1, "1.6.3" )]
    class InstallSystemData : Migration
    {
        public override void Up()
        {
            //
            // Create the Survey table.
            //
            Sql( @"
CREATE TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
    [Guid] [uniqueidentifier] NOT NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
	[CreatedByPersonAliasId] [int] NULL,
	[ModifiedByPersonAliasId] [int] NULL,
	[ForeignKey] [nvarchar](50) NULL,
	[ForeignGuid] [uniqueidentifier] NULL,
	[ForeignId] [int] NULL,
    [PassingGrade] [decimal] NULL,
	[CategoryId] [int] NULL,
	[InstructionTemplate] [nvarchar](max) NULL,
	[ResultTemplate] [nvarchar](max) NULL,
    [LastAttemptDateAttributeId] [int] NULL,
    [LastPassedDateAttributeId] [int] NULL,
    [RecordAnswers] [bit] NULL,
	[AnswerData] [nvarchar](max) NULL,
	CONSTRAINT [PK_com_shepherdchurch_SurveySystem_Survey] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_CreatedByPersonAlias]
	FOREIGN KEY([CreatedByPersonAliasId])
	REFERENCES [dbo].[PersonAlias] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_CreatedByPersonAlias]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_ModifiedByPersonAlias]
	FOREIGN KEY([ModifiedByPersonAliasId])
	REFERENCES [dbo].[PersonAlias] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_ModifiedByPersonAlias]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_Category]
	FOREIGN KEY([CategoryId])
	REFERENCES [dbo].[Category] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_Category]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_LastAttemptDateAttribute]
	FOREIGN KEY([LastAttemptDateAttributeId])
	REFERENCES [dbo].[Attribute] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_LastAttemptDateAttribute]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_LastPassedDateAttribute]
	FOREIGN KEY([LastPassedDateAttributeId])
	REFERENCES [dbo].[Attribute] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_LastPassedDateAttribute]
" );

            //
            // Create the SurveyResult table.
            //
            Sql( @"
CREATE TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [Guid] [uniqueidentifier] NOT NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
	[CreatedByPersonAliasId] [int] NULL,
	[ModifiedByPersonAliasId] [int] NULL,
	[ForeignKey] [nvarchar](50) NULL,
	[ForeignGuid] [uniqueidentifier] NULL,
	[ForeignId] [int] NULL,
	[SurveyId] [int] NOT NULL,
    [TestResult] [decimal] NULL,
    [DidPass] [bit] NULL,
	CONSTRAINT [PK_com_shepherdchurch_SurveySystem_SurveyResult] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_CreatedByPersonAlias]
	FOREIGN KEY([CreatedByPersonAliasId])
	REFERENCES [dbo].[PersonAlias] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_CreatedByPersonAlias]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_ModifiedByPersonAlias]
	FOREIGN KEY([ModifiedByPersonAliasId])
	REFERENCES [dbo].[PersonAlias] ([Id])
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_ModifiedByPersonAlias]

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_Survey]
	FOREIGN KEY([SurveyId])
	REFERENCES [dbo].[_com_shepherdchurch_SurveySystem_Survey] ([Id])
	ON DELETE CASCADE
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult]
	CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_SurveyResult_Survey]
" );
        }

        public override void Down()
        {
            Sql( "DROP TABLE [dbo].[_com_shepherdchurch_SurveySystem_SurveyResult" );
            Sql( "DROP TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey" );
        }
    }
}
