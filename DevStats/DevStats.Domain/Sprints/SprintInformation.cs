using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Sprints
{
    public class SprintInformation
    {
        public int BoardId { get; private set; }

        public string BoardName { get; private set; }

        public int SprintId { get; private set; }

        public string SprintName { get; private set; }

        public string FullName
        {
            get { return string.Format("{0} - {1}", BoardName, SprintName); }
        }

        public SprintInformation(NameField board, NameField sprint)
        {
            BoardId = board.Id;
            BoardName = board.Name;
            SprintId = sprint.Id;
            SprintName = sprint.Name;
        }
    }
}