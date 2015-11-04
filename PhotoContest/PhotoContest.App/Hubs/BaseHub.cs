using Microsoft.AspNet.SignalR;
namespace PhotoContest.App.Hubs
{

    public class BaseHub : Hub
    {
        public BaseHub()
        {

        }

        public void AddMessage(string gosho)
        {
            this.Clients.Caller.gosho();
        }
    }
}