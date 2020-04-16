using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspRestApiWorkshop.Models
{
    public class ApiModel : LinkedResourceBaseDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
