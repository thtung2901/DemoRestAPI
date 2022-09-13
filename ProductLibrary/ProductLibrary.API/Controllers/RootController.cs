using ProductLibrary.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {  
            // create links for root
            var links = new List<LinkDto>();

            links.Add(
              new LinkDto(Url.Link("GetRoot", new { }),
              "self",
              "GET"));

            links.Add(
              new LinkDto(Url.Link("GetCategories", new { }),
              "categories",
              "GET"));

            links.Add(
              new LinkDto(Url.Link("CreateCategory", new { }),
              "create_category",
              "POST"));

            return Ok(links);

        }
    }
}
