using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using grproLib;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using System.IO;
namespace DockSample
{
    public partial class Form_XXXX : Office2007Form
    {
        string wxbh;
        SqlConnection con;
        string gzbz;
        private GridppReport Report = new GridppReport();
        public Form_XXXX(string str_temp)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str_temp;
        }

        private void Form_XXXX_Load(object sender, EventArgs e)
        {
            Queue_data();
            if (MainForm.str_zlxg != "1")
            {
                buttonX3.Enabled = false;
                buttonX2.Enabled = false;
                buttonX1.Enabled = false;
                buttonX4.Enabled = false;
                buttonX5.Enabled = false;
                buttonX6.Enabled = false;
            }
            else
            {
                buttonX3.Enabled = true;
                buttonX2.Enabled = true;
                buttonX1.Enabled = true;
                buttonX4.Enabled = true;
                buttonX5.Enabled = true;
                buttonX6.Enabled = true;
            }
        }
        private void Queue_data()
        {

            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_维修处理表 a left outer join  J_客户信息 b on a.客户编号=b.客户编号 where  维修编号='" + wxbh + "'";
                str += ";select * from J_维修积分表  where  维修编号='" + wxbh + "' and 保期返修!=1";//0 未结单维修 1 结单维修
                str += ";select * from J_报价详细表 where  维修编号='" + wxbh + "'";
                str += ";select * from J_收支流水 where  维修编号 ='" + wxbh + "' and 项目名称='维修收费'";
                str += ";select * from J_应收明细 where  维修编号='" + wxbh + "'";
                str += ";select * from J_配件领用 where  维修编号 ='" + wxbh + "'";
                str += ";select * from J_维修资料库 where  维修编号 ='" + wxbh + "'";
                str += ";select * from J_维修外观表 where  维修编号 ='" + wxbh + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);

                textBoxX1.Text = ds.Tables[0].Rows[0]["维修编号"].ToString();
                string temp = ds.Tables[0].Rows[0]["当前状态"].ToString();
                switch (temp)
                {
                    case "0": temp = "待报价";
                        break;
                    case "1": temp = "待派工";
                        break;
                    case "2": temp = "维修中";
                        break;
                    case "3": temp = "待质检";
                        break;
                    case "4": temp = "待结算";
                        break;
                    case "5": temp = "待审核";
                        break;
                    case "6": temp = "已结束";
                        break;
                    case "7": temp = "已取消";
                        break;
                    case "8": temp = "待验收";
                        break;
                    case "9": temp = "待收款";
                        break;
                    case "10": temp = "送修中";
                        break;
                }
                textBoxX2.Text = temp;
                textBoxX3.Text = ds.Tables[0].Rows[0]["接修日期"].ToString();
                textBoxX4.Text = ds.Tables[0].Rows[0]["业务员"].ToString();
                textBoxX5.Text = ds.Tables[0].Rows[0]["技术员"].ToString();
                textBoxX6.Text = ds.Tables[0].Rows[0]["预约日期"].ToString();
                textBoxX7.Text = ds.Tables[0].Rows[0]["修品SN1"].ToString();
                textBoxX8.Text = ds.Tables[0].Rows[0]["修品型号"].ToString();
                textBoxX9.Text = ds.Tables[0].Rows[0]["修品品牌"].ToString();
                textBoxX10.Text = ds.Tables[0].Rows[0]["规格参数"].ToString();

                textBoxX11.Text = ds.Tables[0].Rows[0]["故障描述"].ToString();
                textBoxX12.Text = ds.Tables[0].Rows[0]["修品PN号"].ToString();
                //textBoxX13.Text = ds.Tables[0].Rows[0]["随机附件"].ToString();
                textBoxX14.Text = ds.Tables[0].Rows[0]["修品SN2"].ToString();
                //textBoxX15.Text = ds.Tables[0].Rows[0]["保修情况"].ToString();
                textBoxX16.Text = "1";
                textBoxX17.Text = ds.Tables[0].Rows[0]["修品大类"].ToString();
                textBoxX18.Text = ds.Tables[0].Rows[0]["修品小类"].ToString();
                textBoxX19.Text = ds.Tables[0].Rows[0]["外观"].ToString();
                textBoxX20.Text = ds.Tables[0].Rows[0]["优先级"].ToString();

