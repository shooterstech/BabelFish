using Scopos.BabelFish.APIClients;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.DataModel.Definitions {
	public static class DefinitionFactory {

		private static ClubsAPIClient ClubsAPIClient = new ClubsAPIClient();
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="owner"></param>
		/// <returns></returns>
		public static async Task<T> Build<T>( string owner, string properName, string nameSpaceToUse="" ) where T : Definition, new() {

			var definition = new T();

			var clubDetailResponse = await ClubsAPIClient.GetClubDetailPublicAsync( owner );
			
			//Check that the club / owner the user passed in could be looked up.
			if (clubDetailResponse.StatusCode != System.Net.HttpStatusCode.OK) {
				var msg = $"Could not learn information about the Orion Club '{owner}' when looking them up received status code '{clubDetailResponse.StatusCode}'.";
				Logger.Error( msg );
				throw new ScoposAPIException( msg );
			}

			var clubDetail = clubDetailResponse.ClubDetail;

			if (clubDetail.NamespaceList.Count == 0 ) {
				var msg = $"Could not create a new Definition with Orion Club '{owner}' because that account does not have any namespaces associated with it.";
				Logger.Error( msg );
				throw new ScoposException( msg );
			}

			//Use either the namespace the user passed in, if it is part of the owner's list
			//or the first avaliable one, if not
			var nameSpace = clubDetail.NamespaceList[0].Namespace;
			foreach (var ns in clubDetail.NamespaceList)
				if (ns.Namespace == nameSpaceToUse)
					nameSpace = ns.Namespace;

			definition.Owner = clubDetail.OwnerId;
			definition.Version = "1.1";
			definition.HierarchicalName = $"{nameSpace}:{properName}";
			definition.CommonName = properName;
			definition.Description = properName;

			return definition;
		}
	}
}
