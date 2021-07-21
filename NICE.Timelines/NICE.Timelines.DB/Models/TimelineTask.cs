﻿using System;

namespace NICE.Timelines.DB.Models
{
    public class TimelineTask
    {
        public TimelineTask()
        { }

        public TimelineTask(int acid, int taskTypeId, int phaseId, string phaseDescription, string clickUpSpaceId, string clickUpFolderId, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate, TaskType taskType, Phase phase)
        {
            ACID = acid;
            TaskTypeId = taskTypeId;
            PhaseId = phaseId;
            PhaseDescription = phaseDescription;
            ClickUpSpaceId = clickUpSpaceId;
            ClickUpFolderId = clickUpFolderId;
            ClickUpListId = clickUpListId;
            ClickUpTaskId = clickUpTaskId;
            DueDate = dueDate;
            ActualDate = actualDate;
            TaskType = taskType;
            Phase = phase;
        }

        public int TimelineTaskId { get; set; }
        public int ACID { get; set; }

        public int TaskTypeId { get; set; }
        public int PhaseId { get; set; }
        public string PhaseDescription { get; set; }

        public string ClickUpSpaceId { get; set; }
        public string ClickUpFolderId { get; set; }
        public string ClickUpListId { get; set; }
        public string ClickUpTaskId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? ActualDate { get; set; }

        public TaskType TaskType { get; set; }
        public Phase Phase { get; set; }
    }
}
