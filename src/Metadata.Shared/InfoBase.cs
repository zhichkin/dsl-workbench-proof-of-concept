using OneCSharp.Persistence.Shared;
using System;
using System.Collections.Generic;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(1)]
    public sealed class InfoBase : MetadataObject
    {
        public InfoBase() : base() { }
        public InfoBase(Guid key) : base(key) { }
        public override int TypeCode { get { return 1; } }

        private string _server = string.Empty;
        private string _database = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private List<Namespace> _namespaces = new List<Namespace>();

        public string Server { set { Set<string>(value, ref _server); } get { return Get<string>(ref _server); } }
        public string Database { set { Set<string>(value, ref _database); } get { return Get<string>(ref _database); } }
        public string UserName { set { Set<string>(value, ref _username); } get { return Get<string>(ref _username); } }
        public string Password { set { Set<string>(value, ref _password); } get { return Get<string>(ref _password); } }
        public List<Namespace> Namespaces { get { return _namespaces; } }
    }
}
