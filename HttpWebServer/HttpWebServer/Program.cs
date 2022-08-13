using HttpWebServer;
using HttpWebServer.Responses;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using HttpWebServer.HTTP;
using System.Web;


//"127.0.0.1", 8080
public class Program
{
    private const string HtmlForm = @"<form action='/HTML' method= 'POST'>
                                       Name: <input type='text' name = 'Name' />
                                       Age: <input type='number' name='Age' />
                                        <input type='submit' value='Save' />
                                      </form>";

    private const string DownLoadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
   </form>";

    private const string FileName = "content.text";
    public static async Task Main()
    {
        await DownloadSitesAsTextFile(Program.FileName, new string[] { "https://judge.softuni.org/", "https://softuni.org/" });
        var server = new HttpServer(routes => routes
.MapGet(" / ", new TextResponse("Hellow from the server!"))
.MapGet("/Redirect", new RedirectResponse("https://softuni.org"))
.MapGet("/HTML", new HtmlResponse(Program.HtmlForm))
.MapPost("/HTML", new TextResponse("", AddFormDataAction))
.MapGet("/Content", new HtmlResponse(Program.DownLoadForm))
.MapPost("/Content", new TextFileResponse(Program.FileName))
.MapGet("/Cookies", new HtmlResponse("",Program.AddCookiesAction)));

    await server.Start();
    }

    //server.Start();

    private static void AddFormDataAction(Request request, Response response)
    {
        response.Body = "";

        foreach (var (key, value) in request.Form)
        {
            response.Body += $"{key} - {value}";
            response.Body += Environment.NewLine;

        }
    }

    private static async Task<string> DownloadWebSiteContent(string url)
    {
        var httpClient = new HttpClient();
        using(httpClient)
        {
            var response = await httpClient.GetAsync(url);

            var html = await response.Content.ReadAsStringAsync();

            return html.Substring(0, 2000);
        }
    }

    private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
    {
        var downloads = new List<Task<string>>();

        foreach (var url in urls)
        {
            downloads.Add(DownloadWebSiteContent(url));
        }
        var responses = await Task.WhenAll(downloads);

        var responsesString = string.Join(Environment.NewLine + new String('-', 100), responses);

        await File.WriteAllTextAsync(fileName, responsesString);
    }

    private static void AddCookiesAction(Request request, Response response)
    {
        var requestHasCookies = request.Cookies.Any(c => c.Name != Session.SessionCookieName);
        var bodyText = "";
        if (requestHasCookies)
        {
            var cookieText = new StringBuilder();
            cookieText.AppendLine($"<h1>Cookies</h1>");

            cookieText.Append("<table border = '1'><tr><th>Name</th><th>Value</th></tr>");

            foreach (var cookie in request.Cookies)
            {
                cookieText.Append("<tr>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                cookieText.Append("</tr>");

            }
            cookieText.Append("</table>");
            bodyText = cookieText.ToString();
        }
        else
        {
            bodyText = "<h1>Cookies Set!</h1>";
        }

        if(!requestHasCookies)
        {
            response.Cookies.Add("My-Cookie", "My-Value");
            response.Cookies.Add("My-Second-Cookie", "My-Second-Value");


        }
    }
}




