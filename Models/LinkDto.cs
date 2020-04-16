﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AspRestApiWorkshop.Models
{
    [DataContract]
    public class LinkDto
    {
        public string Href { get; private set; }

        public string Rel { get; private set; }

        public string Method { get; private set; }

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
