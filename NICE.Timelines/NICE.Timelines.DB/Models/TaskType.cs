using System.Collections.Generic;

namespace NICE.Timelines.DB.Models
{
    public class TaskType
    {
        public int TaskTypeId { get; set; }

        public ICollection<TimelineTask> TimelineTasks { get; set; }
    }
}
