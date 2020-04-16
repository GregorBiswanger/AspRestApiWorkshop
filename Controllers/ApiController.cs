using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspRestApiWorkshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AspRestApiWorkshop.Controllers
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public ApiController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public ActionResult<ApiModel[]> Get()
        {
            return new ApiModel[]
            {
                new ApiModel
                {
                    Name = "Camps",
                    Description = "Dev Conferences",
                    Links = new List<LinkDto>
                    {
                        new LinkDto(_linkGenerator.GetUriByAction(HttpContext, "GetCamps", "camps"),
                                    "get_camps",
                                        "GET")
                    }
                }
            };
        }
    }
}