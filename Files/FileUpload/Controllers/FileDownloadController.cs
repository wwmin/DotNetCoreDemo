using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        public FileDownloadController()
        {

        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        [HttpPost("section")]
        public async Task DownloadStream([FromBody] FileDownloadDto option)
        {
            await HttpContext.DownLoadFile(option.filePath);
            //不需要设置返回ok状态码，因为context的内容和状态已经自己设置，否则会报`StatusCode cannot be set because the response has already started`错误
            //return Ok(HttpContext.Response.Body);
        }
    }
    public class FileDownloadDto
    {
        public string filePath { get; set; }
    }
}
