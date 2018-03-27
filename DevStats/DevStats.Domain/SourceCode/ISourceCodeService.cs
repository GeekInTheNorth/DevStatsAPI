namespace DevStats.Domain.SourceCode
{
    public interface ISourceCodeService
    {
        SourceCodeBranches Get(string repoName);
    }
}
