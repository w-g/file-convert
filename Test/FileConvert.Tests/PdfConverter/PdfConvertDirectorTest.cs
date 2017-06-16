using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using V4TOR.FileConvert;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileConvert.Tests
{
    [TestClass]
    public class PdfConvertDirectorTest
    {
        protected readonly string _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles/Uploads/");

        [TestMethod]
        public void TestAddFile()
        {
            FileConvertDirector<PdfConverter>.AddConvertingFile(new ConvertingFile(_basePath + "LICENSE.txt"));

            // 无异常即通过
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestAddFiles()
        {
            ConvertingFile[] convertingFiles = {
                    new ConvertingFile(_basePath + "LICENSE.txt"),
                    new ConvertingFile(_basePath + "Lucene与全文检索.ppt"),
                    new ConvertingFile(_basePath + "Lucene与全文检索.pptx"),
                    new ConvertingFile(_basePath + "开发计划.xls"),
                    new ConvertingFile(_basePath + "开发计划.xlsx"),
                    new ConvertingFile(_basePath + "我的简历.pdf"),
                    new ConvertingFile(_basePath + "易化360工作移交.doc"),
                    new ConvertingFile(_basePath + "易化360工作移交.docx")
            };

            FileConvertDirector<PdfConverter>.AddConvertingFiles(convertingFiles);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestRunAndOnConvertFinished()
        {
            ConvertingFile[] convertingFiles = {
                    new ConvertingFile(_basePath + "LICENSE.txt"),
                    new ConvertingFile(_basePath + "Lucene与全文检索.ppt"),
                    new ConvertingFile(_basePath + "Lucene与全文检索.pptx"),
                    new ConvertingFile(_basePath + "开发计划.xls"),
                    new ConvertingFile(_basePath + "开发计划.xlsx"),
                    new ConvertingFile(_basePath + "我的简历.pdf"),
                    new ConvertingFile(_basePath + "易化360工作移交.doc"),
                    new ConvertingFile(_basePath + "易化360工作移交.docx")
            };

            var successCount = 0;

            FileConvertDirector<PdfConverter>.AddConvertingFiles(convertingFiles);

            FileConvertDirector<PdfConverter>.OnConvertFinished += (sender, args) =>
            {
                var convertedFile = sender as ConvertingFile;
                Assert.IsTrue(convertingFiles.Any(f => f.FilePath == convertedFile.FilePath));
                Assert.IsTrue(File.Exists(convertedFile.OutputPath) || Directory.Exists(convertedFile.OutputPath));

                successCount++;
            };

            FileConvertDirector<PdfConverter>.Run();

            while (successCount != convertingFiles.Length - 1)
            {
                Thread.Sleep(200);
            }

            Assert.AreEqual(1, 1);
        }
    }
}
