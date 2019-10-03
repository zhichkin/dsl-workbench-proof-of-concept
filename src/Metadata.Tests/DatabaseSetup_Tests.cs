using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.Metadata.Server;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public class DatabaseSetup_Tests
    {
        [TestMethod]
        public void GetAppSettingsFile()
        {
            DatabaseSetupService dbs = new DatabaseSetupService();
            Assert.IsTrue(dbs.CheckServerConnection());
            Assert.IsTrue(dbs.CheckDatabaseConnection());
            dbs.SetupDatabase();
            Assert.IsTrue(dbs.CheckTables());
        }
    }
}
