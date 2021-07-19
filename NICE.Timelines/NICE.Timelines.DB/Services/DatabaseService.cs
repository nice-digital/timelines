using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IDatabaseService
    {
        Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
        void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly TimelinesContext _context;
        private readonly IConversionService _conversionService;

        public DatabaseService(TimelinesContext context, IConversionService conversionService)
        {
            _context = context;
            _conversionService = conversionService;
        }

        public async Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask)
        {
            var timelineTaskToSaveOrUpdate = _conversionService.ConvertToTimelineTask(clickUpTask);

            return await _context.SaveChangesAsync(); //TODO change this so save only happens once in the Clickup service
        }

        public void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase)
        {
            var allTasksInDatabase = _context.TimelineTasks.Where(tt => tt.ACID.Equals(acid)).ToList();

            var tasksThatNeedDeleting = allTasksInDatabase.Where(t => !clickUpIdsThatShouldExistInTheDatabase.Contains(t.ClickUpTaskId));

            _context.RemoveRange(tasksThatNeedDeleting);
        }
    }
}