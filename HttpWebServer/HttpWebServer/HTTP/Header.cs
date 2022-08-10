﻿using HttpWebServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpWebServer.HTTP
{
    public class Header
    {
        public const string ContentType = "Content-Type";

        public const string ContentLength = "Content-Length";

        public const string Date = "Date";

        public const string Location = "Location";
        public const string Server = "Server";


        public Header(string _name, string _value)
        {
            Guard.AgainstNull("name", nameof(_name));
            Guard.AgainstNull("value", nameof(_value));

            Name = _name;
            Value = _value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        => $"{this.Name}: {this.Value}";
    }
}
