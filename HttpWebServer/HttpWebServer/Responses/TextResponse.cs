using HttpWebServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpWebServer.Responses
{
    public class TextResponse : ContentResponse
    {
        public TextResponse(string text) 
            : base(text, ContentType.PlainText)
        {
        }
    }
}
