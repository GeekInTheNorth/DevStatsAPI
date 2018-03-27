using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DevStats.Domain.SourceCode
{
    public class SourceCodeBranch
    {
        private string jiraIdRegEx = @"([a-zA-Z]{2,9}[\-]{1}[0-9]{1,9})";
        private string jiraId;
        private bool? isJiraBranch;

        [JsonProperty("branch")]
        public string Name { get; set;  }

        [JsonIgnore]
        public string JiraId
        {
            get
            {
                if (isJiraBranch == null)
                    CheckBranchName();

                return jiraId;
            }
        }

        [JsonIgnore]
        public bool IsJiraBranch
        {
            get
            {
                if (isJiraBranch == null)
                    CheckBranchName();

                return isJiraBranch.Value;
            }
        }

        [JsonIgnore]
        public string Title { get; set; }

        [JsonIgnore]
        public string Status { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        private void CheckBranchName()
        {
            var regEx = new Regex(jiraIdRegEx);
            isJiraBranch = regEx.IsMatch(Name);
            jiraId = isJiraBranch.Value ? regEx.Match(Name).ToString().ToUpper() : string.Empty;
        }
    }
}
