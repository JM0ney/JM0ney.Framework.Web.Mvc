using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM0ney.Framework.Web.Mvc.ValidationAttributes {

    /// <summary>
    /// Represents a selectable item amongst a list of selectable items
    /// </summary>
    public interface ISelectionItem {

        /// <summary>
        /// Returns whetheror not this item is selected
        /// </summary>
        bool IsSelected { get; }

    }

}
