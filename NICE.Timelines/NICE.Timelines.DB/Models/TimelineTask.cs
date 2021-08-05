using System;

namespace NICE.Timelines.DB.Models
{
    public class TimelineTask
    {
        public TimelineTask()
        { }

        public TimelineTask(int acid, int? taskTypeId, int phaseId, string clickUpSpaceId, string clickUpFolderId, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? dateCompleted, Phase? phase)
        {
            ACID = acid;
            TaskTypeId = taskTypeId;
            PhaseId = phaseId;
            ClickUpSpaceId = clickUpSpaceId;
            ClickUpFolderId = clickUpFolderId;
            ClickUpListId = clickUpListId;
            ClickUpTaskId = clickUpTaskId;
            DueDate = dueDate;
            DateCompleted = dateCompleted;
            Phase = phase;
        }

        public int TimelineTaskId { get; set; }
        public int ACID { get; set; }

        public int? TaskTypeId { get; set; }
        public int PhaseId { get; set; }

        public string ClickUpSpaceId { get; set; }
        public string ClickUpFolderId { get; set; }
        public string ClickUpListId { get; set; }
        public string ClickUpTaskId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? DateCompleted { get; set; }

        public Phase Phase { get; set; }
    }
}
