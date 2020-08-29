using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CS_Note1
{
	/// <summary>
	/// FormSave 的摘要说明。
	/// </summary>
	public class FormSave : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1; 
		private System.Windows.Forms.Button BtnYes;
		private System.Windows.Forms.Button BtnNo;
		private System.Windows.Forms.Button BtnCancel;
		public System.Windows.Forms.ListBox LBFlieName;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormSave()
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
			this.label1 = new System.Windows.Forms.Label();
			this.BtnYes = new System.Windows.Forms.Button();
			this.BtnNo = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.LBFlieName = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "是否保存对下列各项的更改(&S)？";
			// 
			// BtnYes
			// 
			this.BtnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.BtnYes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BtnYes.Location = new System.Drawing.Point(152, 184);
			this.BtnYes.Name = "BtnYes";
			this.BtnYes.Size = new System.Drawing.Size(80, 22);
			this.BtnYes.TabIndex = 2;
			this.BtnYes.Text = "是(&Y)";
			this.BtnYes.Click += new System.EventHandler(this.BtnYes_Click);
			// 
			// BtnNo
			// 
			this.BtnNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.BtnNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BtnNo.Location = new System.Drawing.Point(240, 184);
			this.BtnNo.Name = "BtnNo";
			this.BtnNo.Size = new System.Drawing.Size(72, 22);
			this.BtnNo.TabIndex = 3;
			this.BtnNo.Text = "否(&N)";
			this.BtnNo.Click += new System.EventHandler(this.BtnNo_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BtnCancel.Location = new System.Drawing.Point(320, 184);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(80, 22);
			this.BtnCancel.TabIndex = 4;
			this.BtnCancel.Text = "取消";
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// LBFlieName
			// 
			this.LBFlieName.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LBFlieName.ItemHeight = 15;
			this.LBFlieName.Location = new System.Drawing.Point(8, 24);
			this.LBFlieName.Name = "LBFlieName";
			this.LBFlieName.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.LBFlieName.Size = new System.Drawing.Size(392, 139);
			this.LBFlieName.TabIndex = 5;
			this.LBFlieName.UseTabStops = false;
			// 
			// FormSave
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(410, 216);
			this.Controls.Add(this.LBFlieName);
			this.Controls.Add(this.BtnCancel);
			this.Controls.Add(this.BtnNo);
			this.Controls.Add(this.BtnYes);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSave";
			this.Text = "NOTE Save Form";
			this.ResumeLayout(false);

		}
		#endregion

		private void BtnYes_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Yes ;
		}

		private void BtnNo_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No ;
		}

		private void BtnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel ;

		}

	
	}
}
