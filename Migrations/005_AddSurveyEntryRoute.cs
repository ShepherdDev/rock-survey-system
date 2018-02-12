using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    public class AddSurveyEntryRoute : Migration
    {
        public override void Up()
        {
            RockMigrationHelper.AddPageRoute( SystemGuid.Page.SURVEY_ENTRY, "surveyentry/{SurveyId}" );
        }

        public override void Down()
        {
        }
    }
}
