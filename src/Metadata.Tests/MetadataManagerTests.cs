using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public sealed class MetadataManagerTests
    {
        private string _catalogPath = "C:\\temp";
        private string _connectionString = "Data Source=ZHICHKIN;Initial Catalog=accounting_3_0_72_72_demo;Integrated Security=True";
        
        [TestMethod]
        public void DBNames()
        {
            string logPath = Path.Combine(_catalogPath, "log.txt");
            MetadataManager manager = new MetadataManager(logPath);
            manager.ImportMetadataToFiles(_connectionString, _catalogPath);
        }
    }
}
