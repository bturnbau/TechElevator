using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        private TransactionScope _tran = null;
        private IReservationDAO resDAO = null;
        DateTime dateStart = new DateTime(2019, 06, 17);
        DateTime dateEnd = new DateTime(2019, 06, 21);

        [TestInitialize]
        public void Initialize()
        {
            resDAO = new ReservationDAO("Data Source=localhost\\sqlexpress;Initial Catalog=npcampground;Integrated Security=True");
            _tran = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }


            [TestMethod]
            public void InsertNewBookingTest()
            {
            Reservation reservation = new Reservation()
            {
                from_date = dateStart,
                to_date = dateEnd,
                reservation_name = "Test",
                site_id = 1
            };

            int id = resDAO.InsertNewBooking(reservation);
            Assert.AreNotEqual(0, id);

            Reservation getReservation = resDAO.FindReservation(id);
            Assert.AreEqual(reservation.from_date, getReservation.from_date);
            Assert.AreEqual(reservation.to_date, getReservation.to_date);
            Assert.AreEqual(reservation.reservation_name, getReservation.reservation_name);
            Assert.AreEqual(reservation.site_id, getReservation.site_id);
        }
    }
}
