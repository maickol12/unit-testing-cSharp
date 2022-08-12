using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    public class HtmlFormatterTests
    {
        private HtmlFormatter formatter;
        [SetUp]
        public void Setup()
        {
            formatter = new HtmlFormatter();    
        }

        [Test]
        public void FormatAsBold_WhenCalled_ShouldEncloseTheStringWithStrongElement()
        {
            var result = formatter.FormatAsBold("abc");

            // Specific 
            Assert.That(result, Is.EqualTo("<strong>abc</strong>").IgnoreCase);

            // More general
            Assert.That(result, Does.StartWith("<strong>").IgnoreCase);
            Assert.That(result, Does.EndWith("</strong>").IgnoreCase);
            //Assert.That(result, Does.Contain("abs").IgnoreCase);
        }
    }
}
