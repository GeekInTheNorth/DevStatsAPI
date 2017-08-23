using System.Collections.Generic;
using System.Linq;

namespace DevStats.Models.Jira
{
    public class CommitSprintModel
    {
        public List<string> Keys { get; set; }

        public bool IsValid()
        {
            if (Keys == null || !Keys.Any())
                return false;

            return true;
        }
    }
}