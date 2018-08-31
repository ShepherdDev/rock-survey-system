using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 8, "1.7.3" )]
    public class AddSurveyAttributeField : Migration
    {
        public override void Up()
        {
            RockMigrationHelper.UpdateFieldType( "Survey",
                "Selects a single survey.",
                "com.shepherdchurch.SurveySystem",
                typeof( Field.SurveyFieldType ).FullName,
                SystemGuid.FieldType.SURVEY );
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteFieldType( SystemGuid.FieldType.SURVEY );
        }
    }
}
