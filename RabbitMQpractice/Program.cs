using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ;
using RabbitMQ.Client;

namespace RabbitMQpractice
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Step 1: Create a connection to RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                // Step 2: Declare a queue named "hello"
                await channel.QueueDeclareAsync(
                    queue: "hello",          // Queue name
                    durable: false,          // Queue is not durable (not persistent across restarts)
                    exclusive: false,        // Queue is not exclusive to this connection
                    autoDelete: false,       // Queue will not be automatically deleted
                    arguments: null          // No additional arguments
                );

                // Step 3: Create a message to send to the queue
                const string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                // Step 4: Publish the message to the queue
                await channel.BasicPublishAsync(
                    exchange: string.Empty,  // Use the default exchange
                    routingKey: "hello",     // Routing key is the name of the queue
                    body: body              // The message in byte array form
                );

                // Step 5: Inform the user that the message has been sent
                Console.WriteLine($" [x] Sent '{message}'");

                // Step 6: Wait for the user to press Enter before exiting
                Console.WriteLine("Press [Enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
