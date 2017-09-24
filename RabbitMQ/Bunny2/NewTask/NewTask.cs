using System;
using RabbitMQ.Client;
using System.Text;
using System.Linq;
using System.Collections.Generic;

class NewTask
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null );
            var messages = GetMessage(args);
            foreach(var message in messages){
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }            
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static IEnumerable<string> GetMessage(string[] args)
    {
        return ((args.Length > 0) ? args.SelectMany(one=>one.Split(";")) :new System.Collections.Generic.List<string>{"Hello World!"});
    }
}
