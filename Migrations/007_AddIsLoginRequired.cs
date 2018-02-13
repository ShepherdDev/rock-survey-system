using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 7, "1.6.3" )]
    public class AddIsLoginRequired : Migration
    {
        public override void Up()
        {
            Sql( "ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey] ADD [IsLoginRequired] BIT NOT NULL CONSTRAINT [DF_com_shepherdchurch_SurveySystem_Survey_IsLoginRequired] DEFAULT 0 WITH VALUES" );
        }

        public override void Down()
        {
            Sql( "ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey] DROP CONSTRAINT [DF_com_shepherdchurch_SurveySystem_Survey_IsLoginRequired], COLUMN [IsLoginRequired]" );
        }
    }
}
