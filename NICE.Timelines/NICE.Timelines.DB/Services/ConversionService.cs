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

            return new TimelineTask();
        }
    }
}