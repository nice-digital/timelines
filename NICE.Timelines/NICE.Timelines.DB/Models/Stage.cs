using System.Collections.Generic;

namespace NICE.Timelines.DB.Models
{
    public class Stage
    {
        public int StageId { get; set; }
        public string Description { get; set; }

        public ICollection<TimelineTask> TimelineTasks { get; set; }
    }
}