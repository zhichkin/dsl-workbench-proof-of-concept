using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.Metadata.Server;
using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public class CRUD_Tests
    {
        private static IPersistentContext context;
        private static Guid testKey = new Guid("C2FCA0B4-CE8B-4B4D-B3A2-90A62C4BF04A");
        private static string connectionString = "Data Source=ZHICHKIN;Initial Catalog=Z;Integrated Security=True";
        static CRUD_Tests()
        {
            Initialize();
        }
        public static void Initialize()
        {
            context = new PersistentContext(connectionString);
            context.AddDataPersister(typeof(InfoBase), new InfoBaseDataPersister(context));
        }
        [TestMethod]
        public void GetInfoBaseDataPersisterByType()
        {
            IDataPersister persister = context.GetDataPersister(typeof(InfoBase));
            var expected = persister as InfoBaseDataPersister;
            Assert.AreEqual(typeof(InfoBaseDataPersister), expected.GetType());
        }
        [TestMethod]
        public void GetInfoBaseDataPersisterByTypeCode()
        {
            InfoBase ib = new InfoBase(new Guid("A074E116-B84E-4044-8E4D-9B75F86D84D1"));
            
            IDataPersister persister = context.GetDataPersister(ib.TypeCode);
            var expected = persister as InfoBaseDataPersister;
            Assert.AreEqual(typeof(InfoBaseDataPersister), expected.GetType());
        }
        [TestMethod]
        public void InfoBase_00_Load()
        {
            InfoBase ib = new InfoBase(new Guid("A074E116-B84E-4044-8E4D-9B75F86D84D1"));
            Assert.AreEqual(PersistentState.New, ib.State);

            context.Load(ib);
            Assert.AreEqual(PersistentState.Original, ib.State);

            Console.WriteLine("InfoBase.Name == " + ib.Name);
        }
        [TestMethod]
        public void InfoBase_01_Insert()
        {
            InfoBase ib = new InfoBase(testKey);
            ib.Name = "Test";
            ib.Server = "Test";
            ib.Database = "Test";
            ib.UserName = "Test";
            ib.Password = "Test";

            Assert.AreEqual(PersistentState.New, ib.State);
            context.Save(ib);
            Assert.AreEqual(PersistentState.Original, ib.State);
        }
        [TestMethod]
        public void InfoBase_02_Update()
        {
            InfoBase ib = new InfoBase(testKey);
            
            Assert.AreEqual(PersistentState.New, ib.State);
            context.Load(ib);
            Assert.AreEqual(PersistentState.Original, ib.State);

            ib.Name = "Test*";
            ib.Server = "Test*";
            ib.Database = "Test*";
            ib.UserName = "Test*";
            ib.Password = "Test*";
            Assert.AreEqual(PersistentState.Changed, ib.State);
            context.Save(ib);
            Assert.AreEqual(PersistentState.Original, ib.State);
        }
        [TestMethod]
        public void InfoBase_03_Delete()
        {
            InfoBase ib = new InfoBase(testKey);

            Assert.AreEqual(PersistentState.New, ib.State);
            context.Load(ib);
            Assert.AreEqual(PersistentState.Original, ib.State);

            context.Kill(ib);
            Assert.AreEqual(PersistentState.Deleted, ib.State);
        }
        [TestMethod]
        public void ObjectReference_Test()
        {
            InfoBase ib = new InfoBase(new Guid("A074E116-B84E-4044-8E4D-9B75F86D84D1"));
            context.Load(ib);

            var obj1 = ib.GetReference();
            var obj2 = new ObjectReference(ib);
            var obj3 = new ObjectReference(ib.TypeCode, ib.PrimaryKey, ib.ToString());
            Assert.AreEqual(obj1, obj2);
            Assert.AreEqual(obj1, obj3);
            Assert.AreEqual(obj2, obj3);

            bool equal = (obj1 == obj2);
            Assert.AreEqual(true, equal);

            bool notEqual = object.ReferenceEquals(obj1, obj2);
            Assert.AreEqual(false, notEqual);

            Assert.AreEqual(obj1.Presentation, obj2.Presentation);
        }
    }
}
