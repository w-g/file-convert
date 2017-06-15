using System.IO;

namespace V4TOR.FileConvert
{
    public class PowerPoint2007PdfConverter : PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "pptx";
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            using (Aspose.Slides.Pptx.PresentationEx pres = new Aspose.Slides.Pptx.PresentationEx(sourceFilePath))
            {
                pres.Save(targetPdfPath, Aspose.Slides.Export.SaveFormat.Pdf);
            }

            return File.Exists(targetPdfPath) && new FileInfo(targetPdfPath).Length > 0;
        }
    }
}
