using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Strings formatted as value series will have the form:
    /// <list type="bullet">
    /// <item>*        Applies to all </item>
    /// <item>n        Applies to the nth value in the list.</item>
    /// <item>n..m     Applies to the nth through the mth values in the list.</item>
    /// <item>n..m,s   Applies to the nth through the mth values in the list, with a step of s.</item>
    /// </list>
    /// where n is start value, m is end value, s is step. If unable to parse then "*" is assumed. 
    /// </summary>
    public class ValueSeries {

        public const string APPLY_TO_ALL_FORMAT = "*";

        private string ? _format = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="format">
        /// format must be one of the following. If unable to parse then "*" is assumed. 
        /// <list type="bullet">
        /// <item>*        Applies to all </item>
        /// <item>n        Applies to the nth value in the list.</item>
        /// <item>n..m     Applies to the nth through the mth values in the list.</item>
        /// <item>n..m,s   Applies to the nth through the mth values in the list, with a step of s.</item>
        /// </list>
        /// where n is start value, m is end value, s is step
        /// </param>
        public ValueSeries(string ? format) {
            this._format = format;
            Parse();
        }

        /// <summary>
        /// The starting value of this Value Series.
        /// </summary>
        /// <remarks>
        /// Value series have a starting index value of 1, not 0 (we do apologize emphatically for this). 
        /// </remarks>
        public int StartValue { get; private set;}


        /// <summary>
        /// The ending value of this Value Series.
        /// </summary>
        /// <remarks>
        /// Value series have a starting index value of 1, not 0 (we do apologize emphatically for this). 
        /// </remarks>
        public int EndValue { get; private set; }

        /// <summary>
        /// The step value for this Value Series.
        /// </summary>
        /// <remarks>Step values are almost always 1 or -1.</remarks>
        public int Step { get; private set; }

        /// <summary>
        /// Interpres the Value Series, and returns a compiled and complete list of integers
        /// as specified by the Value Series.
        /// </summary>
        /// <returns></returns>
        public List<int> GetAsList() {
            List<int> list = new List<int>();

            if (StartValue > EndValue)
                for (int i = StartValue; i >= EndValue; i -= Step)
                    list.Add( i );
            else
                for (int i = StartValue; i <= EndValue; i += Step)
                    list.Add( i );


            return list;
        }

        /// <summary>
        /// Interprets the Value Series and returns a list of compiled event names.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the passed in eventName does not contain the place holder '{}'.</exception>
        public List<string> GetAsList(string eventName) {
            if (!eventName.Contains( "{}" ))
                throw new ArgumentException( $"The passed in eventName '{eventName}' string must contain '{{}}' for its values to be replaced." );

            List<string> list = new List<string>();

            if (StartValue > EndValue)
                for (int i = StartValue; i >= EndValue; i -= Step)
                    list.Add( eventName.Replace( "{}", i.ToString() ) );
            else
                for (int i = StartValue; i <= EndValue; i += Step)
                    list.Add( eventName.Replace( "{}", i.ToString() ) );

            return list;
        }

        private void Parse() {

            //Check for the special case of "*"
            if ( string.IsNullOrEmpty( _format) ) {
                StartValue = 1;
                EndValue = 1;
                Step = 1;
                return;
            }

            if (_format == APPLY_TO_ALL_FORMAT) {
                StartValue = 1;
                EndValue = int.MaxValue;
                Step = 1;
                return;
            }

            var intStrings = _format.Split(new string[] { "..", "," }, StringSplitOptions.RemoveEmptyEntries);

            List<int> list = new List<int>();

            foreach (var intStr in intStrings) {
                int a;
                if (int.TryParse(intStr, out a))
                    list.Add(a);
            }

            switch (list.Count) {
                case 0:
                    StartValue = 1;
                    EndValue = 1;
                    Step = 1;
                    break;
                case 1:
                    StartValue = list[0];
                    EndValue = list[0];
                    Step = 1;
                    break;
                case 2:
                    StartValue = list[0];
                    EndValue = list[1];
                    Step = 1;
                    break;
                case 3:
                default:
                    StartValue = list[0];
                    EndValue = list[1];
                    Step = Math.Abs( list[2] );
                    break;
            }

            //Do not allow Step values of 0. Which would cause an infinite loop if allowed.
            if (Step == 0)
                Step = 1;
        }

        /// <inheritdoc/>
        /// <remarks>Returns this ValueSeries in the fomat "n..m,s"</remarks>
        public override string ToString() {
            if ( Step == 1 
                && StartValue == EndValue )
                return StartValue.ToString();

            if (Step == 1)
                return $"{StartValue}..{EndValue}";

            if (Step == 1 
                && StartValue == 1 
                && EndValue == int.MaxValue)
                return APPLY_TO_ALL_FORMAT;

            return $"{StartValue}..{EndValue}, {Step}";
        }
    }
}
