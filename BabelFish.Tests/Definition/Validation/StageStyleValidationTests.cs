using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using System.Diagnostics;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {

    [TestClass]
    public class StageStyleValidationTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
        public async Task HappyPathStageStyleIsValid() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsStageStyleValid();

            var valid = await validation.IsSatisfiedByAsync( stageStyle );

            Assert.IsTrue( valid, string.Join(", ", validation.Messages ) );
        }

        [TestMethod]
        public async Task IsShotsInSeriesValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsStageStyleShotsInSeriesValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );
            
            //0 shots should fail
            stageStyle.ShotsInSeries = 0;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //101 shots should fail
            stageStyle.ShotsInSeries = 101;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }

        [TestMethod]
        public async Task IsScoreFormatCollectionDefValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsStageStyleScoreFormatCollectionDefValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //null should fail
            stageStyle.ScoreFormatCollectionDef = null;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //empty string should fail
            stageStyle.ScoreFormatCollectionDef = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //Invalid set name should fail
            stageStyle.ScoreFormatCollectionDef = "not a real set name";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //valid set name but doesn't exist should fail
            stageStyle.ScoreFormatCollectionDef = "v1.0:orion:not a real definition";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }

        [TestMethod]
        public async Task IsScoreConfigDefaultValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsStageStyleScoreConfigDefaultValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //null should fail
            stageStyle.ScoreConfigDefault = null;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //empty string should fail
            stageStyle.ScoreConfigDefault = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //Invalid valud should fail
            stageStyle.ScoreConfigDefault = "not a real value";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }

        [TestMethod]
        public async Task IsRelatedStageStylesValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsStageStyleRelatedStageStylesValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //An empty list should pass
            stageStyle.RelatedStageStyles.Clear();
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //adding an empty string should fail
            stageStyle.RelatedStageStyles.Clear();
            stageStyle.RelatedStageStyles.Add( "" );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //adding null should fail
            stageStyle.RelatedStageStyles.Clear();
            stageStyle.RelatedStageStyles.Add( null );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //adding an fake set name should fail
            stageStyle.RelatedStageStyles.Clear();
            stageStyle.RelatedStageStyles.Add( "v1.0:orion:not a real definition" );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //adding an real one and a fake set name should fail
            stageStyle.RelatedStageStyles.Clear();
            stageStyle.RelatedStageStyles.Add( "v1.0:nra:Sporter Air Rifle Standing" );
            stageStyle.RelatedStageStyles.Add( "v1.0:orion:not a real definition" );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }
    }
}
