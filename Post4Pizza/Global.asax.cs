using System.IO;
using System.Web;
using System.Web.Http;
using Serilog;

namespace Post4Pizza
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var logDirectory = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "logs");
            Directory.CreateDirectory(logDirectory);
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.RollingFile(Path.Combine(logDirectory, "log.txt"))
               .CreateLogger();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}