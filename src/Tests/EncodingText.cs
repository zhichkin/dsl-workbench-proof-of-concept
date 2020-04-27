using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace OneCSharp.Tests
{
    [TestClass]
    public class EncodingText
    {
        [TestMethod]
        public void TestEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            
            Console.WriteLine(encoder.Encode("тест"));
            Console.WriteLine(JavaScriptEncoder.Default.Encode("тест"));
            JsonEncodedText text = JsonEncodedText.Encode("тест");
            Console.WriteLine(text.ToString());
            
            Console.WriteLine();

            // To find out source and target
            byte[] bytes = Encoding.UTF8.GetBytes(new char[] { '\u0442', '\u0435', '\u0441', '\u0442' });
            string source = Encoding.UTF8.GetString(bytes); //"\u0442\u0435\u0441\u0442"; //"Ðàáîòà â ãåðìàíèè";
            const string destination = "тест"; //"Работа в германии";

            var decodedCyrillic = Encoding.GetEncoding(1251).GetString(bytes);
            Console.WriteLine(decodedCyrillic);

            foreach (var sourceEncoding in Encoding.GetEncodings())
            {

                bytes = sourceEncoding.GetEncoding().GetBytes(source);
                foreach (var targetEncoding in Encoding.GetEncodings())
                {
                    if (targetEncoding.GetEncoding().GetString(bytes) == destination)
                    {
                        Console.WriteLine("Source Encoding: {0} TargetEncoding: {1}", sourceEncoding.CodePage, targetEncoding.CodePage);
                    }

                }
            }

            // Result1: Source Encoding: 1252 TargetEncoding: 1251
            // Result2: Source Encoding: 28591 TargetEncoding: 1251
            // Result3: Source Encoding: 28605 TargetEncoding: 1251

            // The code for you to use 
            //var decodedCyrillic = Encoding.GetEncoding(1251).GetString(Encoding.GetEncoding(1252).GetBytes(source));
            Console.WriteLine(decodedCyrillic);
            // Result: Работа в германии
        }
    }
}