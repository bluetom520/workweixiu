using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace DockSample
{
    public partial class Form_LCT : DockContent
    {
        public static int hwnd = 0;

        public Form_LCT()
        {
            InitializeComponent();
        }

        private void Form_LCT_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
        }

        private void Form_LCT_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }
    }
}
