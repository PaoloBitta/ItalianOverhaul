using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = DevConsole.Console;

namespace ItalianOverhaul
{
    public class IoExtendedCompany : Company
    {
        IoExtendedCompany() 
        {
            CompanyType = IoCompanyType.SocietaResponsabilitaLimitata;

            // If the company is not player-owned, generate a random name.
            if (!IsPlayerOwned())
            {
                string companyName = IoCompanyNameGen.GenerateCompanyName(this);

                // Set the company name.
                IoUtils.SetReadOnlyField(this, "Name", companyName);
            }
            else
            {
                // Update the company name.
                UpdateCompanyName();
            }
        }

        public enum IoCompanyType
        {
            SocietaResponsabilitaLimitata,          // SRL - pretty much the same as a LLC.
            SocietaPerAzioni,                       // SpA - public company, requires a minimum of 50,000€ to transition to.
        }

        internal Dictionary<IoCompanyType, string> IoCompanyTypeNames = new Dictionary<IoCompanyType, string>
        {
            { IoCompanyType.SocietaResponsabilitaLimitata, "SRL" },
            { IoCompanyType.SocietaPerAzioni, "S.p.A." },
        };

        public IoCompanyType CompanyType { get; set; }
        
        private IoCompanyNameGen IoCompanyNameGen = new IoCompanyNameGen();

        public bool CanTransitionToSpa()
        {
            return Money > 50000;
        }

        public int CountFounders()
        {
            int founders = 0;

            foreach (Employee e in NetworkEmployees)
            {
                if (e.Founder)
                {
                    founders++;
                }
            }

            // Should always be at least 1 founder!
            return founders;
        }

        public Employee GetSingleFounderEmployee()
        {
            foreach (Employee e in NetworkEmployees)
            {
                if (e.Founder)
                {
                    return e;
                }
            }

            return null;
        }

        public void TransitionToSpa()
        {
            if (CanTransitionToSpa())
            {
               GameSettings.Instance.MyCompany.MakeTransaction(-50000, Company.TransactionCategory.Legal, "Transition to SpA");
               CompanyType = IoCompanyType.SocietaPerAzioni;
               UpdateCompanyName();
            }
            else
            {
                // TODO: The player should be notified that they don't have enough money to transition.
            }

            // TODO: Add a notification for the player (gotta figure out how to do that).
        }

        private bool UpdateCompanyName()
        {
            try
            {
                if (IoCompanyTypeNames.TryGetValue(CompanyType, out string companyType))
                {
                    // Build the new company name.
                    string newSocialReason = $"{Name} {companyType}";

                    // Set the new company name.
                    IoUtils.SetReadOnlyField(GameSettings.Instance.MyCompany, "Name", newSocialReason);

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.LogWarning($"Error updating company name: {e.Message}");
            }

            return false;
        }
    }
}
