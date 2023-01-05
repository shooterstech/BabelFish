using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.DataModel {

    /// <summary>
    /// An ITokenItems interface is for DataModels that are returned as a Response objects primary data model, and then follow
    /// the standard NextToken / Token programming model. 
    /// 
    /// The request needs to have 'token' as an optional query string parameter.
    /// 
    /// The top level response object may return NextToken, to indicate more data is avaliable.
    /// The response object's primary data model must also include a List<>. It is this list that could be combined accross multiple calls. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITokenItems<T> {

        List<T> Items { get; set; }
    }
}
