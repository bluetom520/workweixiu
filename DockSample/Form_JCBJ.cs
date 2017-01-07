using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.SqlClient;
using grproLib;
namespace DockSample
{
    public partial class Form_JCBJ : DockContent
    {
        SqlConnection con;
        public static DataGridViewRow row;
        public static int hwnd = 0;
        private GridppReport Report = new GridppReport();
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_JCBJ()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JCBJ_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 维修编号,接修日期,修品大类,修品小类,修品型号,修品品牌,修品SN1,规格参数,故障描述,外观,优先级,预付款,维修报价,客户编号,业务员,预约日期 from J_维修处理表 where  接修日期 between '" + d1 + "' and '" + d2 + "' and 当前状态=0";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["接修日期"].DefaultCellStyle.Format = "yyyy-MM-dd";
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["入库编号"].Width = 110;
                //this.dataGridViewX1.Columns["配件类别"].Width = 70;
                //this.dataGridViewX1.Columns["配件编号"].Width = 70;
                //this.dataGridViewX1.Columns["存储位置"].Width = 100;
                //this.dataGridViewX1.Columns["规格型号"].Width = 100;
                //this.dataGridViewX1.Columns["入库日期"].Width = 110;
                //this.dataGridViewX1.Columns["入库数量"].Width = 60;
                //this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["购买单价"].Width = 60;
                //this.dataGridViewX1.Columns["销售单价"].Width = 60;
                //this.dataGridViewX1.Columns["供货商家"].Width = 100;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;

                //如果父窗体已注册了自定义事件
                if (DataArrivalEvent != null)
                {
                    DataArrivalEvent(dt.Rows.Count.ToString());
                }
            }
            catch
            {
            }
            con.Close();
        }
        

        private void buttonX1_Click(object sender, EventArgs e)
        {
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void 报价ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                row = this.dataGridViewX1.SelectedRows[0];
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修报价"].Value.ToString();
                if (str2 != "")
                {
                    //MessageBox.Show("您已报价，是否修改？");
                    if (MessageBox.Show("您已报价，是否修改？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Form_BJ frm = new Form_BJ("XG");
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
                    Form_BJ frm2 = new Form_BJ("BJ");
                    if (frm2.ShowDialog() == DialogResult.OK)
                    {
                        Queue_data();
                    }
                }
            }

        }

        private void Form_JCBJ_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_XXXX frm = new Form_XXXX(wxbh);
                frm.ShowDialog();
            }
        }

        private void 转入派工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                try
                {
                    string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修报价"].Value.ToString();
                    if (str2 == "")
                    {
                        MessageBox.Show("您还没有报价！");
                        return;
                    }
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                     str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                    string str1 = "update J_维修处理表 set 当前状态=1  where 维修编号='" + str2 + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                    SQL3.Dispose();

                }
                catch
                {

                }
                con.Close();
            }
            Queue_data();
        }

        private void 打印报价单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //载入报表模板文件，必须保证 Grid++Report 的安装目录在‘C:\Grid++Report 5.0’下，
                //关于动态设置报表路径与数据绑定参数请参考其它例子程序
                Report.LoadFromFile(@"报价单.grf");
                //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                //    @"User ID=Admin;Data Source=C:\Grid++Report 5.0\\Samples\Data\Northwind.mdb";
                ReportInitialize();
                Report.PrintPreview(true);
            }
        }

        private void ReportInitialize()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {

                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();

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

            }
        }

        private void 预付款ToolStripMenuItem_Click(object sender, EventArgs e)//需要测试
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["预付款"].Value.ToString();
                string str3 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                if (str2 != "")
                {
                    if (Convert.ToDecimal(str2) > 0)
                    {
                        MessageBox.Show("已预付款！");
                        return;
                    }
                    else
                    {
                        Form_XJSZ frm = new Form_XJSZ(str3);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            str2 = frm.DJ;
                            try
                            {

                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                //str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                                decimal temp = Convert.ToDecimal(str2);
                                string str1 = "update J_维修处理表 set 预付款=" + temp + "  where 维修编号='" + str3 + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                                SQL3.Dispose();

                            }
                            catch
                            {

                            }
                            con.Close();
                        }
                    Queue_data();

                }
 


                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report.LoadFromFile(@"报价单.grf");
            DesignForm theForm = new DesignForm();
            theForm.AttachReport(Report);
            theForm.ShowDialog();
        }
    }
}
