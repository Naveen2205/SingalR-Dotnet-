using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;

namespace SignalRServer.Hubs{
    public class ChatHub : Hub {

        //Fires when we get new connection in hub
        public override Task OnConnectedAsync()
        {
            //In SignalR Context Object will provide unique connection Id
            Console.WriteLine("--> Connection Opened: "+ Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message){
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            Console.WriteLine("To: "+ routeOb.To.ToString());
            Console.WriteLine("Message Recieved on: "+ Context.ConnectionId);
            //If Sender haven't specify the receiver connection id,
            //it will send message to all connected clients.
            if(routeOb.To.ToString() == string.Empty){
                Console.WriteLine("Broadcast");
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            else{
                string toClient = routeOb.To;
                Console.WriteLine("Target on: "+ toClient);
                await Clients.Client(toClient).SendAsync("ReceiveMessage", message);
            }
        }
    }
}