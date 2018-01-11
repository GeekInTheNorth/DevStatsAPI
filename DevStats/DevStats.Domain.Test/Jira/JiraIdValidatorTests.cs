using DevStats.Domain.Jira;
using NUnit.Framework;

namespace DevStats.Domain.Test.Jira
{
    [TestFixture]
    public class JiraIdValidatorTests
    {
        [Test]
        [TestCase("CHR-12345")]
        [TestCase("CPR-12345")]
        [TestCase("OCT-12345")]
        [TestCase("KFPAYROLL-12345")]
        [TestCase("OPAYSLIP-12345")]
        [TestCase("PROJECT-123456789")]
        [TestCase("SE32-123456789")]
        [TestCase("SIP-123456789")]
        [TestCase("SIQ-123456789")]
        [TestCase("SGP-123456789")]
        [TestCase("SPM-123456789")]
        [TestCase("S12P-123456789")]
        public void GivenAValidJiraId_WhenIValidate_ThenIShouldGetAPositiveResult(string jiraId)
        {
            var validator = new JiraIdValidator();

            Assert.That(validator.Validate(jiraId), Is.True);
        }

        [Test]
        [TestCase("TEXTONLY")]
        [TestCase("01234")]
        [TestCase("TEXT-TEXT")]
        [TestCase("ABC=123")]
        [TestCase("TOOLONGPROJECT-123")]
        [TestCase("PROJECT-1234567890")]
        public void GivenAnInValidJiraId_WhenIValidate_ThenIShouldGetANegativeResult(string jiraId)
        {
            var validator = new JiraIdValidator();

            Assert.That(validator.Validate(jiraId), Is.False);
        }
    }
}