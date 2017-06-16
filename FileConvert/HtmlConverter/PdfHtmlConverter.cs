using Common.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace V4TOR.FileConvert
{
    public class PdfHtmlConverter : HtmlConverter
    {
        private readonly string Pdf2htmlEXPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"HtmlConverter\pdf2htmlEX\pdf2htmlEX.exe");

        protected ILog Logger
        {
            get
            {
                return LogManager.GetLogger(GetType());
            }
        } 

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

        public override bool Convert(string sourceFilePath, string targetFilePath)
        {
            sourceFilePath = sourceFilePath.Replace('/', '\\');
            targetFilePath = targetFilePath.Replace('/', '\\');

            using (var process = new Process())
            {
                process.StartInfo.FileName = Pdf2htmlEXPath;
                process.StartInfo.Arguments = string.Format("--embed cfijO --first-page 1 --last-page 100 --fit-width 1800 --font-size-multiplier 1 --split-pages 1 --dest-dir {0} --page-filename page-%d.html {1}", targetFilePath, sourceFilePath);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, args) => 
                {
                    Logger.Debug((args as DataReceivedEventArgs).Data);
                };

                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, args) =>
                {
                    Logger.Error((args as DataReceivedEventArgs).Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                if (process.WaitForExit(20000))
                {
                    // 20秒内转换工作未完成被强制退出
                    Logger.Error(string.Format("{0} takes too long time", sourceFilePath));
                }
            }

            return Directory.Exists(targetFilePath) && Directory.GetFiles(targetFilePath).Length > 0;
        }
    }
}
