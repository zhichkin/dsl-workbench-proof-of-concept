using System;

namespace OneCSharp.Core
{
    public enum PropertyPurpose
    {
        /// <summary>Default value.</summary>
        Property,
        /// <summary>This property is used to reference parent (adjacency list).</summary>
        Hierarchy,
        /// <summary>This property is used to reference children (adjacency list).</summary>
        Children,
        /// <summary>The property is used as a dimension.</summary>
        Dimension,
        /// <summary>The property is used as a measure.</summary>
        Measure,
        /// <summary>The property is used to present object in user interface.</summary>
        Presentation,
        /// <summary>The property is used to reference parent type.</summary>
        Inheritance
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class PropertyPurposeAttribute : Attribute
    {
        public PropertyPurposeAttribute(PropertyPurpose purpose)
        {
            Purpose = purpose;
        }
        public PropertyPurpose Purpose { get; private set; }
    }
}