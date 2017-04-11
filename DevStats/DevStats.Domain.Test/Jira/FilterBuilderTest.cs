using DevStats.Domain.Jira;
using NUnit.Framework;

namespace DevStats.Domain.Test.Jira
{
    [TestFixture]
    public class FilterBuilderTest
    {
        [Test]
        public void GivenIHaveNoFilters_WhenIBuild_ThenIShouldGetAnEmptyString()
        {
            var filter = FilterBuilder.Create()
                                      .Build();

            Assert.That(filter, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCase(JiraProject.CascadePayroll, "project in (CPR)")]
        [TestCase(JiraProject.CascadeHR, "project in (CHR)")]
        [TestCase(JiraProject.CascadeGo, "project in (OCT)")]
        public void GivenIFilterJustOneProject_WhenIBuild_ThenIShouldGetJustOneInTheFilter(JiraProject project, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithAProject(project)
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        [TestCase(JiraProject.CascadePayroll, JiraProject.CascadeHR, "project in (CPR,CHR)")]
        [TestCase(JiraProject.CascadeHR, JiraProject.CascadePayroll, "project in (CHR,CPR)")]
        [TestCase(JiraProject.CascadeHR, JiraProject.CascadeGo, "project in (CHR,OCT)")]
        public void GivenIFilterMoreThanOneProject_WhenIBuild_ThenIShouldGetAllRequestedProjects(JiraProject project1, JiraProject project2, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithAProject(project1)
                                      .WithAProject(project2)
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        [TestCase(JiraProject.CascadePayroll, "project in (CPR)")]
        [TestCase(JiraProject.CascadeHR, "project in (CHR)")]
        [TestCase(JiraProject.CascadeGo, "project in (OCT)")]
        public void GivenIAddAProjectTwice_WhenIBuild_ThenTheFilterShouldNotIncludeDuplicateProjects(JiraProject project, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithAProject(project)
                                      .WithAProject(project)
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        [TestCase(JiraState.All, "")]
        [TestCase(JiraState.Todo, "status = \"To Do\"")]
        [TestCase(JiraState.Done, "status = \"Done\"")]
        public void GivenIAddAState_WhenIBuild_ThenTheFilterShouldSpecifyTheState(JiraState status, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithIssueStatesOf(status)
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        [TestCase(JiraProject.CascadePayroll, JiraState.Todo, "project in (CPR) AND status = \"To Do\"")]
        [TestCase(JiraProject.CascadeHR, JiraState.Done, "project in (CHR) AND status = \"Done\"")]
        public void GivenIAddAStateAndProject_WhenIBuild_ThenTheFilterShouldSpecifyBothOptions(JiraProject project, JiraState status, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithAProject(project)
                                      .WithIssueStatesOf(status)
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        public void GivenIWantUpdatedItemsOnly_WhenIBuild_ThenTheFilterShouldSpecifyTheDateRange()
        {
            var filter = FilterBuilder.Create()
                                      .WithItemsUpdatedToday()
                                      .Build();

            Assert.That(filter, Is.EqualTo("updatedDate >= startOfDay() and updatedDate<endOfDay()"));
        }
    }
}