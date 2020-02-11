using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using freeSqlWeb1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace freeSqlWeb1.Controllers
{
    /// <summary>
    /// Blog
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        // GET api/Blog

        IFreeSql _fsql;
        public BlogController(IFreeSql fsql)
        {
            _fsql = fsql;
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Blog>> Get()
        {
            List<Blog> blogs = _fsql.Select<Blog>().OrderByDescending(r => r.CreateTime).ToList();

            return blogs;
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Blog> blogs = await _fsql.Select<Blog>().ToListAsync();
            return Ok(blogs);
        }

        // GET api/blog/5
        [HttpGet("{id}")]
        public ActionResult<Blog> Get(int id)
        {
            return _fsql.Select<Blog>(id).ToOne();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Blog input)
        {
            var i = await _fsql.Insert<Blog>(input).ExecuteIdentityAsync();
            return Ok(i);
        }


        // DELETE api/blog/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _fsql.Delete<Blog>(new { BlogId = id }).ExecuteAffrows();
        }
    }
}