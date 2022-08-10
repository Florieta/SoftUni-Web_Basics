using HttpWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpWebServer.Responses
{
    public class UnauthorisedResponse : Response
    {
        public UnauthorisedResponse() 
            : base(StatusCode.NotFound)
        {
        }
    }
}
