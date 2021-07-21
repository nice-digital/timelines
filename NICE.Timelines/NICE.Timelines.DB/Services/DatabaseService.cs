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
            var existingTimelineTask = _context.TimelineTasks.SingleOrDefault(t => t.ClickUpTaskId.Equals(timelineTaskToSaveOrUpdate.ClickUpTaskId));

            if (existingTimelineTask != null) //it's an update
            {
                if (!TimelineTasksDiffer(existingTimelineTask, timelineTaskToSaveOrUpdate)) //task matches the task in the database, so don't bother updating it.
                {
                    return 0;
                }
            }
            else //save new task
            {
                _context.Add(timelineTaskToSaveOrUpdate);
            }

            return await _context.SaveChangesAsync(); //TODO change this so save only happens once in the Clickup service
        }

        public void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase)
        {
            var allTasksInDatabase = _context.TimelineTasks.Where(tt => tt.ACID.Equals(acid)).ToList();

            var tasksThatNeedDeleting = allTasksInDatabase.Where(t => !clickUpIdsThatShouldExistInTheDatabase.Contains(t.ClickUpTaskId));

            _context.RemoveRange(tasksThatNeedDeleting);
        }

        private static bool TimelineTasksDiffer(TimelineTask task1, TimelineTask task2)
        {
            if ((!task1.ACID.Equals(task2.ACID) ||
                 (!task1.TaskTypeId.Equals(task2.TaskTypeId)) ||
                 (!task1.PhaseId.Equals(task2.PhaseId)) ||
                 (!task1.PhaseDescription.Equals(task2.PhaseDescription)) ||
                 (!task1.ClickUpSpaceId.Equals(task2.ClickUpSpaceId)) ||
                 (!task1.ClickUpFolderId.Equals(task2.ClickUpFolderId)) ||
                 (!task1.ClickUpTaskId.Equals(task2.ClickUpTaskId)) ||
                 (!task1.ActualDate.Equals(task2.ActualDate)) ||
                 (!task1.DueDate.Equals(task2.DueDate))))
            {
                return true;
            }

            return false;
        }
    }
}