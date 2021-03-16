using FullApp1.Services.Interfaces;

namespace FullApp1.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
