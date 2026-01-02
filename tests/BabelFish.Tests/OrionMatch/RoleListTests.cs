using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Tests;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class RoleListTests : BaseTestClass {

        [TestMethod]
        public void AddAndRemoveRoleTests() {

            RoleList rl = new RoleList();
            Assert.IsNotNull(rl);
            Assert.AreEqual(0, rl.Count);

            //Add the first Role, and check we now have exactly one item in the list.
            rl.AddRole( MatchParticipantRole.ATHLETE );
            Assert.AreEqual(1, rl.Count);
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.ATHLETE ));
            Assert.IsFalse( rl.HasRole( MatchParticipantRole.COACH ) );

            //Add a second Role, and check we now have exactly two item in the list.
            rl.AddRole( MatchParticipantRole.COACH );
            Assert.AreEqual( 2, rl.Count );
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.ATHLETE ) );
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.COACH ) );

            //try and add the coach role a second time, and check it did not modify the list
            rl.AddRole( MatchParticipantRole.COACH );
            Assert.AreEqual( 2, rl.Count );
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.ATHLETE ) );
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.COACH ) );

            //Remove the Coach role, we should now have only one item in the list.
            rl.RemoveRole( MatchParticipantRole.COACH );
            Assert.AreEqual( 1, rl.Count );
            Assert.IsTrue( rl.HasRole( MatchParticipantRole.ATHLETE ) );
            Assert.IsFalse( rl.HasRole( MatchParticipantRole.COACH ) );
        }
    }
}
