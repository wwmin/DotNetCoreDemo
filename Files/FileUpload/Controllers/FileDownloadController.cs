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
        public async Task<IActionResult> DownloadStream([FromBody] FileDownloadDto option)
        {
            await HttpContext.DownLoadFile(option.filePath);
            return Ok(HttpContext.Response.Body);
        }
    }
    public class FileDownloadDto
    {
        public string filePath { get; set; }
    }
}
