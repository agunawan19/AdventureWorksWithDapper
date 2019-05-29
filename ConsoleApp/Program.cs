using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccess dataAccess = new DataAccess();
            var customers = dataAccess.GetCustomers();
            var count = 0;

            foreach (var customer in customers)
            {
                Console.WriteLine($"{(++count).ToString().PadLeft(4, ' ')} {customer.CustomerId} {customer.FirstName} {customer.LastName}");
            }

            customers = dataAccess.GetCustomersByLastName("Clark");
            count = 0;

            Console.WriteLine();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{(++count).ToString().PadLeft(4, ' ')} {customer.CustomerId} {customer.FirstName} {customer.LastName}");
            }

            Console.ReadKey();
        }
    }
}
