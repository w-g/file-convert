using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V4TOR.FileConvert;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

            FileConvertDirector<PdfConverter>.OutputPath = "E:/Temp/PDF";
            FileConvertDirector<HtmlConverter>.OutputPath = "E:/Temp/HTML";

            FileConvertDirector<PdfConverter>.AddConvertingFiles(new ConvertingFile[]
            {
                new ConvertingFile(Path.Combine(basePath, "0979BF52-E3D7-4199-AEC4-3E538C8A64A3.doc")),
                new ConvertingFile(Path.Combine(basePath, "0979BF52-E3D7-4199-AEC4-3E538C8A64A3.docx")),
                new ConvertingFile(Path.Combine(basePath, "208A3214-3BA6-4CEC-8A0A-F707FBF8E58F.xls")),
                new ConvertingFile(Path.Combine(basePath, "208A3214-3BA6-4CEC-8A0A-F707FBF8E58F.xlsx")),
                new ConvertingFile(Path.Combine(basePath, "46164C79-5EB9-468A-9C1F-83490C47E21E.txt")),
                new ConvertingFile(Path.Combine(basePath, "6A962133-5C21-4B26-9183-82C72481C330.pdf")),
                new ConvertingFile(Path.Combine(basePath, "F5E1D185-A97D-40E6-8CAF-4873714B241E.ppt")),
                new ConvertingFile(Path.Combine(basePath, "F5E1D185-A97D-40E6-8CAF-4873714B241E.pptx"))
            });

            FileConvertDirector<PdfConverter>.OnConvertFinished += (sender, eventArgs) =>
            {
                var convertedFile = sender as ConvertingFile;
                Console.WriteLine("PDF Converted: {0}", convertedFile.OutputPath);

                FileConvertDirector<HtmlConverter>.AddConvertingFile(new ConvertingFile(convertedFile.OutputPath));
            };

            FileConvertDirector<PdfConverter>.Run();

            FileConvertDirector<HtmlConverter>.AddConvertingFiles(new ConvertingFile[]
            {
                new ConvertingFile(Path.Combine(basePath, "6A962133-5C21-4B26-9183-82C72481C330.pdf")),
            });

            FileConvertDirector<HtmlConverter>.OnConvertFinished += (sender, eventArgs) =>
            {
                var convertedFile = sender as ConvertingFile;
                Console.WriteLine("HTML Converted: {0}", convertedFile.OutputPath);
            };

            FileConvertDirector<HtmlConverter>.Run();

            Console.ReadKey();
        }
    }
}
