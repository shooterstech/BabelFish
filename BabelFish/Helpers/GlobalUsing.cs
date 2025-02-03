global using System;
global using System.Collections.Generic;
global using System.Text;
global using Scopos.BabelFish.Helpers;
global using NLog;

/*
 * When applying Attributes to Properties, by convention, list System.Text.Json attributes first,
 * then Newtonsoft.Json attributes,and finally any system attributes.
 * 
 * Use the global defined using names below. And Remove using statements for these name spaces from the top of .cs files.
 * 
 * For example
 * 
    [G_STJ_SER.JsonPropertyOrder( 1 )]
    [G_NS.JsonProperty( Order = 1 )]
    [DefaultValue( "" )]
 */

global using G_NS = Newtonsoft.Json;
global using G_NS_CONV = Newtonsoft.Json.Converters;
global using G_BF_NS_CONV = Scopos.BabelFish.Converters.Newtonsoft;
global using G_STJ = System.Text.Json;
global using G_STJ_SER = System.Text.Json.Serialization;
global using G_BF_STJ_CONV = Scopos.BabelFish.Converters.Microsoft;