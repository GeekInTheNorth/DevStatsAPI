using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevStats.Models.Jira
{
    public class CommitSprintModel : IValidatableObject
    {
        public string TeamName { get; set; }

        public List<string> Keys { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Keys == null || !Keys.Any())
                results.Add(new ValidationResult("At least one story must be committed to the sprint."));

            if (string.IsNullOrWhiteSpace(TeamName))
                results.Add(new ValidationResult("A team name must be included when committing a sprint."));

            return results;
        }
    }
}