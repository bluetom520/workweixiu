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
    public partial class Form_YGXX : Office2007Form
    {
        SqlConnection con;
        DataTable dt2;

        private byte[] b;
        public Form_YGXX()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_YGXX_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            Queue();
        }
        private void Queue()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 序号,编号,姓名 from J_员工信息表 order by 编号";


                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

                con.Close();
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                Queue_data();
            }
        }
        private void Queue_data()
        {

            try
            {
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_员工信息表 where 序号='" + str2 + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                dt2 = new DataTable();
                da.Fill(dt2);
                show_data();
            }
            catch
            {
            }
            con.Close();
        }


        private void show_data()
        {
            textBox_Name.Text= dt2.Rows[0]["姓名"].ToString();
            comboBox_Sex.Text=  dt2.Rows[0]["性别"].ToString();
            comboBox_Folk.Text=dt2.Rows[0]["民族"].ToString();
            comboBox_Kultur.Text= dt2.Rows[0]["学历"].ToString();
            comboBox_Marriage.Text=  dt2.Rows[0]["婚姻状况"].ToString();
            dateTimePicker1.Text = dt2.Rows[0]["出生年月"].ToString();
            textBox_IDCardNo.Text= dt2.Rows[0]["身份证"].ToString();
            textBox_CellPhone.Text = dt2.Rows[0]["联系电话"].ToString();
            textBox_Address.Text=  dt2.Rows[0]["家庭住址"].ToString();
            textBox_University.Text = dt2.Rows[0]["毕业学校"].ToString();
            textBox_Speciality.Text=  dt2.Rows[0]["专业名称"].ToString();
            textBox_HomePhone.Text=  dt2.Rows[0]["家庭电话"].ToString();
            textBox_OfficePhone.Text=  dt2.Rows[0]["办公电话"].ToString();
            textBox_Email.Text=  dt2.Rows[0]["电子信箱"].ToString();
            dateTimePicker2.Text = dt2.Rows[0]["毕业时间"].ToString();
            textBox1.Text = dt2.Rows[0]["编号"].ToString();

            comboBox_Branch.Text=  dt2.Rows[0]["部门"].ToString();
            comboBox_Business.Text=  dt2.Rows[0]["职务"].ToString();
            comboBox_Title.Text=  dt2.Rows[0]["角色"].ToString();
            comboBox_KindOfSalary.Text=  dt2.Rows[0]["级别"].ToString();
            comboBox_NativePlace_Province.Text = dt2.Rows[0]["籍贯省份"].ToString();
            comboBox_NativePlace_City.Text = dt2.Rows[0]["籍贯城市"].ToString();
            textBox2.Text = dt2.Rows[0]["参加工作时间"].ToString();
            textBox3.Text = dt2.Rows[0]["合同开始时间"].ToString();
            textBox5.Text = dt2.Rows[0]["合同结束时间"].ToString();
            textBox4.Text = dt2.Rows[0]["实习结束时间"].ToString();
            if (dt2.Rows[0]["照片"].ToString() != "")
            {
                byte[] b2 = (byte[])dt2.Rows[0]["照片"];
                if (b2.Length > 0)
                {
                    string temp = Encoding.Default.GetString(b2);
                    MemoryStream m = new MemoryStream(b2);
                    Bitmap pic = new Bitmap(m);
                    this.pictureBox1.Image = pic;
                }
            }
            else
                this.pictureBox1.Image = DockSample.Properties.Resources.kuser;


        }


        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str1 = "select 级别 from J_工资级别表";
                str1 += ";select 名称 from J_基础信息表 where 类别='部门'";
                str1 += ";select 名称 from J_基础信息表 where 类别='职位'";
                str1 += ";select 角色 from J_权限管理表";
                str1 += ";select DISTINCT Province from tb_City ORDER BY Province";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBox_KindOfSalary.Items.Add(ds.Tables[0].Rows[i]["级别"].ToString());
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    comboBox_Branch.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    comboBox_Business.Items.Add(ds.Tables[2].Rows[i]["名称"].ToString());
                }
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    comboBox_Title.Items.Add(ds.Tables[3].Rows[i]["角色"].ToString());
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    comboBox_NativePlace_Province.Items.Add(ds.Tables[4].Rows[i]["Province"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

                con.Close();
            
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                change_data();
                Queue();
            }
        }
        private void change_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {

                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_员工信息表 where  序号='" + str2 + "'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    dt.Rows[0]["姓名"] = textBox_Name.Text; 
                    dt.Rows[0]["性别"]=comboBox_Sex.Text ;
                    dt.Rows[0]["民族"]=comboBox_Folk.Text ;
                    dt.Rows[0]["学历"]=comboBox_Kultur.Text ;
                    dt.Rows[0]["婚姻状况"]=comboBox_Marriage.Text;
                    dt.Rows[0]["出生年月"]=dateTimePicker1.Text ;
                    dt.Rows[0]["身份证"]=textBox_IDCardNo.Text ;
                    dt.Rows[0]["联系电话"]=textBox_CellPhone.Text ;
                    dt.Rows[0]["家庭住址"]=textBox_Address.Text ;
                    dt.Rows[0]["毕业学校"]=textBox_University.Text ;
                    dt.Rows[0]["专业名称"]=textBox_Speciality.Text ;
                    dt.Rows[0]["家庭电话"]=textBox_HomePhone.Text ;
                    dt.Rows[0]["办公电话"]=textBox_OfficePhone.Text;
                    dt.Rows[0]["电子信箱"]=textBox_Email.Text ;
                    dt.Rows[0]["毕业时间"]= dateTimePicker2.Text ;
                    dt.Rows[0]["编号"]=textBox1.Text;

                    dt.Rows[0]["部门"]=comboBox_Branch.Text ;
                    dt.Rows[0]["职务"]=comboBox_Business.Text ;
                    dt.Rows[0]["角色"]=comboBox_Title.Text;
                    dt.Rows[0]["级别"]=comboBox_KindOfSalary.Text;
                    dt.Rows[0]["籍贯省份"] = comboBox_NativePlace_Province.Text;
                    dt.Rows[0]["籍贯城市"] = comboBox_NativePlace_City.Text;
                    dt.Rows[0]["参加工作时间"] = textBox2.Text;
                    dt.Rows[0]["合同开始时间"] = textBox3.Text;
                    dt.Rows[0]["合同结束时间"] = textBox5.Text;
                    dt.Rows[0]["实习结束时间"] = textBox4.Text;
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save("a.bmp");
                    FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    b = binaryReader.ReadBytes((int)fileStream.Length);
                    binaryReader.Close();
                    fileStream.Close();
                    File.Delete("a.bmp");
                    dt.Rows[0]["照片"] = b;
                    da.Update(dt);
                    default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();


            }
            else
            {
                MessageBox.Show("请选择要修改的列！");
            }
        }
        private void add_data()
        {
            try
            {
                if (textBox_Name.Text == "" || textBox1.Text == "")
                {
                    MessageBox.Show("请输入员工姓名或员工编号！");
                    return;
                }
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string[] str_data = new string[26];
                str_data[0] = textBox_Name.Text;
                str_data[1] = comboBox_Sex.Text;
                str_data[2] = comboBox_Folk.Text;
                str_data[3] = comboBox_Kultur.Text;
                str_data[4] = comboBox_Marriage.Text;
                str_data[5] = dateTimePicker1.Text;
                str_data[6] = textBox_IDCardNo.Text;
                str_data[7] = textBox_CellPhone.Text;
                str_data[8] = textBox_Address.Text;

                str_data[9] = textBox_University.Text;
                str_data[10] = textBox_Speciality.Text;
                str_data[11] = textBox_HomePhone.Text;
                str_data[12] = textBox_OfficePhone.Text;
                str_data[13] = textBox_Email.Text;
                str_data[14] = dateTimePicker2.Text;
                str_data[15] = textBox1.Text;
                str_data[16] = comboBox_Branch.Text;
                str_data[17] = comboBox_Business.Text;
                str_data[18] = comboBox_Title.Text;
                str_data[19] = comboBox_KindOfSalary.Text;
                str_data[20] = comboBox_NativePlace_Province.Text;
                str_data[21] = comboBox_NativePlace_City.Text;
                str_data[22] = textBox2.Text;
                str_data[23] = textBox3.Text;
                str_data[24] = textBox5.Text;
                str_data[25] = textBox4.Text;
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save("a.bmp");
                FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                b = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();
                File.Delete("a.bmp");


                string str = "insert into J_员工信息表 (姓名,性别,民族,学历,婚姻状况,出生年月,身份证,联系电话,家庭住址,毕业学校,专业名称,家庭电话,办公电话,电子信箱,毕业时间,编号,部门,职务,角色,级别,籍贯省份,籍贯城市,参加工作时间,合同开始时间,合同结束时间,实习结束时间,照片)values(";
                for (int i = 0; i < str_data.Length; i++)
                {
                    if (i == 5 || i == 14)
                    {
                        str += "'" +Convert.ToDateTime(str_data[i]) + "',";
                    }
                    else
                    {
                        str += "'" + str_data[i] + "',";
                    }
                }
                str += "  @1  )";
                SqlCommand sqlcom = new SqlCommand(str, con);
                sqlcom.Parameters.Add(new SqlParameter("@1", SqlDbType.Image));
                sqlcom.Parameters["@1"].Value =b;
                sqlcom.ExecuteNonQuery();
                default_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
                con.Close();

            
        }

        private void button_ChoosePic_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog1.DefaultExt = "JPG";
            //openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = new Bitmap(this.openFileDialog1.FileName);


                //MemoryStream m = new MemoryStream(b);
                //Bitmap pic = new Bitmap(m);
                //this.pictureBox1.Image = pic;
            }

        }
        private void default_data()
        {
            textBox_Name.Text = "";
            comboBox_Sex.Text = "";
            comboBox_Folk.Text = "";
            comboBox_Kultur.Text = "";
            comboBox_Marriage.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();
            textBox_IDCardNo.Text = "";
            textBox_CellPhone.Text = "";
            textBox_Address.Text = "";
            textBox_University.Text = "";
            textBox_Speciality.Text = "";
            textBox_HomePhone.Text = "";
            textBox_OfficePhone.Text = "";
            textBox_Email.Text = "";
            dateTimePicker2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox_Branch.Text = "";
            comboBox_Business.Text = "";
            comboBox_Title.Text = "";
            comboBox_KindOfSalary.Text = "";

            this.pictureBox1.Image = DockSample.Properties.Resources.kuser;
            
        }

        private void button_ClearPic_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = DockSample.Properties.Resources.kuser;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            add_data();
            Queue();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            default_data();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                Queue();
            }
        }
        private void delete_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_员工信息表 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();

            }
            else
            {
                MessageBox.Show("请选择要删除的列！");
            }
        }

        private void textBox_Practice_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_TimeOfContractBegin_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = dateTimePicker_TimeOfContractBegin.Value.ToString("yyyy年MM月dd日");
        }

        private void comboBox_NativePlace_Province_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                comboBox_NativePlace_City.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select City from tb_City  where Province='" + comboBox_NativePlace_Province.Text + "' ORDER BY City";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt2 = new DataTable();
                da.Fill(dt2);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    comboBox_NativePlace_City.Items.Add(dt2.Rows[i]["City"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void dateTimePicker_TimeOfWork_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = dateTimePicker_TimeOfWork.Value.ToString("yyyy年MM月dd日");
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = dateTimePicker3.Value.ToString("yyyy年MM月dd日");
        }

        private void dateTimePicker_TimeOfContractEnd_ValueChanged(object sender, EventArgs e)
        {
            textBox5.Text = dateTimePicker_TimeOfContractEnd.Value.ToString("yyyy年MM月dd日");
        }
    }
}
