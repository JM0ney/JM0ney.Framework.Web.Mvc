//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JM0ney.Framework;

namespace JM0ney.Framework.Web.Mvc.Controllers {

    public abstract class ControllerBase : Controller {

        #region Fields

        private readonly SerializableCookieHelper _CookieHelper = new SerializableCookieHelper( );

        #endregion Fields

        #region Constructor(s)

        public ControllerBase( ) {
            this.FillViewData( );
        }

        #endregion Constructor(s)

        #region Properties

        protected SerializableCookieHelper CookieHelper {
            get {
                return _CookieHelper;
            }
        }

        #endregion Properties

        protected virtual void TryAddModelError( IResult result, String modelKey = "" ) {
            if ( !result.IsSuccess )
                this.ModelState.AddModelError( modelKey, result.Message );
        }

        protected virtual void SetActionStatusMessage( IResult result, bool clientIsRedirecting = true ) {
            if ( clientIsRedirecting ) {
                // A cookie will be created.  An action filter will then transfer the values of the cookie to the ViewData and delete the cookie
                String cookieText = String.Format( "{0}{1}", result.IsSuccess ? "1" : "0", result.Message );
                HttpCookie cookie = new HttpCookie( ActionFilters.ActionStatusCookieInvalidatorActionFilter.MessageCookieName );
                cookie.Expires = DateTime.Now.AddMinutes( 5 );
                cookie.Value = cookieText;
                cookie.Shareable = false;
                Web.CookieHelper.SetCookie( cookie );
            }
            else {
                // Just add the result to the ViewData so the user can be displayed the result immediately
                Result tempResult;
                if ( result.IsSuccess )
                    tempResult = Result.SuccessResult( result.Message );
                else
                    tempResult = Result.ErrorResult( result.Message );
                this.ViewData.Add( new KeyValuePair<string, object>( ActionFilters.ActionStatusCookieInvalidatorActionFilter.ViewDataKey, tempResult ) );
            }
        }

        protected virtual void FillViewData( ) { }

        protected virtual void FillHtmlDocumentViewData(String documentTitle, String metaDescription, String metaKeywords = "" ) {
            this.ViewData[ "Title" ] = documentTitle;
            this.ViewData[ "Description" ] = metaDescription;
            if ( !String.IsNullOrWhiteSpace( metaKeywords ) )
                this.ViewData[ "Keywords" ] = metaKeywords;
        }

    }

}
