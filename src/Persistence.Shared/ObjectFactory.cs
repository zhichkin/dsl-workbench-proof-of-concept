using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IObjectFactory
    {
        ///// <summary>
        ///// Creates new instance of the given type
        ///// </summary>
        //IReferenceObject New(Type type); // New
        ///// <summary>
        ///// Creates new instance of the given type with specified identity value
        ///// </summary>
        //IReferenceObject New(Type type, Guid identity); // New
        ///// <summary>
        ///// Creates virtual instance of the given type code and identity value
        ///// </summary>
        //IReferenceObject New(int typeCode, Guid identity); // Virtual
        ///// <summary>
        ///// Creates virtual instance of the given type and identity value
        ///// </summary>
        //T New<T>(Guid identity) where T : IReferenceObject; // Virtual
    }
}
