using Microsoft.AspNet.SignalR;
namespace PhotoContest.App.Hubs
{

    public class BaseHub : Hub
    {
        public string AddMesssage()
        {
            return "asd";
        }
    }
}