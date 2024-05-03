using Microsoft.AspNetCore.SignalR;
using WebDating.DTOs.Post;

namespace WebDating.SignalR
{
    public class CommentSignalR : Hub
    {
       
        public async Task SendComment(CommentPostDto comment)
        {
            await Clients.All.SendAsync("ReceiveComment", comment);
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
