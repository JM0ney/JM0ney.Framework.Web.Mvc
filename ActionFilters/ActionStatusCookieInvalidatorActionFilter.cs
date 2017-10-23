using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JM0ney.Framework.Web.Mvc.ActionFilters {

    /// <summary>
    /// Parses ActionStatus cookie data and stores it in the ViewData to be displayed as a message to the requesting client.
    /// </summary>
    public class ActionStatusCookieInvalidatorActionFilter : ActionFilterAttribute {

        #region Fields

        private static String _MessageCookieName = "JM0neyActionStatus";
        private static String _ViewDataKey = "ActionStatus";

        #endregion Fields

        #region Constructor(s)

        /// <summary>
        /// Constructs the <see cref="ActionStatusCookieInvalidatorActionFilter" />
        /// </summary>
        /// <param name="messageCookieName">The name of the <see cref="HttpCookie"/> to look for</param>
        public ActionStatusCookieInvalidatorActionFilter( String messageCookieName ) {
            ActionStatusCookieInvalidatorActionFilter.MessageCookieName = messageCookieName;
        }

        #endregion Constructor(s)

        #region Properties

        /// <summary>
        /// The name of the <see cref="HttpCookie"/> to look for
        /// </summary>
        public static String MessageCookieName {
            get {
                return _MessageCookieName;
            }

            private set {
                _MessageCookieName = value;
            }
        }

        /// <summary>
        /// The <see cref="ViewDataDictionary"/> key value to store message data into
        /// </summary>
        public static String ViewDataKey {
            get {
                return _ViewDataKey;
            }

            set {
                _ViewDataKey = value;
            }
        }

        #endregion Properties

        #region Overrides

        public override void OnActionExecuting( ActionExecutingContext filterContext ) {
            base.OnActionExecuting( filterContext );
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies[ ActionStatusCookieInvalidatorActionFilter.MessageCookieName ];
            if ( cookie != null && !String.IsNullOrWhiteSpace( cookie.Value ) ) {
                String readValue = cookie.Value;
                String message = readValue.Substring( 1 );
                bool isSuccess = ( cookie.Value[ 0 ] == '1' );
                Result result;
                if ( isSuccess )
                    result = Result.SuccessResult( message );
                else
                    result = Result.ErrorResult( message );
                filterContext.Controller.ViewData.Add( new KeyValuePair<string, object>( ActionStatusCookieInvalidatorActionFilter.ViewDataKey, result ) );
                CookieHelper.DeleteCookie( ActionStatusCookieInvalidatorActionFilter.MessageCookieName );
            }
        }

        #endregion Overrides

    }

}
