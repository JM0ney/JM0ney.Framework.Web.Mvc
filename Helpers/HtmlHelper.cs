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
    /// Default parts of the Route to inquire about
    /// </summary>
    public enum RouteParts {
        Controller,
        Action,
        Area
    }

    /// <summary>
    /// Extension methods for <see cref="System.Web.Mvc.HtmlHelper"/>
    /// </summary>
    public static class HtmlHelper {


        /// <summary>
        /// Returns a "checked" attribute if the <paramref name="booleanExpression"/> evaluates to true.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="booleanExpression"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckedAttributeFor<TModel>( this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, Boolean>> booleanExpression ) {
            String returnString = String.Empty;
            bool isSelected = booleanExpression.Compile( ).Invoke( htmlHelper.ViewData.Model );
            if ( isSelected )
                returnString = "checked";
            return MvcHtmlString.Create( returnString );
        }

        /// <summary>
        /// Reads and returns the description from the expression's metadata
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString DescriptionFor<TModel, TValue>( this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression ) {
            var metadata = ModelMetadata.FromLambdaExpression( expression, self.ViewData );
            var description = metadata.Description;

            return MvcHtmlString.Create( description );
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

        public static MvcHtmlString ReCaptchaComponent( ) {
            return ActionFilters.ReCaptchaValidationActionFilter.RenderComponent( );
        }

        public static MvcHtmlString ReCaptchaScript( ) {
            return ActionFilters.ReCaptchaValidationActionFilter.RenderScriptReference( );
        }

        /// <summary>
        /// Returns a "selected" attribute if the <paramref name="booleanExpression"/> evaluates to true.
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
