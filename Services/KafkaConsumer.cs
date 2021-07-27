using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DuneDaqMonitoringPlatform.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DuneDaqMonitoringPlatform.Services
{
    public class KafkaConsumer : BackgroundService
    {

        private readonly string topic;
        private readonly IConsumer<string, string> kafkaConsumer;


        public KafkaConsumer(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            string path;

            #if DEBUG
                path = webHostEnvironment.ContentRootPath;
            #else
                path = "/opt/app-root/src/";
            #endif
            
            ConsumerConfig consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = configuration["KafkaConfig:Logging:BootstrapServers"],
                GroupId = configuration["KafkaConfig:Logging:GroupId"],
            };
            kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            topic = configuration["KafkaConfig:Topics:MessengerTopic"];
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;

        }
        
        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            kafkaConsumer.Subscribe(this.topic);
            InputsMessenger messenger = InputsMessenger.Instance;

            int consumerCalls = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    consumerCalls++;

                    var consumeResult = kafkaConsumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        // Handle message...

                        if (PerformenceTimer.TimerVariable.executionFinished)
                        {
                            int round = PerformenceTimer.TimerVariable.executionRound + 1;
                            PerformenceTimer.TimerVariable = new PerformanceTimeObject { executionTime = DateTime.Now.Ticks, executionRound = round, executionFinished = false };
                            Console.WriteLine("Message incoming 0, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());
                        }
                        else
                        {
                            Console.WriteLine("Incoming before end of processing");
                        }

                        messenger.InputMessage(consumeResult.Message.Value);                        
                    }
                    
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }

            }
        }
    }
}
