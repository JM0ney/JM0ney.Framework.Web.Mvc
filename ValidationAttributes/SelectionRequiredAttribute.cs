//
//  Do you like this project? Do you find it helpful? Pay it forward by hiring me as a consultant!
//  https://jason-iverson.com
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM0ney.Framework.Web.Mvc.ValidationAttributes {

    /// <summary>
    /// Requires the selection of at least 1 of the items contained in the IEnumerable property to which this attribute is decorated
    /// </summary>
    public class SelectionRequiredAttribute : RequiredAttribute {

        public override bool IsValid( object value ) {
            bool valid = false;
            if ( value is System.Collections.IEnumerable ) {
                foreach ( Object item in value as System.Collections.IEnumerable ) {
                    if ( item is ISelectionItem ) {
                        ISelectionItem listItem = ( ISelectionItem ) item;
                        if ( listItem.IsSelected )
                            return true;
                    }
                    else {
                        throw new InvalidOperationException( String.Format( Localization.ErrorMessages.SelectionRequiredAttributeISelectionItem_FS,
                            this.GetType( ).FullName, typeof( ISelectionItem ).FullName ) );
                    }
                }
            }
            else {
                throw new InvalidOperationException( String.Format( Localization.ErrorMessages.SelectionRequiredAttributeIEnumerable_FS, this.GetType( ).FullName ) );
            }
            return valid;
        }

    }

}
