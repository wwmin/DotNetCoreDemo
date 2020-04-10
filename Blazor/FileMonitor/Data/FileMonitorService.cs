using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMonitor.Data
{
    public class FileMonitorService
    {
        private readonly IConfiguration _config;
        public FileMonitorService(IConfiguration config)
        {
            _config = config;
        }

        public Task<FileMonitorModel[]> GetFileStateAsync()
        {
            var fileRoots = _config["FileMonitorRoots"].Split(",").ToList();
            var files = fileRoots.SelectMany(root =>
             {
                 var provider = new PhysicalFileProvider(root);
                 var content = provider.GetDirectoryContents(string.Empty);
                 return content.Where(con => !con.IsDirectory).Select(con =>
                   {
                       return new FileMonitorModel
                       {
                           Date = con.LastModified.DateTime,
                           FileName = con.Name,
                           FileSize = con.Length / 1024,
                           Summary = con.PhysicalPath
                       };
                   });
             });

            //var files = content.Select(p => provider.GetFileInfo(p.PhysicalPath)).ToList();
            //var filesInfo = provider.GetFileInfo(string.Empty);

            return Task.FromResult(files.ToArray());
        }
    }
}
