using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public sealed class MetadataManagerTests
    {
        private string _catalogPath = "C:\\temp";
        private string _infoBaseName = "reverse_engineering"; //"accounting_3_0_72_72_demo";
        private string _serverAddress = "ZHICHKIN";

        [TestMethod]
        public void DBNames()
        {
            string logPath = Path.Combine(_catalogPath, "log.txt");
            TextFileLogger logger = new TextFileLogger(logPath);
            MetadataServer manager = new MetadataServer(_serverAddress);
            manager.UseLogger(logger);
            
            InfoBase infoBase = manager.GetInfoBases()
                .Where(ib => ib.Database == _infoBaseName)
                .FirstOrDefault();

            if (infoBase == null)
            {
                logger.WriteEntry($"InfoBase {_infoBaseName} not found at server address {_serverAddress}");
                return;
            }

            //manager.ImportMetadata(infoBase, true);
            manager.ImportMetadata(infoBase, false);

            //MetadataSerializer serializer = new MetadataSerializer();
            //serializer.UseLogger(logger);
            //foreach (var ns in infoBase.Namespaces)
            //{
            //    foreach (var dbo in ns.DbObjects)
            //    {
            //        serializer.Serialize(dbo);
            //    }
            //}
        }
    }
}
