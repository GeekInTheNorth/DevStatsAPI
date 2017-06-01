namespace DevStats.Data.Entities
{
    public class JiraTransistion
    {
        public int ID { get; set; }

        public string Project { get; set; }

        public string IssueType { get; set; }

        public string StatusName { get; set; }

        public int TransitionId { get; set; }
    }
}