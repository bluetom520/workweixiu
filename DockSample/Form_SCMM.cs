using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
namespace DockSample
{
    public partial class Form_SCMM : Office2007Form
    {
        public Form_SCMM()
        {
            InitializeComponent();
        }

        private void Form_SCMM_Load(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (textBoxX1.Text == "111111")
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("密码错误，请重新输入！");
                textBoxX1.Text = "";
            }
        }
    }
}
