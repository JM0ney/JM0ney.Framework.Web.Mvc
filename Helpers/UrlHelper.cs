//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM0ney.Framework.Web.Mvc.Helpers {

    /// <summary>
    /// Extension methods for <see cref="System.Web.Mvc.UrlHelper"/>
    /// </summary>
    public static class UrlHelper {

        /// <summary>
        /// Essentially the same as Url.Content( ), but automatically prepends the '~' and '/' characters as needed
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="contentPathValue"></param>
        /// <returns></returns>
        public static String ContentFromRoot( this System.Web.Mvc.UrlHelper urlHelper, String contentPathValue ) {
            String realPath = contentPathValue ?? String.Empty;
            if ( !realPath.StartsWith( "~" ) ) {
                if ( !realPath.StartsWith( "/" ) ) {
                    realPath = "/" + realPath;
                }
                realPath = "~" + realPath;
            }
            return urlHelper.Content( realPath );
        }

    }

}
