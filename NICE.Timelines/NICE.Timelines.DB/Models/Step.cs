using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NICE.Timelines.DB.Models
{
    public class Step
    {
        public int StepId { get; set; }
        public string Description { get; set; }

        public ICollection<TimelineTask> TimelineTasks { get; set; }
    }
}
