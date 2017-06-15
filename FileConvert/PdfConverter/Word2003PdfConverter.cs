using System.IO;

namespace V4TOR.FileConvert
{
    public class Word2003PdfConverter : PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "doc";
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            var doc = new Aspose.Words.Document(sourceFilePath);
            Aspose.Words.Saving.PdfSaveOptions saveOptions = new Aspose.Words.Saving.PdfSaveOptions();
            saveOptions.OutlineOptions.HeadingsOutlineLevels = 3;
            doc.Save(targetPdfPath, saveOptions);

            return File.Exists(targetPdfPath) && new FileInfo(targetPdfPath).Length > 0;
        }
    }
}
