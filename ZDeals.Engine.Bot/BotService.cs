using MassTransit;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Bot
{
    class BotService<T> : IHostedService where T: ICrawler
    {
        private readonly T _crawler;
        private readonly CrawlerOption<T> _option;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<BotService<T>> _logger;


        private CancellationTokenSource _cts;
        private Task _runningTask;

        public BotService(T crawler, CrawlerOption<T> option, 
            IHostApplicationLifetime hostApplicationLifetime, 
            IPublishEndpoint publishEndpoint, 
            ILoggerFactory loggerFactory)
        {
            _crawler = crawler;
            _option = option;
            _hostApplicationLifetime = hostApplicationLifetime;
            _publishEndpoint = publishEndpoint;
            _logger = loggerFactory.CreateLogger<BotService<T>>();

            if(crawler != null)
                crawler.PageParsed += Crawler_PageParsed;
        }

        private void Crawler_PageParsed(object sender, PageParsedEventArgs e)
        {
            _logger.LogInformation($"Page parsed: {e.PageUri}");
            _publishEndpoint.Publish<PageParsed>(new PageParsed
            {
                Url = e.PageUri.ToString(),
                ParseTime = DateTime.Now
            })
            .GetAwaiter()
            .GetResult();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource(_option.Timeout);

            var type = typeof(T);
            if(_crawler == null)
            {
                _logger.LogError($"Cannot resolve service type {type.FullName}.");
                return Task.CompletedTask;
            }

            _runningTask = Task.Run(async () =>
            {
                _logger.LogInformation($"Starting Crawler {type.FullName}");

                try
                {
                    // do work
                    await _crawler.StartCrawling(_option.StartUrl, _cts);
                    _logger.LogInformation($"Crawler {type.FullName} finished.");
                }
                catch(TaskCanceledException ex)
                {
                    _logger.LogWarning("Crawling task is cancelled.", ex);
                    return;
                }
                catch(OperationCanceledException ex)
                {
                    _logger.LogWarning("Crawling operation is cancelled.", ex);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error when crawling. ", ex);
                }

                // once the crawler finishes its work, we stop the host application
                _logger.LogInformation("Stopping Application ...");
                _hostApplicationLifetime.StopApplication();

            }, cancellationToken);

            if (_runningTask.IsCompleted) return _runningTask;

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_runningTask == null) return Task.CompletedTask;

            _logger.LogInformation("Cancelling crawler ...");
            try
            {
                _cts.Cancel();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when stopping BotService.", ex);
            }
            finally
            {
                _logger.LogInformation("Token cancelled.");
                //_runningTask.Dispose();
                _runningTask = null;

                // TODO: shutdown the crawler gracefully

                //if (_runningTask.IsCompleted == false)
                //{
                //    _logger.LogInformation("Waiting for the crawler to stop ...");
                //    try
                //    {
                //        await Task.WhenAny(_runningTask);
                //        _logger.LogInformation("Crawler stopped.");
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError("Error when waiting for crawler.", ex);
                //    }
                //    finally
                //    {
                //        _runningTask = null;
                //    }
                //}
            }

            return Task.CompletedTask;
        }
    }
}
