using System.ComponentModel;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Converters.TypeConverters {
    public class SetNameConverter : TypeConverter {
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            return sourceType == typeof( string ) || base.CanConvertFrom( context, sourceType );
        }

        public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value ) {
            if (value is string s) {
                return SetName.Parse( s ); // or whatever your constructor/parse logic is
            }

            return base.ConvertFrom( context, culture, value );
        }
    }
}
