using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OcelotGoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1 from GoodApi", "value2 from GoodApi"};
        }

        [HttpGet("one")]
        public ActionResult<string> GetOne([FromQuery]int id)
        {
            return $"{id} from GoodApi";
        }
    }
}