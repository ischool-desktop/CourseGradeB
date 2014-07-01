namespace CourseGradeB.StuAdminExtendControls
{
    partial class SubjectConductSettingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.itemPanle1 = new DevComponents.DotNetBar.ItemPanel();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // itemPanle1
            // 
            this.itemPanle1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.itemPanle1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanle1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanle1.ContainerControlProcessDialogKey = true;
            this.itemPanle1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanle1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemPanle1.Location = new System.Drawing.Point(13, 13);
            this.itemPanle1.Name = "itemPanle1";
            this.itemPanle1.Size = new System.Drawing.Size(173, 359);
            this.itemPanle1.TabIndex = 0;
            this.itemPanle1.Text = "itemPanel1";
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(750, 349);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroup,
            this.colTitle});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgv.Location = new System.Drawing.Point(193, 13);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(632, 330);
            this.dgv.TabIndex = 4;
            // 
            // colGroup
            // 
            this.colGroup.HeaderText = "Group";
            this.colGroup.Name = "colGroup";
            // 
            // colTitle
            // 
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            // 
            // SubjectConductSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 377);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.itemPanle1);
            this.DoubleBuffered = true;
            this.Name = "SubjectConductSettingForm";
            this.Text = "科目補充設定";
            this.Load += new System.EventHandler(this.SubjectConductSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanle1;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
    }
}