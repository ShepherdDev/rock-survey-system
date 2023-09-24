using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;
using Rock.Lava;
using Rock.Security;

namespace com.shepherdchurch.SurveySystem.Model
{
    [Table( "_com_shepherdchurch_SurveySystem_SurveyResult" )]
    [DataContract]
    public class SurveyResult : Model<SurveyResult>, IRockEntity
    {
        #region Entity Properties

        /// <summary>
        /// The identifier of the survey this result belongs to.
        /// </summary>
        [DataMember]
        public int SurveyId { get; set; }

        /// <summary>
        /// Provides the percentage of correct answers as a decimal value between 0 and 100.
        /// Set to Null if survey was not in test mode.
        /// </summary>
        [DataMember]
        public decimal? TestResult { get; set; }

        /// <summary>
        /// If true then the user passed the test. If Null then the Survey was not in test mode.
        /// </summary>
        [DataMember]
        public bool? DidPass { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Survey"/>.
        /// </summary>
        [LavaVisible]
        public virtual Survey Survey { get; set; }

        /// <summary>
        /// Gets the parent security authority for this Attachment instance.
        /// </summary>
        public override ISecured ParentAuthority
        {
            get
            {
                return Survey ?? base.ParentAuthority;
            }
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// Defines the Entity Framework configuration for the <see cref="SurveyResult"/>  model.
    /// </summary>
    public partial class SurveyResultConfiguration : EntityTypeConfiguration<SurveyResult>
    {
        public SurveyResultConfiguration()
        {
            this.HasRequired( r => r.Survey )
                .WithMany( s => s.SurveyResults )
                .HasForeignKey( s => s.SurveyId )
                .WillCascadeOnDelete( false );

            this.HasEntitySetName( "SurveyResult" );
        }
    }

    #endregion
}
