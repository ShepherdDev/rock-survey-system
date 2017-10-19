using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 2, "1.6.3" )]
    class SetupPages : Migration
    {
        public override void Up()
        {
            //
            // Add Entity Types.
            //
            RockMigrationHelper.UpdateEntityType( "com.shepherdchurch.SurveySystem.Model.Survey",
                SystemGuid.EntityType.SURVEY, true, true );

            RockMigrationHelper.UpdateEntityType( "com.shepherdchurch.SurveySystem.Model.SurveyResult",
                SystemGuid.EntityType.SURVEY_RESULT, true, true );

            //
            // Add Block Types.
            //
            RockMigrationHelper.UpdateBlockType( "Survey Detail",
                "Displays the details for a survey.",
                "~/Plugins/com_shepherdchurch/SurveySystem/SurveyDetail.ascx",
                "Shepherd Church > Survey System", SystemGuid.BlockType.SURVEY_DETAIL );

            RockMigrationHelper.UpdateBlockType( "Survey Result List",
                "Lists survey results in the system.",
                "~/Plugins/com_shepherdchurch/SurveySystem/SurveyResultList.ascx",
                "Shepherd Church > Survey System", SystemGuid.BlockType.SURVEY_RESULT_LIST );

            RockMigrationHelper.UpdateBlockType( "Survey Result Detail",
                "Displays the details for a survey result.",
                "~/Plugins/com_shepherdchurch/SurveySystem/SurveyResultDetail.ascx",
                "Shepherd Church > Survey System", SystemGuid.BlockType.SURVEY_RESULT_DETAIL );

            RockMigrationHelper.UpdateBlockType( "Survey Entry",
                "Displays a survey for the user to enter results into.",
                "~/Plugins/com_shepherdchurch/SurveySystem/SurveyEntry.ascx",
                "Shepherd Church > Survey System", SystemGuid.BlockType.SURVEY_ENTRY );

            //
            // Add Block Type Attributes.
            //
            RockMigrationHelper.UpdateBlockTypeAttribute( SystemGuid.BlockType.SURVEY_DETAIL,
                Rock.SystemGuid.FieldType.PAGE_REFERENCE, "Results Page", "ResultsPage",
                string.Empty, "The page that is used to list results for this survey.",
                0, string.Empty, SystemGuid.Attribute.SURVEY_DETAIL_RESULTS_PAGE );

            RockMigrationHelper.UpdateBlockTypeAttribute( SystemGuid.BlockType.SURVEY_RESULT_LIST,
                Rock.SystemGuid.FieldType.PAGE_REFERENCE, "Detail Page", "DetailPage",
                string.Empty, "The page that allows the user to view the details of a result.",
                0, string.Empty, SystemGuid.Attribute.SURVEY_RESULT_LIST_DETAIL_PAGE );

            //
            // Add Pages.
            //
            RockMigrationHelper.AddPage( SystemGuid.Page.ROCK_PLUGINS, SystemGuid.Layout.ROCK_LEFT_SIDEBAR,
                "Surveys", string.Empty, SystemGuid.Page.SURVEYS, "fa fa-question-circle-o" );

            RockMigrationHelper.AddPage( SystemGuid.Page.SURVEYS, SystemGuid.Layout.ROCK_FULL_WIDTH,
                "Survey Results", string.Empty, SystemGuid.Page.SURVEY_RESULTS );

            RockMigrationHelper.AddPage( SystemGuid.Page.SURVEY_RESULT_DETAILS, SystemGuid.Layout.ROCK_FULL_WIDTH,
                "Survey Result Details", string.Empty, SystemGuid.Page.SURVEY_RESULT_DETAILS );

            RockMigrationHelper.AddPage( SystemGuid.Page.SURVEYS, SystemGuid.Layout.ROCK_FULL_WIDTH,
                "Survey Entry", string.Empty, SystemGuid.Page.SURVEY_ENTRY );
            RockMigrationHelper.AddPageRoute( SystemGuid.Page.SURVEY_ENTRY, "SurveyEntry/{SurveyId}" );

            //
            // Add Blocks to Pages.
            //
            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEYS, string.Empty,
                SystemGuid.BlockType.ROCK_CATEGORY_TREE_VIEW, "Category Tree View",
                "Sidebar1", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW );

            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEYS, string.Empty,
                SystemGuid.BlockType.ROCK_CATEGORY_DETAIL, "Category Detail",
                "Main", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEYS_CATEGORY_DETAIL );

            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEYS, string.Empty,
                SystemGuid.BlockType.SURVEY_DETAIL, "Survey Detail",
                "Main", string.Empty, string.Empty, 1, SystemGuid.Block.SURVEYS_SURVEY_DETAIL );

            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEY_RESULTS, string.Empty,
                SystemGuid.BlockType.SURVEY_RESULT_LIST, "Survey Result List",
                "Main", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEY_RESULTS_SURVEY_RESULT_LIST );

            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEY_RESULT_DETAILS, string.Empty,
                SystemGuid.BlockType.SURVEY_RESULT_DETAIL, "Survey Result Detail",
                "Main", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEY_RESULT_DETAILS_SURVEY_RESULT_DETAIL );

            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEY_ENTRY, string.Empty,
                SystemGuid.BlockType.SURVEY_ENTRY, "Survey Entry",
                "Main", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEY_ENTRY_SURVEY_ENTRY );

            //
            // Add Block Attributes.
            //
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW,
                "D2596ADF-4455-42A4-848F-6DFD816C2867", "fa fa-list-ol" ); // Default Icon CSS Class
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW,
                "AEE521D8-124D-4BB3-8A80-5F368E5CEC15", SystemGuid.Page.SURVEYS ); // Detail Page
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW,
                "06D414F0-AA20-4D3C-B297-1530CCD64395", SystemGuid.EntityType.SURVEY ); // Entity Type
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW,
                "AA057D3E-00CC-42BD-9998-600873356EDB", "SurveyId" ); // Page Parameter Key

            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_CATEGORY_DETAIL,
                "FF3A33CF-8897-4FC6-9C16-64FA25E6C297", SystemGuid.EntityType.SURVEY ); // Entity Type

            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEYS_SURVEY_DETAIL,
                SystemGuid.Attribute.SURVEY_DETAIL_RESULTS_PAGE, SystemGuid.Page.SURVEY_RESULTS );

            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEY_RESULTS_SURVEY_RESULT_LIST,
                SystemGuid.Attribute.SURVEY_RESULT_LIST_DETAIL_PAGE, SystemGuid.Page.SURVEY_RESULT_DETAILS );
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEY_ENTRY_SURVEY_ENTRY );
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEY_RESULT_DETAILS_SURVEY_RESULT_DETAIL );
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEY_RESULTS_SURVEY_RESULT_LIST );
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEYS_SURVEY_DETAIL );
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEYS_CATEGORY_DETAIL );
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEYS_CATEGORY_TREE_VIEW );

            RockMigrationHelper.DeletePage( SystemGuid.Page.SURVEY_ENTRY );
            RockMigrationHelper.DeletePage( SystemGuid.Page.SURVEY_RESULT_DETAILS );
            RockMigrationHelper.DeletePage( SystemGuid.Page.SURVEY_RESULTS );
            RockMigrationHelper.DeletePage( SystemGuid.Page.SURVEYS );
        }
    }
}
