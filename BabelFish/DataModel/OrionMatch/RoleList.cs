
namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class RoleList : List<RoleAuthorization> {

        /// <summary>
        /// Returns true, if this RoleList has the passed in role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole( MatchParticipantRole role ) {

            foreach( var ra in this )
                if ( ra.Role == role )
                    return true;

            return false;
        }

        /// <summary>
        /// Adds the passed in MatchParticipantRole to the RoleList.
        /// <para>If the passed in MatchParticipantRole already exists in the RoleList, the RoleList is not modified.</para>
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public void AddRole( MatchParticipantRole role ) {

            if ( ! HasRole( role ) ) {
                this.Add( new RoleAuthorization( role ) );
            }
        }

        /// <summary>
        /// Removes all RoleAuthorizations from this RoleList with MatchPartiipantRole.
        /// </summary>
        /// <param name="role"></param>
        public void RemoveRole( MatchParticipantRole role ) {

            this.RemoveAll( p => p.Role == role );

        }
    }
}
