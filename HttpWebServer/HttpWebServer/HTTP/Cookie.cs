using HttpWebServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpWebServer.HTTP
{
    public class Cookie
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public Cookie(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            Name = name;
            Value = value;
        }

        public override string ToString()
        => $"{this.Name}={this.Value}";
    }
}
