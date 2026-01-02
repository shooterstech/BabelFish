using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Clubs {

    /// <summary>
    /// Describes a Reconfigurable Rulebook namespace.
    /// </summary>
    public class NamespaceDetail : IEquatable<NamespaceDetail> {

        /// <summary>
        /// The namespace name.
        /// </summary>
        public string Namespace { get; set; }

        /// <inheritdoc/>
        public bool Equals( NamespaceDetail other ) {
            return this.Namespace == other.Namespace;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"Namespace: {Namespace}";
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if ( obj == null || obj is not NamespaceDetail) return false;

            return Equals( (NamespaceDetail) obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return this.Namespace.GetHashCode();
        }
    }
}
