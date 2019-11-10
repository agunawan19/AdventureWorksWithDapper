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
        protected string _connectionName = string.Empty;

        private enum MachineName
        {
            PAVILION,
            RD1014
        }

        public DataAccess()
        {
            if (Environment.MachineName.ToUpper().Contains(MachineName.PAVILION.ToString()))
            {
                _connectionName = ConnectionString.PavilionConnection;
            }
            else if (Environment.MachineName.ToUpper().Contains(MachineName.RD1014.ToString()))
            {
                _connectionName = ConnectionString.OtherConnection;
            }
            else
            {
                _connectionName = ConnectionString.DefaultConnection;
            }
        }

        public DataAccess(string name) => _connectionName = name;

        private IDbConnection GetConnection() =>
            new System.Data.SqlClient.SqlConnection(Helper.GetDatabaseConnection(_connectionName));

        public List<Customer> GetCustomers()
        {
            using (IDbConnection connection = GetConnection())
            {
                string query = CustomerQuery;

                return connection.Query<Customer>(query).ToList();
            }
        }

        public async Task<List<Customer>> GetCustomersAsync() =>
            await Task.Run(() => GetCustomers());

        private string CustomerQuery
        {
            get
            {
                string query =
                    "SELECT TOP 20 CustomerId, Title, FirstName, MiddleName, LastName " +
                    "FROM SalesLT.Customer";

                return query;
            }
        }

        public List<Customer> GetCustomersByLastName(string lastName)
        {
            using (IDbConnection connection = GetConnection())
            {
                string query = GenerateCustomersByLastNameQuery;

                return connection.Query<Customer>(query, new { LastName = lastName }).ToList();
            }
        }

        public async Task<List<Customer>> GetCustomersByLastNameAsync(string lastName) =>
            await Task.Run(() => GetCustomersByLastName(lastName));

        private string GenerateCustomersByLastNameQuery
        {
            get
            {
                string query =
                    "SELECT CustomerId, Title, FirstName, MiddleName, LastName " +
                    "FROM SalesLT.Customer " +
                    "WHERE LastName LIKE '%' + @LastName + '%'";

                return query;
            }
        }
    }
}