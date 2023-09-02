using System.Data.SqlClient;
using System.Data.SqlTypes;
using static System.Console;
namespace Vegetables_and_fruits
{
    internal class Program
    {
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        public Program()
        {
            conn = new SqlConnection("Data Source=DESKTOP-0BAGMG4\\SQLEXPRESS;Initial Catalog=Vegetables_and_fruits;Integrated Security=True;");
        }

        public void ConnectQuery()
        {
            bool exit = true;
            try
            {
                conn.Open();
                WriteLine("Подключение открыто\n");

            }
            catch (SqlException ex)
            {
                WriteLine(ex.Message);
            }
            finally
            {
                WriteLine("Свойства подключения");
                WriteLine("\tСтрока подключения: {0}", conn.ConnectionString);
                WriteLine("\tБаза данных: {0}", conn.Database);
                WriteLine("\tСервер: {0}", conn.DataSource);
                WriteLine("\tВерсия сервера: {0}", conn.ServerVersion);
                WriteLine("\tСостояние: {0}", conn.State);
                WriteLine("\tWorkstationId: {0}", conn.WorkstationId);
                WriteLine("Press ENTER to continue");
                ReadLine();
            }
            do
            {
                Clear();
                WriteLine("1. Проверить статус соединения");
                WriteLine("2. Закрыть соединение");
                Write("Select an item and press Enter -> ");
                string? ch = ReadLine();
                switch (ch)
                {
                    case "1":
                        WriteLine("Статус: " + conn.State);
                        WriteLine("Press ENTER to continue");
                        ReadLine();
                        break;
                    case "2":
                        conn.Close();
                        WriteLine(conn.State);
                        exit = false;
                        break;
                } 
            } while (exit);

        }

        public void DisconnectQuery()
        {
            if (conn != null)
            {
                conn.Close();
                WriteLine("Подключение закрыто...");
                WriteLine("Состояние: {0}", conn.State);
            }
        }

        public void InsertQuery()
        {
            WriteLine("Введите название овоща или фрукта");
            string title = ReadLine();
            WriteLine("Введите тип (Овощ или Фрукт)");
            string type = ReadLine();
            WriteLine("Введите цвет");
            string color = ReadLine();
            WriteLine("Введите Ккал");
            float calorific = float.Parse(ReadLine());
            try
            {
                conn.Open();
                string insertString = "insert into Tbl1(Title, Type, Color, Сalorific) values(@p1, @p2, @p3, @p4)";
                SqlCommand cmd = new SqlCommand(insertString, conn);

                cmd.Parameters.Add("@p1", System.Data.SqlDbType.NVarChar).Value = title;
                cmd.Parameters.Add("@p2", System.Data.SqlDbType.NVarChar).Value = type;
                cmd.Parameters.Add("@p3", System.Data.SqlDbType.NVarChar).Value = color;
                cmd.Parameters.Add("@p4", System.Data.SqlDbType.Float).Value = calorific;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { WriteLine(ex.Message); }
            finally { conn.Close(); }
        }

        public void ReadAllDate()
        {
            try
            { 
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;

                while (rdr.Read()) 
                { 
                    if(line == 0)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++) 
                        {
                            Write(rdr.GetName(i).ToString()+"\t");
                        }
                    }
                    WriteLine();
                    line++;
                    WriteLine(rdr[0] +"\t"+ rdr[1] +"\t"+ rdr[2] +"\t"+ rdr[3] + "\t"+ rdr[4]+" Ккал");
                }
            }
            finally 
            { 
                rdr.Close();
                conn.Close(); 
            }
        }

