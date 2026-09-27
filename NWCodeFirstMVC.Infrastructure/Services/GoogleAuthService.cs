using Microsoft.Extensions.Options;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static NWCodeFirstMVC.Domain.GoogleAuthModels;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class GoogleAuthService: IGoogleAuthService
    {
        private readonly HttpClient _http;
        private readonly GoogleAuthOptions _options;

        public GoogleAuthService(HttpClient http, IOptions<GoogleAuthOptions> options)
        {
            _http = http;
            _options = options.Value;
        }

        public async Task<GoogleTokenResponse> ExchangeCodeAsync(string code)
        {
            Console.WriteLine($"Using redirect_uri: {_options.RedirectUri}");
            var form = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["redirect_uri"] = _options.RedirectUri,
                ["grant_type"] = "authorization_code"
            };

            var response = await _http.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(form));

            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Google token exchange failed: {body}");

            return JsonSerializer.Deserialize<GoogleTokenResponse>(body)!;
        }

        public async Task<GoogleUserInfo> GetUserInfoAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://openidconnect.googleapis.com/v1/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Fetching Google user info failed: {body}");

            return JsonSerializer.Deserialize<GoogleUserInfo>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
