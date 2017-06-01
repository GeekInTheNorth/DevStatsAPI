namespace DevStats.Domain.Jira.Transitions
{
    public interface IJiraTransitionService
    {
        void UpdateTransitions(string project, bool fullRefresh);

        void UpdateTransitions(bool fullRefresh);
    }
}
