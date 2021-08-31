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

            if (timelineTaskToSaveOrUpdate.ACID == 0 || timelineTaskToSaveOrUpdate.PhaseId == 0 || timelineTaskToSaveOrUpdate.OrderInPhase == 0)
            {
                _logger.LogError($"{clickUpTask.ClickUpTaskId} - {clickUpTask.Name} was not saved, ACID, PhaseId or OrderInPhase missing");
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
                !task1.OrderInPhase.Equals(task2.OrderInPhase) ||
                !task1.ClickUpSpaceId.Equals(task2.ClickUpSpaceId) ||
                !task1.ClickUpFolderId.Equals(task2.ClickUpFolderId) ||
                !task1.ClickUpTaskId.Equals(task2.ClickUpTaskId) ||
                !task1.CompletedDate.Equals(task2.CompletedDate) ||
                !task1.DueDate.Equals(task2.DueDate) ||
                !task1.KeyDate.Equals(task2.KeyDate) ||
                !task1.KeyInfo.Equals(task2.KeyInfo) ||
                !task1.MasterSchedule.Equals(task2.MasterSchedule) ||
                !task1.ClickUpFolderName.Equals(task2.ClickUpFolderName) ||
                !task1.TaskName.Equals(task2.TaskName))
            {
                return true;
            }

            return false;
        }

        private TimelineTask UpdateExistingTimelineTask(TimelineTask existingTimelineTask, TimelineTask timelineTaskToSaveOrUpdate)
        {
            existingTimelineTask.TaskName = timelineTaskToSaveOrUpdate.TaskName;
            existingTimelineTask.ACID = timelineTaskToSaveOrUpdate.ACID;

            existingTimelineTask.TaskTypeId = timelineTaskToSaveOrUpdate.TaskTypeId;
            existingTimelineTask.PhaseId = timelineTaskToSaveOrUpdate.PhaseId;
            existingTimelineTask.OrderInPhase = timelineTaskToSaveOrUpdate.OrderInPhase;

            existingTimelineTask.ClickUpSpaceId = timelineTaskToSaveOrUpdate.ClickUpSpaceId;
            existingTimelineTask.ClickUpFolderId = timelineTaskToSaveOrUpdate.ClickUpFolderId;
            existingTimelineTask.ClickUpFolderName = timelineTaskToSaveOrUpdate.ClickUpFolderName;
            existingTimelineTask.ClickUpTaskId = timelineTaskToSaveOrUpdate.ClickUpTaskId;

            existingTimelineTask.CompletedDate = timelineTaskToSaveOrUpdate.CompletedDate;
            existingTimelineTask.DueDate = timelineTaskToSaveOrUpdate.DueDate;

            existingTimelineTask.KeyDate = timelineTaskToSaveOrUpdate.KeyDate;
            existingTimelineTask.KeyInfo = timelineTaskToSaveOrUpdate.KeyInfo;
            existingTimelineTask.MasterSchedule = timelineTaskToSaveOrUpdate.MasterSchedule;

            return existingTimelineTask;
        }
    }
}