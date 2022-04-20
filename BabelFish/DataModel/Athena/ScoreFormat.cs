using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.Athena {
    /// <summary>
    /// Depricated in favor of ShootersTech.DataModel.Athena.ScoreFormat, which doesn't use the rulebook specifics or obfuscated values.
    ///  * i => Display the Integer Score(I).
    ///  * d => Display the Decimal Score(D).
    ///  * x => Display the Inner Ten ring score(X).
    ///  * X => Display an asterix(*) if number of inner tens is greater than 0
    ///  * s => Display the special sum score(S). If S is not included the Decimal Score is displayed instead.
    ///  * All other characters are carried forward.
    /// </summary>
    public class ScoreFormat {

        private string format = "d";

        public ScoreFormat() {
            this.format = "d";
        }

        public ScoreFormat(string format) {
            this.format = format;
        }

        public string Format {
            get { return format; }
            set { format = value; }
        }

        public string FormattedScore(Score s) {
            StringBuilder value = new StringBuilder();
            foreach (char c in format.ToCharArray()) {
                switch (c) {
                    case 'i':
                        value.Append(s.I);
                        break;
                    case 'd':
                        value.Append(s.D.ToString("F1"));
                        break;
                    case 'x':
                        value.Append(s.X);
                        break;
                    case 'X':
                        if (s.X > 0)
                            value.Append('*');
                        break;
                    case 's':
                        value.Append(s.S.ToString("F1"));
                        break;
                    default:
                        value.Append(c);
                        break;
                }
            }
            return value.ToString();
        }

        public static bool ValidFormat(string format) {
            return format.Contains("i")
                || format.Contains("d")
                || format.Contains("x")
                || format.Contains("X")
                || format.Contains("s");
        }
    }
}
