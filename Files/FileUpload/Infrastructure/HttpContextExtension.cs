using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FileUpload.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// 通过文件流下载文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath">文件完整路径</param>
        /// <param name="contentType">访问这里 https://tool.oschina.net/commons</param>
        public static async Task DownLoadFile(this HttpContext context, string filePath, string contentType = "application/octet-stream")
        {
            var fileName = Path.GetFileName(filePath);
            int bufferSize = 1024;
            context.Response.ContentType = contentType;
            context.Response.Headers.Append("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            //使用FileStream开始循环读取要下载文件的内容
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (context.Response.Body)
                {
                    long contentLength = fs.Length;
                    context.Response.ContentLength = contentLength;
                    byte[] buffer;
                    long hasRead = 0;
                    while (hasRead < contentLength)
                    {
                        if (context.RequestAborted.IsCancellationRequested)
                        {
                            break;
                        }
                        buffer = new byte[bufferSize];
                        //从下载文件中读取bufferSize（1024字节)大小的内容到服务器内存中
                        int currentRead = fs.Read(buffer, 0, bufferSize);
                        await context.Response.Body.WriteAsync(buffer, 0, currentRead);
                        await context.Response.Body.FlushAsync();
                        hasRead += currentRead;
                    }
                    context.Response.Body.Close();
                }
                fs.Close();
            }
        }
    }
}
