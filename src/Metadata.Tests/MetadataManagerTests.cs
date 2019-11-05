using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace OneCSharp.Metadata.Tests
{
    [TestClass]
    public sealed class MetadataManagerTests
    {
        private string _catalogPath = "C:\\temp";
        private string _infoBaseName = "trade_11_2_3_159_demo"; //"accounting_3_0_72_72_demo" "reverse_engineering" "trade_11_2_3_159_demo"
        private string _serverAddress = "ZHICHKIN";

        [TestMethod]
        public void DBNames()
        {
            string logPath = Path.Combine(_catalogPath, "log.txt");
            TextFileLogger logger = new TextFileLogger(logPath);
            DbServer server = new DbServer() { Address = _serverAddress };

            MetadataProvider manager = new MetadataProvider();
            manager.AddServer(server);
            manager.UseLogger(logger);
            
            InfoBase infoBase = manager.GetInfoBases(server)
                .Where(ib => ib.Database == _infoBaseName)
                .FirstOrDefault();

            if (infoBase == null)
            {
                logger.WriteEntry($"InfoBase {_infoBaseName} not found at server address {_serverAddress}");
                return;
            }

            infoBase.Server = server;
            manager.ImportMetadata(infoBase);
            //manager.ImportMetadata(infoBase, true);

            //MetadataSerializer serializer = new MetadataSerializer();
            //serializer.UseLogger(logger);

            //foreach (var ns in infoBase.Namespaces.Where(ns => ns.Name == "Reference"))
            //{
            //    foreach (var dbo in ns.DbObjects.Where(dbo => dbo.Name == "АвансовыйОтчетПрисоединенныеФайлы"))
            //    {
            //        serializer.Serialize(dbo);
            //        manager.SaveMetadataToFile(dbo);
            //    }
            //}

            //foreach (var ns in infoBase.Namespaces.Where(ns => ns.Name == "Enum"))
            //{
            //    foreach (var dbo in ns.DbObjects.Where(dbo => dbo.Name == "Периодичность"))
            //    {
            //        serializer.Serialize(dbo);
            //        manager.SaveMetadataToFile(dbo);
            //    }
            //}

            //foreach (var ns in infoBase.Namespaces.Where(ns => ns.Name == "Document"))
            //{
            //    foreach (var dbo in ns.DbObjects.Where(dbo => dbo.Name == "ПоступлениеТоваровУслуг"))
            //    {
            //        serializer.Serialize(dbo);
            //        manager.SaveMetadataToFile(dbo);
            //    }
            //}

            //foreach (var ns in infoBase.Namespaces.Where(ns => ns.Name == "InfoRg"))
            //{
            //    foreach (var dbo in ns.DbObjects.Where(dbo => dbo.Name == "ЦеныНоменклатурыПоставщиков"))
            //    {
            //        serializer.Serialize(dbo);
            //        manager.SaveMetadataToFile(dbo);
            //    }
            //}

            //foreach (var ns in infoBase.Namespaces.Where(ns => ns.Name == "AccumRg"))
            //{
            //    foreach (var dbo in ns.DbObjects.Where(dbo => dbo.Name == "ПартииТоваровОрганизаций"))
            //    {
            //        serializer.Serialize(dbo);
            //        manager.SaveMetadataToFile(dbo);
            //    }
            //}
        }
    }
}
