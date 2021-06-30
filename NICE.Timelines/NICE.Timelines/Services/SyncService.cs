using System;
using System.Threading.Tasks;

namespace NICE.Timelines.Services
{
    public interface ISyncService
    {
        public Task Process();
    }

    public class SyncService : ISyncService
    {
        private readonly IClickUpService _clickUpService;

        public SyncService( IClickUpService clickUpService)
        {
            _clickUpService = clickUpService;
        }

        public async Task Process()
        {
            Console.WriteLine("Started processing");

            var record = await _clickUpService.GetSpace();

            Console.WriteLine(record);

            Console.WriteLine("Ended processing");
        }
    }
}
