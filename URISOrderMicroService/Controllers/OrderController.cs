using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using URISOrderMicroService.DataAccess;
using URISOrderMicroService.Models;
using URISUtil.DataAccess;

namespace URISOrderMicroService.Controllers
{
    public class OrderController : ApiController
    {
        /// <summary>
        /// Gets all users based on filters
        /// </summary>
        /// <param name="userType">User type</param>
        /// <param name="userName">User name</param>
        /// <param name="active">Indicates if the user is active or not</param>
        /// <param name="order">Ordering</param>
        /// <param name="orderDirection">Order direction (asc/desc)</param>
        /// <returns>List of users</returns>
        [Route("api/User"), HttpGet]
        public IEnumerable<Order> GetOrders([FromUri]string userType = null, [FromUri]string userName = null, [FromUri]ActiveStatusEnum active = ActiveStatusEnum.Active,
                                          [FromUri]UserOrderEnum order = UserOrderEnum.Id,
                                          [FromUri]OrderEnum orderDirection = OrderEnum.Asc)
        {
            return OrderDB.GetOrders(userType, userName, active, order, orderDirection);
        }

        /// <summary>
        /// Get single order based on id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Single order</returns>
        [Route("api/Order/{id}"), HttpGet]
        public Order GetOrder(int id)
        {
            return OrderDB.GetOrder(id);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="order">User as json</param>
        /// <returns>Created user</returns>        
        [Route("api/Order"), HttpPost]
        public Order CreateOrder([FromBody]Order order)
        {
            return OrderDB.CreateOrder(order);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">User as json</param>
        /// <returns>Updated user</returns>
        [Route("api/Order"), HttpPut]
        public Order UpdateOrder([FromBody]Order order)
        {
            return OrderDB.UpdateOrder(order);
        }

        /// <summary>
        /// Set Active status of user to false
        /// </summary>
        /// <param name="id">User Id</param>
        [Route("api/Order/{id}"), HttpDelete]
        public void DeleteOrder(int id)
        {
            OrderDB.DeleteOrder(id);
        }
    }
}