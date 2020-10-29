using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        protected readonly IFreeSql _sql;
        protected readonly IMapper _mapper;
        public VideoController(IFreeSql freeSql, IMapper mapper)
        {
            _sql = freeSql;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询所有video
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Video>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Video>>> Get()
        {

            List<Video> videos = await _sql.Select<Video>().OrderByDescending(r => r.title).ToListAsync();
            return videos;
        }

        /// <summary>
        /// 查询所有video name
        /// </summary>
        /// <returns></returns>
        [HttpGet("allName")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllName()
        {
            List<string> videos = await _sql.Select<Video>().OrderByDescending(r => r.title).ToListAsync(x => x.title);

            return videos;
        }

        /// <summary>
        /// 添加video
        /// </summary>
        /// <param name="videoDtos"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult> AddVideos([FromBody] List<Video> videoDtos)
        {
            var r = await _sql.Insert<Video>(videoDtos).ExecuteIdentityAsync();
            return Ok(r);
        }
    }
}
