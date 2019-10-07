using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IObjectFactory
    {
        /// <summary>
        /// Creates new instance of the given type
        /// </summary>
        IPersistentObject New(Type type);
        /// <summary>
        /// Creates new instance by type code
        /// </summary>
        IPersistentObject New(int typeCode);
        /// <summary>
        /// Creates new instance of the given type and key value
        /// </summary>
        IPersistentObject New(Type type, object key);
        /// <summary>
        /// Creates new instance by type code and key value
        /// </summary>
        IPersistentObject New(int typeCode, object key);
    }
}
