using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public sealed class MetadataManagerTests
    {
        private string _catalogPath = "C:\\Users\\User\\Desktop\\GitHub\\one-c-sharp-server\\src\\reving"; //"C:\\temp";
        private string _connectionString = "Data Source=ZHICHKIN;Initial Catalog=reverse_engineering;Integrated Security=True"; // accounting_3_0_72_72_demo

        [TestMethod]
        public void DBNames()
        {
            string logPath = Path.Combine(_catalogPath, "log.txt");
            MetadataManager manager = new MetadataManager(logPath);
            manager.ImportMetadata(_connectionString, _catalogPath);
            manager.ImportMetadataToFiles(_connectionString, _catalogPath);
        }
    }
}
