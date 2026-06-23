namespace Scopos.BabelFish.DataModel.Common {

    /// <summary>
    /// Represents the permissions a user has on returned objects from the API. These permissions are used to determine what actions a user can take on the returned objects.
    /// For example, if a user has the "match_parent.edit" permission on a parent match object, they should be able to edit that parent match.
    /// If they don't have that permission, they should not be able to edit that parent match.
    /// </summary>
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.PermissionConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.PermissionConverter ) )]
    public class Permission : IEquatable<Permission> {

        /*
         * Static instances created from the sql table permissions and iwth some help from AI.
         * 
         *   select *
         *   from permissions
         */

        public static readonly Permission DEFAULT = new Permission( "none", "A default permission that doesn't grant anything." );

        [Obsolete( "Replaced with CLUB_READ_PRIVATE_SETTINGS" )]
        public static readonly Permission CLUB_GET_DETAIL = new Permission( "club.get_detail", "Read general information about a club." );

        public static readonly Permission CLUB_MANAGE_ADMIN = new Permission( "club.manage_admin", "Ability to specify a club member as an admin of the club. " );
        public static readonly Permission CLUB_MANAGE_MEMBER = new Permission( "club.manage_member", "Ability to add, remove, update, or delete club members. Does not include assigning the admin role to a member." );
        public static readonly Permission CLUB_MANAGE_TEAMS = new Permission( "club.manage_teams", "May view and edit Club Teams." );
        public static readonly Permission CLUB_MANAGE_CLUB_PAGE = new Permission( "club.manage_club_page", "Ability to manage the Club's page on Rezults. Including enabling or disabling its visibility." );
        public static readonly Permission CLUB_PAYER = new Permission( "club.payer", "May make payments to renew a Club's Orion licenses, subscriptions, and warrentees." );
        public static readonly Permission CLUB_READ_LICENSE = new Permission( "club.read_license", "Read a club license" );
        public static readonly Permission CLUB_READ_TEAMS = new Permission( "club.read_teams", "May view Club Teams." );
        public static readonly Permission CLUB_MANAGE_SETTINGS = new Permission( "club.manage_settings", "Ability to manage club settings including a Club's contact information." );
        public static readonly Permission CLUB_READ_PUBLIC_SETTINGS = new Permission( "club.read_public_settings", "Ability to read a Club's settings that are marked with Public visibility." );
        public static readonly Permission CLUB_READ_PRIVATE_SETTINGS = new Permission( "club.read_private_settings", "Ability to read a Club's settings that are marked with Private visibility." );

        public static readonly Permission MATCH_CHILD_ACCEPT_INVITE = new Permission( "match_child.accept_invite", "Accept the invite to join a parent match as a child" );
        public static readonly Permission MATCH_CHILD_READ = new Permission( "match_child.read", "Read child match" );
        public static readonly Permission MATCH_PARENT_CREATE = new Permission( "match_parent.create", "Create parent match" );
        public static readonly Permission MATCH_PARENT_DELETE = new Permission( "match_parent.delete", "Delete parent match" );
        public static readonly Permission MATCH_PARENT_EDIT = new Permission( "match_parent.edit", "Modify parent match" );
        public static readonly Permission MATCH_PARENT_INVITE = new Permission( "match_parent.invite", "Invite others to join the parent match. This is equivalent to creating a child match of the parent." );
        public static readonly Permission MATCH_PARENT_READ = new Permission( "match_parent.read", "Read parent match" );
        public static readonly Permission TOURNAMENT_ADD_MEMBER = new Permission( "tournament.add_member", "Invite others to join a tournament" );
        public static readonly Permission TOURNAMENT_CREATE = new Permission( "tournament.create", "Create tournament" );
        public static readonly Permission TOURNAMENT_DELETE = new Permission( "tournament.delete", "Delete tournament" );
        public static readonly Permission TOURNAMENT_EDIT = new Permission( "tournament.edit", "Modify tournament" );
        public static readonly Permission TOURNAMENT_JOIN = new Permission( "tournament.join", "Request to join any tournament" );
        public static readonly Permission TOURNAMENT_LEAVE = new Permission( "tournament.leave", "leave a tournament" );
        public static readonly Permission TOURNAMENT_READ = new Permission( "tournament.read", "Permission to read a tournament" );
        public static readonly Permission TOURNAMENT_REMOVE_MEMBER = new Permission( "tournament.remove_member", "Remove a member from the tournament" );
        public static readonly Permission CREATE_MERGED_RESULTLIST = new Permission( "match.create_merged_resultlist", "Create and manage merged result lists for the match" );
        /// <summary>
        /// Default constructor. Purposefully made private.
        /// To crate a new instance, use the constructor that takes in a permission name, or use the static Parse instances defined in this class.
        /// </summary>
        private Permission() {
        }

        /// <summary>
        /// Initializes a new instance of the Permission class using the specified permission name as both the name and
        /// display name.
        /// </summary>
        /// <param name="permissionName">The name of the permission to assign. This value is used to identify and display the permission.</param>
        public Permission( string permissionName ) : this( permissionName, permissionName ) {
        }

        /// <summary>
        /// Initializes a new instance of the Permission class with the specified permission name and description.
        /// </summary>
        /// <param name="permissionName">The name of the permission. This value cannot be null or empty.</param>
        /// <param name="description">A description of the permission, providing additional context about its purpose.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="permissionName"/> is null or empty.</exception>
        public Permission( string permissionName, string description ) {
            if (string.IsNullOrEmpty( permissionName )) {
                throw new ArgumentException( "Permission name cannot be null or empty.", nameof( permissionName ) );
            }
            PermissionName = permissionName;
            Description = description;
        }

        /// <summary>
        /// Parses the specified permission name and returns a corresponding Permission object.
        /// </summary>
        /// <param name="permissionName">The name of the permission to parse. This value cannot be null or empty.</param>
        /// <param name="throwExceptionOnError">true to throw an exception if the permission name is null or empty; otherwise, false to return a default
        /// Permission.</param>
        /// <returns>A Permission object that corresponds to the specified permission name. Returns a default Permission if the
        /// permission name is null or empty and throwExceptionOnError is false.</returns>
        /// <exception cref="ArgumentException">Thrown if permissionName is null or empty and throwExceptionOnError is true.</exception>
        public static Permission Parse( string permissionName, bool throwExceptionOnError = false ) {

            if (string.IsNullOrEmpty( permissionName )) {
                if (throwExceptionOnError) {
                    throw new ArgumentException( "Permission name cannot be null or empty.", nameof( permissionName ) );
                } else {
                    return DEFAULT;
                }
            }

            return new Permission( permissionName );
        }

        /// <summary>
        /// Attempts to parse the specified permission name and returns a value that indicates whether the operation
        /// succeeded.
        /// </summary>
        /// <remarks>If the input string is null or empty, the method returns false and the out parameter
        /// is set to its default value.</remarks>
        /// <param name="permissionName">The name of the permission to parse. This parameter cannot be null or empty.</param>
        /// <param name="permission">When this method returns, contains the parsed Permission object if the parsing was successful; otherwise,
        /// contains the default Permission value.</param>
        /// <returns>true if the permission name was successfully parsed; otherwise, false.</returns>
        public static bool TryParse( string permissionName, out Permission permission ) {

            permission = DEFAULT;
            if (string.IsNullOrEmpty( permissionName )) {
                return false;
            }

            permission = new Permission( permissionName );
            return true;
        }

        /// <summary>
        /// Gets the name of the permission associated with the current context.
        /// </summary>
        /// <remarks>
        /// Values are taken from the 'permission' column in the'permissions' table in the database, and are used to determine what actions a user can take on returned objects from the API. 
        /// </remarks>
        public string PermissionName { get; private set; }

        /// <summary>
        /// Gets the description of the permission, providing additional context about its purpose.
        /// </summary>
        /// <remarks>
        /// Values are taken from the 'description' column in the 'permissions' table in the database, and are used to provide additional context about the permission's purpose.
        /// </remarks>
        public string Description { get; private set; }

        /// <summary>
        /// Returns a string that represents the name of the permission.
        /// </summary>
        /// <returns>A string containing the name of the permission.</returns>
        public override string ToString() {
            return PermissionName;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current Permission instance based on the permission.
        /// If the passed in object is a Permission and has the same permission name as the current instance, this method returns true; otherwise, it returns false.
        /// </summary>
        /// <param name="obj">The object to compare with the current Permission instance. The comparison returns true only if the object
        /// is a Permission with the same permission name.</param>
        /// <returns>true if the specified object is a Permission and has the same permission name as the current instance;
        /// otherwise, false.</returns>
        public override bool Equals( object obj ) {
            if (obj is Permission otherPermission) {
                return PermissionName == otherPermission.PermissionName;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current Permission object is equal to the specified Permission object based on the
        /// PermissionName property.
        /// </summary>
        /// <remarks>Two instances can have the same value for PermissionName and different values for Description and still be considered equal.</remarks>
        /// <param name="other">The Permission object to compare with the current Permission object. This parameter can be null.</param>
        /// <returns>true if the specified Permission object is not null and has the same PermissionName as the current object;
        /// otherwise, false.</returns>
        public bool Equals( Permission other ) {
            if (other == null) return false;
            return PermissionName == other.PermissionName;
        }

        /// <summary>
        /// Returns a hash code for the current instance that is based on the value of the PermissionName property.
        /// </summary>
        /// <remarks>Two instances can have the same value for PermissionName and different values for Description and still be considered equal.</remarks>
        /// <returns>A 32-bit signed integer hash code derived from the PermissionName property if it is not null or consists
        /// only of whitespace; otherwise, 0.</returns>
        public override int GetHashCode() {
            return !string.IsNullOrWhiteSpace( PermissionName ) ? PermissionName.GetHashCode() : 0;
        }
    }
}
