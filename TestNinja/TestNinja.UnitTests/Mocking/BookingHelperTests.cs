using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperTests_OverlappingBookingsExistsTests
    {
        private Booking _booking;
        private Mock<IBookingRepository> _repository = new Mock<IBookingRepository>();
        [SetUp]
        public void SetUp()
        {
            _booking = new Booking
            {
                Id = 2,
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "a"
            };
            
            _repository.Setup(el => el.GetActiveBookings(1)).Returns(new List<Booking>
            {
                _booking
            }.AsQueryable());

        }
        [Test]
        public void BookingStartAndFinishesBeforeAndExistingBooking_ReturnEmptyString()
        {
         
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_booking.ArrivalDate,days:3),
                DepartureDate = Before(_booking.ArrivalDate),
                Reference = "a"
            }, _repository.Object);;
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnsExistingBookingReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_booking.ArrivalDate),
                DepartureDate = After(_booking.ArrivalDate),
                Reference = "a"
            }, _repository.Object); ;
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        [Test]
        public void BookingStartsBeforeAndAfterAnExistingBooking_ReturnsExistingBookingReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_booking.ArrivalDate),
                DepartureDate = After(_booking.DepartureDate),
                Reference = "a"
            }, _repository.Object); ;
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        [Test]
        public void BookingStartsAndFinishesAnExistingBooking_ReturnsExistingBookingReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_booking.ArrivalDate),
                DepartureDate = Before(_booking.DepartureDate),
                Reference = "a"
            }, _repository.Object); ;
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        [Test]
        public void BookingStartsInTheMiddleAnExistingBookingButFinishesAfter_ReturnsExistingBookingReference()
        {

            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = After(_booking.ArrivalDate),
                DepartureDate = After(_booking.DepartureDate),
                Reference = "a"
            }, _repository.Object); ;
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }
        private DateTime After(DateTime dateTime)
        {
            return dateTime.AddDays(1);
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day,14,0,0);
        }
        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
