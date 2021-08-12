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
        int? GetTaskTypeId(ClickUpTask clickUpTask);
        int GetPhaseId(ClickUpTask clickUpTask);
        DateTime? GetDateCompleted(ClickUpTask clickUpTask);
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
            var orderInPhase = GetOrderInPhase(clickUpTask);
            var actualDate = GetDateCompleted(clickUpTask);
            var dueDate = GetDueDate(clickUpTask);

            return new TimelineTask(acid, taskTypeId, phaseId, orderInPhase, clickUpTask.Space.Id, clickUpTask.Folder.Id, clickUpTask.List.Id, clickUpTask.ClickUpTaskId, dueDate, actualDate, null);
        }

        public int GetACID(ClickUpTask clickUpTask)
        {
            var acidId = 0;
            var acid = clickUpTask.CustomFields.First(field => field.FieldId.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase));
            if (acid != null && acid.Value.ValueKind != JsonValueKind.Undefined)
            {
                var id = acid.Value.ToObject<string>();
                acidId = int.Parse(id);
            }
            else
                _logger.LogError($"acid for task: {clickUpTask.ClickUpTaskId} - {clickUpTask.Name} is null or undefined");

            return acidId;
        }

        public int? GetTaskTypeId(ClickUpTask clickUpTask)
        {
            int? taskTypeId = 0;
            var taskType = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.TaskTypeId, StringComparison.InvariantCultureIgnoreCase));
            if (taskType != null && taskType.Value.ValueKind != JsonValueKind.Undefined)
            {
                var id = taskType.Value.ToObject<string>();
                taskTypeId = int.Parse(id);
            }
            
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
                _logger.LogError($"phaseId for task:{clickUpTask.ClickUpTaskId} - {clickUpTask.Name}  is null or undefined");

            return phaseId;
        }

        public int? GetOrderInPhase(ClickUpTask clickUpTask)
        {
            int? orderInPhase = null;

            var orderInPhaseValue = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.OrderInPhase, StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (orderInPhaseValue != null && orderInPhaseValue.Value.ValueKind != JsonValueKind.Undefined)
            {
                var orderInPhaseString = orderInPhaseValue.Value.ToObject<string>();
                orderInPhase = int.Parse(orderInPhaseString);
            }

            return orderInPhase;
        }

        public DateTime? GetDateCompleted(ClickUpTask clickUpTask)
        {
            DateTime? dateCompleted = null;
            var dateCompletedValue = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.CompletedDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (dateCompletedValue != null && dateCompletedValue.Value.ValueKind != JsonValueKind.Undefined)
            {
                var dateCompletedString = dateCompletedValue.Value.ToObject<string>();
                dateCompleted = double.Parse(dateCompletedString).ToDateTime();
            }

            return dateCompleted;
        }

        public DateTime? GetDueDate(ClickUpTask clickUpTask)
        {
            DateTime? dueDate = null;
            if (!string.IsNullOrEmpty(clickUpTask.DueDateSecondsSinceUnixEpochAsString)) 
                dueDate = double.Parse(clickUpTask.DueDateSecondsSinceUnixEpochAsString).ToDateTime();

            return dueDate;
        }
    }
}