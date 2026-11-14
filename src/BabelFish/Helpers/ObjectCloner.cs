
namespace Scopos.BabelFish.Helpers {

    /// <summary>
    /// Provides a method for performing a deep copy-ish of an object using JSON serialization and deserialization.
    /// The drawback of this method is that it is SLOW and only copies the public properties of the object. If there
    /// are non-public properties that need to be copied these may be implemented using the IOnCloned interface, which
    /// will be called after cloning is complete, allowing for any necessary post-processing after cloning, such as setting any backward pointers.
    /// </summary>
    public static class ObjectCloner {

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. 
        /// The drawback of this method is that it is SLOW and only copies the public properties of the object. If there
        /// are non-public properties that need to be copied these may be implemented using the IOnCloned interface, which
        /// will be called after cloning is complete, allowing for any necessary post-processing after cloning, such as setting any backward pointers.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        /// <remarks>Because this method uses JSON serialization and deserialization to create a deep copy of the instance, which is SLOW,
        /// if using this method repeatedly, consider authoring a custom deep copy method (or copy constructor) for the object.</remarks>
        public static T Clone<T>( this T source ) {
            if (source == null) {
                return default;
            }

            var options = SerializerOptions.SystemTextJsonDeserializer;

            string jsonString = G_NS.JsonConvert.SerializeObject( source, Helpers.SerializerOptions.NewtonsoftJsonSerializer );
            var clonedObject = G_STJ.JsonSerializer.Deserialize<T>( jsonString, options );

            if (clonedObject is IOnCloned onCloned) {
                onCloned.OnCloned( source );
            }

            return clonedObject;
        }
    }

    /// <summary>
    /// The ObjectCloner.Clone method will call the OnCloned method of this interface if implemented by the type being cloned, allowing for any necessary post-processing after cloning.
    /// Such as setting any backward pointers.
    /// </summary>
    public interface IOnCloned {

        /// <summary>
        /// Called by ObjectCloner.Clone after cloning is complete, allowing for any necessary post-processing after cloning, such as setting any backward pointers.
        /// </summary>
        /// <param name="source">The original object that was cloned.</param>
        void OnCloned( object source );
    }
}
