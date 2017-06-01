using System.Linq;
using DevStats.Domain.Jira.Transitions;

namespace DevStats.Data.Repositories
{
    public class JiraTransitionRepository : BaseRepository, IJiraTransitionRepository
    {
        public int? GetTransitionFor(string project, string issueType, string status)
        {
            var transition = Context.JiraTransitions
                                    .Where(x => x.Project == project && x.IssueType == issueType && x.StatusName == status)
                                    .FirstOrDefault();

            if (transition == null)
                return null;

            return transition.TransitionId;
        }

        public void RemoveAllTransitions(string project)
        {
            var transitions = Context.JiraTransitions.Where(x => x.Project == project).ToList();

            foreach (var transition in transitions)
                Context.JiraTransitions.Remove(transition);

            Context.SaveChanges();
        }

        public void SaveTransition(string project, string issueType, string status, int transitionId)
        {
            var transition = Context.JiraTransitions
                                    .Where(x => x.Project == project && x.IssueType == issueType && x.StatusName == status)
                                    .FirstOrDefault();

            if (transition == null)
            {
                transition = new Entities.JiraTransistion
                {
                    Project = project,
                    IssueType = issueType,
                    StatusName = status,
                    TransitionId = transitionId
                };

                Context.JiraTransitions.Add(transition);
            }

            transition.TransitionId = transitionId;

            Context.SaveChanges();
        }
    }
}