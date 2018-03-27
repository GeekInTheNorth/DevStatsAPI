using System.Collections.Generic;

namespace DevStats.Domain.SourceCode
{
    public class SourceCodeBranches
    {
        public string RepositoryName { get; set; }

        public List<SourceCodeBranch> Branches { get; set; }
    }
}
