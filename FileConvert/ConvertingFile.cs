using Common.Logging;
using System;

namespace V4TOR.FileConvert
{
    /// <summary>
    /// 要格式转换的文件
    /// </summary>
    public class ConvertingFile
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILog Logger
        {
            get
            {
                return LogManager.GetLogger(GetType());
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public ConvertingFile(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// 在格式转换完成后发生
        /// </summary>
        public event EventHandler OnFileConvertFinished;

        /// <summary>
        /// 格式转换器
        /// </summary>
        public IFileConverter Converter { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 转换后文件的输出路径
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <remarks>
        /// 调用此方法前，必须制定格式转换器
        /// </remarks>
        public void Convert()
        {
            Converter.Convert(FilePath, OutputPath);

            if (OnFileConvertFinished != null)
            {
                foreach (EventHandler invocation in OnFileConvertFinished.GetInvocationList())
                {
                    try
                    {
                        invocation.BeginInvoke(this, null, null, null);
                    }
                    catch (Exception ecp)
                    {
                        Logger.Error("error when call OnFileConvertFinished:" + ecp.Message);
                    }
                }
            }
        }
    }
}
