using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.shepherdchurch.SurveySystem.Field;
using Rock.Attribute;

namespace com.shepherdchurch.SurveySystem.Attribute
{
    /// <summary>
    /// Survey Field Attribute. Stored as Survey's Guid.
    /// </summary>
    public class SurveyFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyFieldAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="category">The category.</param>
        /// <param name="order">The order.</param>
        /// <param name="key">The key.</param>
        public SurveyFieldAttribute( string name, string description = "", bool required = true, string defaultValue = "", string category = "", int order = 0, string key = null ) :
            base( name, description, required, defaultValue, category, order, key, typeof( SurveyFieldType ).FullName, "com.shepherdchurch.SurveySystem" )
        {
        }
    }
}
