using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Rock.Data;
using Rock.Model;
using Rock.Security;

namespace com.shepherdchurch.SurveySystem.Model
{
    [Table( "_com_shepherdchurch_SurveySystem_Survey" )]
    [DataContract]
    public class Survey : Model<Survey>, IRockEntity, ICategorized
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the name of this survey.
        /// </summary>
        [MaxLength( 100 )]
        [Required( ErrorMessage = "Name is required" )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this survey.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the passing grade percentage. A null value means treat it as a survey and
        /// not a test. A value of 100 means that all questions must be correct to pass.
        /// </summary>
        [DataMember]
        public decimal? PassingGrade { get; set; }

        /// <summary>
        /// The identifier of the category this survey belongs to.
        /// </summary>
        [DataMember]
        public int? CategoryId { get; set; }

        /// <summary>
        /// The instruction Lava template to include at the start of the survey.
        /// </summary>
        [DataMember]
        public string InstructionTemplate { get; set; }

        /// <summary>
        /// The Lava template to display to the user when they have completed the survey.
        /// </summary>
        [DataMember]
        public string ResultTemplate { get; set; }

        /// <summary>
        /// Person Attribute to record the last attempt date into.
        /// </summary>
        [DataMember]
        public int? LastAttemptDateAttributeId { get; set; }

        /// <summary>
        /// Person Attribute to record the last passed date into.
        /// </summary>
        [DataMember]
        public int? LastPassedDateAttributeId { get; set; }

        /// <summary>
        /// If true, then the user's answers will be saved to the database in a SurveyResult record.
        /// </summary>
        [DataMember]
        public bool RecordAnswers { get; set; }

        /// <summary>
        /// JSON encoded string containing all the answer values.
        /// </summary>
        [DataMember]
        public string AnswerData { get; set; }

        /// <summary>
        /// If false then the survey is no longer active and should not be available for entry.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Category"/>.
        /// </summary>
        [LavaInclude]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets a collection containing the <see cref="SurveyResults">SurveyResults</see> that belong to this survey.
        /// </summary>
        [DataMember]
        public virtual ICollection<SurveyResult> SurveyResults
        {
            get { return _surveyResults ?? ( _surveyResults = new Collection<SurveyResult>() ); }
            set { _surveyResults = value; }
        }
        private ICollection<SurveyResult> _surveyResults;

        /// <summary>
        /// Gets or the Answers dictionary, can be used by Lava to display correct answer information.
        /// </summary>
        [LavaInclude]
        public virtual IDictionary<string, string> Answers
        {
            get
            {
                try
                {
                    IDictionary<string, string> answers = null;

                    answers = JsonConvert.DeserializeObject<Dictionary<string, string>>( AnswerData );
                    if ( answers == null )
                    {
                        answers = new Dictionary<string, string>();
                    }

                    return answers;
                }
                catch
                {
                    return new Dictionary<string, string>();
                }
            }
        }

        /// <summary>
        /// Gets the parent security authority for this Attachment instance.
        /// </summary>
        public override ISecured ParentAuthority
        {
            get
            {
                return Category ?? base.ParentAuthority;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies the properties from another Survey object to this Survey object.
        /// </summary>
        /// <param name="source">The source object to copy from.</param>
        public void CopyPropertiesFrom( Survey source )
        {
            this.Id = source.Id;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.Name = source.Name;
            this.Description = source.Description;
            this.PassingGrade = source.PassingGrade;
            this.CategoryId = source.CategoryId;
            this.InstructionTemplate = source.InstructionTemplate;
            this.ResultTemplate = source.ResultTemplate;
            this.LastAttemptDateAttributeId = source.LastAttemptDateAttributeId;
            this.LastPassedDateAttributeId = source.LastPassedDateAttributeId;
            this.RecordAnswers = source.RecordAnswers;
            this.AnswerData = source.AnswerData;
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// Defines the Entity Framework configuration for the <see cref="Survey"/>  model.
    /// </summary>
    public partial class SurveyConfiguration : EntityTypeConfiguration<Survey>
    {
        public SurveyConfiguration()
        {
            this.HasOptional( s => s.Category )
                .WithMany()
                .HasForeignKey( s => s.CategoryId )
                .WillCascadeOnDelete( false );

            this.HasEntitySetName( "Survey" );
        }
    }

    #endregion
}
