using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 4, "1.6.3" )]
    class MoveSurveyEntryPage : Migration
    {
        public override void Up()
        {
            RockMigrationHelper.MovePage( SystemGuid.Page.SURVEY_ENTRY, Rock.SystemGuid.Page.SUPPORT_PAGES_INTERNAL_HOMEPAGE );
        }

        public override void Down()
        {
            RockMigrationHelper.MovePage( SystemGuid.Page.SURVEY_ENTRY, SystemGuid.Page.SURVEYS );
        }
    }
}
