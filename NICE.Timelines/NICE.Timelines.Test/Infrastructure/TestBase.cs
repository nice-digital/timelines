using System;
using Microsoft.EntityFrameworkCore;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.Test.Infrastructure
{
    public class TestBase
    {
        private const string DatabaseName = "TimelinesDB";
        private readonly DbContextOptions<TimelinesContext> _options;

        public TestBase()
        {
            var databaseName = DatabaseName + Guid.NewGuid();
            _options = new DbContextOptionsBuilder<TimelinesContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

		protected static DbContextOptions<TimelinesContext> GetContextOptions()
        {
            var databaseName = DatabaseName + Guid.NewGuid();
            return new DbContextOptionsBuilder<TimelinesContext>()
                .UseInMemoryDatabase(databaseName)
                .EnableSensitiveDataLogging()
                .Options;
        }

        protected int AddTimelineTask(TimelinesContext context, string taskName, int acid, int taskTypeId, int phaseId, int orderInPhase, string clickUpSpaceId, string clickUpFolderId, string clickUpFolderName, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate, bool keyDate, bool keyInfo, bool masterSchedule)
        {
            var task = new TimelineTask(taskName, acid, taskTypeId,  phaseId, orderInPhase, clickUpSpaceId, clickUpFolderId, clickUpFolderName, clickUpListId, clickUpTaskId, dueDate, actualDate, keyDate, keyInfo, masterSchedule, null);

            context.TimelineTasks.Add(task);
            context.SaveChanges();

            return task.TimelineTaskId;
        }
	}
}