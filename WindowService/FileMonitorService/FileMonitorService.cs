using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace FileMonitorService
{
    partial class FileMonitorService : ServiceBase
    {
        //定时器
        System.Timers.Timer t = null;
        private readonly ILogger<FileMonitorService> _logger;
        public FileMonitorService(ILogger<FileMonitorService> logger)
        {
            _logger = logger;
            InitializeComponent();

            base.CanPauseAndContinue = true;

            //每5秒执行一次
            t = new System.Timers.Timer(5000);
            //设置是执行一次(false)还是一直执行(true)
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件
            t.Enabled = true;
            //到达时间的时候执行事件
            t.Elapsed += new System.Timers.ElapsedEventHandler(DoSomeThing);

        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            _logger.LogInformation("启动服务");
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            _logger.LogInformation("关闭服务");
        }

        public void DoSomeThing(object source, System.Timers.ElapsedEventArgs e)
        {
            _logger.LogInformation("do some thing");
        }
    }
}
