using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel {
    /// <summary>
    /// Defines a set of functions and properties a class needs to have in order 
    /// to be stored in a SQL Table.
    /// </summary>
    public interface IObjectRelationalMapper {

        /// <summary>
        /// Indicates if the instance class represents a new record (on that
        /// has not been saved yet) in the SQL table. A value of false means the
        /// record was previously saved to the SQL table.
        /// </summary>
        bool NewRecord { get; set; }
    }
}
