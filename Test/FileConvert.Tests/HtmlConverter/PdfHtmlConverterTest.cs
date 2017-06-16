using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using V4TOR.FileConvert;
using System.IO;

namespace FileConvert.Tests
{
    [TestClass]
    public class PdfHtmlConverterTest
    {
        protected readonly string BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles/Uploads");

        [TestMethod]
        public void TestConvert()
        {
            var converter = new PdfHtmlConverter();

            var sourceFilePath = Path.Combine(BasePath, "6A962133-5C21-4B26-9183-82C72481C330.pdf");
            var targetFilePath = Path.Combine(BasePath, "6A962133-5C21-4B26-9183-82C72481C330");

            converter.Convert(sourceFilePath, targetFilePath);

            Assert.IsTrue(Directory.Exists(targetFilePath) && Directory.GetFiles(targetFilePath).Length > 0);
        }
    }
}
