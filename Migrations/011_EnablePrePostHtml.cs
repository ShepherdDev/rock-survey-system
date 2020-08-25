using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 11, "1.10.2" )]
    public class EnablePrePostHtml : Migration
    {
        public override void Up()
        {
            Sql( $"UPDATE [EntityType] SET [AttributesSupportPrePostHtml] = 1 WHERE [Guid] = '{SystemGuid.EntityType.SURVEY_RESULT}'" );
        }

        public override void Down()
        {
            Sql( $"UPDATE [EntityType] SET [AttributesSupportPrePostHtml] = 0 WHERE [Guid] = '{SystemGuid.EntityType.SURVEY_RESULT}'" );
        }
    }
}
