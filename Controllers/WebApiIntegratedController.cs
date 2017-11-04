using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JM0ney.Framework;
using JM0ney.AssetLibrary.ViewModels;
using JM0ney.Framework.Data.Localization;

namespace JM0ney.Framework.Web.Mvc.Controllers {

    public abstract class WebAPIIntegratedController<TViewModel, TWebAPIController> : ControllerBase
        where TViewModel : AssetLibrary.ViewModels.Base.ViewModelBase, new()
        where TWebAPIController : class, WebAPI.IWebAPIController<TViewModel>, new() {


        #region Action Methods

        #region Index

        [HttpGet]
        public virtual ActionResult Index( Guid? identity ) {
            IndexViewModel<TViewModel> viewModel = new IndexViewModel<TViewModel>( identity, null );
            Result<IndexViewModel<TViewModel>> result = this.DoIndexGet( identity );
            if ( !result.IsSuccess )
                this.SetActionStatusMessage( result, false );
            else
                viewModel = result.ReturnValue;
            return this.View( viewModel );
        }
        
        #endregion Index

        #region New

        [HttpGet]
        public virtual ActionResult New( Guid? parentIdentity ) {
            TViewModel viewModel = new TViewModel( );
            Result<TViewModel> result = this.DoNewGet( parentIdentity );
            if ( !result.IsSuccess )
                this.SetActionStatusMessage( result, false );
            else
                viewModel = result.ReturnValue;
            return this.View( viewModel );
        }

        [HttpPost]
        public virtual ActionResult New( Guid? parentIdentity, TViewModel viewModel ) {
            if ( this.ModelState.IsValid ) {
                Result result = this.DoNewPost( viewModel );
                if ( result.IsSuccess ) {
                    return this.OnNewRedirect( true, viewModel );
                }
                else {
                    this.SetActionStatusMessage( result, false );
                }
            }
            return this.View( viewModel );
        }

        #endregion New

        #region Edit

        [HttpGet]
        public virtual ActionResult Edit( Guid identity ) {
            TViewModel viewModel = new TViewModel( );
            Result<TViewModel> result = this.DoEditGet( identity );
            if ( !result.IsSuccess )
                this.SetActionStatusMessage( result, false );
            else
                viewModel = result.ReturnValue;
            return this.View( viewModel );
        }

        [HttpPost]
        public virtual ActionResult Edit( Guid identity, TViewModel viewModel ) {
            if ( this.ModelState.IsValid ) {
                Result result = this.DoEditPost( viewModel );
                if ( result.IsSuccess )
                    return this.OnEditRedirect( true, viewModel );
                else
                    this.SetActionStatusMessage( result, false );
            }
            return this.View( viewModel );
        }

        #endregion

        #region Delete

        [HttpGet]
        public virtual ActionResult Delete( Guid identity ) {
            TViewModel viewModel = new TViewModel( );
            Result<TViewModel> result = this.DoDeleteGet( identity );
            if ( !result.IsSuccess )
                this.SetActionStatusMessage( result, false );
            else
                viewModel = result.ReturnValue;
            return this.View( viewModel );
        }

        [HttpPost]
        public virtual ActionResult Delete( Guid identity, TViewModel viewModel ) {
            if ( this.ModelState.IsValid ) {
                Result result = this.DoDeletePost( identity );
                if ( result.IsSuccess ) {
                    return this.OnDeleteRedirect( true, viewModel );
                }
                else {
                    this.SetActionStatusMessage( result, false );
                }
            }
            return this.View( viewModel );
        }

        #endregion Delete

        #endregion Action Methods

        #region Redirect Methods

        protected virtual ActionResult OnNewRedirect( bool setActionStatusMessage, TViewModel viewModel ) {
            // After a record is created, redirect to edit it
            if ( setActionStatusMessage )
                this.SetActionStatusMessage( Result.SuccessResult( String.Format(  Messages.Created_FS, this.FriendlyNameSingular ) ) );
            return this.RedirectToAction( "Edit", new { identity = viewModel.Identity } );
        }

        protected virtual ActionResult OnEditRedirect( bool setActionStatusMessage, TViewModel viewModel ) {
            // After a record is edited, redirect to edit it again
            if ( setActionStatusMessage )
                this.SetActionStatusMessage( Result.SuccessResult( String.Format( Messages.Edited_FS, this.FriendlyNameSingular ) ) );
            return this.RedirectToAction( "Edit", new { identity = viewModel.Identity } );
        }

        protected virtual ActionResult OnDeleteRedirect( bool setActionStatusMessage, TViewModel viewModel ) {
            // After deleting, redirect to the index page
            if ( setActionStatusMessage )
                this.SetActionStatusMessage( Result.SuccessResult( String.Format( Messages.Deleted_FS, this.FriendlyNameSingular ) ) );
            return this.RedirectToAction( "Index" );
        }

        #endregion Redirect Methods

        #region Overrides

        protected override void FillViewData( ) {
            base.FillViewData( );
            this.ViewData[ "NameSingular" ] = this.FriendlyNameSingular;
            this.ViewData[ "NamePlural" ] = this.FriendlyNamePlural;
        }

        #endregion Overrides

        #region Protected Members

        protected Result<IndexViewModel<TViewModel>> DoIndexGet( Guid? identity ) {
            using ( TWebAPIController webApi = this.GetWebAPIController( ) ) {
                Result<IndexViewModel<TViewModel>> result = null;
                IndexViewModel<TViewModel> returnValue = null;
                var listResult = webApi.List( );
                if ( listResult.IsSuccess ) {
                    returnValue = new IndexViewModel<TViewModel>( identity, listResult.ReturnValue );
                    result = Result.SuccessResult( returnValue );
                }
                else {
                    result = Result.ErrorResult<IndexViewModel<TViewModel>>( listResult.Message );
                }
                return result;
            }
        }

        protected Result<TViewModel> DoNewGet( Guid? parentIdentity ) {
            using ( TWebAPIController apiController = this.GetWebAPIController( ) ) {
                return apiController.Instantiate( parentIdentity );
            }
        }

        protected Result DoNewPost( TViewModel viewModel ) {
            using ( TWebAPIController apiController = this.GetWebAPIController( ) ) {
                return apiController.Create( viewModel );
            }
        }

        protected Result<TViewModel> DoEditGet( Guid identity ) {
            using ( TWebAPIController apiController = this.GetWebAPIController( ) ) {
                return apiController.Load( identity );
            }
        }

        protected Result DoEditPost( TViewModel viewModel ) {
            using ( TWebAPIController apiController = this.GetWebAPIController( ) ) {
                return apiController.Save( viewModel );
            }
        }

        protected Result<TViewModel> DoDeleteGet( Guid identity ) {
            return this.DoEditGet( identity );
        }

        protected Result DoDeletePost( Guid identity ) {
            using ( TWebAPIController apiController = this.GetWebAPIController( ) ) {
                return apiController.Delete( identity );
            }
        }

        protected String FriendlyNameSingular {
            get { return this.GetWebAPIController( ).FriendlyNameSingular; }
        }

        protected String FriendlyNamePlural {
            get { return this.GetWebAPIController( ).FriendlyNamePlural; }
        }


        #endregion Protected Members

        #region Private Members

        private TWebAPIController GetWebAPIController( ) {
            if ( this._Controller == null )
                this._Controller = new TWebAPIController( );
            return this._Controller;
        }
        private TWebAPIController _Controller = null;

        #endregion Private Members
    }

}
