using Rock.Plugin;

namespace com.shepherdchurch.SurveySystem.Migrations
{
    [MigrationNumber( 12, "1.15.0" )]
    public class AddSurveyDataViews : Migration
    {
        public override void Up()
        {
            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	ADD [MustBeInDataViewId] [int] NULL

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustBeInDataViewId]
	FOREIGN KEY ([MustBeInDataViewId]) REFERENCES [dbo].[DataView] ([Id])

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustBeInDataViewId]
" );

            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	ADD [MustNotBeInDataViewId] [int] NULL

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] WITH CHECK
	ADD CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustNotBeInDataViewId]
	FOREIGN KEY ([MustNotBeInDataViewId]) REFERENCES [dbo].[DataView] ([Id])

ALTER TABLE [dbo].[_com_shepherdchurch_SurveySystem_Survey] CHECK CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustNotBeInDataViewId]
" );
        }

        public override void Down()
        {
            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustNotBeInDataViewId]

ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP COLUMN [MustNotBeInDataViewId]
" );

            Sql( @"
ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP CONSTRAINT [FK_com_shepherdchurch_SurveySystem_Survey_MustBeInDataViewId]

ALTER TABLE [_com_shepherdchurch_SurveySystem_Survey]
	DROP COLUMN [MustBeInDataViewId]
" );
        }
    }
}
