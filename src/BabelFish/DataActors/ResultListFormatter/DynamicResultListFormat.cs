using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    /// <summary>
    /// Abstract class to generate dynamic RESULT LIST FORMAT definitions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DynamicResultListFormat<T> {

        /// <summary>
        /// Method to call to generate the RESULT LIST FORMAT.
        /// <para>Concrete classes are responsible for composing this method.</para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract Task<ResultListFormat> GenerateAsync( T item );
    }
}
