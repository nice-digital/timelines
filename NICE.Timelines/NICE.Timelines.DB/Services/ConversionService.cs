using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Common;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IConversionService
    {
        TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask);
        int GetACID(ClickUpTask clickUpTask);
        int GetTaskTypeId(ClickUpTask clickUpTask);
        int GetPhaseId(ClickUpTask clickUpTask);
        DateTime? GetActualDate(ClickUpTask clickUpTask);
        DateTime? GetDueDate(ClickUpTask clickUpTask);
    }

    public class ConversionService : IConversionService
    {
        private readonly ILogger<ConversionService> _logger;

        public ConversionService(ILogger<ConversionService> logger)
        {
            _logger = logger;
        }

        public TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask)
        {
            var acid = GetACID(clickUpTask);
            var taskTypeId = GetTaskTypeId(clickUpTask);
            var phaseId = GetPhaseId(clickUpTask);
            var actualDate = GetActualDate(clickUpTask);
            var dueDate = GetDueDate(clickUpTask);

            return new TimelineTask(acid, taskTypeId, phaseId, clickUpTask.Space.Id, clickUpTask.Folder.Id, clickUpTask.List.Id, clickUpTask.ClickUpTaskId, dueDate, actualDate, null);
        }

        public int GetACID(ClickUpTask clickUpTask)
        {
            return int.Parse(clickUpTask.CustomFields.First(field => field.FieldId.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>());
            //TODO: ACID is a string in clickup. needs to be a number.
        }

        public int GetTaskTypeId(ClickUpTask clickUpTask)
        {
            var taskTypeId = 0;
            var taskType = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.TaskTypeId, StringComparison.InvariantCultureIgnoreCase));
            if (taskType != null && taskType.Value.ValueKind != JsonValueKind.Undefined)
            {
                var id = taskType.Value.ToObject<string>();
                taskTypeId = int.Parse(id);
            }
            else
                _logger.LogError($"taskType for task:{clickUpTask.ClickUpTaskId} is null or undefined");
            
            return taskTypeId;
        }

        public int GetPhaseId(ClickUpTask clickUpTask)
        {
            var phaseId = 0;

            var phaseField = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.PhaseId, StringComparison.InvariantCultureIgnoreCase));
            if (phaseField != null && phaseField.Value.ValueKind != JsonValueKind.Undefined)
            {
                var id = phaseField.Value.ToObject<string>();
                phaseId = int.Parse(id);
            }
            else
                _logger.LogError($"phaseId for task:{clickUpTask.ClickUpTaskId} is null or undefined");

            return phaseId;
        }

        public DateTime? GetActualDate(ClickUpTask clickUpTask)
        {
            DateTime? actualDate = null;
            var actualDateValue = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (actualDateValue != null && actualDateValue.Value.ValueKind != JsonValueKind.Undefined)
            {
                var actualDateString = actualDateValue.Value.ToObject<string>();
                actualDate = double.Parse(actualDateString).ToDateTime();
            }
            else
                _logger.LogError($"Actual date for task:{clickUpTask.ClickUpTaskId} is null or empty");

            return actualDate;
        }

        public DateTime? GetDueDate(ClickUpTask clickUpTask)
        {
            DateTime? dueDate = null;
            if (!string.IsNullOrEmpty(clickUpTask.DueDateSecondsSinceUnixEpochAsString)) 
                dueDate = double.Parse(clickUpTask.DueDateSecondsSinceUnixEpochAsString).ToDateTime();
            else
                _logger.LogError($"dueDate for task:{clickUpTask.ClickUpTaskId} is null or empty");

            return dueDate;
        }
    }
}