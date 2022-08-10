using HttpWebServer;
using HttpWebServer.Responses;
using System.Net;
using System.Net.Sockets;
using System.Text;

//"127.0.0.1", 8080
public class Program
{
    private const string HtmlForm = @"<form action='/HTML' method= 'POST'>
                                       Name: <input type='text' name = 'Name' />
                                       Age: <input type='number' name='Age' />
                                        <input type='submit' value='Save' />
                                      </form>";
    public static void Main()
=> new HttpServer(routes => routes
.MapGet("/", new TextResponse("Hellow from the server!"))
.MapGet("/Redirect", new RedirectResponse("https://softuni.org"))
.MapGet("/HTML", new HtmlResponse(Program.HtmlForm))
.MapPost("/HTML", new TextResponse("")))
    .Start();
    //server.Start();
}


