using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspRestApiWorkshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCamps()
        {
            if (true) return NotFound();

            return Ok(new { Moniker = "DWX2020", Name = "Developer Week 2020" });
        }
    }
}