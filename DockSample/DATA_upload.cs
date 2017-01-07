/*
 * Created by SharpDevelop.
 * User: admin
 * Date: 2011-11-21
 * Time: 14:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using DevComponents.DotNetBar;
namespace DockSample
{
	/// <summary>
	/// Description of SCTB.
	/// </summary>
    public partial class DATA_upload : Office2007Form
	{
        SqlConnection con_1 ;
        private DataGridViewCellStyle m_RowStyleNormal;
        private DataGridViewCellStyle m_RowStyleAlternate;
        private DataGridViewCellStyle m_skyblue;	
        public string hostname;
        public int port;
        public string user;
        public string password;
        string flag;
        string wxbh;

        public DATA_upload(string str)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            con_1 = new SqlConnection(MainForm.connetstring);
            wxbh = str;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        class DoubleBufferListView : DataGridView        ///双缓冲DataGridView，解决闪烁
        {
            public DoubleBufferListView()
            {
                SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                UpdateStyles();

            }
        }
        private void SetRowStyle()
        {
            //可根据需要设置更多样式属性，如字体、对齐、前景色、背景色等
            m_RowStyleNormal = new DataGridViewCellStyle();
            m_RowStyleNormal.BackColor = Color.LightBlue;
            m_RowStyleNormal.SelectionBackColor = Color.LightSteelBlue;

            m_RowStyleAlternate = new DataGridViewCellStyle();
            m_RowStyleAlternate.BackColor = Color.LightGray;
            m_RowStyleAlternate.SelectionBackColor = Color.LightSlateGray;

            m_skyblue = new DataGridViewCellStyle();
            m_skyblue.BackColor = Color.SkyBlue;

        }
		void DataGridView1ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
            DataGridView dgv = (DataGridView)sender;
            dgv.Invalidate();
			
		}
		
		void DataGridView1RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{
            if ((e.PaintParts & DataGridViewPaintParts.Background) ==
                DataGridViewPaintParts.Background)
            {
                Color bColor1, bColor2;
                if ((e.PaintParts & DataGridViewPaintParts.SelectionBackground) ==
                        DataGridViewPaintParts.SelectionBackground &&
                    (e.State & DataGridViewElementStates.Selected) ==
                        DataGridViewElementStates.Selected)
                {
                    bColor1 = e.InheritedRowStyle.SelectionBackColor;
                    bColor2 = Color.Black;
                }
                else
                {
                    bColor1 = e.InheritedRowStyle.BackColor;
                    bColor2 = Color.YellowGreen;
                }

                DataGridView dgv = (DataGridView)sender;
                int rectLeft2 = dgv.RowHeadersVisible ? dgv.RowHeadersWidth : 0;
                int rectLeft = rectLeft2 - dgv.HorizontalScrollingOffset;
                int rectWidth = dgv.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible);
                Rectangle rect = new Rectangle(rectLeft, e.RowBounds.Top,
                    rectWidth, e.RowBounds.Height - 1);

                using (System.Drawing.Drawing2D.LinearGradientBrush b =
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                    rect, bColor1, bColor2,
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    rect.X = rectLeft2;
                    rect.Width -= dgv.HorizontalScrollingOffset;

                    e.Graphics.FillRectangle(b, rect);
                }
                e.PaintHeader(true);
                e.PaintParts &= ~DataGridViewPaintParts.Background;
            }
			
		}

        private void button3_Click(object sender, EventArgs e)
        {

        }

        




        private void SCTB_Load(object sender, EventArgs e)
        {
            //SetRowStyle();

            //Data_initial();
            get_ftp();
            //Queue();

        }
        //private void Data_initial()
        //{
        //    //comboBox1.Items.Clear();
        //    try
        //    {
        //        con_1.Open();
        //        string str1 = "select 名称 from J_值班字典表 where 类别='资料类别'";
        //        SqlDataAdapter da = new SqlDataAdapter(str1, con_1);
        //        SqlCommandBuilder t_bu = new SqlCommandBuilder(da);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            comboBox1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
        //        }
        //        comboBox1.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        con_1.Close();
        //    }
        //}
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""|| richTextBox1.Text == "")
            {
                MessageBox.Show("您还有周结要素未填写，请检查！");
            }
            else
            {
                //string str2 = this.dataGridView1.SelectedRows[0].Cells["审阅标记"].Value.ToString();
                //if (str2 == "1")
                //{
                //    MessageBox.Show("周结已审阅，不能修改！");
                //}
                //else
                //{
                    if (MessageBox.Show("确定要修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        change_data();
                        //Queue();

                    }
                //}
            }
        }

        private void change_data()
        {
            ////if (this.dataGridView1.SelectedRows.Count > 0)
            ////{
            //    try
            //    {
            //        con_1.Open();
            //        string str = "";
            //        //string str = "select * from J_周结记录表 where 序号='" + this.dataGridView1.SelectedRows[0].Cells["序号"].Value.ToString() + "'";
            //        string str_data = "SELECT TOP 1 GETDATE() AS Expr1 FROM dbo.[人员表]";//取数据库时间
            //        SqlCommand cmd = new SqlCommand(str_data, con_1);
            //        SqlDataReader dr = cmd.ExecuteReader();
            //        while (dr.Read())
            //        {
            //            str_data = dr[0].ToString();
            //        }
            //        dr.Close();
            //        SqlDataAdapter da = new SqlDataAdapter(str, con_1);
            //        SqlCommandBuilder t_build = new SqlCommandBuilder(da);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);


            //        dt.Rows[0]["上报时间"] = str_data;
            //        dt.Rows[0]["标题"] = textBox1.Text;
            //        dt.Rows[0]["姓名"] = LoginXT.username;
            //        dt.Rows[0]["周结"] = richTextBox1.Text;
            //        if (textBox3.Text != "")
            //        {
            //            dt.Rows[0]["附件标志"] = 1;
            //            dt.Rows[0]["附件名"] = textBox3.Text;
            //            dt.Rows[0]["附件路径"] = DateTime.Now.ToShortDateString() + "\\" + textBox3.Text;///待从上传的文件中取值
            //        }
            //        da.Update(dt);
            //        MessageBox.Show("修改成功！");
            //        clear_data();


            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //        con_1.Close();
            //    }

        }


        private void write_data()
        {
            try
            {
                if (con_1.State == ConnectionState.Closed)
                    con_1.Open();



                string[] data = new string[8];
                data[0] = comboBoxEx1.Text;
                data[1] = textBox1.Text;
                data[2] = richTextBox1.Text;
                data[3] = "";
                data[4] = DateTime.Now.ToString();
                data[5] = LoginXT.username;
                data[6] = textBox3.Text;



                data[7] = DateTime.Now.ToString("yyyyMMdd") + "\\" + textBox3.Text;
                string str = "insert into J_维修资料库 values('" + wxbh + "','" + data[0] + "'";
                str += ",'" + data[1] + "','" + data[2] + "','" + data[4] + "','" + data[5] + "','" + data[6] + "','" + data[7] + "')";


                SqlCommand sqlcom = new SqlCommand(str, con_1);
                sqlcom.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con_1.Close();

        }





        private void clear_data()
        {
            textBox1.Text = "";

            textBox3.Text = "";
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void data_update(string str2)
        {
            Zjs.Ftp.FTPClient ftpclient = new Zjs.Ftp.FTPClient();
            bool flag = false;
            string str = DateTime.Now.ToString("yyyyMMdd");//上传，每天按日期建一个新目录

            ftpclient.RemoteHost = ftp1.Hostname;
            ftpclient.RemotePort = ftp1.Port;
            ftpclient.RemoteUser = user;
            ftpclient.RemotePass = password;
            ftp1.Remote = "";
            string[] dirs = ftp1.listfile();
            foreach (string dir in dirs)
            {
                if (dir.StartsWith("d") && !string.IsNullOrEmpty(dir))
                {
                    if (dir.IndexOf(str) >= 0)  //筛选出真实的目录
                    {

                        flag = true;

                    }


                }
            }
            if (flag)
            {
                ftpclient.ChDir(str);
            }
            else
            {
                ftpclient.MkDir(str);
                ftpclient.ChDir(str);
            }
            //string str = comboBox1.Text;
            ftp1.Remote = str;
            ftp1.putfile(str2);
            timer1.Start();

            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear_data();
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Paste();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 30;
            label7.Text = ftp1.progressBar.Value.ToString() + "%";
            if (ftp1.progressBar.Value == 100)
            {
                timer1.Stop();
            }
        }


        private void get_ftp()
        {
            try
            {
                if (con_1.State == ConnectionState.Closed)
                    con_1.Open();

                string str = "select * from J_FTP服务";
                SqlDataAdapter da = new SqlDataAdapter(str, con_1);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //hostname = dt.Rows[0]["IP"].ToString();
                IniFiles inifile = new IniFiles("config.ini");

                string ds = inifile.ReadString("数据库", "IP", "");
                hostname = ds;
   
                port = Convert.ToInt32(dt.Rows[0]["端口"].ToString());
                user = dt.Rows[0]["帐号"].ToString();
                password = dt.Rows[0]["密码"].ToString();
                ftp1.Hostname = hostname;
                ftp1.Port = port;
                ftp1.User = user;
                ftp1.Password = password;	
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

                con_1.Close();
            

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Cut();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowColor = true;
            this.fontDialog1.ShowDialog();
            this.richTextBox1.ForeColor = this.fontDialog1.Color;
            this.richTextBox1.Font = this.fontDialog1.Font;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Queue();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //string str2 = this.dataGridView1.SelectedRows[0].Cells["审阅标记"].Value.ToString();
            //if (str2 == "1")
            //{
            //    MessageBox.Show("周结已审阅，不能删除！");
            //}
            //else
            //{
                if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    //delete_data();
                    //Queue();
                }
            //}
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string path = "";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                    data_update(path);

                    textBox3.Text = Path.GetFileName(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.Text == "" || textBox1.Text == "" || textBox3.Text == "" || richTextBox1.Text == "")
            {
                MessageBox.Show("您还有要素未填写，请检查！");
            }
            else
            {
                if (MessageBox.Show("确定增加吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    write_data();
                    this.DialogResult = DialogResult.OK;

                }
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void delete_data()
        //{
        //    if (this.dataGridView1.SelectedRows.Count > 0)
        //    {
        //        string str1 = this.dataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
        //        try
        //        {
        //            con_1.Open();

        //            string str = "delete from J_周结记录表 where 序号='" + str1 + "'";
        //            SqlCommand sqlcom = new SqlCommand(str, con_1);
        //            sqlcom.ExecuteNonQuery();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        finally
        //        {
        //            con_1.Close();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("请选择要删除的列！");
        //    }
        //}

        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    DataGridView dgv = (DataGridView)sender;

        //    if (dgv.Columns[e.ColumnIndex].Name == "类别")
        //    {
        //        string getdata = dgv.Rows[e.RowIndex].Cells["类别"].Value.ToString();
        //        switch (getdata)
        //        {
        //            case "0": e.Value = "周结";
        //                break;
        //            case "1": e.Value = "资料";
        //                break;
        //            case "2": e.Value = "通知";
        //                break;
        //            case "3": e.Value = "文件";
        //                break;
        //        }

        //        // 		        e.FormattingApplied=true;

        //    }
        //}
	}
}
