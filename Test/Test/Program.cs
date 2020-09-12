using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {

        static SqlConnection coon = new SqlConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString
        };

        void InsertQ()
        {
            try
            {
                coon.Open();
                string incertString = @"Delete from Boks  where ID = 3";
                SqlCommand cmd = new SqlCommand
                {
                    Connection = coon,
                    CommandText = incertString
                };
                cmd.ExecuteNonQuery();


            }
            catch(Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            finally
            {
                coon.Close();
            }
        }

        void SelectQ()
        {
            try
            {
                coon.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors ; select * from Boks", coon);             
                cmd.Parameters.Add("@p1", SqlDbType.Int).Value = 1;
                
                var rnd = cmd.ExecuteReader();

                do
                {

                    for (int i = 0; i < rnd.FieldCount; i++)
                    {
                        Console.Write(rnd.GetName(i) + "\t\t");
                    }
                    Console.WriteLine();

                    while (rnd.Read())
                    {
                        for (int i = 0; i < rnd.FieldCount; i++)
                            Console.Write(rnd[i] + "    \t\t");
                        Console.WriteLine();
                    };
                    Console.WriteLine();

                } while (rnd.NextResult());



            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            finally
            {
                coon.Close();
            }
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.InsertQ();
            program.SelectQ();
            
        }
    }
}
