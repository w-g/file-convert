using System;
using System.Configuration;
using V4TOR.FileConvert;
using NPoco;
using System.Threading;

namespace SampleApp
{
    public class ConvertService
    {
        public void Start()
        {
            FileConvertDirector<PdfConverter>.OutputPath = ConfigurationManager.AppSettings["converter-pdf-output-path"];
            FileConvertDirector<PdfConverter>.Run();
            FileConvertDirector<PdfConverter>.OnConvertFinished += OnConvertPdfFinished;

            FileConvertDirector<HtmlConverter>.OutputPath = ConfigurationManager.AppSettings["converter-html-output-path"];
            FileConvertDirector<HtmlConverter>.Run();
            FileConvertDirector<HtmlConverter>.OnConvertFinished += OnConvertHtmlFinished;

            FeedConvertingFileQueue();
        }

        public void Stop()
        {
        }

        protected void OnConvertPdfFinished(object sender, EventArgs args)
        {
            var convertedFile = sender as ConvertingFile;
            FileConvertDirector<HtmlConverter>.AddConvertingFile(new ConvertingFile(convertedFile.OutputPath));
        }

        protected void OnConvertHtmlFinished(object sender, EventArgs args)
        {
            // todo
        }

        protected void FeedConvertingFileQueue()
        {
            TimerCallback callback = (state) => 
            {
                using (var database = new Database("converting-files"))
                {
                    var submitedFiles = database.Fetch<SubmitedFile>("SELECT TOP 100 * FROM SubmitedFile ORDER BY ID").ToArray();

                    if (submitedFiles != null && submitedFiles.Length > 0)
                    {
                        foreach (var file in submitedFiles)
                        {
                            if (file.Path.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                            {
                                FileConvertDirector<HtmlConverter>.AddConvertingFile(new SubmittedConvertingFile(file.Path, file.ResourceId));
                            }
                            else
                            {
                                FileConvertDirector<PdfConverter>.AddConvertingFile(new SubmittedConvertingFile(file.Path, file.ResourceId));
                            }
                        }

                        int maxId = submitedFiles[submitedFiles.Length - 1].Id; 
                        database.Execute("DELETE FROM SubmitedFile WHERE ID <= @0", maxId);
                    }
                }
            };

            var timer = new Timer(callback);
            timer.Change(3000, 10000);
        }

        //public void AddPdfConvertingFile(string filePath)
        //{

        //}

        //public void AddHtmlConvertingFile(string filePath)
        //{

        //}
    }

    [TableName("SubmitedFile")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class SubmitedFile
    {
        [Column(Name = "Id")]
        public int Id { get; set; }

        [Column(Name = "Path")]
        public string Path { get; set; }

        [Column(Name = "ResourceId")]
        public string ResourceId { get; set; }
    }

    public class SubmittedConvertingFile : ConvertingFile
    {
        public SubmittedConvertingFile(string filePath, string resourceId)
            : base(filePath)
        {
            ResourceId = resourceId;
        }

        public string ResourceId { get; set; }
    }
}