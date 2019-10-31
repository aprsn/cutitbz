using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CutitBz.Hubs
{
    public class StatusHub: Hub
    {
        public async Task UpdateStatus(string user, string totalClick, string totalView)
        {
            await Clients.User(user).SendAsync("ReceiveStatus", totalClick, totalView);
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

    }
}