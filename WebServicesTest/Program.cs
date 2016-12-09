using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace WebServicesTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Endpoint=sb://kimspace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=hNXAPUssGofW9TxU9WUEJe975/+Mcwbh+gv9afNdHls=";
            const string queueName = "testqueue";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            List<string> messages = new List<string>();

            client.OnMessage(msg =>
            {
                messages.Add(msg.GetBody<String>());
                /*
                Console.WriteLine(string.Format("Message body: {0}", message.GetBody<String>()));
                Console.WriteLine(string.Format("Message id: {0}", message.MessageId));
                Console.WriteLine("");*/
            });

            Console.WriteLine("Ange ett användarnamn: ");

            string namn = Console.ReadLine();

            bool authenticated = false;

            if (!string.IsNullOrEmpty(namn))
            {
                authenticated = true;
            }
            else
            {
                authenticated = false; 
            }

            while (authenticated)
            {
                Console.WriteLine("Skriv ett meddelande: ");
                string text = namn + ": " + Console.ReadLine();
                var message = new BrokeredMessage(text);
                client.Send(message);

                Console.WriteLine("Message sent successfully!");

                Console.WriteLine("Write 'logout' to log out or 'new' to send a new message!");

                string r = Console.ReadLine();

                if (r == "logout")
                {
                    authenticated = false;
                }
                else if (r == "new")
                {
                    authenticated = true;
                }
            }

            Console.WriteLine("Messages: \n");

            foreach (var msg in messages)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
