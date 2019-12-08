namespace OneCSharp.Metadata.Services
{
    // Help class for Query Builder SELECT statement

    //public sealed class SelectPropertyManager
    //{
    //    private readonly PropertyReference property;

    //    private Dictionary<int, string> ordinals = new Dictionary<int, string>();
    //    private Dictionary<DbFieldPurpose, int> purposes = new Dictionary<DbFieldPurpose, int>();

    //    public PropertyReferenceManager(PropertyReference property)
    //    {
    //        this.property = property;
    //    }
    //    public string ToSQL()
    //    {
    //        string sql = "";
    //        foreach (KeyValuePair<int, string> item in ordinals)
    //        {
    //            sql += item.Value;
    //        }
    //        return sql;
    //    }
    //    public void Prepare(ref int currentOrdinal) // start ordinal for the table fields in SELECT clause - move to constructor ?
    //    {
    //        string propertyAlias = ((PropertyObject)property.PropertySource).Name;
    //        AliasSyntaxNode alias = property.Parent as AliasSyntaxNode;
    //        if (alias != null) { propertyAlias = alias.Alias; }

    //        bool isMultiValued = (property.Property.Fields.Count > 1);

    //        foreach (Field field in property.Property.Fields)
    //        {
    //            string name = string.Empty;
    //            if (currentOrdinal > 0) { name += $"\n\t,"; }

    //            name += $"[{property.Table.Alias}].[{field.Name}] AS ";
    //            if (isMultiValued)
    //            {
    //                name += $"[{propertyAlias}_{GetDbFieldPurposeSuffix(field)}]";
    //            }
    //            else
    //            {
    //                name += $"[{propertyAlias}]";
    //            }
    //            ordinals.Add(currentOrdinal, name);
    //            purposes.Add(field.Purpose, currentOrdinal);
    //            currentOrdinal++;
    //        }
    //    }


    //    private string GetDbFieldPurposeSuffix(DbField field)
    //    {
    //        if (field.Purpose == DbFieldPurpose.Discriminator)
    //        {
    //            return "TYPE";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.TypeCode)
    //        {
    //            return "T";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.Value)
    //        {
    //            return string.Empty;
    //        }
    //        else if (field.Purpose == DbFieldPurpose.Object)
    //        {
    //            return "R";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.Boolean)
    //        {
    //            return "L";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.Numeric)
    //        {
    //            return "N";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.String)
    //        {
    //            return "S";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.Binary)
    //        {
    //            return "B";
    //        }
    //        else if (field.Purpose == DbFieldPurpose.DateTime)
    //        {
    //            return "D";
    //        }
    //        else
    //        {
    //            return string.Empty;
    //        }
    //    }

    //    public object GetValue(IDataReader reader)
    //    {
    //        if (property.Property.Fields.Count == 1)
    //        {
    //            return TranslateSingleType(reader);
    //        }
    //        else
    //        {
    //            return TranslateComplexType(reader);
    //        }
    //    }
    //    private object TranslateSingleType(IDataReader reader)
    //    {
    //        DbField field = property.Property.Fields[0];
    //        int index = purposes[field.Purpose]; // DbFieldPurpose.Value || DbFieldPurpose.Object

    //        object value = reader[index];
    //        if (value == DBNull.Value) return DbUtilities.GetDefaultValueAsObject(field);

    //        if (field.Purpose == DbFieldPurpose.Object)
    //        {
    //            DbObject entity = property.Property.Relations[0].Entity;

    //            Guid identity = Guid.Empty;
    //            if (value is Guid)
    //            {
    //                identity = (Guid)value;
    //            }
    //            else
    //            {
    //                identity = new Guid((byte[])value);
    //            }

    //            return new ReferenceProxy(entity, identity);
    //        }

    //        return value;
    //    }
    //    private object TranslateComplexType(IDataReader reader)
    //    {
    //        byte[] buffer;
    //        object value;

    //        int typeCode = -1;
    //        if (purposes.TryGetValue(DbFieldPurpose.TypeCode, out typeCode))
    //        {
    //            value = reader[typeCode];
    //            if (value is int)
    //            {
    //                typeCode = (int)value;
    //            }
    //            else
    //            {
    //                buffer = (byte[])value;
    //                typeCode = BitConverter.ToInt32(buffer, 0);
    //            }
    //        }

    //        if (typeCode > 0) // ReferenceObject
    //        {
    //            Relation relation = property.Property.Relations.Where(r => r.Entity.Code == typeCode).FirstOrDefault();
    //            if (relation == null) return null; // unsupported value type for this property

    //            int index = -1;
    //            if (!purposes.TryGetValue(DbFieldPurpose.Object, out index))
    //            {
    //                index = purposes[DbFieldPurpose.Value];
    //            }

    //            Guid identity = Guid.Empty;
    //            value = reader[index];
    //            if (value is Guid)
    //            {
    //                identity = (Guid)value;
    //            }
    //            else
    //            {
    //                buffer = (byte[])reader[index];
    //                identity = new Guid(buffer);
    //            }

    //            return new ReferenceProxy(relation.Entity, identity);
    //        }

    //        // primitive types

    //        int locator = purposes[DbFieldPurpose.Discriminator];
    //        buffer = (byte[])reader[locator];
    //        locator = buffer[0];

    //        value = null;
    //        if (locator == 1) // Неопределено
    //        {
    //            return null;
    //        }
    //        else if (locator == 2) // Булево
    //        {
    //            value = ((byte[])reader[purposes[DbFieldPurpose.Boolean]])[0] == 0 ? false : true;
    //        }
    //        else if (locator == 3) // Число
    //        {
    //            value = (decimal)reader[purposes[DbFieldPurpose.Numeric]];
    //        }
    //        else if (locator == 4) // Дата
    //        {
    //            value = reader.GetDateTime(purposes[DbFieldPurpose.DateTime]);
    //        }
    //        else if (locator == 5) // Строка
    //        {
    //            value = (string)reader[purposes[DbFieldPurpose.String]];
    //        }
    //        else if (locator == 8) // Ссылка
    //        {
    //            Relation relation = property.Property.Relations.Where(r => r.Entity.Code == typeCode).FirstOrDefault();
    //            if (relation == null) return null; // unsupported value type for this property

    //            buffer = (byte[])reader[purposes[DbFieldPurpose.Object]];
    //            Guid identity = new Guid(buffer);
    //            value = new ReferenceProxy(relation.Entity, identity);
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //        return value;
    //    }
    //}
}
