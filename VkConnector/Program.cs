using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace VkConnector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:8080")
                .UseStartup<Startup>()
                .Build();
        }
    }
}