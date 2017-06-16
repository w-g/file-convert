using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using V4TOR.FileConvert;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileConvert.Tests
{
    [TestClass]
    public class HtmlConvertDirectorTest
    {
        protected readonly string _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles/Uploads/");

        [TestMethod]
        public void TestAddFile()
        {
            FileConvertDirector<HtmlConverter>.AddConvertingFile(new ConvertingFile(_basePath + "F5E1D185-A97D-40E6-8CAF-4873714B241E.pdf"));

            // 无异常即通过
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestAddFiles()
        {
            ConvertingFile[] convertingFiles = {
                    new ConvertingFile(_basePath + "6A962133-5C21-4B26-9183-82C72481C330.pdf")
            };

            FileConvertDirector<HtmlConverter>.AddConvertingFiles(convertingFiles);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestRunAndOnConvertFinished()
        {
            ConvertingFile[] convertingFiles = {
                    new ConvertingFile(_basePath + "6A962133-5C21-4B26-9183-82C72481C330.pdf"),
                    new ConvertingFile(_basePath + "0979BF52-E3D7-4199-AEC4-3E538C8A64A3.docx")
            };

            var successCount = 0;

            FileConvertDirector<HtmlConverter>.OutputPath = "E:/Temp";
            FileConvertDirector<HtmlConverter>.AddConvertingFiles(convertingFiles);

            FileConvertDirector<HtmlConverter>.OnConvertFinished += (sender, args) =>
            {
                var convertedFile = sender as ConvertingFile;
                Assert.IsTrue(convertingFiles.Any(f => f.FilePath == convertedFile.FilePath));
                Assert.IsTrue(File.Exists(convertedFile.OutputPath) || Directory.Exists(convertedFile.OutputPath));

                successCount++;
            };

            FileConvertDirector<HtmlConverter>.Run();

            while (successCount != convertingFiles.Length - 1)
            {
                Thread.Sleep(200);
            }

            Assert.AreEqual(1, 1);
        }
    }
}
