using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
	public static class IRLIFListExtensions {

		/// <summary>
		/// Returns a unique list of relays that one more more participants are squadded on. 
		/// <para>Relay "0" is excluded.</para>
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<string> ListOfRelays( this IRLIFList list ) {
			List<string> relays = new List<string>();

			foreach (var item in list.GetAsIRLItemsList()) {
				if (item.SquaddingAssignment != null
					&& item.SquaddingAssignment is SquaddingAssignmentFiringPoint safp
					&& !relays.Contains( safp.Relay )
					&& !string.IsNullOrEmpty( safp.Relay )
					&& safp.Relay != "0") {
					relays.Add( safp.Relay );
				} else if (item.SquaddingAssignment != null
					&& item.SquaddingAssignment is SquaddingAssignmentBank sab
					&& !relays.Contains( sab.Relay )
					&& !string.IsNullOrEmpty( sab.Relay )
					&& sab.Relay != "0") {
					relays.Add( sab.Relay );
				}
			}

			return relays
				.Select( s => int.Parse( s ) )
				.OrderBy( n => n )
				.Select( n => n.ToString() )
				.ToList();
		}
	}
}
