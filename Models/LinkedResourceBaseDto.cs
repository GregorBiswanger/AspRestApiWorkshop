using System.Collections.Generic;

namespace AspRestApiWorkshop.Models
{
    public class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
