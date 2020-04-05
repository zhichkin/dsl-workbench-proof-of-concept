using System;
using System.Collections.Generic;

namespace OneCSharp.Integrator.Model
{
    public sealed class DummyAssembly
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public List<DummyNamespace> Namespaces { get; } = new List<DummyNamespace>();
    }
    public sealed class DummyNamespace
    {
        public string Name { get; set; } = string.Empty;
        public List<DummyType> Types { get; } = new List<DummyType>();
        public List<DummyNamespace> Namespaces { get; } = new List<DummyNamespace>();
    }
    public sealed class DummyType
    {
        public string Name { get; set; } = string.Empty;
        public List<DummyType> NestedTypes { get; } = new List<DummyType>();
        public List<DummyProperty> Properties { get; } = new List<DummyProperty>();
    }
    public sealed class DummyProperty
    {
        public string Name { get; set; } = string.Empty;
        public Type PropertyType { get; set; }
        public DummyType PropertyDummyType { get; set; }
    }
}