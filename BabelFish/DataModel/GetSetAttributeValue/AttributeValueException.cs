using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.Runtime;

namespace ShootersTech.DataModel.GetSetAttributeValue
{
    [Serializable]
    public class InvalidBabelFishException : ShootersTechException {
        // https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        public InvalidBabelFishException() : base() { }
        public InvalidBabelFishException(string message) : base(message) { }
        public InvalidBabelFishException(string message, Exception inner) : base(message, inner) { }
    }

    internal static class AttributeValueException
    {
        public static string GetExceptionDefinitionError(string extraText = "")
        {
            return ExceptionTextFor("DefinitionError", extraText);
        }
        public static string GetExceptionFieldNameError(string extraText = "")
        {
            return ExceptionTextFor("FieldNameError", extraText);
        }
        public static string GetExceptionFieldValueError(string extraText = "")
        {
            return ExceptionTextFor("FieldValueError", extraText);
        }
        public static string GetExceptionFieldTypeError(string extraText = "")
        {
            return ExceptionTextFor("FieldTypeError", extraText);
        }
        public static string GetExceptionKeyFieldNameError(string extraText = "")
        {
            return ExceptionTextFor("KeyFieldNameError", extraText);
        }
        public static string GetExceptionJSONParseError(string extraText = "")
        {
            return ExceptionTextFor("JSONParseError", extraText);
        }

        private static string ExceptionTextFor(string exceptionSubject, string extraText = "")
        {
            string returnException = "";
            switch (exceptionSubject)
            {
                case "DefinitionError":
                    returnException = "Definition exception";
                    break;
                case "FieldNameError":
                    returnException = "Field Name exception";
                    break;
                case "FieldValueError":
                    returnException = "Field Value exception";
                    break;
                case "FieldTypeError":
                    returnException = "Field Type exception";
                    break;
                case "KeyFieldNameError":
                    returnException = "Key Field Name exception";
                    break;
                case "JSONParseError":
                    returnException = "Error attempting to parse returned json";
                    break;
                default:
                    returnException = exceptionSubject;
                    break;
            }
            return $"{returnException} {extraText}".TrimEnd(' ');
        }

    }
}
