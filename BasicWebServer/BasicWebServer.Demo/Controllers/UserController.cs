using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Demo.Controllers
{
    public class UserController : Controller
    {
        private const string LoginForm = @"";

        private const string Username = "user";

        private const string Password = "user123";

        public UserController(Request request)
            : base(request)
        {
        }

        public Response LoginUser()
        {
            Request.Session.Clear();

            var bodyText = "";

            var usernameMatches = Request.Form["Username"] == Username;
            var passwordMatches = Request.Form["Password"] == Password;

            if (usernameMatches && passwordMatches)
            {
                SignIn(Guid.NewGuid().ToString());
                CookieCollection cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName,
                    Request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";

                return Html(bodyText, cookies);
            }
            

            return Redirect("/Login");
        }
        [Authorize]
        public Response GetUserData()
        {
            return Html($"<h3>Currently logged-in user is with id '{User.Id}'</h3>");
        }

        public Response Logout()
        {
            SignOut();

            return Html("<h3>Logged out successfully!</h3>");
        }

        public Response Login() => View();
    }
}
