using System;
using Magellan.Framework;
using Wizard.Infrastructure;
using Wizard.Model;

namespace Wizard.Controllers
{
    [Sleep]
    public class WizardController : Controller
    {
        public ActionResult Welcome()
        {
            return Page("Welcome").ClearNavigationHistory();
        }

        public ActionResult AccountDetails()
        {
            return Page("AccountDetails", new NewAccount { FullName = Environment.UserName });
        }

        public ActionResult SelectServerType(NewAccount wizardState)
        {
            return Page("ServerType", wizardState);
        }

        public ActionResult EditPopServerDetails(NewAccount wizardState)
        {
            wizardState.Server = new PopServerSettings() {IncomingMailServerPort = 110, OutgoingMailServerPort = 25};
            return Page("PopServer", wizardState);
        }

        public ActionResult EditExchangeServerDetails(NewAccount wizardState)
        {
            wizardState.Server = new ExchangeServerSettings();
            return Page("ExchangeServer", wizardState);
        }

        public ActionResult SaveServerDetails(NewAccount wizardState)
        {
            return Page("Summary", wizardState);
        }

        public ActionResult Commit(NewAccount wizardState)
        {
            // This is the point where we might choose to commit all of the data gathered in the wizard
            return Redirect("Welcome");
        }
    }
}


