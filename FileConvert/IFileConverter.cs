
namespace V4TOR.FileConvert
{
    /// <summary>
    /// 文件转换器
    /// </summary>
    public interface IFileConverter
    {
        /// <summary>
        /// 转换器适配的源文件扩展名
        /// </summary>
        string SourceFileExtName { get; }

        /// <summary>
        /// 转换器适配的目标文件扩展名
        /// </summary>
        string TargetFileExtName { get; }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetPdfPath">目标文件路径</param>
        /// <returns>转换是否成功</returns>
        bool Convert(string sourceFilePath, string targetPdfPath);
    }
}
