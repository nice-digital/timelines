using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ISyncService> _logger;
        private readonly IEmailService _emailService;

        public SyncService(ClickUpConfig clickUpConfig, IClickUpService clickUpService, ILogger<ISyncService> logger, IEmailService emailService)
        {
            _clickUpConfig = clickUpConfig;
            _clickUpService = clickUpService;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Process()
        {
            Console.WriteLine("Started processing");
            _logger.LogInformation("Started processing");

            try
            {
                foreach (var spaceId in _clickUpConfig.SpaceIds)
                {
                    Console.WriteLine($"Started with space: {spaceId}");
                    var record = await _clickUpService.ProcessSpace(spaceId);
                    Console.WriteLine($"finished with space: {spaceId} records saved or updated: {record}");
                }
            }
            catch (Exception e)
            {
                _emailService.SendEmail("Timelines, an error occured", e.Message);
            }

            Console.WriteLine("Ended processing");
            _logger.LogInformation("Ended processing");
        }
    }
}
