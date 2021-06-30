using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Configuration;
using NICE.Timelines.Models;

namespace NICE.Timelines.Services
{
    public interface IClickUpService
    {
        Task<ClickUpSpace> GetSpace();
    }

    public class ClickUpService : IClickUpService
    {
        private readonly ClickUpConfig _clickUpConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClickUpService> _logger;

        public ClickUpService(ClickUpConfig clickUpConfig, IHttpClientFactory httpClientFactory, ILogger<ClickUpService> logger)
        {
            _clickUpConfig = clickUpConfig;
            _httpClientFactory = httpClientFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClickUpSpace> GetSpace()
        {
            var spaceId = _clickUpConfig.SpaceId;
            var relativeUri = $"space/{spaceId}/folder?archived=false";
            var requestUri = new Uri(new Uri("https://api.clickup.com/api/v2/"), relativeUri);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(_clickUpConfig.AccessToken);
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                using var response = await httpClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Non-200 received from ClickUp: {(int) response.StatusCode}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClickUpSpace>(responseJson);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return null;
            }
        }
    }
}
