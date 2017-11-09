//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JM0ney.Framework.Web.Mvc.ActionFilters {

    /// <summary>
    /// Automatically infers the culture and language information based on RouteData, resulting in a localized version of a web page being served to the requesting client
    /// </summary>
    public class DynamicLocalizationActionFilter : ActionFilterAttribute {

        private static String _GlobalizationRouteKeyName = "lang";

        /// <summary>
        /// Gets or Sets the name of the key value used when configuring RouteData with the appropriate localization setting.  Defaults to "lang"
        /// </summary>
        public static String GlobalizationRouteKeyName {
            get {
                return _GlobalizationRouteKeyName;
            }

            set {
                _GlobalizationRouteKeyName = value;
            }
        }

        public override void OnActionExecuting( ActionExecutingContext filterContext ) {
            base.OnActionExecuting( filterContext );
            Object theLang = filterContext.RouteData.Values[ GlobalizationRouteKeyName ];
            if ( theLang == null ) {
                theLang = System.Globalization.CultureInfo.CurrentCulture.Name;
                filterContext.RouteData.Values[ GlobalizationRouteKeyName ] = theLang;
            }

            System.Globalization.CultureInfo cultureInfo = null;
            try {
                cultureInfo = System.Globalization.CultureInfo.GetCultureInfo( theLang.ToString( ) );
            }
            catch {
                cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            }

            if ( System.Threading.Thread.CurrentThread.CurrentCulture != cultureInfo )
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            if ( System.Threading.Thread.CurrentThread.CurrentUICulture != cultureInfo )
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

    }

}
