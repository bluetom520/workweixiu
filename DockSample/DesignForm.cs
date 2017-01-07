using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using grproLib;
using System.IO;
namespace DockSample
{
	/// <summary>
	/// DesignForm 的摘要说明。
	/// </summary>
	public class DesignForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolTip toolTip1;
		private AxgrdesLib.AxGRDesigner axGRDesigner1;
        private SaveFileDialog saveFileDialog1;
		private System.ComponentModel.IContainer components;
        GridppReport Report2;
		public DesignForm()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.axGRDesigner1 = new AxgrdesLib.AxGRDesigner();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.axGRDesigner1)).BeginInit();
            this.SuspendLayout();
            // 
            // axGRDesigner1
            // 
            this.axGRDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axGRDesigner1.Enabled = true;
            this.axGRDesigner1.Location = new System.Drawing.Point(0, 0);
            this.axGRDesigner1.Name = "axGRDesigner1";
            this.axGRDesigner1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGRDesigner1.OcxState")));
            this.axGRDesigner1.Size = new System.Drawing.Size(640, 446);
            this.axGRDesigner1.TabIndex = 0;
            this.axGRDesigner1.SaveReport += new System.EventHandler(this.axGRDesigner1_SaveReport);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.InitialDirectory = "Directory.GetCurrentDirectory()";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // DesignForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(640, 446);
            this.Controls.Add(this.axGRDesigner1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DesignForm";
            this.Text = "设计报表";
            this.Load += new System.EventHandler(this.DesignForm_Load);
            this.Closed += new System.EventHandler(this.DesignForm_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.axGRDesigner1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		public void AttachReport(GridppReport Report)
		{
			//设定查询显示器关联的报表
			axGRDesigner1.Report = Report;
            Report2 = Report;
		}

		private void DesignForm_Closed(object sender, System.EventArgs e)
		{
			if (axGRDesigner1.Dirty)
				axGRDesigner1.Post();		
		}

        private void axGRDesigner1_SaveReport(object sender, EventArgs e)
        {

            //bool ToSave = true;
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            ////saveFileDialog1.FileName = openFileDialog1.FileName;
            //if (saveFileDialog1.FileName == "")
            //    ToSave = saveFileDialog1.ShowDialog() == DialogResult.OK;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            { axGRDesigner1.Post();
            Report2.SaveToFile(saveFileDialog1.FileName);
            }
            //if (ToSave)
            //{
            //    axGRDesigner1.Post();
            //    Report.(saveFileDialog1.FileName);
            //}
            
            //将 DefaultAction 属性为假, 忽略掉设计器控件本身的保存行为
            axGRDesigner1.DefaultAction = false;
        }

        private void DesignForm_Load(object sender, EventArgs e)
        {

        }
	}
}