        public void ReadTitleDate()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;

                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 1; i < 2; i++)
                        {
                            Write(rdr.GetName(i).ToString());
                        }
                    }
                    WriteLine();
                    line++;
                    WriteLine(rdr[1]);
                }
            }
            finally 
            {
                rdr.Close() ;
                conn.Close(); 
            }
        }
        public void ReadColorDate()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select DISTINCT Color from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;

                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        WriteLine("Color");
                    }

                    WriteLine();
                    line++;
                    WriteLine(rdr[0]);
                }
            }
            finally 
            {
                rdr.Close() ;
                conn.Close(); 
            }
        }

        public void ReadMaxCalorofocDate()
        {
            string sqlExpression = "SELECT MAX(Сalorific) FROM Tbl1";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlExpression, conn);
                object maxCalorofoc = cmd.ExecuteScalar();

                WriteLine("Максимальная калорийность: {0} Ккал" +
                    "", maxCalorofoc);
            }
            finally { conn.Close(); }
        }

        public void ReadMinCalorofocDate()
        {
            string sqlExpression = "SELECT MIN(Сalorific) FROM Tbl1";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlExpression, conn);
                object minCalorofoc = cmd.ExecuteScalar();

                WriteLine("Минимальная калорийность: {0} Ккал", minCalorofoc);
            }
            finally { conn.Close(); }
        }

        public void ReadAvgCalorofocDate()
        {
            string sqlExpression = "SELECT AVG(Сalorific) FROM Tbl1";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlExpression, conn);
                object avgCalorofoc = cmd.ExecuteScalar();

                WriteLine("Средняя калорийность: {0} Ккал", avgCalorofoc);
            }
            finally { conn.Close(); }
        }

        public void NumberVegetables()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                while (rdr.Read())
                {


                   if((string)rdr[2] == "Овощ")
                    { line++; }

                }
                WriteLine("Количество овощей в БД: {0} шт.", line);               
            }
            finally 
            { 
                conn.Close(); 
                rdr.Close();
            }
            
        }
         public void NumberFruits()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                while (rdr.Read())
                {


                   if((string)rdr[2] == "Фрукт")
                    { line++; }

                }
                WriteLine("Количество фруктов в БД: {0} шт.", line);               
            }
            finally 
            { 
                conn.Close(); 
                rdr.Close();
            }
            
        }
        public void NumberVegetablesColor()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                Write("Ввседите цвет овощей: ");
                string color = ReadLine();
                while (rdr.Read())
                {


                    if ((string)rdr[2] == "Овощ" && (string)rdr[3]=="Зелёный")
                    { line++; }

                }
                WriteLine("Количество овощей цвета \"{1}\" в БД: {0} шт.", line, color);
            }
            finally
            {
                conn.Close();
                rdr.Close();
            }

        }

        public void NumberFruitsColor()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                Write("Ввседите цвет фруктов: ");
                string color = ReadLine();
                while (rdr.Read())
                {


                    if ((string)rdr[2] == "Фрукт" && (string)rdr[3]=="Зелёный")
                    { line++; }

                }
                WriteLine("Количество фруктов цвета \"{1}\" в БД: {0} шт.", line, color);
            }
            finally
            {
                conn.Close();
                rdr.Close();
            }

        }
        
        public void CalorificLower()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                Write("Введите Калорийность ");
                double calorific = double.Parse(ReadLine());
                int line = 0;
                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            Write(rdr.GetName(i).ToString() + "\t\t");
                        }
                        WriteLine();
                    }
                    line++;
                    if ((double)rdr[4] < calorific)
                    {
                        WriteLine(rdr[0] + "\t\t" + rdr[1] + "\t\t" + rdr[2] + "\t\t" + rdr[3] + "\t\t" + rdr[4] + " Ккал");
                    }
                    
                }
            }
            finally 
            { 
                conn.Close();
                rdr.Close();
            }
        }

        public void CalorificHigher()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                Write("Введите калорийность: ");
                double calorific = double.Parse(ReadLine());
                int line = 0;
                while (rdr.Read())
                {
                    if (line == 0)

                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            Write(rdr.GetName(i).ToString() + "\t\t");
                        }
                        WriteLine();
                    }
                    
                    line++;
                    if ((double)rdr[4] > calorific)
                    {
                       
                       
                        WriteLine(rdr[0] + "\t\t" + rdr[1] + "\t\t" + rdr[2] + "\t\t" + rdr[3] + "\t\t" + rdr[4] + " Ккал");
                    }
                }
            }
            finally 
            { 
                conn.Close(); 
                rdr.Close() ;
            }
        }

        public void CountColor()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select DISTINCT Color, COUNT(*) AS countColor from Tbl1 where Color IN (select DISTINCT Color from Tbl1)GROUP BY Color", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    WriteLine(rdr[0] + "\t" + rdr[1]);
                }
                
            }
            finally
            {
                conn.Close();
            }
        }

        public void CalorificRange()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                WriteLine("Введите начальное значение диапазона");
                double start = double.Parse(ReadLine());
                WriteLine("Введите конечное значение диапазона");
                double end = double.Parse(ReadLine());  
                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            Write(rdr.GetName(i).ToString() + "\t");
                        }
                        WriteLine();
                    }
                    line++;
                    if ((double)rdr[4] >= start && (double)rdr[4] <= end)
                    {
                        WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2] + "\t" + rdr[3] + "\t" + rdr[4] + " Ккал");
                    }
                }
            }
            finally
            {
                rdr.Close();
                conn.Close();
            }
        }

        public void YellowAndRedColor()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Tbl1", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            Write(rdr.GetName(i).ToString() + "\t");
                        }
                        WriteLine();
                    }
                    line++;
                    if ((string)rdr[3] == "Жёлтый" || (string)rdr[3] == "Красный")
                    {
                        WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2] + "\t" + rdr[3] + "\t" + rdr[4] + " Ккал");
                    }
                }
            }
            finally
            {
                rdr.Close();
                conn.Close();
            }
        }

        public void ConnState()
        {
            WriteLine(conn.State);
        }

        static void Main(string[] args)
        {
            bool exit = true;
            do
            {
                Program program = new Program();
                Clear();
                WriteLine("### Vegetables_and_fruits ###");
                WriteLine("1. Connect");
                WriteLine("2. Insert");
                WriteLine("3. Show all");
                WriteLine("4. All Title");
                WriteLine("5. All Color");
                WriteLine("6. MAX Сalorific");
                WriteLine("7. MIN Сalorific");
                WriteLine("8. AVG Сalorific");
                WriteLine("9. Number of vegetables");
                WriteLine("10. Number of fruits");
                WriteLine("11. Number of vegetables by color");
                WriteLine("12. Number of fruits by color");
                WriteLine("13. Vegetables and fruits Clorific lower");
                WriteLine("14. Vegetables and fruits Clorific higher");
                WriteLine("15. Count color");
                WriteLine("16. Calorific Range");
                WriteLine("17. Show all with yellow and red color");
                WriteLine("20. Состояние Connection.State");
                WriteLine("30. Exit");
                Write("Select an item and press Enter -> ");
                string? ch = ReadLine();

                switch (ch)
                {
                    case "1":
                        program.ConnectQuery();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "2":
                        program.InsertQuery();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "3":
                        program.ReadAllDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "4":
                        program.ReadTitleDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "5":
                        program.ReadColorDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "6":
                        program.ReadMaxCalorofocDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "7":
                        program.ReadMinCalorofocDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "8":
                        program.ReadAvgCalorofocDate();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "9":
                        program.NumberVegetables();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "10":
                        program.NumberFruits();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "11":
                        program.NumberVegetablesColor();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "12":
                        program.NumberFruitsColor();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "13":
                        program.CalorificLower();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "14":
                        program.CalorificHigher();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "15":
                        program.CountColor();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "16":
                        program.CalorificRange();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "17":
                        program.YellowAndRedColor();
                        WriteLine();
                        WriteLine("To continue, press Enter");
                        ReadLine();
                        break;
                    case "30":
                        try
                        {

                        }
                        finally 
                        {
                            program.DisconnectQuery() ;
                            exit = false;
                            WriteLine("Выход из программы...");
                            Thread.Sleep(1500);
                        }
                        break;
                    case "20":
                        program.ConnState();
                        WriteLine();
                        WriteLine("To continue? press Enter");
                        ReadLine();
                        break;
                }
            } while (exit);

        }
    }
}