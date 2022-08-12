

using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        public void GetOutPut_InputIsDivisibleBy3And5_ReturnFizzBuzz() 
        {
            var result = FizzBuzz.GetOutput(15);

            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }
        [Test]
        public void GetOutPut_InputIsDivisibleBy3Only_ReturnFizz()
        {
            var result = FizzBuzz.GetOutput(3);

            Assert.That(result, Is.EqualTo("Fizz"));
        }
        [Test]
        public void GetOutPut_InputIsDivisibleBy5Only_ReturnBuzz()
        {
            var result = FizzBuzz.GetOutput(10);

            Assert.That(result, Is.EqualTo("Buzz"));
        }
        [Test]
        public void GetOutPut_InputIsNotDivisibleBy30Or5_ReturnSameNumber()
        {
            var result = FizzBuzz.GetOutput(1);

            Assert.That(result, Is.EqualTo("1"));
        }
    

    }
}
