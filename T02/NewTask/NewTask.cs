﻿using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: string.Empty,
                    routingKey: "task_queue",
                    basicProperties: null,
                    body: body);

Console.WriteLine($" [x] Send {message}");
Console.WriteLine($" Press [Enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}