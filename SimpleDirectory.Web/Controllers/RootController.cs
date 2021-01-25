using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleDirectory.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public string GetRoot()
        {
            return "Simple directory RESTful web services that have implemented by Görkem ÖZTÜRK.";
        }
    }
}