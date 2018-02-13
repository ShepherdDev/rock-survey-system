using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Newtonsoft.Json;
using Rock;
using Rock.Data;
using Rock.Lava;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;

using com.shepherdchurch.SurveySystem.Model;

namespace RockWeb.Plugins.com_shepherdchurch.SurveySystem
{
    [DisplayName( "Survey Entry" )]
    [Category( "Shepherd Church > Survey System" )]
    [Description( "Displays a survey for the user to enter results into." )]
    public partial class SurveyEntry : RockBlock
    {
        #region Base Method Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

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
                ShowDetail( PageParameter( "SurveyId" ).AsInteger() );
            }
            else
            {
                if ( pnlDetails.Visible )
                {
                    var surveyResult = new SurveyResult { Id = 0, SurveyId = PageParameter( "SurveyId" ).AsInteger() };

                    surveyResult.LoadAttributes();

                    Rock.Attribute.Helper.AddEditControls( surveyResult, phAttributes, false, BlockValidationGroup );
                }
            }
        }

        #endregion

        #region Core Methods

        /// <summary>
        /// Show the details of the given survey for the user to enter information into.
        /// </summary>
        /// <param name="surveyId">The identifier of the survey for this user to take.</param>
        protected void ShowDetail( int surveyId )
        {
            var rockContext = new RockContext();
            var survey = new SurveyService( rockContext ).Get( surveyId );

            //
            // Ensure the user is allowed to view this survey.
            //
            if ( survey == null || !survey.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
            {
                nbUnauthorized.Text = "The survey was not found or has expired.";
                pnlDetails.Visible = false;

                return;
            }

            //
            // Check if an active login is required for this survey.
            //
            if ( survey.IsLoginRequired && CurrentUser == null )
            {
                var site = RockPage.Site;

                if ( site.LoginPageId.HasValue )
                {
                    site.RedirectToLoginPage( true );
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                return;
            }

            pnlDetails.Visible = true;

            //
            // Display instructions.
            //
            if ( !string.IsNullOrWhiteSpace( survey.InstructionTemplate ) )
            {
                var mergeFields = LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );

                mergeFields.Add( "Survey", survey );

                lInstructions.Text = survey.InstructionTemplate.ResolveMergeFields( mergeFields );
            }

            //
            // Show all the attributes (questions) in this survey.
            //
            var surveyResult = new SurveyResult { Id = 0, SurveyId = survey.Id };

            surveyResult.LoadAttributes( rockContext );

            Rock.Attribute.Helper.AddEditControls( surveyResult, phAttributes, true, BlockValidationGroup );
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            int surveyId = PageParameter( "SurveyId" ).AsInteger();

            if ( surveyId != 0 )
            {
                ShowDetail( surveyId );
            }
        }

        /// <summary>
        /// Handles the Submit event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSubmit_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var survey = new SurveyService( rockContext ).Get( PageParameter( "SurveyId" ).AsInteger() );
            var surveyResultService = new SurveyResultService( rockContext );
            var surveyResult = new SurveyResult { Id = 0, SurveyId = survey.Id };
            var mergeFields = LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
            var person = CurrentPerson != null ? new PersonService( rockContext ).Get( CurrentPerson.Id ) : null;
            var correctAnswers = new List<string>();
            var incorrectAnswers = new List<string>();

            pnlDetails.Visible = false;
            pnlResults.Visible = true;

            if ( person != null )
            {
                person.LoadAttributes( rockContext );
            }

            //
            // Get the user's answers.
            //
            surveyResult.CreatedByPersonAliasId = CurrentPersonAliasId;
            surveyResult.LoadAttributes( rockContext );
            Rock.Attribute.Helper.GetEditValues( phAttributes, surveyResult );

            //
            // Setup the merge fields.
            //
            mergeFields.Add( "Survey", survey );
            mergeFields.Add( "Result", surveyResult );
            mergeFields.Add( "CorrectAnswers", correctAnswers );
            mergeFields.Add( "IncorrectAnswers", incorrectAnswers );
            mergeFields.Add( "Answers", surveyResult.AttributeValues.ToDictionary( v => v.Key, v => v.Value.Value ) );

            //
            // If this is a test-mode survey, process their answers.
            //
            if ( survey.PassingGrade.HasValue )
            {
                Dictionary<string, string> answers;

                //
                // Get all the correct answers.
                //
                try
                {
                    answers = JsonConvert.DeserializeObject<Dictionary<string, string>>( survey.AnswerData );
                }
                catch
                {
                    answers = new Dictionary<string, string>();
                }

                //
                // Calculate the number of correct answers.
                //
                foreach ( var value in surveyResult.AttributeValues )
                {
                    if ( answers.ContainsKey( value.Key ) && string.Equals( value.Value.Value, answers[value.Key], StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        correctAnswers.Add( value.Key );
                    }
                    else
                    {
                            incorrectAnswers.Add( value.Key );
                    }
                }

                //
                // Determine their grade and if they passed or not.
                //
                surveyResult.TestResult = ( decimal ) correctAnswers.Count / surveyResult.AttributeValues.Count * 100;
                surveyResult.DidPass = surveyResult.TestResult >= survey.PassingGrade;

                //
                // If they passed set the last passed attribute date and display the pass template.
                //
                if ( surveyResult.DidPass.Value && survey.LastPassedDateAttributeId.HasValue && person != null )
                {
                    var attribute = AttributeCache.Read( survey.LastPassedDateAttributeId.Value );

                    if ( attribute != null )
                    {
                        person.SetAttributeValue( attribute.Key, RockDateTime.Now.ToString() );
                    }
                }
            }

            lResults.Text = survey.ResultTemplate.ResolveMergeFields( mergeFields );

            //
            // If we supposed to record the last attempt date, do so.
            //
            if ( survey.LastAttemptDateAttributeId.HasValue && person != null )
            {
                var attribute = AttributeCache.Read( survey.LastAttemptDateAttributeId.Value );

                if ( attribute != null )
                {
                    person.SetAttributeValue( attribute.Key, RockDateTime.Now.ToString() );
                }
            }

            rockContext.WrapTransaction( () =>
            {
                //
                // If this survey records answers, then mark the .
                //
                if ( survey.RecordAnswers )
                {
                    surveyResultService.Add( surveyResult );
                }

                rockContext.SaveChanges();

                person.SaveAttributeValues( rockContext );

                if ( survey.RecordAnswers )
                {
                    surveyResult.SaveAttributeValues( rockContext );
                }
            } );
        }

        #endregion
    }
}