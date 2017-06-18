using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace V4TOR.FileConvert
{
    /// <summary>
    /// 文件格式转换的Director类
    /// </summary>
    public class FileConvertDirector<TConverter> where TConverter : class, IFileConverter
    {
        ///// <summary>
        ///// 受保护的构造函数
        ///// </summary>
        //protected FileConvertDirector()
        //{ }

        ///// <summary>
        ///// Singleton
        ///// </summary>
        //private static FileConvertDirector<TConverter> _instance = new FileConvertDirector<TConverter>();

        ///// <summary>
        ///// 获取唯一实例的方法
        ///// </summary>
        ///// <returns>唯一的实例</returns>
        //public static FileConvertDirector<TConverter> Instance()
        //{
        //    return _instance;
        //}

        /// <summary>
        /// Logger
        /// </summary>
        private static ILog Logger
        {
            get
            {
                return LogManager.GetLogger(typeof(FileConvertDirector<TConverter>));
            }
        }

        /// <summary>
        /// 所有可用的文件格式转换器
        /// </summary>
        private static IEnumerable<TConverter> Converters { get; set; }

        /// <summary>
        /// 获取指定文件扩展名的文件格式转换器
        /// </summary>
        /// <param name="fileExtName">文件扩展名</param>
        /// <returns>指定文件扩展名的文件格式转换器</returns>
        private static TConverter GetConverter(string fileExtName)
        {
            if (Converters == null)
            {
                var converterTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes().Where(t => typeof(TConverter).IsAssignableFrom(t) && !t.IsAbstract));

                if (!converterTypes.Any())
                {
                    throw new Exception("can not find a concrete class for " + typeof(TConverter).Name);
                }

                Converters = converterTypes.Select(t => AppDomain.CurrentDomain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName) as TConverter);
            }

            return Converters.FirstOrDefault(c => c != null && c.SourceFileExtName == fileExtName);
        }

        /// <summary>
        /// 待转文件队列
        /// </summary>
        private static Queue<ConvertingFile> ConvertingFileQueue = new Queue<ConvertingFile>();

        /// <summary>
        /// 增加待转文件
        /// </summary>
        /// <param name="convertingFile">待转文件</param>
        public static void AddConvertingFile(ConvertingFile convertingFile)
        {
            convertingFile.FilePath = convertingFile.FilePath.Replace('/', '\\');

            if (!string.IsNullOrWhiteSpace(convertingFile.OutputPath))
            {
                convertingFile.OutputPath = convertingFile.OutputPath.Replace('/', '\\');
            }

            ConvertingFileQueue.Enqueue(convertingFile);
        }

        /// <summary>
        /// 增加多个待转文件
        /// </summary>
        /// <param name="convertingFiles">待转文件列表</param>
        public static void AddConvertingFiles(IEnumerable<ConvertingFile> convertingFiles)
        {
            foreach (var convertingFile in convertingFiles)
            {
                AddConvertingFile(convertingFile);
            }
        }

        /// <summary>
        /// 转换后文件的输出目录
        /// </summary>
        public static string OutputPath { get; set; }

        /// <summary>
        /// 每个文件转换完成后发生
        /// </summary>
        public static event EventHandler OnConvertFinished;

        /// <summary>
        /// 开始文件转换任务
        /// </summary>
        /// <returns></returns>
        public static Task Run()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    if (ConvertingFileQueue.Count == 0)
                    {
                        Thread.Sleep(200);
                        continue;
                    }

                    var convertingFile = ConvertingFileQueue.Dequeue();

                    var fileExtName = convertingFile.FilePath.Split('.').Last();
                    var converter = GetConverter(fileExtName);

                    if (converter == null)
                    {
                        Logger.Error("not surport file with " + fileExtName + " extension name");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(convertingFile.OutputPath))
                    {
                        // 缺省转换后的文件输出路径
                        if (string.IsNullOrWhiteSpace(OutputPath))
                        {
                            // 输出至当前目录
                            convertingFile.OutputPath = Path.Combine(Path.GetDirectoryName(convertingFile.FilePath),
                                Path.GetFileNameWithoutExtension(convertingFile.FilePath));

                            if (!string.IsNullOrWhiteSpace(converter.TargetFileExtName))
                            {
                                convertingFile.OutputPath += "." + converter.TargetFileExtName;
                            }
                        }
                        else
                        {
                            // 输出至配置目录
                            convertingFile.OutputPath = Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(convertingFile.FilePath));
                            if (!string.IsNullOrWhiteSpace(converter.TargetFileExtName))
                            {
                                convertingFile.OutputPath += "." + converter.TargetFileExtName;
                            }
                        }
                    }

                    convertingFile.OutputPath = convertingFile.OutputPath.Replace('/', '\\');
                    convertingFile.Converter = converter;
                    convertingFile.Convert();

                    if (OnConvertFinished != null)
                    {
                        foreach (EventHandler invocation in OnConvertFinished.GetInvocationList())
                        {
                            try
                            {
                                invocation.BeginInvoke(convertingFile, null, null, null);
                            }
                            catch(Exception ecp)
                            {
                                Logger.Error("error when call OnConvertFinished:" + ecp.Message);
                            }
                        }

                    }
                }
            });
        }
    }
}
