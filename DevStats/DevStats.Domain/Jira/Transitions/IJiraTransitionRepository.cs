namespace DevStats.Domain.Jira.Transitions
{
    public interface IJiraTransitionRepository
    {
        int? GetTransitionFor(string project, string issueType, string status);

        void RemoveAllTransitions(string project);

        void SaveTransition(string project, string issueType, string status, int transitionId);
    }
}