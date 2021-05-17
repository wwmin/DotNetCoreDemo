using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkServiceDemo
{
    public class Worker : BackgroundService
    {
        //是否正在停止工作
        private bool _isStopping = false;
        //注入IHostApplicationLifetime 以便自己退出服务
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<Worker> _logger;

        public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(StartAsync)}: 服务开始");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        await SomeMethodThatDoesTheWork(stoppingToken);
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, "Global exception occurred. Will resume in a moment.");
                    }
                    // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    // await Task.Delay(1000, stoppingToken);
                    // await Task.Delay(1000);
                    // await SomeMethodThatDoesTheWork(stoppingToken);

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                _logger.LogInformation("等待退出 {time}", DateTimeOffset.Now);
            }
            finally
            {
                _logger.LogWarning("Exiting application...");
                GetOffWork(stoppingToken);
                //手动调用StopApplication
                _hostApplicationLifetime.StopApplication();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("太好了，下班时间到了，output from StopAsync at: {time}", DateTimeOffset.Now);

            _isStopping = true;

            _logger.LogInformation("去洗洗茶杯先……", DateTimeOffset.Now);
            Task.Delay(3_000).Wait();
            _logger.LogInformation("茶杯洗好了。", DateTimeOffset.Now);

            _logger.LogInformation("下班喽 ^_^", DateTimeOffset.Now);

            return base.StopAsync(cancellationToken);
        }

        private async Task SomeMethodThatDoesTheWork(CancellationToken cancellationToken)
        {
            if (_isStopping)
            {
                _logger.LogInformation("假装还在埋头苦干ing…… 其实我去洗杯子了");
            }
            else
            {
                _logger.LogInformation("我爱工作，埋头苦干ing。。。");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 关闭前需要完成的工作
        /// </summary>
        private void GetOffWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("等待 3 秒，Wait 1 ");
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();
            _logger.LogInformation("再等待 2 秒，Wait 2 ");
            Task.Delay(TimeSpan.FromSeconds(2)).Wait();
            _logger.LogInformation("完成。。。！");
        }
    }
}
