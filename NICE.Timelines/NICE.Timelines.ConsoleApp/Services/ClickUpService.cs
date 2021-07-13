using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Common.Models;
using NICE.Timelines.Configuration;
using NICE.Timelines.DB.Services;
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
        private readonly IDatabaseService _databaseService;
        private readonly IConversionService _conversionService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClickUpService> _logger;

        public ClickUpService(ClickUpConfig clickUpConfig, IDatabaseService databaseService, IConversionService conversionService, IHttpClientFactory httpClientFactory, ILogger<ClickUpService> logger)
        {
            _clickUpConfig = clickUpConfig;
            _databaseService = databaseService;
            _conversionService = conversionService;
            _httpClientFactory = httpClientFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> ProcessSpace(string spaceId)
        {
            var allListsInSpace = new List<ClickUpList>();
            var recordsSaveOrUpdated = 0;

            var allFoldersInSpace = (await GetFoldersInSpaceAsync(spaceId)).Folders;
            if (allFoldersInSpace.Any())
                allListsInSpace = await GetListsInFolder(allFoldersInSpace);
                    
            var folderlessLists = (await GetListsInSpaceThatAreNotInFolders(spaceId)).Lists;
            if (folderlessLists.Any())
                allListsInSpace.AddRange(folderlessLists);

            foreach (var list in allListsInSpace) //a list should have a unique ACID
            {
                var tasks = (await GetTasksInList(list.Id)).Tasks;
                int? acid = null;
                foreach (var task in tasks) //TODO: batching reduce the number of database hits
                {
                    acid = _conversionService.GetACIDFromClickUpTask(task); //TODO: get the ACID from the list, not from a task.
                    recordsSaveOrUpdated += await _databaseService.SaveOrUpdateTimelineTask(task);
                }
            }

            return recordsSaveOrUpdated;
        }

        private async Task<ClickUpFolders> GetFoldersInSpaceAsync(string spaceId)
        {
            return await ReturnClickUpData<ClickUpFolders>(_clickUpConfig.GetFolders, spaceId);
        }

        private async Task<List<ClickUpList>> GetListsInFolder(IList<ClickUpFolder> allFoldersInSpace)
        {
            var allListsInSpace = new List<ClickUpList>();

            foreach (var folder in allFoldersInSpace)
            {
                var lists = (await ReturnClickUpData<ClickUpLists>(_clickUpConfig.GetLists, folder.Id)).Lists;
                if (lists.Any())
                {
                    allListsInSpace.AddRange(lists);
                }
            }
            
            return allListsInSpace;
        }

        private async Task<ClickUpLists> GetListsInSpaceThatAreNotInFolders(string spaceId)
        {
            return (await ReturnClickUpData<ClickUpLists>(_clickUpConfig.GetFolderlessLists, spaceId));
        }

        private async Task<ClickUpTasks> GetTasksInList(string listId)
        {
            return (await ReturnClickUpData<ClickUpTasks>(_clickUpConfig.GetTasks, listId));
        }

        private async Task<T> ReturnClickUpData<T>(string uri, string id)
        {
            var relativeUri = string.Format(uri, id);
            var requestUri = _clickUpConfig.BaseUrl + relativeUri;
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(_clickUpConfig.AccessToken);

            var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"Non-200 received from ClickUp: {(int)response.StatusCode}");
                throw new Exception($"Non-200 received from ClickUp: {(int) response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseJson);
        }
    }
}
