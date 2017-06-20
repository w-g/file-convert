using Common.Logging;
using Topshelf;

namespace SampleApp
{
    class Program
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                
            {
                x.Service<ConvertService>(s =>                       
                {
                    s.ConstructUsing(name => new ConvertService());  
                    s.WhenStarted(tc => tc.Start());        
                    s.WhenStopped(tc => tc.Stop());    
                });
                x.RunAsLocalSystem();
                x.OnException((ecp) => 
                {
                    _logger.Error(ecp.Message);
                });

                x.SetDescription("File Convert Server");       
                x.SetDisplayName("FileConvertServer");                      
                x.SetServiceName("FileConvertServer");                       
            });
        }
    }
}