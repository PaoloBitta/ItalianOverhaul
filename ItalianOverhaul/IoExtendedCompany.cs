using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianOverhaul
{
    internal class IoExtendedCompany : Company
    {
        IoExtendedCompany() { }

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

        public bool CanTransitionToSpa()
        {
            return Money > 50000;
        }

        public void TransitionToSpa()
        {
            if (CanTransitionToSpa())
            {
               MakeTransaction(-50000, TransactionCategory.Legal, "Transition to S.p.A.");
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
            if (IoCompanyTypeNames.TryGetValue(CompanyType, out string companyType))
            {
                // Build the new company name.
                string newSocialReason = $"{Name} {companyType}";

                // Set the new company name.
                IoUtils.SetReadOnlyField(this, "Name", newSocialReason);

                return true;
            }

            return false;
        }
    }
}
