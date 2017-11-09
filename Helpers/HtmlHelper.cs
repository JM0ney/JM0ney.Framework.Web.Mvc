//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JM0ney.Framework.Web.Mvc.Helpers {

    /// <summary>
    /// Extension methods for <see cref="System.Web.Mvc.HtmlHelper"/>
    /// </summary>
    public static class HtmlHelper {

        /// <summary>
        /// Default parts of the Route to inquire about
        /// </summary>
        public enum RouteParts {
            Controller,
            Action,
            Area
        }

        /// <summary>
        /// Returns a particular portion of the RouteData
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="whichPart"></param>
        /// <returns></returns>
        public static String GetRouteData( this System.Web.Mvc.HtmlHelper htmlHelper, RouteParts whichPart ) {
            Object obj;
            String returnValue = String.Empty;
            String key = "action";
            if ( whichPart == RouteParts.Area )
                key = "area";
            else if ( whichPart == RouteParts.Controller )
                key = "controller";
            obj = htmlHelper.ViewContext.RouteData.Values[ key ];
            if ( obj != null )
                returnValue = obj.ToString( );
            return returnValue;
        }

        /// <summary>
        /// Based on the <paramref name="booleanExpression"/> provided, returns a "selected" attribute if true is the result of the evaluation
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="booleanExpression"></param>
        /// <returns></returns>
        public static MvcHtmlString SelectedAttributeFor<TModel>( this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, Boolean>> booleanExpression ) {
            String returnString = String.Empty;
            bool isSelected = booleanExpression.Compile( ).Invoke( htmlHelper.ViewData.Model );
            if ( isSelected )
                returnString = "selected";
            return MvcHtmlString.Create( returnString );
        }

    }

}
