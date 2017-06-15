using System.IO;

namespace V4TOR.FileConvert
{
    public class PowerPoint2003PdfConverter : PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "ppt";
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            Aspose.Slides.Presentation pres = new Aspose.Slides.Presentation(sourceFilePath);
            pres.Save(targetPdfPath, Aspose.Slides.Export.SaveFormat.Pdf);

            return File.Exists(targetPdfPath) && new FileInfo(targetPdfPath).Length > 0;
        }
    }
}
