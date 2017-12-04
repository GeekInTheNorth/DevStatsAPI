namespace DevStats.Data.Entities
{
    public class ReleaseQuality
    {
        public int ID { get; set; }

        public string Product { get; set; }

        public string Version { get; set; }

        public int Reworks { get; set; }

        public int CodeReviewFailures { get; set; }

        public int TestPlanReviewFailures { get; set; }

        public int BacklogBugsResolved { get; set; }

        public int RegressionBugs { get; set; }

        public int DeploymentIssues { get; set; }

        public int Hotfixes { get; set; }

        public int DaysDelayed { get; set; }
    }
}