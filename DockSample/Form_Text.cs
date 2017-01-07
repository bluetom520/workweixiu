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
    public partial class Form_Text : Office2007Form
    {
        private string str;
        public string str_temp
        {
            get { return str; }
            set { str = value; }
        }
        public Form_Text()
        {
            InitializeComponent();
        }

        private void Form_Text_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            str = textBoxX1.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
