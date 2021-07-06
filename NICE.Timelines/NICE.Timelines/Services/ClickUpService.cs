using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Configuration;
using NICE.Timelines.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NICE.Timelines.Services
{
    public interface IClickUpService
    {
        Task<int> ProcessSpace(string spaceId);
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

        public async Task<int> ProcessSpace(string spaceId)
        {
            var recordsSaveOrUpdated = 0;

            var allFoldersInSpace = (await GetFoldersInSpace(spaceId)).Folders;

            return recordsSaveOrUpdated;
        }

        public async Task<ClickUpFolders> GetFoldersInSpace(string spaceId)
        {
            var relativeUri = string.Format(_clickUpConfig.GetFolders, spaceId);
            return await ReturnClickUpData<ClickUpFolders>(relativeUri);
        }

        public async Task<T> ReturnClickUpData<T>(string relativeUri)
        {
            var requestUri = _clickUpConfig.BaseUrl + relativeUri;
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(_clickUpConfig.AccessToken);

            var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Non-200 received from ClickUp: {(int) response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseJson);
        }
    }
}
