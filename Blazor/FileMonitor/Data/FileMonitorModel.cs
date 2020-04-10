using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMonitor.Data
{
    public class FileMonitorModel
    {
        public DateTime Date { get; set; }

        public string FileName { get; set; }

        public double FileSize { get; set; }

        public string Summary { get; set; }
    }
}
