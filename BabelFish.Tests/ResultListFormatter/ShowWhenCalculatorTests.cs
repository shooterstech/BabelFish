using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.ResultListFormatter {
	[TestClass]
	public class ShowWhenCalculatorTests : BaseTestClass {

		/// <summary>
		/// Tests the static method .GetLargestShowWhenResolution()
		/// </summary>
		[TestMethod]
		public void GetLargestResolutionTests() {

			//Basic conditions testing
			var xs = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_LT_SMALL };
			var small = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_SMALL };
			var medium = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_MEDIUM };
			var large = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_LARGE };
			var xLarge = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_EXTRA_LARGE };
			var xxLarge = new ShowWhenVariable() { Condition = ShowWhenCondition.DIMENSION_EXTRA_EXTRA_LARGE };
			Assert.AreEqual( 0, ShowWhenCalculator.GetLargestShowWhenResolution( xs ) );
			Assert.AreEqual( 576, ShowWhenCalculator.GetLargestShowWhenResolution( small ) );
			Assert.AreEqual( 768, ShowWhenCalculator.GetLargestShowWhenResolution( medium ) );
			Assert.AreEqual( 992, ShowWhenCalculator.GetLargestShowWhenResolution( large ) );
			Assert.AreEqual( 1200, ShowWhenCalculator.GetLargestShowWhenResolution( xLarge ) );
			Assert.AreEqual( 1400, ShowWhenCalculator.GetLargestShowWhenResolution( xxLarge ) );

			//Decisively not a show when dimension. Should return 0
			var sg = new ShowWhenSegmentGroup();
			Assert.AreEqual( 0, ShowWhenCalculator.GetLargestShowWhenResolution( sg ) );

			//Equations should return tthe largest of the arguments
			var equationOne = new ShowWhenEquation();
			equationOne.Arguments.Add( small );
			equationOne.Arguments.Add( large );
			Assert.AreEqual( 992, ShowWhenCalculator.GetLargestShowWhenResolution( equationOne ) );

			var equationTwo = new ShowWhenEquation();
			equationTwo.Arguments.Add( medium );
			equationTwo.Arguments.Add( sg );
			Assert.AreEqual( 768, ShowWhenCalculator.GetLargestShowWhenResolution( equationTwo ) );

			//And now some added recursion
			var equationThree = new ShowWhenEquation();
			equationThree.Arguments.Add( equationOne );
			equationThree.Arguments.Add( equationTwo );
			Assert.AreEqual( 992, ShowWhenCalculator.GetLargestShowWhenResolution( equationThree ) );
		}
	}
}
