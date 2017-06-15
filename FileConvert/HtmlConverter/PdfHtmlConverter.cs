using System;
using System.Diagnostics;
using System.IO;

namespace V4TOR.FileConvert
{
    public class PdfHtmlConverter : HtmlConverter
    {
        public override string SourceFileExtName
        {
            get
            {
                return "pdf";
            }
        }

        public override string TargetFileExtName
        {
            get
            {
                // 目标文件为一个目录，无扩展名
                return string.Empty;
            }
        }

        public override bool Convert(string sourceFilePath, string targetPdfPath)
        {
            var pdf2htmlEXPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"HtmlConverter\pdf2htmlEX\pdf2htmlEX.exe");
            var pdf2htmlEXArgs = string.Format("--embed cfijO --first-page 1 --last-page 100 --fit-width 1800 --font-size-multiplier 1 --split-pages 1 --dest-dir {0} --page-filename page-%d.html {1}", targetPdfPath, sourceFilePath);

            var processStartInfo = new ProcessStartInfo(pdf2htmlEXPath, pdf2htmlEXArgs);
            processStartInfo.CreateNoWindow = true;

            Process.Start(processStartInfo);

            return Directory.Exists(targetPdfPath) && Directory.GetFiles(targetPdfPath).Length > 0;
        }
    }
}
