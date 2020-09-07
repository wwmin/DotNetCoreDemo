using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using freeSqlWeb1.Domain;
using freeSqlWeb1.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace freeSqlWeb1.Controllers
{
    /// <summary>
    /// Blog
    /// </summary>
    public class BlogController : BaseController
    {
        // GET api/Blog

        public BlogController(IFreeSql freesql, IMapper mapper) : base(freesql, mapper)
        {
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Blog>> Get()
        {
            List<Blog> blogs = _freesql.Select<Blog>().OrderByDescending(r => r.CreateTime).ToList();

            return blogs;
        }

        /// <summary>
        /// 查询所有blog
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Blog> blogs = await _freesql.Select<Blog>().ToListAsync();
            return Ok(blogs);
        }

        // GET api/blog/5
        [HttpGet("{id}")]
        public ActionResult<Blog> Get(int id)
        {
            return _freesql.Select<Blog>(id).ToOne();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Blog input)
        {
            var i = await _freesql.Insert<Blog>(input).ExecuteIdentityAsync();
            return Ok(i);
        }


        // DELETE api/blog/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _freesql.Delete<Blog>(new { BlogId = id }).ExecuteAffrows();
        }
    }
}