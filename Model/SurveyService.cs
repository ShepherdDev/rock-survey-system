using System;
using System.Linq;

using Rock.Data;

namespace com.shepherdchurch.SurveySystem.Model
{
    /// <summary>
    /// Survey Service class.
    /// </summary>
    public class SurveyService : Service<Survey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SurveyService( RockContext context ) : base( context ) { }
    }
}