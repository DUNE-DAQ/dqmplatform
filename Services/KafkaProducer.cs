using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using DuneDaqMonitoringPlatform.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DuneDaqMonitoringPlatform.Services
{
    public class KafkaProducer
    {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public KafkaProducer(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }



        public async Task SendMessageAsync(string message)
        {
            string path;

            #if DEBUG
                path = webHostEnvironment.ContentRootPath;
            #else
                path = "/opt/app-root/src/";
            #endif

            var config = new ProducerConfig {
                /*BootstrapServers = configuration["KafkaConfig:Logging:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCaLocation = path + configuration["KafkaConfig:Logging:SslCaLocation"],
                SslCertificateLocation = path + configuration["KafkaConfig:Logging:SslCertificateLocation"],
                SslKeyLocation = path + configuration["KafkaConfig:Logging:SslKeyLocation"],*/
                BootstrapServers = configuration["KafkaConfig2:Logging:BootstrapServers"]
            };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(configuration["KafkaConfig2:Topics:MessengerTopicOutput"], new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
        

        //Hanging there until useful
        public async Task CreateTopicAsync()
        {

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = configuration["KafkaConfig2:Logging:BootstrapServers"] }).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = configuration["KafkaConfig2:Topics:MessengerTopicOutput"], ReplicationFactor = 1, NumPartitions = 1 } });
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }
    }
}
