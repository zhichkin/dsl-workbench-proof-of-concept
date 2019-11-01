using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCSharp.Metadata
{
    public sealed class MetadataSerializer
    {
        private ILogger _logger;
        public void UseLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Serialize(DbObject dbo)
        {
            if (dbo.Owner == null)
            {
                _logger.WriteEntry("");
                _logger.WriteEntry("***");
            }
            _logger.WriteEntry($"{dbo.Name} ({dbo.TableName}) {dbo.Token} + {dbo.TypeCode}");
            foreach (var property in dbo.Properties)
            {
                string typeCodesString = "";
                foreach (var type in property.Types)
                {
                    string typeName = (type.DbObject == null) ? type.Name : type.DbObject.TableName;
                    typeCodesString += (string.IsNullOrEmpty(typeCodesString) ? string.Empty : ", ") + $"{typeName}";
                }
                _logger.WriteEntry($"   + {property.Name} ({typeCodesString})");
                foreach (var field in property.Fields)
                {
                    _logger.WriteEntry($"      {field.Name}");
                }
            }
            foreach (var nested in dbo.NestedObjects)
            {
                Serialize(nested);
            }
        }
    }
}
