using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Runtime.Authentication;

namespace BabelFish.Tests.OrionMatch {

    [TestClass]
    public class CompareTests {

        [TestMethod]
        public async Task CompareParticipatns() {

            //Call the GetSquaddingList api
            var squaddingList = new List<SquaddingAssignment>();

            var sortByLastNameAsc = new CompareParticipant( CompareParticipant.CompareMethod.LASTNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            squaddingList.Sort( sortByLastNameAsc );


            var sortByDisplayNameDsc = new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            squaddingList.Sort( sortByLastNameAsc );
        }
}
