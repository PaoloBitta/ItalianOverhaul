using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = DevConsole.Console;

namespace ItalianOverhaul
{
    internal class IoCoreLogic
    {
        // This is the core logic of the Italian Overhaul mod.
        // This class will handle the core loop of the mod, like generating events, renaming companies, etc.
        // This class will be instantiated when the game is loaded, and will be the main class of the mod.

        public IoCoreLogic()
        {

        }

        List<IoExtendedCompany> Companies = new List<IoExtendedCompany>();

        IoExtendedCompany PlayerCompany = new IoExtendedCompany(GameSettings.Instance.MyCompany);

        public bool ShouldLoopQuit = false;

        public void OnGameQuit(object sender, EventArgs e) { ShouldLoopQuit = true; }

        public void StartLoop()
        {

            if (!InitializeGameSetting())
            {
                Console.LogError("Error initializing game setting. The mod will not start.");
                return;
            }

            while (!ShouldLoopQuit)
            {
                // For now do nothing.
            }
        }

        private bool InitializeGameSetting()
        {
            // Before starting the loop, let's enumerate all companies in the market,
            // populate the Companies list and instantiate the PlayerCompany object.
            try
            {
                IEnumerable<Company> ActiveCompanies = MarketSimulation.Active.GetAllCompanies();

                foreach (Company company in ActiveCompanies)
                {
                    // IoExtendedCompany's constructor should automatically update the company name.
                    IoExtendedCompany newCompany = new IoExtendedCompany(company);

                    if (!company.IsPlayerOwned())
                    {
                        Companies.Add(newCompany);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.LogError($"Error initializing game setting: {e.Message}");
                return false;
            }
        }
    }
}
