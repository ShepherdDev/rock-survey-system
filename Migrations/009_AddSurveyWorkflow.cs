using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 9, "1.8.4" )]
    public class AddSurveyWorkflow : Migration
    {
        public override void Up()
        {
            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	ADD [WorkflowTypeId] [int] NULL

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_WorkflowTypeId]
	FOREIGN KEY ([WorkflowTypeId]) REFERENCES [dbo].[WorkflowType] ([Id])

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_WorkflowTypeId]
" );
        }

        public override void Down()
        {
            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_WorkflowTypeId]

ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP COLUMN [WorkflowTypeId]
" );
        }
    }
}
