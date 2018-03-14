using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevStats.Domain.Bitbucket
{
    public class BuildStatusModel : IValidatableObject
    {
        public string BuildNumber { get; set; }

        public string CommitSha { get; set; }

        public string Status { get; set; }

        public string RepositoryName { get; set; }

        public string BitbucketOrganisation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var allowedStates = new List<string> { "INPROGRESS", "SUCCESSFUL", "FAILED" };

            if (string.IsNullOrWhiteSpace(BuildNumber))
                results.Add(new ValidationResult("BuildNumber must be provided"));

            if (string.IsNullOrWhiteSpace(CommitSha))
                results.Add(new ValidationResult("CommitSha must be provided"));

            if (string.IsNullOrWhiteSpace(Status) || !allowedStates.Contains(Status, StringComparer.OrdinalIgnoreCase))
                results.Add(new ValidationResult("A valid Status must be provided"));

            if (string.IsNullOrWhiteSpace(RepositoryName))
                results.Add(new ValidationResult("RepositoryName must be provided"));

            if (string.IsNullOrWhiteSpace(BitbucketOrganisation))
                results.Add(new ValidationResult("BitbucketOrganisation must be provided"));

            return results;
        }
    }
}