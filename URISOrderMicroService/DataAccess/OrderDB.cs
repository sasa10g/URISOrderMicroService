using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using URISOrderMicroService.Models;
using URISUtil.DataAccess;
using URISUtil.Logging;
using URISUtil.Response;
using System.Diagnostics;

namespace URISOrderMicroService.DataAccess
{
    public class OrderDB
    {
        private static Order ReadRow(SqlDataReader reader)
        {
            Order retVal = new Order();

            retVal.Id = (int)reader["Id"];
            retVal.Date = (DateTime)reader["Date"];
            retVal.DeliveryAddress = reader["DeliveryAddress"] as string;
            retVal.DeliveryCity = reader["DeliveryCity"] as string;
            retVal.DeliveryZipCode = reader["DeliveryZipCode"] as string;
            retVal.DeliveryCountry = reader["DeliveryCountry"] as string;
            retVal.Note = reader["Note"] as string;
            retVal.UserId = (int)reader["UserId"];
            retVal.Price = (decimal)reader["Price"];
            retVal.Quantity = (int)reader["Quantity"];

            return retVal;

        }

        private static int ReadId(SqlDataReader reader)
        {
            return (int)reader["Id"];
        }

        private static string AllColumnSelect
        {
            get
            {
                return @"
                    [Order].[Id],
                    [Order].[Date],
                    [Order].[DeliveryAddress],
                    [Order].[DeliveryCity],
                    [Order].[DeliveryZipCode],
                    [Order].[DeliveryCountry],
                    [Order].[Note],
                    [Order].[UserId],
                    [Order].[Price],
                    [Order].[Quantity],
                    [Order].[Active]
                ";
            }
        }

        private static void FillData(SqlCommand command, Order order)
        {
            command.AddParameter("@Id", SqlDbType.Int, order.Id);
            command.AddParameter("@Date", SqlDbType.DateTime, order.Date);
            command.AddParameter("@DeliveryAddress", SqlDbType.NVarChar, order.DeliveryAddress);
            command.AddParameter("@DeliveryCity", SqlDbType.NVarChar, order.DeliveryCity);
            command.AddParameter("@DeliveryZipCode", SqlDbType.NVarChar, order.DeliveryZipCode);
            command.AddParameter("@DeliveryCountry", SqlDbType.NVarChar, order.DeliveryAddress);
            command.AddParameter("@Note", SqlDbType.NVarChar, order.Note);
            command.AddParameter("@UserId", SqlDbType.Int, order.UserId);
            command.AddParameter("@Price", SqlDbType.Decimal, order.Price);
            command.AddParameter("@Quantity", SqlDbType.Int, order.Quantity);
            command.AddParameter("@Active", SqlDbType.Bit, order.Active);
        }

        private static object CreateLikeQueryString(string str)
        {
            return str == null ? (object)DBNull.Value : "%" + str + "%";
        }



        /*


        public static List<User> GetUsers(string userType, string userName, ActiveStatusEnum active, UserOrderEnum order, OrderEnum orderDirection)
        {
            try
            {
                List<User> retVal = new List<User>();

                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT
                            {0}
                        FROM
                            [user].[User]
                            JOIN [user].[UserType] ON [User].[UserTypeId] = [UserType].[Id]
                        WHERE
                            (@UserType IS NULL OR [user].[UserType].Name LIKE @UserType) AND
                            (@UserName IS NULL OR [user].[User].Name LIKE @UserName) AND
                            (@Active IS NULL OR [user].[User].Active = @Active)
                    ", AllColumnSelect);
                    command.Parameters.Add("@UserType", SqlDbType.NVarChar);
                    command.Parameters.Add("@UserName", SqlDbType.NVarChar);
                    command.Parameters.Add("@Active", SqlDbType.Bit);

                    command.Parameters["@UserType"].Value = CreateLikeQueryString(userType);
                    command.Parameters["@UserName"].Value = CreateLikeQueryString(userName);
                    switch (active)
                    {
                        case ActiveStatusEnum.Active:
                            command.Parameters["@Active"].Value = true;
                            break;
                        case ActiveStatusEnum.Inactive:
                            command.Parameters["@Active"].Value = false;
                            break;
                        case ActiveStatusEnum.All:
                            command.Parameters["@Active"].Value = DBNull.Value;
                            break;
                    }

                    System.Diagnostics.Debug.WriteLine(command.CommandText);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retVal.Add(ReadRow(reader));
                        }
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }


            */


        public static Order GetOrder(int Id)
        {
            try
            {
                Order retVal = new Order();

                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT {0}
                        FROM [order].[Order]
                        WHERE [Id] = @Id
                        ", AllColumnSelect
                        );

                    command.AddParameter("@Id", SqlDbType.Int, Id);
                    Debug.WriteLine(command.CommandText);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retVal = ReadRow(reader);
                        }

                        else
                        {
                            ErrorResponse.ErrorMessage(HttpStatusCode.NotFound);
                        }
                    }

                }

                return retVal;
            }

            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }





        public static Order CreateOrder(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO [order].[Order]
                    (
                        [Date],
                        [DeliveryAddress],
                        [DeliveryCity],
                        [DeliveryZipCode],
                        [DeliveryCountry],
                        [Note],
                        [UserId],
                        [Price],
                        [Quantity],
                        [Active]
                    )
                        VALUES
                        (
                         @Date,
                        @DeliveryAddress,
                        @DeliveryCity,
                        @DeliveryZipCode,
                        @DeliveryCountry,
                        @Note,
                        @UserId,
                        @Price,
                        @Quantity,
                        @Active
                       )
                        SET @Id = SCOPE_IDENTITY();
                            SELECT @Id as Id
                   ";
                    FillData(command, order);
                    connection.Open();

                    int id = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = ReadId(reader);
                        }
                    }
                    return GetOrder(id);

                }
            }

            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static Order UpdateOrder(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                    UPDATE
                        [order].[Order]
                    SET
                    [Date]=@Date,
                    [DeliveryAddress]=@DeliveryAddress,
                    [DeliveryCity]=@DeliveryCity,
                    [DeliveryZipCode]=@DeliveryZipCode,
                    [DeliveryCountry]=@DeliveryCountry,
                    [Note]=@Note,
                    [UserId]=@UserId,
                    [Price]=@Price,
                    [Quantity]=@Quantity,
                    [Active]=@Active
                    
                    WHERE [Id]=@Id"
                        );
                    FillData(command, order);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return GetOrder(order.Id);
                }
            }

            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }

        }

        public static void DeleteOrder(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        UPDATE
                            [order].[Order]
                        SET
                            [Active] = 'False'
                        WHERE
                            [Id] = @Id     
                    ");

                    command.AddParameter("@Id", SqlDbType.Int, orderId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}