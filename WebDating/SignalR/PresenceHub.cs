using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebDating.Extensions;

namespace WebDating.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            var isOnline = await _presenceTracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
            if (isOnline)
                //Bất kỳ ai kết nối cùng 1 trung tâm này đều có thể nhận được tên ng dùng vừa kết nối
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            //Gửi cho khách hàng ds ng dùng đang online
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOffline = await _presenceTracker.UserDisConnected(Context.User.GetUserName(), Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());

            await base.OnDisconnectedAsync(exception);
        }

    }
}
