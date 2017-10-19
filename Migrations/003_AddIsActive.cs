using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 3, "1.6.3" )]
    class AddIsActive : Migration
    {
        public override void Up()
        {
            Sql( @"
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] ADD [IsActive] [bit] NOT NULL
	CONSTRAINT [DF_com_shepherdchurch_SurveySystem_Survey_IsActive] DEFAULT(1)
" );
        }

        public override void Down()
        {
            Sql( @"
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] DROP CONSTRAINT [DF_com_shepherdchurch_SurveySystem_Survey_IsActive]
ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] DROP COLUMN [IsActive]
" );
        }
    }
}
