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
        DateTime? GetDateCompleted(ClickUpTask clickUpTask);
        DateTime? GetDueDate(ClickUpTask clickUpTask);
        bool GetBooleanCustomField(ClickUpTask clickUpTask, string clickUpField);
        int GetIntegerCustomField(ClickUpTask clickUpTask, string clickUpField, bool mandatory, string errorMessage);
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
            var acid = GetIntegerCustomField(clickUpTask, Constants.ClickUp.Fields.ACID, true, "acid");
            var taskTypeId = GetIntegerCustomField(clickUpTask, Constants.ClickUp.Fields.TaskTypeId, false);
            var phaseId = GetIntegerCustomField(clickUpTask, Constants.ClickUp.Fields.PhaseId, true, "phaseId");
            var orderInPhase = GetIntegerCustomField(clickUpTask, Constants.ClickUp.Fields.OrderInPhase, true, "orderInPhase");
            var actualDate = GetDateCompleted(clickUpTask);
            var dueDate = GetDueDate(clickUpTask);
            var keyDate = GetBooleanCustomField(clickUpTask, Constants.ClickUp.Fields.KeyDate);
            var keyInfo = GetBooleanCustomField(clickUpTask, Constants.ClickUp.Fields.KeyInfo);
            var masterSchedule = GetBooleanCustomField(clickUpTask, Constants.ClickUp.Fields.MasterSchedule);

            return new TimelineTask(clickUpTask.Name, acid, taskTypeId, phaseId, orderInPhase, clickUpTask.Space.Id, clickUpTask.Folder.Id, clickUpTask.Folder.Name, clickUpTask.List.Id, clickUpTask.ClickUpTaskId, dueDate, actualDate, keyDate, keyInfo, masterSchedule, null);
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
        
        public bool GetBooleanCustomField(ClickUpTask clickUpTask, string clickUpField)
        {
            var customField = false;
            var customFieldValue = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(clickUpField, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (customFieldValue != null && customFieldValue.Value.ValueKind != JsonValueKind.Undefined)
            {
                var customFieldString = customFieldValue.Value.ToObject<string>(); 
                customField = bool.Parse(customFieldString);
            }
            
            return customField;
        }

        public int GetIntegerCustomField(ClickUpTask clickUpTask, string clickUpField, bool mandatory, string errorMessage = "")
        {
            var customField = 0;

            var customFieldValue = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(clickUpField, StringComparison.InvariantCultureIgnoreCase));
            if (customFieldValue != null && customFieldValue.Value.ValueKind != JsonValueKind.Undefined)
            {
                var customFieldString = customFieldValue.Value.ToObject<string>();
                customField = int.Parse(customFieldString);
            }
            else if (mandatory)
                _logger.LogError($"{errorMessage} for task:{clickUpTask.ClickUpTaskId} - {clickUpTask.Name}  is null or undefined");

            return customField;
        }
    }
}