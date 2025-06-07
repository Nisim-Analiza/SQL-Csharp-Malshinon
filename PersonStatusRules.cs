using System;

using Models;

namespace Services
{
    public class PersonStatusRules
    {
        public bool ShouldBecomePotentialAgent(Person person, string lastReportText)
        {
            return person.Type == "reporter" && person.NumReports >= 10 && lastReportText.Length >= 100;
        }

        public bool ShouldBeFlaggedAsThreat(Person person)
        {
            return person.Type == "target" && person.NumMentions >= 20;
        }
    }
}
