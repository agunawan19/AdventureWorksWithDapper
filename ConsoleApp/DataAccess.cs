using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using AdventureWorks;

namespace ConsoleApp
{
    public class DataAccess
    {
        private IDbConnection GetConnection(string name = "DefaultConnection")
        {
            return new System.Data.SqlClient.SqlConnection(
                Helper.GetDatabaseConnection(name));
        }

        public List<Customer> GetCustomers()
        {
            using (IDbConnection connection = GetConnection())
            {
                string query = 
                    "SELECT TOP 20 CustomerId, Title, FirstName, MiddleName, LastName " + 
                    "FROM SalesLT.Customer";

                return connection.Query<Customer>(query).ToList();
            }
        }

        public List<Customer> GetCustomersByLastName(string lastName)
        {
            using (IDbConnection connection = GetConnection())
            {
                string query =
                    "SELECT CustomerId, Title, FirstName, MiddleName, LastName " +
                    "FROM SalesLT.Customer " +
                    "WHERE LastName LIKE '%' + @LastName + '%'";

                return connection.Query<Customer>(query, new { LastName = lastName }).ToList();
            }
        }
    }
}
