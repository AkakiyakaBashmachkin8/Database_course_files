using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = OpenOrCreateDataBase();
            var readerEmployees = new SqlCommand("select * from Employe", connection).ExecuteReader();
            List<Employee> employeesL = new List<Employee>();
            while(readerEmployees.Read())
            {
                DateTime dateStartWork, dateStopWork;
                DateTime.TryParse(readerEmployees.GetValue(8).ToString(), out dateStartWork);
                DateTime.TryParse(readerEmployees.GetValue(9).ToString(), out dateStopWork);
                Employee t = new Employee(Convert.ToInt32(readerEmployees.GetValue(7)),
                                            (long)readerEmployees.GetValue(11),
                                            (string)readerEmployees.GetValue(1),
                                            (string)readerEmployees.GetValue(2),
                                            (string)readerEmployees.GetValue(3),
                                            (string)readerEmployees.GetValue(4),
                                            (string)readerEmployees.GetValue(5),
                                            readerEmployees.GetValue(10) is System.DBNull?"":(string)readerEmployees.GetValue(10),
                                            dateStartWork,
                                            dateStopWork==DateTime.MinValue?DateTime.MaxValue:dateStopWork);
                employeesL.Add(t);
            }
            readerEmployees.Close();
            CloseConnection(connection);
            Console.WriteLine("\nСписок працівників:");
            PrintCollection(employeesL);


            Employee[] employeesA = employeesL.ToArray();
            Dictionary<int, Employee> employeesD = employeesL.ToDictionary(e => e.registrationNumber);


            var neRobViddil = employeesL.Where(e => e.team == "невиробничий відділ");
            Console.WriteLine("Список працівників невиробничого відділу:");
            PrintCollection(neRobViddil);


            var salaries = employeesL.Select(e => new { e.position, e.salary });
            Console.WriteLine("Список зарплат:");
            foreach (var t in salaries)
            { Console.WriteLine($"{t.position} {t.salary}"); }


            var brigadirs = employeesL.Where(e => e.position.Contains("бригадир"));
            var directors = employeesL.Where(e => e.position.Contains("директор"));
            var bd = brigadirs.Union(directors);
            var lowerEmployees = employeesL.Except(bd);
            var employeAndHisChief = bd.SelectMany(e => lowerEmployees.Where(le => le.team == e.team).Select(le => new { le, e }));
            if (directors.Count() == 1)
            {
                var brigadirAndHisChief = directors.SelectMany(e => brigadirs.Select(le => new { le, e }));
                employeAndHisChief = employeAndHisChief.Union(brigadirAndHisChief);
            }
            Console.WriteLine("\nСписок виду: працівник - безпосередній керівник");
            foreach (var t in employeAndHisChief)
            {
                Console.WriteLine($"{t.le.surname} {t.le.name} {t.le.patronymic} ({t.le.position}) - {t.e.surname} {t.e.name} {t.e.patronymic} ({t.e.position})\n");
            }


            var first5Employees = employeesL.Take(5);
            Console.WriteLine("Перші п`ять працівників:");
            PrintCollection(first5Employees);


            var allBeforFirstNoneKameniar = employeesL.TakeWhile(e => e.position.Contains("каменяр"));
            Console.WriteLine("Працівники до першого, хто не каменяр:");
            PrintCollection(first5Employees);


            Console.ReadKey();
        }

        static void PrintCollection(IEnumerable<Employee> collection)
        {
            foreach (var t in collection)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine();
            }
        }

        static SqlConnection OpenOrCreateDataBase()
        {
            string connStr = @"Data Source=DESKTOP-M6OMTPE;
                            Initial Catalog=buildingCompany;
                            Integrated Security=True";
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
                Console.WriteLine("БД вiдкрита");
            }
            catch (SqlException se)
            {
                if (se.Number == 4060)
                {
                    Console.WriteLine("Зачекайте, йде створення БД");
                    conn.Close();
                    conn = new SqlConnection(@"Data Source=DESKTOP-M6OMTPE;Integrated Security=True");
                    SqlCommand cmdCreateDataBase = new SqlCommand(string.Format("CREATE DATABASE [{0}]", "buildingCompany"), conn);
                    conn.Open();
                    Console.WriteLine("Посилаємо запит");
                    cmdCreateDataBase.ExecuteNonQuery();
                    conn.Close();
                    Thread.Sleep(5000);
                    conn = new SqlConnection(connStr);
                    conn.Open();
                    Console.WriteLine("БД створена");
                }
            }
            return conn;
        }

        static void CloseConnection(SqlConnection connection)
        {
            connection.Close();
            connection.Dispose();
            Console.WriteLine("З’єднання успiшно завершено");
        }

    }
}