                textBoxX21.Text = ds.Tables[0].Rows[0]["备注"].ToString();
                textBoxX22.Text = ds.Tables[0].Rows[0]["返还方式"].ToString();
                textBoxX23.Text = ds.Tables[0].Rows[0]["返回日期"].ToString();
                textBoxX24.Text = ds.Tables[0].Rows[0]["保修时间"].ToString();
                //textBoxX25.Text = ds.Tables[0].Rows[0]["故障描述"].ToString();
                textBoxX26.Text = ds.Tables[0].Rows[0]["快递单号"].ToString();
                temp = ds.Tables[0].Rows[0]["返修次数"].ToString();
                if (temp != "0")
                {
                    checkBoxX1.Checked = true;
                }
                ////////////////////////////////////////////////////////////////////
                textBoxX27.Text = ds.Tables[0].Rows[0]["维修状态"].ToString();
                textBoxX28.Text = temp;
                textBoxX29.Text = ds.Tables[0].Rows[0]["预计积分"].ToString();
                textBoxX30.Text = ds.Tables[0].Rows[0]["维修情况"].ToString();
                dataGridViewX1.DataSource = ds.Tables[5];
                dataGridViewX2.DataSource = ds.Tables[6];
                this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX2.Columns["序号"].Visible = false;

                ///////////////////////////////////////////////////////////////////

                textBoxX31.Text = ds.Tables[0].Rows[0]["客户类别"].ToString();
                textBoxX32.Text = ds.Tables[0].Rows[0]["客户名称"].ToString();
                textBoxX33.Text = ds.Tables[0].Rows[0]["联系人"].ToString();
                textBoxX34.Text = ds.Tables[0].Rows[0]["联系电话"].ToString();
                textBoxX25.Text = ds.Tables[0].Rows[0]["联系人2"].ToString();
                textBoxX35.Text = ds.Tables[0].Rows[0]["联系电话2"].ToString();
                textBoxX38.Text = ds.Tables[0].Rows[0]["联系人3"].ToString();
                textBoxX57.Text = ds.Tables[0].Rows[0]["联系电话3"].ToString();
                textBoxX36.Text = ds.Tables[0].Rows[0]["通信地址"].ToString();
                textBoxX37.Text = ds.Tables[0].Rows[0]["邮政编码"].ToString();

                /////////////////////////////////////////////////////////////////////
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //textBoxX38.Text = ds.Tables[2].Rows[0]["检测费用"].ToString();
                    textBoxX39.Text = ds.Tables[2].Rows[0]["总计费用"].ToString();
                    textBoxX40.Text = ds.Tables[2].Rows[0]["税金"].ToString();

