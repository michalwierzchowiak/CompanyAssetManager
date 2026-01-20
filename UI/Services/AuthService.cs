using System.Net.Http.Headers;
using System.Net.Http.Json;
using API.DTOs; 

namespace UI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public string? Token { get; private set; }

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                Token = await response.Content.ReadAsStringAsync();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

                return true;
            }
            return false;
        }

        public async Task<bool> Register(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Username = username, Password = password });
            return response.IsSuccessStatusCode;
        }
    }
}