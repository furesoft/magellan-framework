using System.ComponentModel;

namespace Wizard.Model
{
    public class ExchangeServerSettings : ServerSettings
    {
        public string Server { get; set; }
        public string Username { get; set; }

        [DisplayName("Use cached Exchange mode")]
        public bool CachedExchangeMode { get; set; }

        [DisplayName("Security mode")]
        public ExchangeSecurityMode SecurityMode { get; set; }
    }
}