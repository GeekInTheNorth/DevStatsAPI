using System.Linq;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Messages;
using DevStats.Domain.Test.Resources;
using NUnit.Framework;

namespace DevStats.Domain.Test.Jira
{
    [TestFixture]
    public class JsonConvertorTests
    {
        [Test]
        public void GivenAValidJsonResult_WhenIConvertTheJson_ThenIShouldGetAValidModel()
        {
            var jsonFile = TestFiles.JiraBugs;
            var convertor = new JsonConvertor();

            var model = convertor.Deserialize<JiraIssues>(jsonFile);

            Assert.That(model.Issues.Count(), Is.EqualTo(3));
        }
    }
}