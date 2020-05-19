using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;

using com.shepherdchurch.SurveySystem.Model;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_shepherdchurch.SurveySystem
{
    [DisplayName( "Survey Result Charts" )]
    [Category( "Shepherd Church > Survey System" )]
    [Description( "Shows survey results with charts." )]

    [IntegerField(
        name: "Max Values Per Question",
        Description = "The maximum number of values to display for a single question.",
        IsRequired = true,
        DefaultIntegerValue = 20,
        Key = "MaxValuesPerQuestion",
        Order = 0 )]
    public partial class SurveyResultCharts : RockBlock
    {
        private readonly int[] _customValueFieldTypeIds = new[]
        {
            FieldTypeCache.GetId( Rock.SystemGuid.FieldType.MULTI_SELECT.AsGuid() ).Value,
            FieldTypeCache.GetId( Rock.SystemGuid.FieldType.SINGLE_SELECT.AsGuid() ).Value
        };

        #region Base Method Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddScriptLink( "~/Scripts/Chartjs/Chart.min.js", false );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Initialize basic information about the page structure and setup the default content.
        /// </summary>
        /// <param name="sender">Object that is generating this event.</param>
        /// <param name="e">Arguments that describe this event.</param>
        protected void Page_Load( object sender, EventArgs e )
        {
            if ( !IsPostBack )
            {
                ShowDetails();
            }
        }

        #endregion

        #region Core Methods

        /// <summary>
        /// Show the details of the survey results.
        /// </summary>
        private void ShowDetails()
        {
            var rockContext = new RockContext();
            var surveyResultService = new SurveyResultService( rockContext );
            int surveyId = PageParameter( "SurveyId" ).AsInteger();

            var survey = new SurveyService( rockContext ).Get( surveyId );

            if ( survey == null || !survey.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
            {
                nbUnauthorizedMessage.Text = EditModeMessage.NotAuthorizedToView( Survey.FriendlyTypeName );
                pnlResults.Visible = false;

                return;
            }

            ltTitle.Text = string.Format( "{0} Results", survey.Name );

            var qry = surveyResultService.Queryable()
                .Where( r => r.SurveyId == surveyId );

            //
            // Date Completed Range Filter.
            //
            if ( drpDateCompleted.LowerValue.HasValue )
            {
                qry = qry.Where( p => p.CreatedDateTime >= drpDateCompleted.LowerValue.Value );
            }
            if ( drpDateCompleted.UpperValue.HasValue )
            {
                DateTime upperDate = drpDateCompleted.UpperValue.Value.Date.AddDays( 1 );
                qry = qry.Where( p => p.CreatedDateTime < upperDate );
            }

            //
            // Store the queried objects in the grid for it to use later.
            //
            var results = qry.ToList();
            results.LoadAttributes( rockContext );

            var tempResult = new SurveyResult
            {
                SurveyId = surveyId
            };
            tempResult.LoadAttributes( rockContext );
            var attributes = tempResult.Attributes
                .Values
                .OrderBy( a => a.Order )
                .ThenBy( a => a.Key )
                .ToList();

            var answers = new Dictionary<string, string>();
            if ( survey.PassingGrade.HasValue )
            {
                answers = survey.AnswerData.FromJsonOrNull<Dictionary<string, string>>() ?? new Dictionary<string, string>();
            }

            var data = new List<object>();
            foreach ( var attribute in attributes )
            {
                var answer = answers.GetValueOrDefault( attribute.Key, string.Empty );

                var resultAnswers = results.SelectMany( a => GetDisplayValues( a, attribute ) );

                var values = resultAnswers.GroupBy( a => a )
                    .ToDictionary( a => a.Key, a => a.Count() );

                double? passRate = null;

                if ( survey.PassingGrade.HasValue && results.Count > 0 )
                {
                    passRate = resultAnswers.Count( a => a == answer ) / ( double ) results.Count;
                }

                data.Add( new
                {
                    title = attribute.Name,
                    values,
                    passRate
                } );
            }

            hfData.Value = data.ToJson();
            hfMaxValues.Value = GetAttributeValue( "MaxValuesPerQuestion" );
        }

        /// <summary>
        /// Gets the display values.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        private IEnumerable<string> GetDisplayValues( SurveyResult result, AttributeCache attribute )
        {
            var rawValue = result.GetAttributeValue( attribute.Key );

            if ( rawValue.IsNullOrWhiteSpace() )
            {
                return new[] { string.Empty };
            }

            if ( _customValueFieldTypeIds.Contains( attribute.FieldTypeId ) )
            {
                var configuredValues = Rock.Field.Helper.GetConfiguredValues( attribute.QualifierValues );
                var selectedValues = rawValue.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).ToList();
                return configuredValues
                    .Where( v => selectedValues.Contains( v.Key ) )
                    .Select( v => v.Value )
                    .ToList();
            }
            else
            {
                return new[] { attribute.FieldType.Field.FormatValue( this, rawValue, attribute.QualifierValues, false ) };
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            ShowDetails();
        }

        /// <summary>
        /// Handles the Click event of the lbApplyFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbApplyFilter_Click( object sender, EventArgs e )
        {
            ShowDetails();
        }

        #endregion
    }
}