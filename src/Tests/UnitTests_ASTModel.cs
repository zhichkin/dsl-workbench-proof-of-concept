using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.CodeGenerator;
using OneCSharp.DDL.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.Tests
{
    [TestClass]
    public class UnitTests_ASTModel
    {
        [TestMethod]
        public void Test1()
        {
            var generator = new OneCSharpCodeGenerator();
            Assembly assembly = generator.Generate();
            foreach (Type type in assembly.GetTypes())
            {
                Console.WriteLine($"{type.Name}");
                EntityAttribute entity = type.GetCustomAttribute<EntityAttribute>();
                if (entity != null)
                {
                    Console.WriteLine($"Table name = {entity.TableName}");
                }
                foreach (PropertyInfo property in type.GetProperties())
                {
                    Console.WriteLine($"Property name = {property.Name}");
                    IEnumerable<FieldAttribute> fields = property.GetCustomAttributes<FieldAttribute>();
                    if (fields != null)
                    {
                        foreach (FieldAttribute field in fields)
                        {
                            Console.WriteLine($"Field name = {field.Name}");
                        }
                    }
                }
            }
        }
    }
}
