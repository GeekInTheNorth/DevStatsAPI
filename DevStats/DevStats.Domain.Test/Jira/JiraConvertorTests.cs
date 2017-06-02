using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Test.Resources;
using NUnit.Framework;

namespace DevStats.Domain.Test.Jira
{
    [TestFixture]
    public class JiraConvertorTests
    {
        [Test]
        public void GivenAValidJsonResult_WhenIConvertTheJson_ThenIShouldGetAValidModel()
        {
            var jsonFile = TestFiles.JiraBugs;
            var convertor = new JiraConvertor();

            var model = convertor.Deserialize<JiraIssues>(jsonFile);

            Assert.That(model.Issues.Count(), Is.EqualTo(3));
        }
    }
}