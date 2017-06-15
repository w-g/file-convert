
namespace V4TOR.FileConvert
{
    public class Word2007PdfConverter : Word2003PdfConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "docx";
            }
        }
    }
}
