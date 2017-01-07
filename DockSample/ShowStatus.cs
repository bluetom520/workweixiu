using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace DockSample
{
    public partial class ShowStatus : DockContent
    {

        SqlConnection con;
        public string warn_message = "";
        int num = 0;
        public static int hwnd = 0;
        public ShowStatus()
        {
            InitializeComponent();

        }

		private string m_fileName = string.Empty;

        public string FileName
        {
            get { return m_fileName; }
            set
            {
                if (value != string.Empty)
                {
                    Stream s = new FileStream(value, FileMode.Open);

                    FileInfo efInfo = new FileInfo(value);

                    string fext = efInfo.Extension.ToUpper();

                    //if (fext.Equals(".RTF"))
                    //richTextBox1.LoadFile(s, RichTextBoxStreamType.RichText);
                    //else
                    //richTextBox1.LoadFile(s, RichTextBoxStreamType.PlainText);
                    s.Close();
                }

                m_fileName = value;
                this.ToolTipText = value;
            }
        }

		// workaround of RichTextbox control's bug:
		// If load file before the control showed, all the text format will be lost
		// re-load the file after it get showed.
		private bool m_resetText = true;
        
		protected override void OnPaint(PaintEventArgs e)
		{
            base.OnPaint(e);

            if (m_resetText)
            {
                m_resetText = false;
                FileName = FileName;
            }
		}

        protected override string GetPersistString()
        {

            return GetType().ToString() + "," + FileName + "," + Text;
        }

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
		}


        private void DummyDoc_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            con = new SqlConnection(MainForm.connetstring);
            timer1.Start();
        }

        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {

        }


        private void menuItem3_Click(object sender, EventArgs e)
        {



        }

        private void menuItem4_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 6000;
            scan_Bulletin();
            if (warn_message != "")
            {
                richTextBox1.Text = warn_message;
                splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }


            //textBox1.Width = Encoding.Default.GetBytes(textBox1.Text).Length * 16;
            if (richTextBox1.Text.Length % 11 == 0)
            {
                richTextBox1.Height = (richTextBox1.Text.Length / 11 + num-1 ) * 28;
            }
            else
            {
                richTextBox1.Height = (richTextBox1.Text.Length / 11 + num) * 28;
            }
        }
        private void scan_Bulletin()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                warn_message = "";
                string str = "select * from J_公告表 where 目的单位 in('全部','"+LoginXT.user_danwei+"') and 无效标志=0";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt1 = new DataTable();

                da.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    int j = 1;

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        DateTime d1 = Convert.ToDateTime(Convert.ToDateTime(dt1.Rows[i]["开始日期"].ToString()).ToShortDateString());
                        DateTime d2 = Convert.ToDateTime(Convert.ToDateTime(dt1.Rows[i]["结束日期"].ToString()).ToShortDateString());
                        DateTime d3 = DateTime.Now;
                        if (d3 >= d1 && d3 <= d2)
                        {
                            warn_message += (j).ToString() + "、" + dt1.Rows[i]["提醒名称"].ToString() + "：" + dt1.Rows[i]["提醒内容"].ToString() + "\n";
                            j++;
                        }


                    }
                    num = j - 1;
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

                con.Close();
                //label2.Text = warn_message;


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = 40;
            richTextBox1.Top -= 2;
            if (richTextBox1.Bottom < 0)
            {
                richTextBox1.Top = panel1.Height;
            }
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            timer2.Stop();
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void ShowStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

    }
}