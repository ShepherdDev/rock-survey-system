using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 10, "1.10.2" )]
    class AddSurveyCharts : Migration
    {
        public override void Up()
        {
            //
            // Add Block Types.
            //
            RockMigrationHelper.UpdateBlockType( "Survey Result Charts",
                "Shows survey results with charts.",
                "~/Plugins/com_shepherdchurch/SurveySystem/SurveyResultCharts.ascx",
                "Shepherd Church > Survey System", SystemGuid.BlockType.SURVEY_RESULT_CHARTS );

            //
            // Add Block Type Attributes.
            //
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( SystemGuid.BlockType.SURVEY_RESULT_LIST,
                Rock.SystemGuid.FieldType.PAGE_REFERENCE, "Chart Page", "ChartPage",
                string.Empty, "The page that allows the user to view the results with charts.",
                1, string.Empty, SystemGuid.Attribute.SURVEY_RESULT_LIST_CHARTS_PAGE );

            //
            // Add Pages.
            //
            RockMigrationHelper.AddPage( SystemGuid.Page.SURVEY_RESULTS, SystemGuid.Layout.ROCK_FULL_WIDTH,
                "Charts", string.Empty, SystemGuid.Page.SURVEY_RESULTS_CHARTS );

            //
            // Add Blocks to Pages.
            //
            RockMigrationHelper.AddBlock( SystemGuid.Page.SURVEY_RESULTS_CHARTS, string.Empty,
                SystemGuid.BlockType.SURVEY_RESULT_CHARTS, "Survey Result Charts",
                "Main", string.Empty, string.Empty, 0, SystemGuid.Block.SURVEY_RESULTS_CHARTS_SURVEY_RESULT_CHARTS );

            //
            // Add Block Attributes.
            //
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SURVEY_RESULTS_SURVEY_RESULT_LIST,
                SystemGuid.Attribute.SURVEY_RESULT_LIST_CHARTS_PAGE, SystemGuid.Page.SURVEY_RESULTS_CHARTS );
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SURVEY_RESULTS_CHARTS_SURVEY_RESULT_CHARTS );

            RockMigrationHelper.DeletePage( SystemGuid.Page.SURVEY_RESULTS_CHARTS );
        }
    }
}
