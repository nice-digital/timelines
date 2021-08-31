using System;

namespace NICE.Timelines.DB.Models
{
    public class TimelineTask
    {
        public TimelineTask()
        { }

        public TimelineTask(string taskName, int acid, int taskTypeId, int phaseId, int orderInPhase, string clickUpSpaceId, string clickUpFolderId, string clickUpFolderName, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? completedDate, bool keyDate, bool keyInfo, bool masterSchedule, Phase? phase)
        {
            TaskName = taskName;
            ACID = acid;
            TaskTypeId = taskTypeId;
            PhaseId = phaseId;
            OrderInPhase = orderInPhase;
            ClickUpSpaceId = clickUpSpaceId;
            ClickUpFolderId = clickUpFolderId;
            ClickUpFolderName = clickUpFolderName;
            ClickUpListId = clickUpListId;
            ClickUpTaskId = clickUpTaskId;
            DueDate = dueDate;
            CompletedDate = completedDate;
            KeyDate = keyDate;
            KeyInfo = keyInfo;
            MasterSchedule = masterSchedule;
            Phase = phase;
        }

        public int TimelineTaskId { get; set; }
        public string TaskName { get; set; }
        public int ACID { get; set; }
        
        public int TaskTypeId { get; set; }
        public int PhaseId { get; set; }
        public int OrderInPhase { get; set; }

        public string ClickUpSpaceId { get; set; }
        public string ClickUpFolderId { get; set; }
        public string ClickUpFolderName { get; set; }
        public string ClickUpListId { get; set; }
        public string ClickUpTaskId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public bool KeyInfo { get; set; }
        public bool KeyDate { get; set; }
        public bool MasterSchedule { get; set; }

        public Phase Phase { get; set; }
    }
}
