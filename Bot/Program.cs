using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
              .ConfigureServices(services =>
              {
                  services.AddHostedService<Worker>();
              })
              .Build();

            host.Run();
        }
    }
}
