namespace CS_Note1
{
    partial class CreateVersion
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
            this.Group_InspName = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BTNAddVision = new System.Windows.Forms.Button();
            this.BTNDelVersion = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Group_InspName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Group_InspName
            // 
            this.Group_InspName.Controls.Add(this.button2);
            this.Group_InspName.Controls.Add(this.button1);
            this.Group_InspName.Controls.Add(this.BTNAddVision);
            this.Group_InspName.Controls.Add(this.BTNDelVersion);
            this.Group_InspName.Controls.Add(this.dataGridView1);
            this.Group_InspName.Location = new System.Drawing.Point(12, 23);
            this.Group_InspName.Name = "Group_InspName";
            this.Group_InspName.Size = new System.Drawing.Size(660, 340);
            this.Group_InspName.TabIndex = 0;
            this.Group_InspName.TabStop = false;
            this.Group_InspName.Text = "groupBox1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(480, 276);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 49);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 49);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BTNAddVision
            // 
            this.BTNAddVision.Location = new System.Drawing.Point(96, 276);
            this.BTNAddVision.Name = "BTNAddVision";
            this.BTNAddVision.Size = new System.Drawing.Size(94, 49);
            this.BTNAddVision.TabIndex = 1;
            this.BTNAddVision.Text = "Add";
            this.BTNAddVision.UseVisualStyleBackColor = true;
            this.BTNAddVision.Click += new System.EventHandler(this.BTNAddVision_Click);
            // 
            // BTNDelVersion
            // 
            this.BTNDelVersion.Location = new System.Drawing.Point(334, 276);
            this.BTNDelVersion.Name = "BTNDelVersion";
            this.BTNDelVersion.Size = new System.Drawing.Size(99, 49);
            this.BTNDelVersion.TabIndex = 2;
            this.BTNDelVersion.Text = "Del";
            this.BTNDelVersion.UseVisualStyleBackColor = true;
            this.BTNDelVersion.Click += new System.EventHandler(this.BTNDelVersion_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(34, 21);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(596, 232);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // CreateVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 384);
            this.Controls.Add(this.Group_InspName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateVersion";
            this.Text = "CreateVersion";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CreateVersion_Load);
            this.Group_InspName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Group_InspName;
        private System.Windows.Forms.Button BTNDelVersion;
        private System.Windows.Forms.Button BTNAddVision;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}