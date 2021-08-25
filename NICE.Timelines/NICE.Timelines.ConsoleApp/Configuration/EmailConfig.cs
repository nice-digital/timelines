using System.Collections.Generic;

namespace NICE.Timelines.Configuration
{
    public class EmailConfig
    {
        public static EmailConfig Current { get; private set; }
        public EmailConfig()
        {
            Current = this;
        }
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderAddress { get; set; }
        public IEnumerable<string> RecipientAddresses { get; set; }
    }
}
