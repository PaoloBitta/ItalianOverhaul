using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianOverhaul
{
    internal class IoCompanyNameGen
    {
        // Italians have a very specific way of naming their companies, which makes zero sense to anyone else.
        // Seriously companies are either the surname of the owner or a completely random word like SITAS or something,
        // the owner probably forgot what it means too.

        // This is a very simple generator that will generate a company name based on multiple factors:
        // - Should the company name be the owner's surname? Or should it be a random acronym? (50/50 chance)
        // - If a company has multiple owners, the company name will be a random acronym but built from the first two letters of each owner's surname. (70/30 chance aganist random acronym)
        // - Based on the company type, the company name will be suffixed with the company type's abbreviation (SRL, S.p.A., etc.)
        // - If a company has some acquired companies, "Group" will be suffixed to the company name before the company type's abbreviation (e.g. "SITAS Group S.p.A.")
        // - Groups are usually SpAs, but can be SRLs too. ITALIAN COMPANIES ARE A MESS.
        // - Sometimes, tech companies will have "techy" names too, often software companies append "Software" to their name, hardware companies append "Hardware", etc.
        //   This is common and is implemented in the generator, but it should be a 25% chance to append a techy name to the company name to not make it too common.

        // ... more weird naming conventions to be added later, trust me there's a lot of them.

        public IoCompanyNameGen() 
        { 

        }

        IoRandom IoRandom = new IoRandom();

        public string[] TechyBullshitItalianCompaniesAppendToTheirSocialReason =
        {
            "Software",
            "Hardware",
            "Solutions",
            "Systems",
            "Sistemi",
            "Soluzioni",
            "Informatica",
            "Innovation",
            "2000",              // yes, some companies still append "2000" to their name. In 2025.
            "Tech",
            "Technologies",
            "Security"

            // Expand this list to add more techy bullshit Italian companies append to their social reason.
        };

        public string[] consonants = { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z" };

        public string[] vowels = { "A", "E", "I", "O", "U" };

        public enum IoCompanyNameType
        {
            OwnerSurname,
            RandomAcronym,
            MultipleOwnerAcronym
        }

        public string GenerateCompanyName(IoExtendedCompany company)
        {
            string companyName = string.Empty;

            // Generate the company name based on the company type.
            switch (company.CompanyType)
            {
                case IoExtendedCompany.IoCompanyType.SocietaResponsabilitaLimitata:
                    companyName = GenerateSrlCompanyName(company);
                    break;
                case IoExtendedCompany.IoCompanyType.SocietaPerAzioni:
                    companyName = GenerateSpaCompanyName(company);
                    break;
            }

            return companyName;
        }

        private string GenerateSrlCompanyName(IoExtendedCompany company)
        {
            return $"{GenerateCompanyNameInternal(company)} {company.IoCompanyTypeNames[IoExtendedCompany.IoCompanyType.SocietaResponsabilitaLimitata]}";
        }

        private string GenerateSpaCompanyName(IoExtendedCompany company)
        {
            return $"{GenerateCompanyNameInternal(company)} {company.IoCompanyTypeNames[IoExtendedCompany.IoCompanyType.SocietaPerAzioni]}";
        }

        private string GenerateCompanyNameInternal(IoExtendedCompany company)
        {
            string companyName = string.Empty;

            bool useOwnerSurname = IoRandom.NextBool();         // Apply a 50/50 chance to use the owner's surname as the company name.
                                                                // If there are multiple owners, an acronym will be generated instead.

            bool appendTechyName = IoRandom.Next(0, 4) == 1;    // 25% chance to append a techy name to the company name.

            bool isGroup = company.CompaniesBought > 0;         // If the company has acquired companies, append "Group" to the company name.

            // First of all, how many owners does the company have?
            if (useOwnerSurname && company.CountFounders() > 1)
            {
                // If there are multiple owners, generate an acronym based on the first two letters of each owner's surname.
                foreach (Employee e in company.NetworkEmployees)
                {
                    if (e.Founder)
                    {
                        companyName += e.FullName.Substring(0, 2);  // note: this may get the name, not the surname, aka needs testing
                    }
                }
            }
            else if (useOwnerSurname)
            {
                // Just use the owner's surname.
                companyName = company.GetSingleFounderEmployee().FullName;  // note: this may get the name, not the surname, aka needs testing
            }
            else
            {
                // Generate a random acronym, made up of one consonant, one vowel and so on until the name is X characters long.
                int nameLength = IoRandom.Next(4, 10);

                for (int i = 0; i < nameLength; i++)
                {
                    if (i % 2 == 0)
                    {
                        companyName += consonants[IoRandom.Next(0, consonants.Length)];
                    }
                    else
                    {
                        companyName += vowels[IoRandom.Next(0, vowels.Length)];
                    }
                }
            }

            // Append a techy name to the company name.
            if (appendTechyName)
            {
                companyName += " " + TechyBullshitItalianCompaniesAppendToTheirSocialReason[IoRandom.Next(0, TechyBullshitItalianCompaniesAppendToTheirSocialReason.Length)];
            }

            // Append "Group" to the company name if the company has acquired companies.
            if (isGroup)
            {
                companyName += " Group";
            }

            return companyName;
        }
    }
}
