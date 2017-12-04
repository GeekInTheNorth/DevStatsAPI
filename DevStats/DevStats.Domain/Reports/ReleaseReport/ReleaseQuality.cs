namespace DevStats.Domain.Reports.ReleaseReport
{
    public class ReleaseQuality
    {
        public int Id { get; private set; }

        public string Product { get; private set; }

        public string Version { get; private set; }

        public int Reworks { get; private set; }

        public int CodeReviewFailures { get; private set; }

        public int TestPlanReviewFailures { get; private set; }

        public int BacklogBugsResolved { get; private set; }

        public int RegressionBugs { get; private set; }

        public int DeploymentIssues { get; private set; }

        public int Hotfixes { get; private set; }

        public int DaysDelayed { get; private set; }

        public int Score { get; private set; }

        public ReleaseQuality(
            int id, 
            string product, 
            string version, 
            int reworks, 
            int codeReviewFailures, 
            int testPlanReviewFailures, 
            int backlogBugsResolved, 
            int regressionBugs, 
            int deploymentIssues, 
            int hotfixes, 
            int daysDelayed)
        {
            Id = id;
            Product = product;
            Version = version;
            Reworks = reworks;
            CodeReviewFailures = codeReviewFailures;
            TestPlanReviewFailures = testPlanReviewFailures;
            BacklogBugsResolved = backlogBugsResolved;
            RegressionBugs = regressionBugs;
            DeploymentIssues = deploymentIssues;
            Hotfixes = hotfixes;
            DaysDelayed = daysDelayed;

            var onePointers = Reworks + CodeReviewFailures + TestPlanReviewFailures - BacklogBugsResolved;
            var tenPointers = RegressionBugs + DaysDelayed + DeploymentIssues;
            var hundredPointers = Hotfixes;

            Score = onePointers + (tenPointers * 10) + (hundredPointers * 100);
        }
    }
}