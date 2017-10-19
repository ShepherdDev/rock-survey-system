using System;
using System.Linq;

using Rock.Data;

namespace com.shepherdchurch.SurveySystem.Model
{
    /// <summary>
    /// Survey Result Service class.
    /// </summary>
    public class SurveyResultService : Service<SurveyResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyResultService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SurveyResultService( RockContext context ) : base( context ) { }
    }
}