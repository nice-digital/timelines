using System.Threading.Tasks;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IDatabaseService
    {
        Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly TimelinesContext _dbContext;
        private readonly IConversionService _conversionService;

        public DatabaseService(TimelinesContext dbContext, IConversionService conversionService)
        {
            _dbContext = dbContext;
            _conversionService = conversionService;
        }

        public async Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask)
        {
            var timelineTaskToSaveOrUpdate = _conversionService.ConvertToTimelineTask(clickUpTask);

            return await _dbContext.SaveChangesAsync();
        }

    }
}