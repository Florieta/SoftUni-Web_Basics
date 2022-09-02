using System.Threading.Tasks;
using BasicWebServer.Server;
using BasicWebServer.Demo.Services;
using BasicWebServer.Server.Routing;

namespace BasicWebServer.Demo
{
    public class Startup
    {

        public static async Task Main()
        {
            var server = new HttpServer(routes => routes
              .MapControllers()
              .MapStaticFiles());

            server.ServiceCollection
                .Add<UserService>();

            await server.Start();

        }

        
    }
}