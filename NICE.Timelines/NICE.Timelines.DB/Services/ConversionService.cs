using System;
using System.Linq;
using NICE.Timelines.Common;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
    public interface IConversionService
    {
        int GetACIDFromClickUpTask(ClickUpTask clickUpTask);
        TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask);
    }

    public class ConversionService : IConversionService
    {
        public int GetACIDFromClickUpTask(ClickUpTask clickUpTask)
        {
            return int.Parse(clickUpTask.CustomFields.First(field => field.FieldId.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>()); //TODO: ACID is a string in clickup. needs to be a number.
        }

        public TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask)
        {
            var acid = GetACIDFromClickUpTask(clickUpTask);

            var TaskTypeId = 0;
            var stepField = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.TaskTypeId, StringComparison.InvariantCultureIgnoreCase));
            if (stepField != null && stepField.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
            {
                var index = stepField.Value.ToObject<int>();
                TaskTypeId = int.Parse(stepField.ClickUpTypeConfig.Options[index].Name);
            }

            return new TimelineTask();
        }
    }
}