using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IDatabaseService
    {
        void SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
        void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly TimelinesContext _context;
        private readonly IConversionService _conversionService;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(TimelinesContext context, IConversionService conversionService, ILogger<DatabaseService> logger)
        {
            _context = context;
            _conversionService = conversionService;
            _logger = logger;
        }

        public void SaveOrUpdateTimelineTask(ClickUpTask clickUpTask)
        {
            var timelineTaskToSaveOrUpdate = _conversionService.ConvertToTimelineTask(clickUpTask);

            if (timelineTaskToSaveOrUpdate.ACID == 0 || timelineTaskToSaveOrUpdate.PhaseId == 0)
            {
                _logger.LogError($"{clickUpTask.ClickUpTaskId} - {clickUpTask.Name} was not saved, ACID or PhaseId missing");
                return;
            }
            
            var existingTimelineTask = _context.TimelineTasks.SingleOrDefault(t => t.ClickUpTaskId.Equals(timelineTaskToSaveOrUpdate.ClickUpTaskId));

            if (existingTimelineTask != null) //it's an update
            {
                if (!TimelineTasksDiffer(existingTimelineTask, timelineTaskToSaveOrUpdate)) //task matches the task in the database, so don't bother updating it.
                    return;

                existingTimelineTask = UpdateExistingTimelineTask(existingTimelineTask, timelineTaskToSaveOrUpdate);
                _context.Update(existingTimelineTask);
            }
            else //save new task
            {
                _context.Add(timelineTaskToSaveOrUpdate);
            }
        }

        public void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase)
        {
            var allTasksInDatabase = _context.TimelineTasks.Where(tt => tt.ACID.Equals(acid)).ToList();

            var tasksThatNeedDeleting = allTasksInDatabase.Where(t => !clickUpIdsThatShouldExistInTheDatabase.Contains(t.ClickUpTaskId));

            _context.RemoveRange(tasksThatNeedDeleting);
        }

        private static bool TimelineTasksDiffer(TimelineTask task1, TimelineTask task2)
        {
            if (!task1.ACID.Equals(task2.ACID) ||
                 !task1.TaskTypeId.Equals(task2.TaskTypeId) ||
                 !task1.PhaseId.Equals(task2.PhaseId) ||
                 !task1.ClickUpSpaceId.Equals(task2.ClickUpSpaceId) ||
                 !task1.ClickUpFolderId.Equals(task2.ClickUpFolderId) ||
                 !task1.ClickUpTaskId.Equals(task2.ClickUpTaskId) ||
                 !task1.DateCompleted.Equals(task2.DateCompleted) ||
                 !task1.DueDate.Equals(task2.DueDate))
            {
                return true;
            }

            return false;
        }

        private TimelineTask UpdateExistingTimelineTask(TimelineTask existingTimelineTask, TimelineTask timelineTaskToSaveOrUpdate)
        {
            existingTimelineTask.ACID = timelineTaskToSaveOrUpdate.ACID;

            existingTimelineTask.TaskTypeId = timelineTaskToSaveOrUpdate.TaskTypeId;
            existingTimelineTask.PhaseId = timelineTaskToSaveOrUpdate.PhaseId;

            existingTimelineTask.ClickUpSpaceId = timelineTaskToSaveOrUpdate.ClickUpSpaceId;
            existingTimelineTask.ClickUpFolderId = timelineTaskToSaveOrUpdate.ClickUpFolderId;
            existingTimelineTask.ClickUpTaskId = timelineTaskToSaveOrUpdate.ClickUpTaskId;

            existingTimelineTask.DateCompleted = timelineTaskToSaveOrUpdate.DateCompleted;
            existingTimelineTask.DueDate = timelineTaskToSaveOrUpdate.DueDate;

            return existingTimelineTask;
        }
    }
}