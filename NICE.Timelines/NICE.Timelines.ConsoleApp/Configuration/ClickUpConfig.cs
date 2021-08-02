using System.Collections.Generic;
using System.Security.Policy;

namespace NICE.Timelines.Configuration
{
    public class ClickUpConfig
    {
        public static ClickUpConfig Current { get; private set; }
        public ClickUpConfig()
        {
            Current = this;
        }

        public string AccessToken { get; set; }
        public IEnumerable<string> SpaceIds { get; set; }
        public string BaseUrl { get; set; }
        public string GetFolders { get; set; }
        public string GetLists { get; set; }
        public string GetFolderlessLists { get; set; }
        public string GetKeyDateTasks { get; set; }
        public string GetMasterScheduleTasks { get; set; }
    }
}
