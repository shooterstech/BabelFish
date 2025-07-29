using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;
using static Scopos.BabelFish.DataActors.Specification.Definitions.IsEventStyleValid;

namespace Scopos.BabelFish.Tests.Definition.Validation {

    [TestClass]
	public class EventStyleValidationTests : BaseTestClass {

		[TestMethod]
		public async Task HappyPathEventStyleIsValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyle = (await client.GetEventStyleDefinitionAsync( setName )).Value;

			var validation = new IsEventStyleValid();

			var valid = await validation.IsSatisfiedByAsync( eventStyle );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}

		[TestMethod]
		public async Task IsEventStylesAndStageStylesValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyleOrig = (await client.GetEventStyleDefinitionAsync( setName )).Value;

			var validation = new IsEventStyleStageStylesAndEventStylesValid();

			//Unalterned should pass
			var valid = await validation.IsSatisfiedByAsync( eventStyleOrig );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//Remove both
			var eventStyle = eventStyleOrig.Clone();
			eventStyle.StageStyles.Clear();
			eventStyle.EventStyles.Clear();
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//Populate both
			eventStyle.StageStyles.Add( "v1.0:ntparc:Sporter Air Rifle Prone" );
			eventStyle.EventStyles.Add( "v1.0:nra:BB Gun" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );
		}

		[TestMethod]
		public async Task IsEventStylesValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyle = (await client.GetEventStyleDefinitionAsync( setName )).Value;

			var validation = new IsEventStyleEventStylesValid();

			//Unalterned should pass
			var valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//A empty string should fail
			eventStyle.EventStyles.Clear();
			eventStyle.EventStyles.Add( "" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A fake set name should fail
			eventStyle.EventStyles.Clear();
			eventStyle.EventStyles.Add( "v1.0:orion:not a real set name" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A real set name should pass
			eventStyle.EventStyles.Clear();
			eventStyle.EventStyles.Add( "v1.0:nra:BB Gun" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid );
			Assert.IsTrue( validation.Messages.Count == 0 );
		}

		[TestMethod]
		public async Task IsRelatedEventStylesValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyle = (await client.GetEventStyleDefinitionAsync( setName )).Value;

			var validation = new IsEventStyleRelatedEventStylesValid();

			//Unalterned should pass
			var valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//Having no Related EventStyles should pass
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//A empty string should fail
			eventStyle.RelatedEventStyles.Clear();
			eventStyle.RelatedEventStyles.Add( "" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A fake set name should fail
			eventStyle.RelatedEventStyles.Clear();
			eventStyle.RelatedEventStyles.Add( "v1.0:orion:not a real set name" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A real set name should pass
			eventStyle.RelatedEventStyles.Clear();
			eventStyle.RelatedEventStyles.Add( "v1.0:nra:BB Gun" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid );
			Assert.IsTrue( validation.Messages.Count == 0 );
		}

		[TestMethod]
		public async Task IsStageStylesValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyle = (await client.GetEventStyleDefinitionAsync( setName )).Value;

			var validation = new IsEventStyleStageStylesValid();

			//Unalterned should pass
			var valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//A empty string should fail
			eventStyle.StageStyles.Clear();
			eventStyle.StageStyles.Add( "" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A fake set name should fail
			eventStyle.StageStyles.Clear();
			eventStyle.StageStyles.Add( "v1.0:orion:not a real set name" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A real set name should pass
			eventStyle.StageStyles.Clear();
			eventStyle.StageStyles.Add( "v1.0:ntparc:Sporter Air Rifle Standing" );
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid );
			Assert.IsTrue( validation.Messages.Count == 0 );
		}

		[TestMethod]
		public async Task IsSimpleCOFValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

			var eventStyleOrig = (await client.GetEventStyleDefinitionAsync( setName )).Value;
			var eventStyle = eventStyleOrig.Clone();

			var validation = new IsEventStyleSimpleCOFsValid();

			//Unalterned should pass
			var valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
			Assert.IsTrue( validation.Messages.Count == 0 );

			//A fake cof set name should fail
			eventStyle.SimpleCOFs[0].CourseOfFireDef = "v1.0:orion:not a real set name" ;
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//An empty Commponents list should fail
			eventStyle = eventStyleOrig.Clone();
			eventStyle.SimpleCOFs[0].Components.Clear() ;
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A Component with 0 shots should fail
			eventStyle = eventStyleOrig.Clone();
			eventStyle.SimpleCOFs[0].Components[0].Shots = 0;
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A Component with an empthy string for the stage style, should fail
			eventStyle = eventStyleOrig.Clone();
			eventStyle.SimpleCOFs[0].Components[0].StageStyleDef = "";
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );

			//A Component with a real stage style, but one not listed, should fail
			eventStyle = eventStyleOrig.Clone();
			eventStyle.SimpleCOFs[0].Components[0].StageStyleDef = "v1.0:nra:BB Gun Kneeling";
			valid = await validation.IsSatisfiedByAsync( eventStyle );
			Assert.IsFalse( valid );
			Assert.IsTrue( validation.Messages.Count > 0 );
		}
	}
}