                    textBoxX41.Text = ds.Tables[2].Rows[0]["小计费用"].ToString();
                    textBoxX42.Text = ds.Tables[2].Rows[0]["管理费用"].ToString();
                    textBoxX43.Text = ds.Tables[2].Rows[0]["运输费用"].ToString();
                    textBoxX44.Text = ds.Tables[2].Rows[0]["现场费用"].ToString();
                    textBoxX45.Text = ds.Tables[2].Rows[0]["材料费用"].ToString();
                    textBoxX46.Text = ds.Tables[2].Rows[0]["检测维修费用"].ToString();
                }

                /////////////////////////////////////////////////////////////////////////
                if (ds.Tables[1].Rows.Count > 0)
                {
                    textBoxX58.Text = ds.Tables[1].Rows[0]["返修次数"].ToString();
                    textBoxX59.Text = ds.Tables[1].Rows[0]["扣分"].ToString();
                    textBoxX60.Text = ds.Tables[1].Rows[0]["加分"].ToString();
                    textBoxX61.Text = ds.Tables[1].Rows[0]["难度"].ToString();
                    textBoxX62.Text = ds.Tables[1].Rows[0]["合计"].ToString();
                    textBoxX63.Text = ds.Tables[1].Rows[0]["新品"].ToString();
                    textBoxX64.Text = ds.Tables[1].Rows[0]["价值"].ToString();
                    textBoxX65.Text = ds.Tables[1].Rows[0]["工时"].ToString();
                }

                ///////////////////////////////////////////////////////////////////////////////

                textBoxX52.Text = ds.Tables[0].Rows[0]["预付款"].ToString();
                textBoxX53.Text = ds.Tables[0].Rows[0]["维修报价"].ToString();
                decimal[] str_data = new decimal[3] { 0, 0, 0 };
                if (textBoxX53.Text != "")
                {
                    str_data[0] = Convert.ToDecimal(textBoxX53.Text);
                }
                if (textBoxX52.Text != "")
                {
                    str_data[1] = Convert.ToDecimal(textBoxX52.Text);
                }
                str_data[2] = str_data[0] - str_data[1];
                textBoxX51.Text = str_data[2].ToString();
                gzbz = ds.Tables[0].Rows[0]["挂账标志"].ToString();
                if (gzbz == "1")
                {
                    checkBoxX2.Checked = true;
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        DateTime d1 = Convert.ToDateTime(ds.Tables[4].Rows[0]["提醒日期"].ToString());
                        DateTime d2 = Convert.ToDateTime(ds.Tables[4].Rows[0]["日期"].ToString());
                        TimeSpan t = d1 - d2;
                        textBoxX47.Text = ((int)t.Days).ToString();
                        textBoxX48.Text = ds.Tables[4].Rows[0]["实收金额"].ToString();
                        textBoxX49.Text = ds.Tables[4].Rows[0]["发票号码"].ToString();
                        textBoxX50.Text = ds.Tables[4].Rows[0]["开票金额"].ToString();
                        //textBoxX54.Text = ds.Tables[4].Rows[0]["账户"].ToString();
                        //textBoxX55.Text = ds.Tables[4].Rows[0]["项目名称"].ToString();
                        textBoxX56.Text = ds.Tables[0].Rows[0]["客户名称"].ToString();
                        textBoxX15.Text = ds.Tables[4].Rows[0]["开票日期"].ToString();
                        textBoxX13.Text = ds.Tables[4].Rows[0]["摘要明细"].ToString();
                    }
                }
                else
                {
                    checkBoxX2.Checked = false;
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        //if (temp == "0")
                        //{
                        textBoxX48.Text = ds.Tables[3].Rows[0]["收入"].ToString();
                        textBoxX49.Text = ds.Tables[3].Rows[0]["发票号码"].ToString();
                        textBoxX50.Text = ds.Tables[3].Rows[0]["开票金额"].ToString();
                        textBoxX54.Text = ds.Tables[3].Rows[0]["账户"].ToString();
                        textBoxX55.Text = ds.Tables[3].Rows[0]["项目名称"].ToString();
                        textBoxX56.Text = ds.Tables[3].Rows[0]["往来单位"].ToString();
                        textBoxX15.Text = ds.Tables[3].Rows[0]["开票日期"].ToString();
                        textBoxX13.Text = ds.Tables[3].Rows[0]["摘要明细"].ToString();
                        //}
                    }
                }


                if (MainForm.str_khzl != "1")
                {

                    tabItem3.Visible = false;
                    //tabItem4.Visible = false;
                }
                if (MainForm.str_sfjs != "1")
                {

                    //tabItem3.Visible = false;
                    tabItem4.Visible = false;
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {

                        if (i == 0)
                        {
                            if (ds.Tables[7].Rows[i]["修品外观"].ToString() != "")
                            {
                                byte[] b2 = (byte[])ds.Tables[7].Rows[i]["修品外观"];
                                if (b2.Length > 0)
                                {
                                    //string temp2 = Encoding.Default.GetString(b2);
                                    MemoryStream m = new MemoryStream(b2);
                                    Bitmap pic = new Bitmap(m);
                                    this.pictureBox1.Image = pic;
                                    this.pictureBox1.Tag = ds.Tables[7].Rows[i]["序号"].ToString();
                                }
                            }
                        }
                        else if (i == 1)
                        {
                            if (ds.Tables[7].Rows[i]["修品外观"].ToString() != "")
                            {
                                byte[] b2 = (byte[])ds.Tables[7].Rows[i]["修品外观"];
                                if (b2.Length > 0)
                                {
                                    // string temp = Encoding.Default.GetString(b2);
                                    MemoryStream m = new MemoryStream(b2);
                                    Bitmap pic = new Bitmap(m);
                                    this.pictureBox2.Image = pic;
                                    this.pictureBox2.Tag = ds.Tables[7].Rows[i]["序号"].ToString();
                                }
                            }
                        }
                        else if (i == 2)
                        {
                            if (ds.Tables[7].Rows[i]["修品外观"].ToString() != "")
                            {
                                byte[] b2 = (byte[])ds.Tables[7].Rows[i]["修品外观"];
                                if (b2.Length > 0)
                                {
                                    //string temp = Encoding.Default.GetString(b2);
                                    MemoryStream m = new MemoryStream(b2);
                                    Bitmap pic = new Bitmap(m);
                                    this.pictureBox3.Image = pic;
                                    this.pictureBox3.Tag = ds.Tables[7].Rows[i]["序号"].ToString();
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            Form_XXXG frm = new Form_XXXG(wxbh);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Queue_data();
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (gzbz != "0")
            {
                Form_FPXG frm = new Form_FPXG(wxbh, gzbz);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Queue_data();
                }
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Form_KHXG frm = new Form_KHXG();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string temp = frm.khbh;
                try
                {

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                    //decimal temp = Convert.ToDecimal(str2);
                    string str1 = "update J_维修处理表 set 客户编号='" + temp + "'  where 维修编号='" + wxbh + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                    SQL3.Dispose();

                }
                catch
                {

                }
                con.Close();
                Queue_data();
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {

                openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                openFileDialog1.DefaultExt = "JPG";
                //openFileDialog1.ShowDialog();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.pictureBox1.Image = new Bitmap(this.openFileDialog1.FileName);
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (this.pictureBox1.Tag != null)
                        {
                            string temp = this.pictureBox1.Tag.ToString();
                            string str = "select * from J_维修外观表 where  序号='" + temp + "'";

                            SqlDataAdapter da = new SqlDataAdapter(str, con);
                            SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            MemoryStream ms = new MemoryStream();
                            pictureBox1.Image.Save("a.bmp");
                            FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
                            BinaryReader binaryReader = new BinaryReader(fileStream);
                            byte[] b= binaryReader.ReadBytes((int)fileStream.Length);
                            binaryReader.Close();
                            fileStream.Close();
                            File.Delete("a.bmp");
                            dt.Rows[0]["修品外观"] = b;
                            da.Update(dt);
                        }
                        else
                        {


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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    con.Close();
                    Queue_data();
                }

            }
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {

                openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                openFileDialog1.DefaultExt = "JPG";
                //openFileDialog1.ShowDialog();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.pictureBox3.Image = new Bitmap(this.openFileDialog1.FileName);
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (this.pictureBox3.Tag != null)
                        {
                            string temp = this.pictureBox3.Tag.ToString();
                            string str = "select * from J_维修外观表 where  序号='" + temp + "'";

                            SqlDataAdapter da = new SqlDataAdapter(str, con);
                            SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            MemoryStream ms = new MemoryStream();
                            pictureBox3.Image.Save("a.bmp");
                            FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
                            BinaryReader binaryReader = new BinaryReader(fileStream);
                            byte[]  b = binaryReader.ReadBytes((int)fileStream.Length);
                            binaryReader.Close();
                            fileStream.Close();
                            File.Delete("a.bmp");
                            dt.Rows[0]["修品外观"] = b;
                            da.Update(dt);
                        }
                        else
                        {


                            MemoryStream ms = new MemoryStream();
                            pictureBox3.Image.Save("a1.bmp");
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    con.Close();
                    Queue_data();
                }

            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {

                openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                openFileDialog1.DefaultExt = "JPG";
                //openFileDialog1.ShowDialog();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.pictureBox2.Image = new Bitmap(this.openFileDialog1.FileName);
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        if (this.pictureBox2.Tag != null)
                        {
                            string temp = this.pictureBox2.Tag.ToString();
                            string str = "select * from J_维修外观表 where  序号='" + temp + "'";

                            SqlDataAdapter da = new SqlDataAdapter(str, con);
                            SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            MemoryStream ms = new MemoryStream();
                            pictureBox2.Image.Save("a.bmp");
                            FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
                            BinaryReader binaryReader = new BinaryReader(fileStream);
                            byte[] b = binaryReader.ReadBytes((int)fileStream.Length);
                            binaryReader.Close();
                            fileStream.Close();
                            File.Delete("a.bmp");
                            dt.Rows[0]["修品外观"] = b;
                            da.Update(dt);
                        }
                        else
                        {

                            MemoryStream ms = new MemoryStream();
                            pictureBox2.Image.Save("a1.bmp");
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    con.Close();
                    Queue_data();
                }

            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {

            //row = this.dataGridViewX1.SelectedRows[0];
            //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修报价"].Value.ToString();
            if (textBoxX39.Text != "")
            {
                //MessageBox.Show("您已报价，是否修改？");
                if (MessageBox.Show("您已报价，是否修改？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Form_BJXG frm = new Form_BJXG("XG",wxbh);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //row.Dispose();
                        Queue_data();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                Form_BJXG frm2 = new Form_BJXG("BJ",wxbh);
                if (frm2.ShowDialog() == DialogResult.OK)
                {
                    Queue_data();
                }
            }

        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            Report.LoadFromFile(@"报价单.grf");
            //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //    @"User ID=Admin;Data Source=C:\Grid++Report 5.0\\Samples\Data\Northwind.mdb";
            ReportInitialize();
            Report.PrintPreview(true);
        }
        private void ReportInitialize()
        {
            //if (this.dataGridViewX1.SelectedRows.Count > 0)
            //{

                //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();

                try
                {

                    //if (con.State == ConnectionState.Closed)
                    //    con.Open();
                    string str = "SELECT " +
                                "b.[客户名称],b.[联系人],b.[联系电话],a.[维修编号],c.[检测维修费用],c.[材料费用],c.[现场费用],c.[运输费用],c.[管理费用],c.[小计费用],c.[税金],c.[总计费用],a.[修品大类],a.[修品小类]," +
                                "a.[故障描述],a.[修品SN1],a.[修品PN号],c.[报价时间],dbo.[J_公司信息].[总负责人],dbo.[J_公司信息].[公司名称],dbo.[J_公司信息].[服务电话],dbo.[J_公司信息].[报价合同],dbo.[J_公司信息].[移动电话]," +
                                "a.[修品型号],a.[规格参数],a.[修品品牌]" +
                                " from [J_维修处理表] a LEFT OUTER JOIN [J_客户信息] b on a.[客户编号]=b.[客户编号] LEFT OUTER JOIN [J_报价详细表] c on a.[维修编号]=c.[维修编号],J_公司信息" +
                                " WHERE a.维修编号='" + wxbh + "'";
                    //SqlDataAdapter da = new SqlDataAdapter(str, con);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    //GridppReportDemo.Utility.FillRecordToReport(Report, dt);
                    //Report.ConnectionString = @"Provider=SQLNCLI.1;" + MainForm.connetstring;//+ ";Provider=SQLOLEDB.1;Persist Security Info=True;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=IIXYBDKBL3N4HVU;Use Encryption for Data=False;Tag with column collation when possible=False";
                    //Report.ConnectionString = "Provider=SQLOLEDB.1;Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=wxxt;Data Source=192.168.0.103;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=IIXYBDKBL3N4HVU;Use Encryption for Data=False;Tag with column collation when possible=False";
                    Report.ConnectionString = "Provider=SQLOLEDB.1;Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=wxxt;Data Source=192.168.0.150,1433\\SERVER-PC\\SQLEXPRESS;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=SERVER-PC;Use Encryption for Data=False;Tag with column collation when possible=False";
                    Report.QuerySQL = str;
                    Report.Title = wxbh;
                    //Report.DetailGrid.Recordset.ConnectionString = MainForm.connetstring;SQLOLEDB.3;
                    //Report.DetailGrid.Recordset.QuerySQL = str;Provider=SQLNCLI10.1
                }
                catch
                {
                }
                //con.Close();

            //}
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {

                DATA_upload frm = new DATA_upload(wxbh);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Queue_data();
                }
           
        }

        private void buttonX10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data2();
                Queue_data();
            }
        }
        private void delete_data2()
        {
            if (this.dataGridViewX2.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX2.SelectedRows[0].Cells["序号"].Value.ToString();

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_维修资料库 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();


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

        private void tabControl1_Click(object sender, EventArgs e)
        {

        }

    }
}