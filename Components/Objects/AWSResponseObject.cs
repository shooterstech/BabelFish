using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BabelFish.Components.Objects
{
	public class AWSResponseObject
	{
		public AWSResponseObject(){}

        #region Properties
		/// <summary>
		/// SerializedResponse from AWS API
		/// </summary>
        public string SerializedResponse { get; set; } = string.Empty;

		/// <summary>
		/// Response Format Type - json default
		/// </summary>
		public string ResponseType { get; set; } = AWSUtility.ResponseTypeEnum.json.ToString();

		/// <summary>
		/// List of any Errors encountered
		/// </summary>
		private List<string> _Errors = new List<string>();
		public List<string> Errors
        {
			get { return _Errors; }
			set 
			{
				// Deep copy on set
				_Errors.Clear();
				foreach ( string error in value )
					_Errors.Add( error );
			}
        }
        #endregion Properties

        #region Methods
        #endregion Methods
    }
}
