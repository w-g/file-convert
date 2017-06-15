
using System.IO;
using System.Text;

namespace V4TOR.FileConvert
{
    public class TxtPdfConverter : PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "txt";
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
            loadOptions.Encoding = Encoding.Default;
            var doc = new Aspose.Words.Document(sourceFilePath, loadOptions);
            doc.Save(targetPdfPath, Aspose.Words.SaveFormat.Pdf);

            return File.Exists(targetPdfPath) && new FileInfo(targetPdfPath).Length > 0;
        }
    }
}
