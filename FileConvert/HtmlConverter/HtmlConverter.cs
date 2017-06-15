
using System;

namespace V4TOR.FileConvert
{
    /// <summary>
    /// 目标为HTML的文件转换器
    /// </summary>
    public abstract class HtmlConverter : IFileConverter
    {
        /// <summary>
        /// 转换器适配的文件扩展名
        /// </summary>
        public abstract string SourceFileExtName { get; }

        /// <summary>
        /// 转换器适配的目标文件扩展名
        /// </summary>
        public virtual string TargetFileExtName
        {
            get
            {
                return "html";
            }
        }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetPdfPath">目标文件路径</param>
        /// <returns>转换是否成功</returns>
        public abstract bool Convert(string sourceFilePath, string targetPdfPath);
    }
}
