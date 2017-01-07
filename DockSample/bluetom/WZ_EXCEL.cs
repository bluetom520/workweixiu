using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;
using System.Media;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Collections;

namespace DockSample
{
    public partial class WZ_EXCEL : Form
    {
        SqlConnection con_1 = new SqlConnection("server=102.12.65.200;database=dn2005;uid=bluetom;pwd=jinbo112358");
        DirectoryInfo Dinfo;
        string D_path;
        public WZ_EXCEL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            queue_data();
        }
        private void queue_data()
        {
            string year1 = this.dateTimePicker1.Text;
            string year2 = this.dateTimePicker2.Text;
            D_path = DateTime.Now.ToString("yyyyMMdd");
            Dinfo = new DirectoryInfo(D_path);
            Dinfo.Create();
            try
            {
                con_1.Open();
                string str;

             

                SqlDataAdapter da = new SqlDataAdapter(str, con_1);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Table_ToExcel t_Excel = new Table_ToExcel();
                string tempPath = Directory.GetCurrentDirectory() + "\\" +D_path+"\\"+ dateTimePicker1.Value.Month.ToString()+"月";
                string data_bb;
                if (radioButton3.Checked)
                {
                    data_bb = "2003";
                }
                else
                {
                    data_bb = "2007";
                }

                t_Excel.u_DataTableToExcel2(dt, tempPath, dt.Rows.Count, data_bb);



            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            finally
            {
              
                con_1.Close();
                //label5.Text = this.dataGridView1.Rows.Count.ToString();

            }
   
  
        }


    }
}
