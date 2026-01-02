using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class CourseOfFire {

		public CourseOfFire() {

		}

		/// <summary>
		/// Indicates if the EST Unit is in Practice Mode, which generally gives the athlete on the firing line more control over the EST Unit.
		/// </summary>
		public bool PracticeMode { get; set; }

		/// <summary>
		/// The SetName of the Course of Fire Definition in use.
		/// </summary>
		public string Definition { get; set; }

		public int RangeScriptIndex { get; set; }

		public int SegmentGroupIndex { get; set; }

		public int CommandIndex { get; set; }

		public int SegmentIndex { get; set; }

		public string Override { get; set; }

		public string CourseOfFireID { get; set; }

		public string ScoreConfigName { get; set; }

		public string TargetCollectionName { get; set; }

		/// <summary>
		/// The SetName of the Event and Stage Style Mapping definition that is used to calcualte the Event Style and Stage Style for the competitor.
		/// </summary>
		public string MappingDef { get; set; }

		public bool Reset { get; set; }
	}
}