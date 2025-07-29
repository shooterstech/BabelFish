﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Strings formatted as value series will have the form
    /// *        Applies to all 
    /// n        Applies to the nth value in the list.
    /// n..m     Applies to the nth through the mth values in the list.
    /// n..m,s   Applies to the nth through the mth values in the list, with a step of s.
    /// where n is start value, m is end value, s is step
    /// </summary>
    public class ValueSeries {

        private string format = "";

        public ValueSeries(string format) {
            this.format = format;
            Parse();
        }

        public int StartValue { get; private set;}

        public int EndValue { get; private set; }

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

            var intStrings = format.Split(new string[] { "..", "," }, StringSplitOptions.RemoveEmptyEntries);

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
        }
    }
}
