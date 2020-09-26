using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBProFacrt
{
    public partial class Form1 : Form
    {
        
        DbConnection conn=null;
        DbProviderFactory fact=null;
        public Form1()
        {
            InitializeComponent();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable t = DbProviderFactories.GetFactoryClasses();
                dataGridView1.DataSource = t;
                comboBox1.Items.Clear();
                foreach (DataRow dr in t.Rows)
                {
                    comboBox1.Items.Add(dr["InvariantName"]);
                }
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text= GetConnectionStringByProvider(comboBox1.SelectedItem.ToString());
            fact = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
            conn = fact.CreateConnection();
            conn.ConnectionString = textBox1.Text;

        }

        private string GetConnectionStringByProvider(string providerName)
        {

            var setting = ConfigurationManager.ConnectionStrings;
          
        
          

            if (setting !=null)
            {
                foreach (ConnectionStringSettings cs in setting)
                {
                    if (cs.ProviderName == providerName)
                    {
                         return cs.ConnectionString;
                       
                    }
                }
            }
            return string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = textBox1.Text;     
             DbDataAdapter adapter = fact.CreateDataAdapter();
            adapter.SelectCommand = conn.CreateCommand();
            adapter.SelectCommand.CommandText = textBox2.
            Text.ToString();
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 5)
                button2.Enabled = true;
            else
                button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            const string AsyncEnabled ="Asynchronous Processing=true";
            if (!textBox1.Text.Contains(AsyncEnabled))
            {
                textBox1.Text = String.Format("{0}; {1}", textBox1.Text, AsyncEnabled);
            }
            ///
            conn = new SqlConnection(textBox1.Text);
            SqlCommand comm = (SqlCommand)conn.CreateCommand();
            /// блок 2
            comm.CommandText = "WAITFOR DELAY '00:00:05'" ;
            comm.CommandType = CommandType.Text;
            comm.CommandTimeout = 30;
            ///
            try
            {
                conn.Open();
                /// блок 3
                AsyncCallback callback = new AsyncCallback(GetDataCallback);                
                comm.BeginExecuteReader(callback, comm);
                MessageBox.Show("Added thread is working...");
                ///
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        DataTable table;
        private void GetDataCallback(IAsyncResult result)
        {
            SqlDataReader reader = null;
            try
            {
               

                SqlCommand command = (SqlCommand)result.AsyncState;
               
                
                reader = command.EndExecuteReader(result);
                
               table = new DataTable();
                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i <
                            reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                      
                  
                    table.Rows.Add(row);
                    }
                } while (reader.NextResult());
                DgvAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("From Callback 1:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("From Callback 2:" +
                    ex.Message);
                }
            }
        }

        private void DgvAction()
        {
           
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(DgvAction));
                return;
            }
            dataGridView1.DataSource = table;
        }
    }
}
    
    

