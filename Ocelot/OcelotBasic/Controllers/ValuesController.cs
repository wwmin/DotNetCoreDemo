using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OcelotBasic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1 from Ocelot", "value2 from Ocelot"};
        }
    }
}