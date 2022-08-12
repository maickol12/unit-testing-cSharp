using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanBeCancelledBy_UserIsAdmin_ReturnTrue()
        {
            // Arrange 
            var reservation = new Reservation();
            //Act
            var result = reservation.CanBeCancelledBy(new User() { IsAdmin = true });
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void CanBeCancelledBy_SameUserCancellingTheReservation_ReturnTrue()
        {
            var user = new User();
            // Arrange 
            var reservation = new Reservation { MadeBy = user};
            //Act
            var result = reservation.CanBeCancelledBy( user );
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCancelledBy_AnotherUserCancellingReservation_ReturnFalse()
        {
            
            // Arrange 
            var reservation = new Reservation { MadeBy = new User() };
            //Act
            var result = reservation.CanBeCancelledBy(new User());
            //Assert
            Assert.IsFalse(result);
        }
    }
}