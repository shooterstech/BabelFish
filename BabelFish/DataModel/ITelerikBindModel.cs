using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel {

	/// <summary>
	/// When populating a Telerik DropDown or ComboBox with a List of objects, this interface
	/// defines common TextField and ValueField properties that may be used as the values for
	/// Telerik TextField and ValueField properties.
	/// </summary>
	/// <example>
	/// Due to limitations of Telerik the ValueField must be a primitive type. See
	/// https://docs.telerik.com/blazor-ui/knowledge-base/dropdowns-get-model?_ga=2.179678756.1060757192.1700668503-1862418264.1683900263&_gl=1*1axe6pe*_gcl_au*ODUxMjU2NzMwLjE3MDAyNTAwNDA.*_ga*MTg2MjQxODI2NC4xNjgzOTAwMjYz*_ga_9JSNBCSF54*MTcwMDY2ODUwMy4xNC4xLjE3MDA2Njk3MzAuMTMuMC4w
	/// And this interface limits it further by forcing a string. In order to get the
	/// object value need to write a OnChange routine that loops through the list
	/// looking for the object with the selected ValueField.
	/// <code>
	///     void GetItemFromModelData()
	///{
	/// extract the data item from the data source by using the value
	///MyDdlModel selectedItem = myDdlData.Where( d => d.MyValueField == DdlValue ).FirstOrDefault();
	///if (selectedItem != null) // e.g., custom text in a combo, or no match for an autocomplete
	///        {
	///            result = selectedItem.MyTextField;
	///	}
	///		else {
	///	result = "no item selected";
	///	}
	///}
	/// </code>
	/// </example>
	public interface ITelerikBindModel {

		/// <summary>
		/// Short, human redable field. Should be unique within all instances of the class.
		/// The string value that the user will see and select from. Each object in the list
		/// should have a unique TextField value.
		/// </summary>
		public string TextField { get; }

		/// <summary>
		/// Unique string value representing this instance. 
		/// A unique string value that represents the object. Each object in the list
		/// must have a unique ValueField value.
		/// </summary>
		public string ValueField {
			get;
		}
	}
}
