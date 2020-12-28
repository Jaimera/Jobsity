using JobsityChallenge.CrossCutting.StockExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobsityChallenge.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static List<string> StoredMsgs = new List<string>();
        private readonly IStockClient stockClient;

        public ChatHub(IStockClient _stockClient)
        {
            stockClient = _stockClient;
        }

        public async Task Connected()
        {
            string name = Context.User.Identity.Name;

            var id = Context.ConnectionId;

            await Clients.Caller.SendAsync("StoredMessages", StoredMsgs);

            string newClientMsg = $"[{DateTime.Now}] - {name} has joined the chat.";
            await Clients.AllExcept(id).SendAsync("ReceiveMessage", newClientMsg);
        }

        public async Task SendMessage(string message)
        {
            string name = Context.User.Identity.Name;

            string msg = $"[{DateTime.Now}] - {name}: {message}";

            Cache(msg);

            if (message.StartsWith("/"))
                await HandleCommand(message);
            else
                await Clients.All.SendAsync("ReceiveMessage", msg);
        }

        private async Task HandleCommand(string message)
        {
            if (message.StartsWith("/stock="))
            {
                await QueueCommand(message);
            } else
            {
                var msg = $"'{message}' is not a known command";
                await Clients.Caller.SendAsync("ReceiveMessage", msg);
            }
        }

        private void Cache(string message)
        {
            if (StoredMsgs.Count > 50)
                StoredMsgs.RemoveAt(0);
            StoredMsgs.Add(message);
        }

        private async Task QueueCommand(string message)
        {
            var code = message.Substring(message.IndexOf("=") + 1).ToLower();

            var result = await stockClient.GetStock(code);

            var msg = $"[{ DateTime.Now}] - JobsityBOT: ";
            if (result.Open != null)
                msg += $"{result.Symbol} quote is ${result.Open} per share";
            else
                msg += $"unable to find value for: {result.Symbol}";
            
            Cache(msg);

            await Clients.All.SendAsync("ReceiveMessage", msg);
            // messageQueue.QueueMessage(message);
        }
    }
}
