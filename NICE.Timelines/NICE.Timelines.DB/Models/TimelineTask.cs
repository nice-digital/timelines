using System;

namespace NICE.Timelines.DB.Models
{
    public class TimelineTask
    {
        public TimelineTask()
        { }

        public TimelineTask(int acid, int stepId, int stageId, string clickUpSpaceId, string clickUpFolderId, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate, Step step, Stage stage)
        {
            ACID = acid;
            StepId = stepId;
            StageId = stageId;
            ClickUpSpaceId = clickUpSpaceId;
            ClickUpFolderId = clickUpFolderId;
            ClickUpListId = clickUpListId;
            ClickUpTaskId = clickUpTaskId;
            DueDate = dueDate;
            ActualDate = actualDate;
            Step = step;
            Stage = stage;
        }

        public int TimelineTaskId { get; set; }
        public int ACID { get; set; }

        public int StepId { get; set; }
        public int StageId { get; set; }


        public string ClickUpSpaceId { get; set; }
        public string ClickUpFolderId { get; set; }
        public string ClickUpListId { get; set; }
        public string ClickUpTaskId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? ActualDate { get; set; }

        public Step Step { get; set; }
        public Stage Stage { get; set; }
    }
}
