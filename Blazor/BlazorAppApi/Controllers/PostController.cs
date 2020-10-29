using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        protected readonly IFreeSql _sql;
        public PostController(IFreeSql freeSql)
        {
            _sql = freeSql;
        }
        /// <summary>
        /// 获取发布的文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Post>> Get(int id)
        {
            Post post = await _sql.Select<Post>().Where(p => p.Id == id).Include(p => p.Blog).OrderByDescending(r => r.ReplyTime).ToOneAsync();
            return Ok(post);
        }

        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Post input)
        {
            var i = await _sql.Insert<Post>(input).ExecuteIdentityAsync();
            if (i > 0) return Ok();
            else return BadRequest();
        }
    }
}
