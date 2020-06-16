using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.shepherdchurch.SurveySystem.Model;
using Rock;
using Rock.Data;
using Rock.Web.UI.Controls;

namespace com.shepherdchurch.SurveySystem.UI
{
    public class SurveySurveyResultPicker : CompositeControl, IRockControl
    {
        #region IRockControl implementation (Custom implementation)

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the form group class.
        /// </summary>
        /// <value>
        /// The form group class.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        Description( "The CSS class to add to the form-group div." )
        ]
        public string FormGroupCssClass
        {
            get { return ViewState["FormGroupCssClass"] as string ?? string.Empty; }
            set { ViewState["FormGroupCssClass"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }

            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the warning text.
        /// </summary>
        /// <value>
        /// The warning text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The warning block." )
        ]
        public string Warning
        {
            get
            {
                return WarningBlock != null ? WarningBlock.Text : string.Empty;
            }

            set
            {
                if ( WarningBlock != null )
                {
                    WarningBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get
            {
                EnsureChildControls();
                return _ddlResult.Required;
            }
            set
            {
                EnsureChildControls();
                _ddlResult.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }

            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets an optional validation group to use.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"] as string; }
            set { ViewState["ValidationGroup"] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the warning block.
        /// </summary>
        /// <value>
        /// The warning block.
        /// </value>
        public WarningBlock WarningBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        #endregion

        #region Controls

        private SurveyPicker _surveyPicker;
        private RockDropDownList _ddlResult;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the survey id.
        /// </summary>
        /// <value>
        /// The survey id.
        /// </value>
        public int? SurveyId
        {
            get
            {
                EnsureChildControls();
                return _surveyPicker.SelectedValue.AsIntegerOrNull();
            }

            set
            {
                EnsureChildControls();
                _surveyPicker.SetValue( value );

                if ( value.HasValue )
                {
                    LoadResults( value.Value );
                }
            }
        }

        /// <summary>
        /// Gets or sets the survey result identifier.
        /// </summary>
        /// <value>
        /// The survey result identifier.
        /// </value>
        public int? SurveyResultId
        {
            get
            {
                EnsureChildControls();
                return _ddlResult.SelectedValue.AsIntegerOrNull();
            }

            set
            {
                EnsureChildControls();
                int resultId = value ?? 0;

                if ( _ddlResult.SelectedValue != resultId.ToString() )
                {
                    if ( !SurveyId.HasValue || SurveyId.Value == 0 )
                    {
                        var result = new SurveyResultService( new RockContext() ).Get( resultId );
                        if ( result != null && _surveyPicker.SelectedValue != result.SurveyId.ToString() )
                        {
                            _surveyPicker.SetValue( result.SurveyId );

                            LoadResults( result.SurveyId );
                        }
                    }

                    _ddlResult.SelectedValue = resultId.ToString();
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTypeGroupPicker"/> class.
        /// </summary>
        public SurveySurveyResultPicker()
            : base()
        {
            HelpBlock = new HelpBlock();
            WarningBlock = new WarningBlock();
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            RockControlHelper.CreateChildControls( this, Controls );

            _surveyPicker = new SurveyPicker
            {
                ID = ID + "_surveyPicker",
                Label = "Survey"
            };
            _surveyPicker.ValueChanged += _surveyPicker_ValueChanged;
            Controls.Add( _surveyPicker );

            _ddlResult = new RockDropDownList
            {
                ID = ID + "_ddlResult",
                Label = "Result"
            };
            Controls.Add( _ddlResult );
        }

        /// <summary>
        /// Handles the ValueChanged event of the _surveyPicker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _surveyPicker_ValueChanged( object sender, EventArgs e )
        {
            int surveyId = _surveyPicker.SelectedValue.AsInteger();

            LoadResults( surveyId );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            _surveyPicker.RenderControl( writer );

            _ddlResult.Visible = SurveyId.HasValue;
            _ddlResult.RenderControl( writer );
        }

        /// <summary>
        /// Loads the groups.
        /// </summary>
        /// <param name="surveyId">The group type identifier.</param>
        private void LoadResults( int? surveyId )
        {
            int? currentResultId = SurveyResultId;

            _ddlResult.SelectedValue = null;
            _ddlResult.Items.Clear();
            
            if ( surveyId.HasValue )
            {
                _ddlResult.Items.Add( Rock.Constants.None.ListItem );

                var resultService = new SurveyResultService( new RockContext() );
                var results = resultService.Queryable().Where( r => r.SurveyId == surveyId.Value ).OrderByDescending( a => a.CreatedDateTime ).ToList();

                foreach ( var r in results )
                {
                    string name = $"#{r.Id}";

                    if ( r.CreatedDateTime.HasValue )
                    {
                        name = r.CreatedDateTime.ToString();
                    }

                    var item = new ListItem( name, r.Id.ToString().ToUpper() )
                    {
                        Selected = r.Id == currentResultId
                    };

                    _ddlResult.Items.Add( item );
                }
            }
        }
    }
}
