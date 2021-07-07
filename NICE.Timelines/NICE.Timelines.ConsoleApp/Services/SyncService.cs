using System;
using System.Threading.Tasks;
using NICE.Timelines.Configuration;

namespace NICE.Timelines.Services
{
    public interface ISyncService
    {
        public Task Process();
    }

    public class SyncService : ISyncService
    {
        private readonly IClickUpService _clickUpService;
        private readonly ClickUpConfig _clickUpConfig;

        public SyncService(ClickUpConfig clickUpConfig, IClickUpService clickUpService)
        {
            _clickUpConfig = clickUpConfig;
            _clickUpService = clickUpService;
        }

        public async Task Process()
        {
            Console.WriteLine("Started processing");

            foreach (var spaceId in _clickUpConfig.SpaceIds)
            {
                Console.WriteLine($"Started with space: {spaceId}");

                var record = await _clickUpService.ProcessSpace(spaceId);

                Console.WriteLine($"finished with space: {spaceId} records saved or updated: {record}");
            }

            Console.WriteLine("Ended processing");
        }
    }
}
