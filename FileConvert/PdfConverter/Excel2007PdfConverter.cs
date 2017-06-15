
namespace V4TOR.FileConvert
{
    public class Excel2007PdfConverter : Excel2003PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "xlsx";
            }
        }
    }
}
