using OneCSharp.Metadata.Model;

namespace OneCSharp.Metadata.Services
{
    public interface IMetadataSerializer
    {

    }
    public sealed class MetadataSerializer : IMetadataSerializer
    {
        private ILogger _logger;
        public void UseLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Serialize(Entity dbo)
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
                    string typeName = (type.Entity == null) ? type.Name : type.Entity.TableName;
                    typeCodesString += (string.IsNullOrEmpty(typeCodesString) ? string.Empty : ", ") + $"{typeName}";
                }
                _logger.WriteEntry($"   + {property.Name} ({typeCodesString})");
                foreach (var field in property.Fields)
                {
                    _logger.WriteEntry($"      {field.Name} {field.TypeName}({(field.Length > 0 ? field.Length : field.Precision)}) {(field.IsNullable ? "NULL" : "NOT NULL")} [{field.Purpose}]");
                }
            }
            foreach (var nested in dbo.NestedEntities)
            {
                Serialize((Entity)nested);
            }
        }
    

    }
}
