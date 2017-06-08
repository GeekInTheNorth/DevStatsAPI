using System.Collections.Generic;
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
        [TestCase("CPR", "project in (CPR)")]
        [TestCase("CHR", "project in (CHR)")]
        [TestCase("OCT", "project in (OCT)")]
        public void GivenIFilterJustOneProject_WhenIBuild_ThenIShouldGetJustOneInTheFilter(string project, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithProjects(new List<string> { project })
                                      .Build();

            Assert.That(filter, Is.EqualTo(expectedFilter));
        }

        [Test]
        [TestCase("CPR", "CHR", "project in (CPR,CHR)")]
        [TestCase("CHR", "CPR", "project in (CHR,CPR)")]
        [TestCase("CHR", "OCT", "project in (CHR,OCT)")]
        public void GivenIFilterMoreThanOneProject_WhenIBuild_ThenIShouldGetAllRequestedProjects(string project1, string project2, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithProjects(new List<string> { project1, project2 })
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
        [TestCase("CPR", JiraState.Todo, "project in (CPR) AND status = \"To Do\"")]
        [TestCase("CHR", JiraState.Done, "project in (CHR) AND status = \"Done\"")]
        public void GivenIAddAStateAndProject_WhenIBuild_ThenTheFilterShouldSpecifyBothOptions(string project, JiraState status, string expectedFilter)
        {
            var filter = FilterBuilder.Create()
                                      .WithProjects(new List<string> { project })
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