namespace PlanIt.Client.Data
{
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

    public class User
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Name { get; set; }
        public int SelectedValue { get; set; }
    }
}
