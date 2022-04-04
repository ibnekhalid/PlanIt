using Microsoft.AspNetCore.SignalR.Client;

namespace PlanIt.Client.Data
{
    public class CommunicationService : ICommunicationService
    {
        private HubConnection hubConnection;
        public async Task Initialize()
        {
            hubConnection = new HubConnectionBuilder()
               //.WithUrl("https://localhost:7248/chathub")
               .WithUrl("https://planiton.azurewebsites.net/chathub")
               .Build();
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
            });
            await hubConnection.StartAsync();
        }
        public void OnReveal(Action<bool> action)
        {
            hubConnection.On("Reveal", action);
        }
        public void OnRegister(Action<List<User>> action)
        {
            hubConnection.On("NewRegister", action);
        }
        public bool IsConnected() =>
          hubConnection?.State == HubConnectionState.Connected;
        public async Task Register(string id, string userId, string userInput) =>
         await hubConnection.SendAsync("Register", id, userId, userInput);
        public async Task Reveal(string id) =>
         await hubConnection.SendAsync("Reveal", id);
        public async Task Select(string id, string userId, int userInput) =>
         await hubConnection.SendAsync("SelectValue", id, userId, userInput);

        public async Task DisposeAsync() =>
         await hubConnection.DisposeAsync();
    }
    public interface ICommunicationService
    {
        Task Initialize();
        Task Select(string id, string userId, int userInput);
        Task Register(string id, string userId, string userInput);
        Task Reveal(string id);
        void OnReveal(Action<bool> action);
        void OnRegister(Action<List<User>> action);
        Task DisposeAsync();
        bool IsConnected();
    }
}
