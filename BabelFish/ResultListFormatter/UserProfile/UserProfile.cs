using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.ResultListFormatter.UserProfile {
	/// <summary>
	/// Represents a row in the user_profile table.
	/// </summary>
	public class UserProfile {

		/// <summary>
		/// The user's GUID formatted UserID, which is assigned by cognito.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The user's choosen Account URL.
		/// A value of null means they do not have a public profile.
		/// </summary>
		public string ?AccountURL { get; set; } = null;

		/// <summary>
		/// The user's choosen profile account visibility.
		/// </summary>
		public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;
	}
}
