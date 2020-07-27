using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace FileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string _targetFilePath = "";
        private readonly IWebHostEnvironment _env;
        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
            _targetFilePath = _env.ContentRootPath+@"\files\";
            if(!Directory.Exists(_targetFilePath))
            {
                Directory.CreateDirectory(_targetFilePath);
            }
        }

        #region 文件上传的两种方式 1.流 2.缓存
        /// <summary>
        /// 流式文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("stream")]
        public async Task<IActionResult> UploadingStream()
        {
            //获取boundary
            var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;
            //得到reader
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            //{BodyLengthLimit=2000};
            var section = await reader.ReadNextSectionAsync();

            //读取section
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                if (hasContentDispositionHeader)
                {
                    var trustedFileNameForFileStorage = Path.GetRandomFileName();
                    await WriteFileAsync(section.Body, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
                }
                section = await reader.ReadNextSectionAsync();
            }
            return Created(nameof(FileUploadController), null);
        }


        /// <summary>
        /// 缓存式文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("form")]
        public async Task<IActionResult> UploadingFormFile(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                await WriteFileAsync(stream, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
                return Created(nameof(FileUploadController), null);
            }

        }

        /// <summary>
        /// 缓存式文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("form/multi")]
        public async Task<IActionResult> UploadingFormFile()
        {
            try
            {
                var files = Request.Form.Files;
                if (files == null || files.Count == 0)
                {
                    return BadRequest("file 不能为空");
                }
                foreach (var file in files)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();
                        await WriteFileAsync(stream, Path.Combine(_targetFilePath, trustedFileNameForFileStorage));
                    }
                }
                return Created(nameof(FileUploadController), null);
            }
            catch (Exception e)
            {
                return BadRequest("上传文件出错," + e.Message);
            }

        }


        /// <summary>
        /// 写文件到磁盘
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="path">文件保存路径</param>
        /// <returns></returns>
        public static async Task<int> WriteFileAsync(System.IO.Stream stream, string path)
        {
            const int FILE_WRITE_SIZE = 84975;//写入缓存区大小
            int writeCount = 0;
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, FILE_WRITE_SIZE, true))
            {
                byte[] byteArr = new byte[FILE_WRITE_SIZE];
                int readCount = 0;
                while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length)) > 0)
                {
                    await fileStream.WriteAsync(byteArr, 0, readCount);
                    writeCount += readCount;
                }
            }
            return writeCount;
        }
        #endregion

        #region 其他方式
        /// <summary>
        /// 多文件上传的
        /// </summary>
        /// <returns></returns>
        [HttpPost("file/multi")]
        public async Task<IActionResult> UploadWithFiles()
        {
            try
            {
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);
                //size >100MB refuse upload
                if (size > 104857600)
                {
                    return BadRequest("文件过大");
                }
                List<string> filePathResultList = new List<string>();
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();
                    fileName = Guid.NewGuid() + "." + fileName.Split(".")[1];
                    string fileFullName = _targetFilePath + fileName;
                    using (FileStream fs=System.IO.File.Create(fileFullName))
                    {
                        await file.CopyToAsync(fs);//一次性写入,有可能会导致内存不足
                        fs.Flush();
                    }
                    filePathResultList.Add(fileName);
                }
                return Ok(filePathResultList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region 使用封装的FormFile上传
        /// <summary>
        /// 使用自定义的UserFile上传
        /// </summary>
        /// <param name="file">UserFile</param>
        /// <returns></returns>
        [HttpPost("file")]
        public async Task<IActionResult> UseFormAttributeUpload([FromForm] UserFile file)
        {
            if (file == null || !file.IsValid)
                return BadRequest("不允许上传的文件类型");
            string newFile = string.Empty;
            if (file != null)
                newFile = await file.SaveAs(_targetFilePath);
            return Ok(newFile);
        }
        #endregion
    }
}