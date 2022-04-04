using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorServerSignalRApp.Data
{
    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;
        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public void SetUsername(string username)
        {
            SetItem<string>("username", username).Wait();
        }
        public string GetUsername()
        {
            return GetItem<string>("username").Result ?? string.Empty;
        }
        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
    public interface ILocalStorageService
    {
        string GetUsername();
        void SetUsername(string username);
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }
}
