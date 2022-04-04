using Microsoft.AspNetCore.SignalR;

namespace PlanIt.Server.Hubs
{
    public class ChatHub : Hub
    {


        public async Task SelectValue(string group, string userId, int value)
        {
            OnlineUsers.SetSelectedValue(userId, group, value);
            await Clients.Group(group).SendAsync("NewRegister", OnlineUsers.GetAll(group));
        }
        public async Task Reveal(string group)
        {
            await Clients.Group(group).SendAsync("Reveal", true);
        }
        public async Task Register(string group, string userId, string username)
        {
            var findUser = OnlineUsers.FirstOrDefault(group, userId);
            if (findUser != null)
            {
                OnlineUsers.RemoveUser(findUser);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            var user = new User { GroupId = group, Id = userId, Name = username };
            OnlineUsers.AddUser(user);
            await Clients.Group(group).SendAsync("NewRegister", OnlineUsers.GetAll(group));

        }
        public async Task Disconnect(string group, string userId)
        {
            var findUser = OnlineUsers.FirstOrDefault(group, userId);
            if (findUser != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                OnlineUsers.RemoveUser(findUser);
                await Clients.Group(group).SendAsync("NewRegister", OnlineUsers.GetAll(group));
            }
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string GroupId { get; set; }
        public string Name { get; set; }
        public int SelectedValue { get; set; }
    }
    public static class OnlineUsers
    {
        private static readonly List<User> userLookup = new List<User>();
        public static void AddUser(User user)
        {
            userLookup.Add(user);
        }
        public static void RemoveUser(User user)
        {
            userLookup.Remove(user);
        }
        public static void SetSelectedValue(string userId, string groupdId, int value)
        {
            var user = userLookup.FirstOrDefault(u => u.Id.Equals(userId) && u.GroupId.Equals(groupdId));
            if (user != null)
            {
                user.SelectedValue = value;
            }
        }
        public static bool IsExists(string group, string id)
        {
            return userLookup.Any(x => x.GroupId.Equals(group) && id.Equals(x.Id));
        }
        public static User FirstOrDefault(string group, string userId)
        {
            return userLookup.FirstOrDefault(x => x.GroupId == group && x.Id == userId);
        }
        public static List<User> GetAll(string group)
        {
            return userLookup.Where(x => x.GroupId == group).ToList();
        }
        public static double Average(string group)
        {
            return userLookup.Where(x => x.GroupId == group).Select(x => x.SelectedValue).Average();
        }
        public static Dictionary<int, int> Result(string group)
        {
            var occ = new Dictionary<int, int>();
            var numbers = userLookup.Where(x => x.GroupId == group).Select(x => x.SelectedValue).ToList();
            var distinctNumbers = numbers.Distinct().ToList();
            distinctNumbers.ForEach(x =>
            {
                occ.Add(x, numbers.Count(n => n == x));
            });


            return occ;
        }
    }
}
