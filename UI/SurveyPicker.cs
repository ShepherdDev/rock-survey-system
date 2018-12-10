using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using com.shepherdchurch.SurveySystem.Model;
using Rock;
using Rock.Data;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace com.shepherdchurch.SurveySystem.UI
{
    public class SurveyPicker : ItemPicker
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            ItemRestUrlExtraParams = "?getCategorizedItems=true&showUnnamedEntityItems=false&showCategoriesThatHaveNoChildren=false";
            ItemRestUrlExtraParams += "&entityTypeId=" + EntityTypeCache.Get( SystemGuid.EntityType.SURVEY.AsGuid() ).Id;
            this.IconCssClass = "fa fa-question-circle-o";
            base.OnInit( e );
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="survey">The survey.</param>
        public void SetValue( Survey survey )
        {
            if ( survey != null )
            {
                ItemId = survey.Id.ToString();

                string parentCategoryIds = string.Empty;
                var parentCategory = survey.Category;
                while ( parentCategory != null )
                {
                    parentCategoryIds = parentCategory.Id + "," + parentCategoryIds;
                    parentCategory = parentCategory.ParentCategory;
                }

                InitialItemParentIds = parentCategoryIds.TrimEnd( new[] { ',' } );
                ItemName = survey.Name;
            }
            else
            {
                ItemId = Rock.Constants.None.IdValue;
                ItemName = Rock.Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="surveys">The surveys.</param>
        public void SetValues( IEnumerable<Survey> surveys )
        {
            var surveyList = surveys.ToList();

            if ( surveyList.Any() )
            {
                var ids = new List<string>();
                var names = new List<string>();
                var parentCategoryIds = string.Empty;

                foreach ( var survey in surveyList )
                {
                    if ( survey != null )
                    {
                        ids.Add( survey.Id.ToString() );
                        names.Add( survey.Name );
                        var parentCategory = survey.Category;

                        while ( parentCategory != null )
                        {
                            parentCategoryIds += parentCategory.Id.ToString() + ",";
                            parentCategory = parentCategory.ParentCategory;
                        }
                    }
                }

                InitialItemParentIds = parentCategoryIds.TrimEnd( new[] { ',' } );
                ItemIds = ids;
                ItemNames = names;
            }
            else
            {
                ItemId = Rock.Constants.None.IdValue;
                ItemName = Rock.Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected override void SetValueOnSelect()
        {
            var survey = new SurveyService( new RockContext() ).Get( int.Parse( ItemId ) );
            SetValue( survey );
        }

        /// <summary>
        /// Sets the values on select.
        /// </summary>
        protected override void SetValuesOnSelect()
        {
            var survey = new SurveyService( new RockContext() ).Queryable().Where( g => ItemIds.Contains( g.Id.ToString() ) );
            this.SetValues( survey );
        }

        /// <summary>
        /// Gets the item rest URL.
        /// </summary>
        /// <value>
        /// The item rest URL.
        /// </value>
        public override string ItemRestUrl
        {
            get { return "~/api/Categories/GetChildren/"; }
        }
    }
}
