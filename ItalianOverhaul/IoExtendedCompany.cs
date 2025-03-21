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
            SocietaPerAzioni,                       // SpA - public company.
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
            }

            // TODO: Add a notification for the player (gotta figure out how to do that).
        }
    }
}
