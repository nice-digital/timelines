using System;
using System.Linq;
using NICE.Timelines.Common;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IConversionService
    {
        TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask);
        int GetACIDFromClickUpTask(ClickUpTask clickUpTask);
        int GetTaskTypeIdFromClickUpTask(ClickUpTask clickUpTask);
        Phase GetPhaseFromClickUpTask(ClickUpTask clickUpTask);
        DateTime? GetActualDateFromClickUpTask(ClickUpTask clickUpTask);
        DateTime? GetDueDateFromClickUpTask(ClickUpTask clickUpTask);
    }

    public class ConversionService : IConversionService
    {
        public TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask)
        {
            var acid = GetACIDFromClickUpTask(clickUpTask);
            var taskTypeId = GetTaskTypeIdFromClickUpTask(clickUpTask);
            var phase = GetPhaseFromClickUpTask(clickUpTask);
            var actualDate = GetActualDateFromClickUpTask(clickUpTask);
            var dueDate = GetDueDateFromClickUpTask(clickUpTask);

            return new TimelineTask(acid, taskTypeId, phase.PhaseId, phase.PhaseDescription, clickUpTask.Space.Id, clickUpTask.Folder.Id, clickUpTask.List.Id, clickUpTask.ClickUpTaskId, dueDate, actualDate, null, phase);
        }

        public int GetACIDFromClickUpTask(ClickUpTask clickUpTask)
        {
            return int.Parse(clickUpTask.CustomFields.First(field => field.FieldId.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>());
            //TODO: ACID is a string in clickup. needs to be a number.
        }

        public int GetTaskTypeIdFromClickUpTask(ClickUpTask clickUpTask)
        {
            var TaskTypeId = 0;
            var taskType = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.TaskTypeId, StringComparison.InvariantCultureIgnoreCase));
            if (taskType != null && taskType.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
            {
                var index = taskType.Value.ToObject<int>();
                TaskTypeId = int.Parse(taskType.ClickUpTypeConfig.Options[index].Name);
            }//TODO Error logging

            return TaskTypeId;
        }

        public Phase GetPhaseFromClickUpTask(ClickUpTask clickUpTask)
        {
            var phase = new Phase();
            phase.PhaseId = 0;
            phase.PhaseDescription = "Not found";
            var phaseField = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.PhaseId, StringComparison.InvariantCultureIgnoreCase));
            if (phaseField != null && phaseField.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
            {
                var index = phaseField.Value.ToObject<int>();
                phase.PhaseId = int.Parse(phaseField.ClickUpTypeConfig.Options[index].Name);

                phase.PhaseDescription = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.PhaseDescription, StringComparison.InvariantCultureIgnoreCase)).ClickUpTypeConfig.Options[index].Name;
            }

            return phase;
        }

        public DateTime? GetActualDateFromClickUpTask(ClickUpTask clickUpTask)
        {
            DateTime? actualDate = null;
            var actualDateStringJsonElement = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (actualDateStringJsonElement.HasValue)
            {
                var actualDateString = actualDateStringJsonElement.Value.ToStringObject();
                if (!string.IsNullOrEmpty(actualDateString))
                {
                    actualDate = double.Parse(actualDateString).ToDateTime();
                }
            }

            return actualDate;
        }

        public DateTime? GetDueDateFromClickUpTask(ClickUpTask clickUpTask)
        {
            return string.IsNullOrEmpty(clickUpTask.DueDateSecondsSinceUnixEpochAsString) ? null : double.Parse(clickUpTask.DueDateSecondsSinceUnixEpochAsString).ToDateTime();
        }
    }
}