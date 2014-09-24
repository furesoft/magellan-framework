using Magellan.ComponentModel;

namespace Wizard.Model
{
    public enum ExchangeSecurityMode
    {
        [EnumDisplayName("Negotiate")]
        Negotiate,
        [EnumDisplayName("NTLM")]
        Ntlm,
        [EnumDisplayName("Kerberos")]
        Kerberos
    }
}
