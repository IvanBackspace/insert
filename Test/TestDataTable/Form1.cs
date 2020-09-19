using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Web.UI.WebControls;
using System.Windows.Documents;
using System.Diagnostics;
using System.IO;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;

namespace TestDataTable
{
    public partial class Form1 : Form
    {
       
        DataSet set;
        SqlDataAdapter da;
        SqlCommandBuilder cmd;
        string FilName;
       
        public Form1()
        {
            InitializeComponent();
        }
        string ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString;
        static SqlConnection coon = new SqlConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString
        };

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                set = new DataSet();

                da = new SqlDataAdapter(textBox1.Text, conn);
                
                var cmd = new SqlCommandBuilder(da);
                //SqlCommand updDateCmd = new SqlCommand("Update Authors set LastName = 'Look', FirstName = 'Tuul' where id = @pId", conn);
                //updDateCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
                //updDateCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
                //updDateCmd.Parameters["@pId"].SourceColumn = "id";
                //da.UpdateCommand = updDateCmd;

                SqlCommand updatesqlCommand = new SqlCommand("UpdateAuthors", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameterCollection parameter = updatesqlCommand.Parameters;
                parameter.Add("@pid", SqlDbType.Int, 0, "id");
                parameter["@pid"].SourceVersion = DataRowVersion.Original;
                   parameter.Add("@pLastName", SqlDbType.VarChar, 0, "LastName");
                parameter.Add("@pFirstName", SqlDbType.VarChar, 0, "FirstName");


                da.Fill(set, "mybook");
                dataGridView1.DataSource = set.Tables["mybook"];

            } 
            catch (Exception exp)
            {
               MessageBox.Show(exp.ToString());
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            da.Update(set, "mybook");
            //Debug.WriteLine(cmd.GetInsertCommand().CommandText);
          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { };
                coon.Open();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FilName = openFileDialog.FileName;
                    var bytes = CreatCoppy(FilName);
                    if ((toolStripTextBox1.Text?.Length ?? 0) != 0 && int.TryParse(toolStripTextBox1.Text, out int index))
                    {
                        var comm = new SqlCommand("Insert into Pictures (Bookid, Name,Picture) values (@Bookid, @Name,@Picture)", coon);
                        comm.Parameters.AddWithValue("@Bookid", index);
                        comm.Parameters.AddWithValue("@Name", Path.GetFileName(Name));
                        comm.Parameters.AddWithValue("@Picture", bytes);
                        comm.ExecuteNonQuery();
                        MessageBox.Show("srtrw");
                    }
                }
                coon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          

        }

        private object CreatCoppy(string filName)
        {
            var img = Image.FromFile(filName);
            int maxWidht = 300;
            int maxHidht = 300;
            int newwidht = (int)(img.Width * (double)maxWidht / img.Width);
            int newHidht = (int)(img.Height * (double)maxHidht / img.Height);

            var imageRation = new Bitmap(newwidht, newHidht);
            var g = Graphics.FromImage(imageRation);
            g.DrawImage(img, 0, 0, newwidht, newHidht);

            using (var stream = new MemoryStream()) 
            using (var reader = new BinaryReader(stream) )
            {
                imageRation.Save(stream, ImageFormat.Jpeg);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return reader.ReadBytes((int)stream.Length);
            }
        }
    
        
    }
}
