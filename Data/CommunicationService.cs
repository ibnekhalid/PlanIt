using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorServerSignalRApp.Data
{
    public class CommunicationService : ICommunicationService
    {
        private HubConnection hubConnection;
        public async Task Initialize()
        {
            hubConnection = new HubConnectionBuilder()
               .WithUrl("http://rahmad-planit.herokuapp.com/chathub")
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
        public bool IsConnected() =>
          hubConnection?.State == HubConnectionState.Connected;
        public async Task Register(string id, string userInput) =>
         await hubConnection.SendAsync("Register", id, userInput);
        public async Task Reveal(string id) =>
         await hubConnection.SendAsync("Reveal", id);
        public async Task Select(string id, int userInput) =>
         await hubConnection.SendAsync("SelectValue", id, userInput);

        public async Task DisposeAsync() =>
         await hubConnection.DisposeAsync();
    }
    public interface ICommunicationService
    {
        Task Initialize();
        Task Select(string id, int userInput);
        Task Register(string id, string userInput);
        Task Reveal(string id);
        void OnReveal(Action<bool> action);
        Task DisposeAsync();
        bool IsConnected();
    }
}
