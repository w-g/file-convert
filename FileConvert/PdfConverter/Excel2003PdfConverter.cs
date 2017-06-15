using System.IO;

namespace V4TOR.FileConvert
{
    public class Excel2003PdfConverter : PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "xls";
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(sourceFilePath);
            Aspose.Cells.PdfSaveOptions saveOptions = new Aspose.Cells.PdfSaveOptions();
            saveOptions.OnePagePerSheet = true;
            workbook.Save(targetPdfPath, saveOptions);

            return File.Exists(targetPdfPath) && new FileInfo(targetPdfPath).Length > 0;
        }
    }
}
