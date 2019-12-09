using System;
using System.Collections.Generic;

namespace OneCSharp.DSL.Services
{
    public interface IReferenceResolver
    {
        void Clear();
        string GetReference(object value, ref bool isNew);
        object ResolveReference(string reference);
    }
    public class JsonReferenceResolver : IReferenceResolver
    {
        private readonly IDictionary<Guid, object> _map_id_to_object = new Dictionary<Guid, object>();
        private readonly IDictionary<object, Guid> _map_object_to_id = new Dictionary<object, Guid>();

        public void Clear()
        {
            _map_id_to_object.Clear();
            _map_object_to_id.Clear();
        }
        public string GetReference(object value, ref bool isNew)
        {
            Guid id;
            if (_map_object_to_id.TryGetValue(value, out id))
            {
                isNew = false;
                return id.ToString();
            }

            id = Guid.NewGuid();
            _map_object_to_id.Add(value, id);

            isNew = true;
            return id.ToString();
        }

        public object ResolveReference(string reference)
        {
            Guid id = new Guid(reference);

            object value;
            _map_id_to_object.TryGetValue(id, out value);

            return value;
        }
    }
}
