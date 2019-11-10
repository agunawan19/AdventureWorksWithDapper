using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Helper
    {
        public static string GetDatabaseConnection(string name) =>
            ConfigurationManager.ConnectionStrings[name].ConnectionString;
    }
}