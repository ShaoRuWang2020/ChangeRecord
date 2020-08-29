namespace CS_Note1
{
    partial class EditVersion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GridView_VersionList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BTNSave = new System.Windows.Forms.Button();
            this.BTNCancel = new System.Windows.Forms.Button();
            this.LabelTag = new System.Windows.Forms.Label();
            this.VersionEditBox = new IP_Format.UserControl1();
            this.ChkBox_BackupNewVersionInsp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridView_VersionList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridView_VersionList
            // 
            this.GridView_VersionList.AllowUserToAddRows = false;
            this.GridView_VersionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView_VersionList.Location = new System.Drawing.Point(16, 21);
            this.GridView_VersionList.Name = "GridView_VersionList";
            this.GridView_VersionList.RowTemplate.Height = 24;
            this.GridView_VersionList.Size = new System.Drawing.Size(559, 176);
            this.GridView_VersionList.TabIndex = 6;
            this.GridView_VersionList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_VersionList_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GridView_VersionList);
            this.groupBox1.Location = new System.Drawing.Point(74, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 218);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // BTNSave
            // 
            this.BTNSave.Location = new System.Drawing.Point(90, 354);
            this.BTNSave.Name = "BTNSave";
            this.BTNSave.Size = new System.Drawing.Size(98, 56);
            this.BTNSave.TabIndex = 8;
            this.BTNSave.Text = "Save";
            this.BTNSave.UseVisualStyleBackColor = true;
            this.BTNSave.Click += new System.EventHandler(this.BTNSave_Click);
            // 
            // BTNCancel
            // 
            this.BTNCancel.Location = new System.Drawing.Point(260, 354);
            this.BTNCancel.Name = "BTNCancel";
            this.BTNCancel.Size = new System.Drawing.Size(109, 56);
            this.BTNCancel.TabIndex = 9;
            this.BTNCancel.Text = "Cancel";
            this.BTNCancel.UseVisualStyleBackColor = true;
            this.BTNCancel.Click += new System.EventHandler(this.BTNCancel_Click);
            // 
            // LabelTag
            // 
            this.LabelTag.AutoSize = true;
            this.LabelTag.Location = new System.Drawing.Point(71, 256);
            this.LabelTag.Name = "LabelTag";
            this.LabelTag.Size = new System.Drawing.Size(46, 17);
            this.LabelTag.TabIndex = 10;
            this.LabelTag.Text = "label1";
            // 
            // VersionEditBox
            // 
            this.VersionEditBox.Location = new System.Drawing.Point(74, 282);
            this.VersionEditBox.Name = "VersionEditBox";
            this.VersionEditBox.Size = new System.Drawing.Size(211, 50);
            this.VersionEditBox.TabIndex = 11;
            // 
            // ChkBox_BackupNewVersionInsp
            // 
            this.ChkBox_BackupNewVersionInsp.AutoSize = true;
            this.ChkBox_BackupNewVersionInsp.Location = new System.Drawing.Point(315, 311);
            this.ChkBox_BackupNewVersionInsp.Name = "ChkBox_BackupNewVersionInsp";
            this.ChkBox_BackupNewVersionInsp.Size = new System.Drawing.Size(87, 21);
            this.ChkBox_BackupNewVersionInsp.TabIndex = 12;
            this.ChkBox_BackupNewVersionInsp.Text = "IsBackup";
            this.ChkBox_BackupNewVersionInsp.UseVisualStyleBackColor = true;
            // 
            // EditVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 441);
            this.Controls.Add(this.ChkBox_BackupNewVersionInsp);
            this.Controls.Add(this.VersionEditBox);
            this.Controls.Add(this.LabelTag);
            this.Controls.Add(this.BTNCancel);
            this.Controls.Add(this.BTNSave);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditVersion";
            this.Text = "EditVersion";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditVersion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridView_VersionList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView GridView_VersionList;
        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BTNCancel;
        private System.Windows.Forms.Label LabelTag;
        public IP_Format.UserControl1 VersionEditBox;
        public System.Windows.Forms.CheckBox ChkBox_BackupNewVersionInsp;
        public System.Windows.Forms.Button BTNSave;
    }
}