using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Reporting;
using Rock.Web.UI.Controls;

namespace com.shepherdchurch.SurveySystem.Reporting.DataFilter.Person
{
    /// <summary>
    /// 
    /// </summary>
    [Description( "Filter people on whether they have taken a survey before." )]
    [Export( typeof( DataFilterComponent ) )]
    [ExportMetadata( "ComponentName", "Person Has Taken Survey" )]
    public class InGroupSimpleFilter : DataFilterComponent
    {
        #region Properties

        /// <summary>
        /// Gets the entity type that filter applies to.
        /// </summary>
        /// <value>
        /// The entity that filter applies to.
        /// </value>
        public override string AppliesToEntityType
        {
            get { return typeof( Rock.Model.Person ).FullName; }
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public override string Section
        {
            get { return "Shepherd Church > Survey System"; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <value>
        /// The title.
        /// </value>
        public override string GetTitle( Type entityType )
        {
            return "Has Taken Survey";
        }

        /// <summary>
        /// Formats the selection on the client-side.  When the filter is collapsed by the user, the Filterfield control
        /// will set the description of the filter to whatever is returned by this property.  If including script, the
        /// controls parent container can be referenced through a '$content' variable that is set by the control before 
        /// referencing this property.
        /// </summary>
        /// <value>
        /// The client format script.
        /// </value>
        public override string GetClientFormatSelection( Type entityType )
        {
            return @"
function () {
    var result = 'Has taken survey: ';

    result += ' ' + $('.js-survey-picker', $content).find(':selected').text();
    var dateRangeText = $('.js-slidingdaterange-text-value', $content).val();
    if (dateRangeText) {
        result += ', date range: ' + dateRangeText;
    }

    return result; 
}
";
        }

        /// <summary>
        /// Formats the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override string FormatSelection( Type entityType, string selection )
        {
            string result = "Has taken survey";
            var rockContext = new RockContext();
            var selectionValues = selection.Split( '|' );
            var survey = new Model.SurveyService( rockContext ).Get( selectionValues[0].AsGuid() );

            if ( survey != null )
            {
                result = string.Format( "Has taken survey: {0}", survey.Name );
            }

            if ( selectionValues.Length >= 2 && !string.IsNullOrWhiteSpace( selectionValues[1] ) )
            {
                var fakeSDRP = new SlidingDateRangePicker();
                fakeSDRP.DelimitedValues = selectionValues[1].Replace( ',', '|' );
                var drValue = SlidingDateRangePicker.FormatDelimitedValues( selectionValues[1].Replace( ',', '|' ) );

                if ( !string.IsNullOrWhiteSpace( drValue ) )
                {
                    result += string.Format( ", date range: {0}", drValue );
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the child controls.
        /// </summary>
        /// <returns></returns>
        public override Control[] CreateChildControls( Type entityType, FilterField filterControl )
        {
            var ddlSurvey = new RockDropDownList();
            ddlSurvey.ID = filterControl.ID + "_ddlSurvey";
            ddlSurvey.Label = "Survey";
            ddlSurvey.CssClass = "js-survey-picker";
            ddlSurvey.Required = true;
            filterControl.Controls.Add( ddlSurvey );

            var surveys = new Model.SurveyService( new RockContext() ).Queryable();
            ddlSurvey.Items.Add( new ListItem() );
            foreach ( var survey in surveys )
            {
                ddlSurvey.Items.Add( new ListItem( survey.Name, survey.Guid.ToString() ) );
            }

            var sDateRange = new SlidingDateRangePicker();
            sDateRange.ID = filterControl.ID + "_sDateRange";
            sDateRange.AddCssClass( "js-sliding-date-range" );
            sDateRange.Label = "Date Range";
            sDateRange.Help = "The date range the survey must have been taken. If not set then any time period is assumed.";
            filterControl.Controls.Add( sDateRange );

            return new Control[2] { ddlSurvey, sDateRange };
        }

        /// <summary>
        /// Renders the controls.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="filterControl">The filter control.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="controls">The controls.</param>
        public override void RenderControls( Type entityType, FilterField filterControl, HtmlTextWriter writer, Control[] controls )
        {
            base.RenderControls( entityType, filterControl, writer, controls );
        }

        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public override string GetSelection( Type entityType, Control[] controls )
        {
            if ( controls.Count() < 2 )
            {
                return null;
            }

            var ddlSurvey = controls[0] as RockDropDownList;
            var sDateRange = controls[1] as SlidingDateRangePicker;


            return string.Format( "{0}|{1}", ddlSurvey.SelectedValue, sDateRange.DelimitedValues.Replace( '|', ',' ) );
        }

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="controls">The controls.</param>
        /// <param name="selection">The selection.</param>
        public override void SetSelection( Type entityType, Control[] controls, string selection )
        {
            var selectionValues = selection.Split( '|' );

            if ( controls.Count() < 2 || selectionValues.Length < 2 )
            {
                return;
            }

            var ddlSurvey = controls[0] as RockDropDownList;
            var sDateRange = controls[1] as SlidingDateRangePicker;

            ddlSurvey.SetValue( selectionValues[0].AsGuidOrNull() );
            sDateRange.DelimitedValues = selectionValues[1].Replace( ',', '|' );
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override Expression GetExpression( Type entityType, IService serviceInstance, ParameterExpression parameterExpression, string selection )
        {
            var rockContext = ( RockContext ) serviceInstance.Context;
            var selectionValues = selection.Split( '|' );

            var surveyService = new Model.SurveyService( rockContext );
            var surveyResultService = new Model.SurveyResultService( rockContext );
            var surveyGuid = selectionValues[0].AsGuid();
            var surveyId = surveyService.Queryable().Where( s => s.Guid == surveyGuid ).Select( s => s.Id ).FirstOrDefault();
            var surveyResultQry = surveyResultService.Queryable().Where( xx => xx.SurveyId == surveyId );

            if ( selectionValues.Length >= 2 && !string.IsNullOrWhiteSpace( selectionValues[1] ) )
            {
                var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( selectionValues[1].Replace( ',', '|' ) );

                if ( dateRange.Start.HasValue )
                {
                    surveyResultQry = surveyResultQry.Where( xx => xx.CreatedDateTime >= dateRange.Start.Value );
                }

                if ( dateRange.End.HasValue )
                {
                    surveyResultQry = surveyResultQry.Where( xx => xx.CreatedDateTime < dateRange.End.Value );
                }
            }

            var qry = new PersonService( ( RockContext ) serviceInstance.Context ).Queryable()
                .Where( p => surveyResultQry.Any( xx => xx.CreatedByPersonAlias.PersonId == p.Id ) );

            Expression extractedFilterExpression = FilterExpressionExtractor.Extract<Rock.Model.Person>( qry, parameterExpression, "p" );

            return extractedFilterExpression;
        }

        #endregion
    }
}