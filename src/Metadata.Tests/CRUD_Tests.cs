using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.Metadata.Server;
using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;
using System.Linq;
using System.Reflection;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public class CRUD_Tests
    {
        private static IPersistentContext context;
        private static string connectionString = "Data Source=ZHICHKIN;Initial Catalog=one-c-sharp;Integrated Security=True";

        private static Guid testKey = new Guid("C2FCA0B4-CE8B-4B4D-B3A2-90A62C4BF04A");
        private static Guid _namespaceTestKey1 = new Guid("A9A23DD5-F897-4DF5-9CA7-897CAE12D67B");
        private static Guid _namespaceTestKey2 = new Guid("83FF8A8C-4341-48FE-BC40-3C2AACE43111");
        private static Guid _entityTestKey = new Guid("E2245197-FE1F-4E68-A667-99EC80259920");

        static CRUD_Tests()
        {
            Initialize();
        }
        public static void Initialize()
        {
            context = new PersistentContext(connectionString);
            context.AddDataPersister(typeof(InfoBase), new InfoBaseDataPersister(context));
            context.AddDataPersister(typeof(Namespace), new NamespaceDataPersister(context));
            context.AddDataPersister(typeof(Entity), new EntityDataPersister(context));
        }
        [TestMethod]
        public void GetInfoBaseDataPersisterByType()
        {
            IDataPersister persister = context.GetDataPersister(typeof(InfoBase));
            var expected = persister as InfoBaseDataPersister;
            Assert.AreEqual(typeof(InfoBaseDataPersister), expected.GetType());
        }
       
        [TestMethod]
        public void GetMetadataAttributes_Test()
        {
            Type type = typeof(InfoBase);

            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                Console.WriteLine($"InfoBase implements {iface.Name} interface");
            }

            var list = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
            foreach (var item in list)
            {
                Console.WriteLine($"1. {((PrimaryKeyAttribute)item).Name}");
            }

            list = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            foreach (var item in list)
            {
                Console.WriteLine($"2. {((PrimaryKeyAttribute)item).Name}");
            }

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                string info = string.Empty;
                var fields = property.GetCustomAttributes(typeof(FieldAttribute), false);
                foreach (var field in fields)
                {
                    var a = (FieldAttribute)field;
                    info += $"{a.Name} ({a.TypeName}),";
                }
                //if (fields.Length > 0)
                //{
                    Console.WriteLine($"{property.Name} : {info}");
                //}
            }
            
            Type i = type.GetInterface("IPersistentObject`1");
            if (i != null)
            {
                Console.WriteLine($"{i.GenericTypeArguments[0].ToString()}");
            }

            i = type.GetInterface("IVersion");
            if (i != null)
            {
                string info = string.Empty;
                var property = i.GetProperties()[0];
                Console.WriteLine($"{property.ToString()}");

                var fields = property.GetCustomAttributes(typeof(FieldAttribute), false);
                foreach (var field in fields)
                {
                    var a = (FieldAttribute)field;
                    info += $"{a.Name} ({a.TypeName}),";
                }
                Console.WriteLine($"{property.Name} : {info}");
            }

            i = type.GetInterfaces().Where((i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPersistentObject<>)).FirstOrDefault();
            Console.WriteLine($"{i.ToString()}");

            var optimist = typeof(IVersion).IsAssignableFrom(type);
            Console.WriteLine($"{type} implements IVersion = {optimist}");
        }
    }
}
