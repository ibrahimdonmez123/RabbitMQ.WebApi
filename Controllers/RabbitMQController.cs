using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class RabbitMQController : ControllerBase
{
    private readonly IConnection _rabbitMQConnection;

    public RabbitMQController(IConnection rabbitMQConnection)
    {
        _rabbitMQConnection = rabbitMQConnection;
    }

    [HttpPost("send-message")]
    public IActionResult SendMessage([FromBody] string message)
    {
        using (var channel = _rabbitMQConnection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

            Console.WriteLine($" [x] Sent {message}");
        }

        return Ok($"Message sent: {message}");
    }
}
