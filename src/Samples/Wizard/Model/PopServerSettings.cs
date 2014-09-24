using System.ComponentModel;

namespace Wizard.Model
{
    public class PopServerSettings : ServerSettings
    {
        [DisplayName("POP server")]
        public string IncomingMailServer { get; set; }
        [DisplayName("Port")]
        public int IncomingMailServerPort { get; set; }
        [DisplayName("SMTP server")]
        public string OutgoingMailServer { get; set; }
        [DisplayName("Port")]
        public int OutgoingMailServerPort { get; set; }
        [DisplayName("Use Secure Password Authentication (SPA)")]
        public bool SecurePasswordAuthentication { get; set; }
    }
}