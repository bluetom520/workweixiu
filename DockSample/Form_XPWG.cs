using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;
using System.IO;
namespace DockSample
{
    public partial class Form_XPWG : Office2007Form
    {
        //private byte[] b;
        SqlConnection con;
        int str_num = 0;
        private string num;
        string wxbh;

        public string ZS
        {
            get { return num; }

            set { num = value; }
        }
        public Form_XPWG(string str_temp)
        {

            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str_temp;
        }

        private void Form_XPWG_Load(object sender, EventArgs e)
        {

        }

        private void button_ClearPic_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = null;
        }

        private void button_ChoosePic_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.JPEG)|*.BMP;*.JPG;*.GIF;*.JPEG";
            openFileDialog1.DefaultExt = "JPG";
            //openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = new Bitmap(this.openFileDialog1.FileName);
                //textBox6.Text = Path.GetFileName(openFileDialog1.FileName);
                //stream = openFileDialog1.OpenFile();
                //int length = (int)stream.Length;
                //b = new byte[length];
                //stream.Read(b, 0, length);


                //MemoryStream m = new MemoryStream(b);
                //Bitmap pic = new Bitmap(m);
                //this.pictureBox1.Image = pic;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.JPEG)|*.BMP;*.JPG;*.GIF;*.JPEG";
            openFileDialog1.DefaultExt = "JPG";
            //openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(this.openFileDialog1.FileName);
                //textBox6.Text = Path.GetFileName(openFileDialog1.FileName);
                //stream = openFileDialog1.OpenFile();
                //int length = (int)stream.Length;
                //b = new byte[length];
                //stream.Read(b, 0, length);


                //MemoryStream m = new MemoryStream(b);
                //Bitmap pic = new Bitmap(m);
                //this.pictureBox1.Image = pic;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.JPEG)|*.BMP;*.JPG;*.GIF;*.JPEG";
            openFileDialog1.DefaultExt = "JPG";
            //openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox3.Image = new Bitmap(this.openFileDialog1.FileName);
                //textBox6.Text = Path.GetFileName(openFileDialog1.FileName);
                //stream = openFileDialog1.OpenFile();
                //int length = (int)stream.Length;
                //b = new byte[length];
                //stream.Read(b, 0, length);


                //MemoryStream m = new MemoryStream(b);
                //Bitmap pic = new Bitmap(m);
                //this.pictureBox1.Image = pic;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox2.Image = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.pictureBox3.Image = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (pictureBox1.Image != null)
                {
                    str_num++;
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save("a1.bmp");
                    FileStream fileStream = new FileStream("a1.bmp", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
                    binaryReader.Close();
                    fileStream.Close();
                    File.Delete("a1.bmp");
                    string str = "insert J_维修外观表 values( '" + wxbh + "',@1) ";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.Parameters.Add(new SqlParameter("@1", SqlDbType.Image));
                    sqlcom.Parameters["@1"].Value = b;
                    sqlcom.ExecuteNonQuery();


                }
                if (pictureBox2.Image !=null)
                {
                    str_num++;
                    MemoryStream ms = new MemoryStream();
                    pictureBox2.Image.Save("a2.bmp");
                    FileStream fileStream = new FileStream("a2.bmp", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
                    binaryReader.Close();
                    fileStream.Close();
                    File.Delete("a2.bmp");
                    string str = "insert J_维修外观表 values( '" + wxbh + "',@1) ";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.Parameters.Add(new SqlParameter("@1", SqlDbType.Image));
                    sqlcom.Parameters["@1"].Value = b;
                    sqlcom.ExecuteNonQuery();
                }
                if (pictureBox3.Image !=null)
                {
                    str_num++;
                    MemoryStream ms = new MemoryStream();
                    pictureBox3.Image.Save("a3.bmp");
                    FileStream fileStream = new FileStream("a3.bmp", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
                    binaryReader.Close();
                    fileStream.Close();
                    File.Delete("a3.bmp");
                    string str = "insert J_维修外观表 values( '" + wxbh + "',@1) ";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.Parameters.Add(new SqlParameter("@1", SqlDbType.Image));
                    sqlcom.Parameters["@1"].Value = b;
                    sqlcom.ExecuteNonQuery();
                }
                con.Close();
                num = str_num.ToString();
                DialogResult = DialogResult.OK;
            }
            catch
            {
            }


        }
    }
}
