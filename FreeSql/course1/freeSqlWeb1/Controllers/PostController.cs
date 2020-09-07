using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using freeSqlWeb1.Domain;
using freeSqlWeb1.Infrastructures;
using freeSqlWeb1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace freeSqlWeb1.Controllers
{
    /// <summary>
    /// 文章发布
    /// </summary>
    public class PostController : BaseController
    {
        private readonly IMessageService _message;
        public PostController(IFreeSql freesql, IMapper mapper, IMessageService message) : base(freesql, mapper)
        {
            _message = message;
        }

        /// <summary>
        /// 获取发布的文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Post Get(int id)
        {
            _message.Send(id.ToString());
            Post post = _freesql.Select<Post>().Where(p => p.Id == id).Include(p => p.Blog).OrderByDescending(r => r.ReplyTime).ToOne();

            return post;
        }

        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Post input)
        {
            var i = await _freesql.Insert<Post>(input).ExecuteIdentityAsync();
            return Ok(i);
        }
    }
}