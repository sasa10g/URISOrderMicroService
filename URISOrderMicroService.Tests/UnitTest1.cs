using System;
using URISOrderMicroService.DataAccess;
using NUnit.Framework;
using URISOrderMicroService.Models;

namespace URISOrderMicroService.Tests
{
    //[TestClass]
    public class OrderTest
    {

        //kreiranje korisnika USPJESNO
        //[Test]
        //public void Create_Order_Success()
        //{
        //    Order testOrder = new Order
        //    {
        //        Id = 1,
        //        Note = "test"
        //    };
        //    OrderDB.CreateOrder(testOrder);
        //    Assert.AreEqual(1, OrderDB.listOfOrders.Count);
        //}

        //citanje korisnika USPJESNO
        [Test]
        public void GetOrderById_Success()
        {
            //prva ocekivanja, druga iz baze
            Assert.AreEqual(1, OrderDB.GetOrder(1).Id);
        }

        //citanje korisnika NEUSPJESNO
        //[Test]
        //public void GetOrderById_Fail()
        //{
        //    //prva ocekivanja, druga iz baze
        //    Assert.AreEqual(2, OrderDB.GetOrder(1).Id);
        //}

        //citanje svih korisnika USPJESNO
        [Test]
        public void GetOrders_Success()
        {
            //prva ocekivanja, count prebrojave vrijednosti iz baze
            Assert.AreEqual(2, OrderDB.GetOrders().Count);
        }


    }
}
