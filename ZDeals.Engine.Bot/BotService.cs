using MassTransit;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Bot
{
    class BotService<T> : IHostedService where T: IPageCrawler
    {
        private readonly T _crawler;
        private readonly GenericCrawlerOption<T> _option;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<BotService<T>> _logger;


        private CancellationTokenSource _cts;
        private Task _runningTask;

        public BotService(T crawler, GenericCrawlerOption<T> option, 
            IHostApplicationLifetime hostApplicationLifetime, 
            IPublishEndpoint publishEndpoint, 
            ILoggerFactory loggerFactory)
        {
            _crawler = crawler;
            _option = option;
            _hostApplicationLifetime = hostApplicationLifetime;
            _publishEndpoint = publishEndpoint;
            _logger = loggerFactory.CreateLogger<BotService<T>>();

            _crawler.OnPageCrawled += Crawler_OnPageCrawled;
        }

        private async void Crawler_OnPageCrawled(object sender, PageCrawledEventArgs e)
        {
            _logger.LogInformation($"Page crawled: {e.Page.Uri}");

            try
            {
                await _publishEndpoint.Publish(e.Page);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to publish PageCrawled event for {e.Page.Uri}", ex);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource(_option.Timeout);            

            _runningTask = Task.Run(async () =>
            {
                var type = typeof(T);
                _logger.LogInformation($"Starting Crawler {type.FullName}");

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // do work
                    await _crawler.StartCrawling(_option, _cts);
                    _logger.LogInformation($"Crawler {type.FullName} has finished crawling {_option.Store}.");

                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogWarning("Crawling task is cancelled.", ex);
                    return;
                }
                catch (OperationCanceledException ex)
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
            if (_runningTask == null || _cts == null) return Task.CompletedTask;

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
