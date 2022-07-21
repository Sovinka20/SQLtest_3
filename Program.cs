//using MySqlConnector;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqltest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=model;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                await connection.OpenAsync();
                Console.WriteLine("Подключение открыто");
                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
                Console.WriteLine($"\tБаза данных: {connection.Database}");
                Console.WriteLine($"\tСервер: {connection.DataSource}");
                Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
                Console.WriteLine($"\tСостояние: {connection.State}");
                Console.WriteLine($"\tWorkstationld: {connection.WorkstationId}");

                // 1. Сотрудника с максимальной заработной платой.
                string sqlExpression = 
                    "SELECT NAME, " +
                    "SALARY FROM EMPLOYEE " +
                    "WHERE SALARY = " +
                    "(SELECT MAX(SALARY) AS MAX_SALARY " +
                    "FROM EMPLOYEE)";

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        string columnName0 = reader.GetName(0);
                        string columnName1 = reader.GetName(1);

                        Console.WriteLine($"{columnName0}\t{columnName1}");

                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            object name = reader.GetValue(0);
                            object salary = reader.GetValue(1);

                            Console.WriteLine($"{name}\t{salary}");
                        }
                    }
                }

                // 2. Отдел с самой высокой заработной платой между сотрудниками. 
                string sqlExpression2 = 
                    "SELECT NAME" +
                    "FROM DEPARTAMENT" +
                    "WHERE ID = (" +
                    "SELECT DEPARTAMENT_ID" +
                    "FROM EMPLOYEE" +
                    "WHERE SALARY = " +
                    "(SELECT MAX(SALARY) AS MAX_SALARY" +
                    "FROM EMPLOYEE))";

                SqlCommand command2 = new SqlCommand(sqlExpression2, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        string columnName0 = reader.GetName(0);

                        Console.WriteLine($"{columnName0}");

                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            object name = reader.GetValue(0);

                            Console.WriteLine($"{name}");
                        }
                    }
                }

                // 3.	Отдел с максимальной суммарной зарплатой сотрудников. 
                string sqlExpression3 = 
                    "SELECT SUM(E.SALARY), E.DEPARTAMENT_ID" +
                    "FROM EMPLOYEE AS E," +
                    "(SELECT MAX(MAX_SALARY) AS M" +
                    "FROM (SELECT SUM(SALARY) AS MAX_SALARY" +
                    "FROM EMPLOYEE" +
                    "GROUP BY DEPARTAMENT_ID) AS S) AS D" +
                    "WHERE SUM(E.SALARY) = D.M " +
                    "GROUP BY E.DEPARTAMENT_ID";

                SqlCommand command3 = new SqlCommand(sqlExpression3, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        string columnName0 = reader.GetName(0);

                        Console.WriteLine($"{columnName0}");

                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            object name = reader.GetValue(0);

                            Console.WriteLine($"{name}");
                        }
                    }
                }


                // 4.	Сотрудника, чье имя начинается на «Р» и заканчивается на «н».
                string sqlExpression4 = 
                    "SELECT Name " +
                    "FROM Employee " +
                    "WHERE Name LIKE 'Р%' " +
                    "AND Name LIKE '%н'";

                SqlCommand command4 = new SqlCommand(sqlExpression4, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        string columnName0 = reader.GetName(0);

                        Console.WriteLine($"{columnName0}");

                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            object name = reader.GetValue(0);

                            Console.WriteLine($"{name}");
                        }
                    }
                }









            }


            Console.WriteLine("Подключение закрыто...");
            Console.WriteLine("Программа завершила работу.");
            Console.Read();

        }
    }
}