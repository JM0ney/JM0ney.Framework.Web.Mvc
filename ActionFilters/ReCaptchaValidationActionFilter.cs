using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JM0ney.Framework.Web.Mvc.ActionFilters {

    public class ReCaptchaValidationActionFilter : ActionFilterAttribute {

        public const String SITE_KEY_SETTING_NAME = "JM0ney:ReCaptcha:SiteKey";
        public const String SECRET_KEY_SETTING_NAME = "JM0ney:ReCaptcha:SecretKey";

        public override void OnActionExecuting( ActionExecutingContext filterContext ) {
            base.OnActionExecuting( filterContext );

            String secret = ConfigurationHelper.TryGetValue( SECRET_KEY_SETTING_NAME, String.Empty );
            String gCaptchaResponse = filterContext.HttpContext.Request.Params[ "g-recaptcha-response" ] ?? String.Empty; ;
            String contentString = String.Format( "secret={0}&response={1}", secret, gCaptchaResponse );
            byte[ ] theBytes = System.Text.Encoding.ASCII.GetBytes( contentString );

            System.Net.WebRequest webRequest = System.Net.WebRequest.Create( @"https://www.google.com/recaptcha/api/siteverify" );
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = theBytes.Length;

            var dataStream = webRequest.GetRequestStream( );
            dataStream.Write( theBytes, 0, theBytes.Length );
            dataStream.Close( );

            System.Net.WebResponse response = webRequest.GetResponse( );

            // Get the stream containing content returned by the server.  
            dataStream = response.GetResponseStream( );
            // Open the stream using a StreamReader for easy access.  
            System.IO.StreamReader reader = new System.IO.StreamReader( dataStream );
            // Read the content.  
            String responseFromServer = reader.ReadToEnd( ).Replace( "error-codes", "errorcodes" );

            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer( );
            Newtonsoft.Json.JsonReader readMe = new Newtonsoft.Json.JsonTextReader( new System.IO.StringReader( responseFromServer ) );
            ReCaptchaResult theRealResult = ser.Deserialize<ReCaptchaResult>( readMe );

            readMe.Close( );

            dataStream.Close( );
            dataStream.Dispose( );

            response.Close( );
            response.Dispose( );

            if ( !theRealResult.success ) {
                String errorMessage = Localization.ErrorMessages.ReCaptchaFailed;
                filterContext.Controller.ViewData.ModelState.AddModelError( String.Empty, errorMessage );
            }
        }

        public static MvcHtmlString RenderComponent( ) {
            String siteKey = ConfigurationHelper.TryGetValue( SITE_KEY_SETTING_NAME, String.Empty ) ?? String.Empty;
            TagBuilder builder = new TagBuilder( "div" );
            builder.MergeAttribute( "class", "g-recaptcha" );
            builder.MergeAttribute( "data-sitekey", siteKey );
            return MvcHtmlString.Create( builder.ToString( TagRenderMode.Normal ) );
        }

        public static MvcHtmlString RenderScriptReference( ) {
            TagBuilder builder = new TagBuilder( "script" );
            builder.MergeAttribute( "src", "https://www.google.com/recaptcha/api.js" );
            return MvcHtmlString.Create( builder.ToString( TagRenderMode.Normal ) );
        }



        private class ReCaptchaResult {

            private bool _success = false;
            private String _challenge_ts = String.Empty;
            private String _hostname = String.Empty;
            private String[ ] _errorcodes = null;

            public Boolean success {
                get {
                    return _success;
                }

                set {
                    this._success = value;
                }
            }

            public String challenge_ts {
                get {
                    return _challenge_ts;
                }

                set {
                    this._challenge_ts = value;
                }
            }

            public String hostname {
                get {
                    return _hostname;
                }

                set {
                    this._hostname = value;
                }
            }

            public String[ ] errorcodes {
                get {
                    return _errorcodes;
                }

                set {
                    this._errorcodes = value;
                }
            }
        }



    }
}
