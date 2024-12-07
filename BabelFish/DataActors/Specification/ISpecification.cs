using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Scopos.BabelFish.DataActors.Specification {

    /// <summary>
    /// Implements the Specificaiton Pattern.
    /// <para><see href="https://matt.berther.io/2005/03/25/the-specification-pattern-a-primer/">The Specification Patter: A Primer</see></para>
    /// </summary>
    /// <remarks>Code largely taken from https://en.wikipedia.org/wiki/Specification_pattern#C#_6.0_with_generics </remarks>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> {

        Task<bool> IsSatisfiedByAsync( T candidate );

        /// <summary>
        /// Error messages to present to the user.
        /// </summary>
        List<string> Messages { get; set; }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T> {
        public abstract Task<bool> IsSatisfiedByAsync( T candidate );

        /// <inheritdoc/>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
