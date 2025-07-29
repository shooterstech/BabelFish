using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {


    /// <summary>
    /// Classes that reference COURSE OF FIRE Definitions should implement this interface.
    /// </summary>
    public interface IGetCourseOfFireDefinition {

        /// <summary>
        /// Retreives the COURSE OF FIRE Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<CourseOfFire> GetCourseOfFireDefinitionAsync();
    }
}
