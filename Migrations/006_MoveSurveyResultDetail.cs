using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber(6, "1.6.3")]
    public class MoveSurveyResultDetails : Migration
    {
        public override void Up()
        {
            RockMigrationHelper.MovePage( SystemGuid.Page.SURVEY_RESULT_DETAILS, SystemGuid.Page.SURVEY_RESULTS );
        }

        public override void Down()
        {
            RockMigrationHelper.MovePage( SystemGuid.Page.SURVEY_RESULT_DETAILS, SystemGuid.Page.SURVEY_RESULT_DETAILS );
        }
    }
}
