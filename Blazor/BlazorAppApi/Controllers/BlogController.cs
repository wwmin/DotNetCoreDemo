using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        protected readonly IFreeSql _sql;
        public BlogController(IFreeSql freeSql)
        {
            _sql = freeSql;
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Blog>> Get()
        {
            List<Blog> blogs = _sql.Select<Blog>().OrderByDescending(r => r.CreateTime).ToList();

            return Ok(blogs);
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAll()
        {
            List<Blog> blogs = await _sql.Select<Blog>().ToListAsync();
            return Ok(blogs);
        }

        // GET api/blog/5
        [HttpGet("{id}")]
        public ActionResult<Blog> Get(int id)
        {
            return _sql.Select<Blog>(id).ToOne();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Blog input)
        {
            var i = await _sql.Insert<Blog>(input).ExecuteIdentityAsync();
            return Ok(i);
        }


        // DELETE api/blog/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _sql.Delete<Blog>(new { BlogId = id }).ExecuteAffrows();
        }
    }
}
