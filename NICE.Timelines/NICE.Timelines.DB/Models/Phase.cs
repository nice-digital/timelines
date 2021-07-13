using System.Collections.Generic;

namespace NICE.Timelines.DB.Models
{
    public class Phase
    {
        public int PhaseId { get; set; }
        public string PhaseDescription { get; set; }

        public ICollection<TimelineTask> TimelineTasks { get; set; }
    }
}