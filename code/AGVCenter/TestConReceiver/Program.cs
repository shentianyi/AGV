using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConReceiver
{
    class Program
    {

        static string host = "192.168.2.116";
        static int port = 5672;
        static string username = "agv";
        static string pwd = "agv";

        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory() { HostName = "192.168.2.136" };
          
                Fanout();
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

        static void Fanout()
        {
            //192.168.2.136
            var factory = new ConnectionFactory() { HostName =host, Port=port};
            factory.UserName = username;
            factory.Password = pwd;


            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.QueueDeclare(queue: "hello",
                    //                     durable: false,
                    //                     exclusive: false,
                    //                     autoDelete: false,
                    //                     arguments: null);

                    channel.ExchangeDeclare(exchange: "helloe", type: ExchangeType.Fanout);

                    QueueDeclareOk queueOK = channel.QueueDeclare();
                    string queueName = queueOK.QueueName;

                    Console.WriteLine("queue name:" + queueName);

                    channel.QueueBind(queueName, "helloe", "hello");

              

                   var consumer = new EventingBasicConsumer(channel);

                    consumer.Registered += Consumer_Registered;
                    consumer.Received += Consumer_Received;
                    consumer.ConsumerCancelled += Consumer_ConsumerCancelled;
                    channel.BasicConsume(queue: queueName,
                                         noAck: true,
                                         consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    
                }
            }
        }

        private static void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine(e.ConsumerTag);
        }

        private static void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine(e.ConsumerTag);
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        }
    }
}