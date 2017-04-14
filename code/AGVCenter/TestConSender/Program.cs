using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConSender
{
    class Program
    {
     static   string host = "192.168.2.116";
     static   int port = 5672;
        static string username = "agv";
        static string pwd = "agv";

        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory() { HostName = "192.168.2.136" };
           // Console.WriteLine("Fanout: F\n Topic: T\n");
            //string k = Console.ReadLine();
            //if (k == "F")
            //{
                Fanout();
          //  }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

        public static void Fanout()
        {
            var factory = new ConnectionFactory() { HostName =host, Port = port };
            factory.UserName = username;
            factory.Password = pwd;


            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare(queue: "hello",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                //channel.ExchangeDeclare(exchange: "helloe", type: "fanout");
                channel.ExchangeDeclare(exchange: "helloe", type: ExchangeType.Fanout);
                
                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "q")
                    {
                        break;
                    }

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "helloe",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                   
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

        }
    }
}
