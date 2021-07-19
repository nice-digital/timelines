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

        protected int AddTimelineTask(TimelinesContext context, int acid, int taskTypeId, int phaseId, string clickUpSpaceId, string clickUpFolderId, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate)
        {
            var task = new TimelineTask(acid, taskTypeId,  phaseId, clickUpSpaceId, clickUpFolderId, clickUpListId, clickUpTaskId, dueDate, actualDate, null, null);

            context.TimelineTasks.Add(task);
            context.SaveChanges();

            return task.TimelineTaskId;
        }
	}
}