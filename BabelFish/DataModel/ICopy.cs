using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel {
    public interface ICopy<T> where T : class, new() {

        /// <summary>
        /// Makes a deep copy of the class instance.
        /// Same as ObjectCloner.Clone(), but faster because it doesn't rely on serialization and deserialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Copy();
    }
}
