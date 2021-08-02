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
using NICE.Timelines.DB.Models;
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
        private readonly TimelinesContext _context;
        private readonly IDatabaseService _databaseService;
        private readonly IConversionService _conversionService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClickUpService> _logger;

        public ClickUpService(ClickUpConfig clickUpConfig, TimelinesContext context, IDatabaseService databaseService, IConversionService conversionService, IHttpClientFactory httpClientFactory, ILogger<ClickUpService> logger)
        {
            _clickUpConfig = clickUpConfig;
            _context = context;
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
                var keyDateTasks = (await GetTasksWithKeyDateInList(list.Id));
                var masterScheduleTasks = (await GetTasksWithMasterScheduleInList(list.Id));
                var tasks = keyDateTasks
                    .Concat(masterScheduleTasks.Where(t =>
                        !keyDateTasks.Any(x => x.ClickUpTaskId.Equals(t.ClickUpTaskId)))).ToList();

                int? acid = null;

                foreach (var task in tasks)
                {
                    acid = _conversionService.GetACID(task); //TODO: get the ACID from the list, not from a task.
                    _databaseService.SaveOrUpdateTimelineTask(task);
                }

                var clickUpIdsThatShouldExistInTheDatabase = tasks.Select(task => task.ClickUpTaskId);
                _databaseService.DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(acid.Value,
                    clickUpIdsThatShouldExistInTheDatabase);
            }

            return _context.SaveChanges();
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

        private async Task<List<ClickUpTask>> GetTasksWithKeyDateInList(string listId)
        {
            var page = 0;
            var tasks = new ClickUpTasks();
            var allTasks = new List<ClickUpTask>();
            do
            {
                tasks = (await ReturnClickUpData<ClickUpTasks>(_clickUpConfig.GetKeyDateTasks, listId, page));
                allTasks.AddRange(tasks.Tasks);
                page += 1;
            } while (tasks.Tasks.Count() == 100);

            return allTasks;
        }

        private async Task<List<ClickUpTask>> GetTasksWithMasterScheduleInList(string listId)
        {
            var page = 0;
            var tasks = new ClickUpTasks();
            var allTasks = new List<ClickUpTask>();
            do
            {
                tasks = (await ReturnClickUpData<ClickUpTasks>(_clickUpConfig.GetMasterScheduleTasks, listId, page));
                allTasks.AddRange(tasks.Tasks);
                page += 1;
            } while (tasks.Tasks.Count() == 100);

            return allTasks;
        }

        private async Task<T> ReturnClickUpData<T>(string uri, string id, int? page = null)
        {
            var relativeUri = string.Format(uri, id, page);
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

        //private async Task<IEnumerable<ClickUpTask>> GetKeyDateTasksAsync(ClickUpList list)
        //{
        //    var page = 0;
        //    var tasks = (await GetTasksWithKeyDateInList(list.Id, page)).Tasks;
        //    return tasks;
        //}
    }
}
