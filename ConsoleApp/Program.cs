using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickType;

namespace ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //await Task.Run(() => DataTableMethodInvestigation());
            //Console.WriteLine("DataTableMethodInvestigation Finished");

            //Func<Func<int, int>, Func<int, int>> twice = f => x => f(f(x));
            Func<int, int> twice(Func<int, int> f) => x => f(f(f(x)));
            //Func<int, int> twice(Func<int, int> f)
            //{
            //    return x => f(f(x));
            //}

            //int plusThree(int x) => x + 3;
            var result = twice(x => x + 3)(7);

            //Func<int, Func<int, int>> curriedAdd3(int a) => b => c => (a + b + c) * 2;
            //int curriedAdd3Result = curriedAdd3(1)(2)(3);
            //Console.WriteLine(curriedAdd3Result);

            //var myCol = GetNewDataColumns().ToList();
            //Console.WriteLine(myCol[0].ColumnName);

            //foreach (var test in GetNewDataColumns())
            //{
            //    Console.WriteLine(test.ColumnName);
            //}

            string machineName = Environment.MachineName;
            Console.WriteLine(machineName);
            Console.WriteLine(new string('-', machineName.Length));

            await GetCustomerDataParallelAsync();

            DataAccess dataAccess = new DataAccess();
            //var customers = dataAccess.GetCustomers();
            var customers = await dataAccess.GetCustomersAsync();
            var count = 0;
            foreach (var customer in customers)
            {
                Console.WriteLine($"{(++count).ToString().PadLeft(4, ' ')} {customer.CustomerId} {customer.FirstName} {customer.LastName}");
            }

            //customers = dataAccess.GetCustomersByLastName("Clark");
            customers = await dataAccess.GetCustomersByLastNameAsync("Clark");
            count = 0;

            Console.WriteLine();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{(++count).ToString().PadLeft(4, ' ')} {customer.CustomerId} {customer.FirstName} {customer.LastName}");
            }

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string jsonText = File.ReadAllText(@"..\..\passage.json");

            string json = JsonConvert.SerializeObject(
                customers,
                Formatting.Indented,
                settings);

            //JObject jToken = JObject.Parse(jsonText);
            var bible = Bible.FromJson(jsonText);

            // var definition = new { Name = "" };

            //var result = JsonConvert.DeserializeObject(jsonText);

            Console.WriteLine(json);

            //_ = Task.Run(() => DataTableMethodInvestigation());
            //Console.WriteLine("DataTableMethodInvestigation Finished");

            Console.ReadKey();
        }

        private static async Task GetCustomerDataParallelAsync()
        {
            var dataAccess = new DataAccess();
            var tasks = new List<Task<List<Customer>>>
            {
                Task.Run(() => dataAccess.GetCustomersAsync()),
                Task.Run(() => dataAccess.GetCustomersByLastNameAsync("Clark"))
            };

            var results = await Task.WhenAll(tasks);

            foreach (var customers in results)
            {
                int count = 0;
                foreach (var customer in customers)
                {
                    string text = $"{(++count).ToString().PadLeft(4, ' ')} {customer.CustomerId} {customer.FirstName} {customer.LastName}";
                    Console.WriteLine(text);
                }

                Console.WriteLine();
            }
        }

        private static void DataTableMethodInvestigation()
        {
            using (DataTable table = CreateDataTable())
            {
                PopulateDataTable(table, 1_500_000);

                var enumer = table.AsEnumerable();
                var enumer1 = enumer
                    .FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                var enumer2 = enumer
                    .FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                var enumer3 = enumer
                    .FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                var enumer4 = enumer
                    .FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));

                //var var1 = table.AsEnumerable().ToList()
                //    .Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                //var var2 = table.AsEnumerable().ToList()
                //    .Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                //var var3 = table.AsEnumerable().ToList()
                //    .Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                //var var4 = table.AsEnumerable().ToList()
                //    .Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));

                //var rows = table.AsEnumerable().ToList();
                //var varA = rows.Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                //var varB = rows.Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
                //var varC = rows.Find(r => string.IsNullOrWhiteSpace(r.Field<string>("ColumnA")));
            }
        }

        private static DataTable CreateDataTable()
        {
            DataTable table = new DataTable();

            foreach (var (columnName, columnType, defaultValue) in GetTableColumns())
            {
                table.Columns.Add(new DataColumn(columnName, columnType)
                {
                    DefaultValue = defaultValue
                });
            }

            return table;
        }

        private static IEnumerable<(string ColumnName,
            Type ColumnType, dynamic DefaultValue)> GetTableColumns()
        {
            const int charNum = 65;
            const int maxColumn = 10;
            int halfOfMaxColumn = maxColumn / 2;

            for (var i = 0; i < maxColumn; i++)
            {
                var columnName = $"Column{Convert.ToChar(charNum + i)}";

                if (i <= halfOfMaxColumn)
                {
                    yield return (columnName, typeof(string), string.Empty);
                }
                else
                {
                    yield return (columnName, typeof(int), 0);
                }
            }
        }

        private static void PopulateDataTable(DataTable table, int numberOfRows)
        {
            var columns = table.Columns;
            DataRow row;
            HashSet<string> columnHash = new HashSet<string>();
            const int charNum = 65;
            const int maxStringColumnCount = 5;

            for (var i = 0; i < maxStringColumnCount; i++)
            {
                columnHash.Add($"Column{Convert.ToChar(charNum + 1)}");
            }

            for (var i = 0; i < numberOfRows; i++)
            {
                row = table.NewRow();

                foreach (DataColumn column in columns)
                {
                    if (columnHash.Contains(column.ColumnName))
                    {
                        row.SetField(column.ColumnName, RandomString(50));
                    }
                    else
                    {
                        row.SetField(column.ColumnName, RandomNumber(100_000, 999_999));
                    }
                }

                table.Rows.Add(row);
            }

            row = table.NewRow();
            foreach (DataColumn column in columns)
            {
                if (columnHash.Contains(column.ColumnName))
                {
                    row.SetField(column.ColumnName, RandomString(50));
                }
                else
                {
                    row.SetField(column.ColumnName, RandomNumber(100_000, 999_999));
                }
            }
            table.Rows.Add(row);
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string RandomString(int size, bool lowerCase = false)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(
                    Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }

            return builder.ToString();
        }

        private static void AddAdditionalColumnsToTimeTable(DataTable toTimeData)
        {
            foreach (var (lcColumnName, loColumnType, loDefaultValue) in GetNewDataColumns())
            {
                DataColumn loNewColumn = new DataColumn(lcColumnName, loColumnType)
                {
                    DefaultValue = loDefaultValue
                };

                toTimeData.Columns.Add(loNewColumn);
            }
        }

        private static IEnumerable<(string ColumnName, Type ColumnType, dynamic DefaultValue)> GetNewDataColumns()
        {
            yield return ("csupid", typeof(string), string.Empty);
            yield return ("csupname", typeof(string), string.Empty);
            yield return ("nApplyPre", typeof(decimal), 0m);
            yield return ("nShifttype", typeof(decimal), 0m);
            yield return ("cLastName", typeof(string), string.Empty);
            yield return ("cFirstName", typeof(string), string.Empty);
            yield return ("nDaysCycle", typeof(decimal), 0m);
            yield return ("lCertRate", typeof(bool), false);
            yield return ("cMonth", typeof(string), string.Empty);
            yield return ("nCertRate", typeof(decimal), 0m);
            yield return ("nTipCode", typeof(decimal), 0m);
            yield return ("nApplyWage", typeof(decimal), 0m);
            yield return ("nWageRate", typeof(decimal), 0m);
            yield return ("nMaxWkReg", typeof(decimal), 0m);
            yield return ("dws_date", typeof(DateTime), DateTime.MinValue);
            yield return ("llastpunch", typeof(bool), false);
            yield return ("cGrpChg", typeof(string), string.Empty);
            yield return ("cGrpDesc", typeof(string), string.Empty);
            yield return ("cShift_Info", typeof(string), string.Empty);
            yield return ("nEarnAmt", typeof(decimal), 0m);
            yield return ("ndedamt", typeof(decimal), 0m);
            yield return ("nTipAmt", typeof(decimal), 0m);
            yield return ("nHoliday", typeof(decimal), 0m);
            yield return ("nvacation", typeof(decimal), 0m);
            yield return ("nsick", typeof(decimal), 0m);
            yield return ("nothers", typeof(decimal), 0m);
            yield return ("ngrace", typeof(decimal), 0m);
            yield return ("ntardy", typeof(decimal), 0m);
            yield return ("nlongmeal", typeof(decimal), 0m);
            yield return ("nearlydep", typeof(decimal), 0m);
            yield return ("nGraceCnt", typeof(decimal), 0m);
            yield return ("nTardyCnt", typeof(decimal), 0m);
            yield return ("nLongCnt", typeof(decimal), 0m);
            yield return ("nEarlyCnt", typeof(decimal), 0m);
            yield return ("npolicy", typeof(decimal), 0m);
            yield return ("nEmpCnt", typeof(decimal), 0m);
            yield return ("dStart", typeof(DateTime), DateTime.MinValue);
            yield return ("dEnd", typeof(DateTime), DateTime.MinValue);

            const int lnMin = 1;
            int lnMax = 20;

            for (var lni = lnMin; lni <= lnMax; lni++)
            {
                yield return ($"Cgrp{lni}", typeof(string), (dynamic)string.Empty);
            }

            lnMax = 5;
            for (var lni = lnMin; lni <= lnMax; lni++)
            {
                yield return ($"not{lni}", typeof(decimal), (dynamic)0m);
            }

            for (var lni = lnMin; lni <= lnMax; lni++)
            {
                yield return ($"nuot{lni}", typeof(decimal), (dynamic)0m);
            }

            Test test = new Test
            {
                myField = 0,
                MyProperty = 0
            };
            test.MyMethod();
        }

        private class Test
        {
            public const string myString = "";
            public int myField;
            public int MyProperty { get; set; }

            public void MyMethod()
            {
                //Func<Func<int, int>, Func<int, int>> twice = f => x => f(f(x));
                Func<int, int> twice(Func<int, int> f) => x => f(f(x));
                //Func<int, int> plusThree = x => x + 3;
                int plusThree(int x) => x + 3;
                Console.WriteLine(twice(plusThree)(7)); // 13
            }

            //private Func<int, int> twice(Func<int, int> f)
            //{
            //    return x => f(f(x));
            //}

            //Func<int, int> plusThree = x => x + 3;
        }
    }
}