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
                new ConvertingFile(Path.Combine(basePath, "CE2F64EA-AAC9-4949-B6E0-7BA2D5A35900.docx")),
                new ConvertingFile(Path.Combine(basePath, "208A3214-3BA6-4CEC-8A0A-F707FBF8E58F.xls")),
                new ConvertingFile(Path.Combine(basePath, "EBF38033-897B-4389-A39E-5A42EDB48E2A.xlsx")),
                new ConvertingFile(Path.Combine(basePath, "46164C79-5EB9-468A-9C1F-83490C47E21E.txt")),
                new ConvertingFile(Path.Combine(basePath, "F5E1D185-A97D-40E6-8CAF-4873714B241E.ppt")),
                new ConvertingFile(Path.Combine(basePath, "31D43DFC-B897-4482-83D7-5349BE3DE58F.pptx"))
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

            //FileConvertDirector<HtmlConverter>.Run();

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
