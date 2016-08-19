namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    partial class SubjectConductStatusForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSGradeYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExam1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExam2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboFilter = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(6, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "學    年    度";
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(6, 457);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(138, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "匯出到 Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Location = new System.Drawing.Point(680, 457);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "關閉";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.BackColor = System.Drawing.Color.Transparent;
            this.btnQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuery.Location = new System.Drawing.Point(665, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查詢";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCourse,
            this.colClass,
            this.colSGradeYear,
            this.colTeacher,
            this.colExam1,
            this.colExam2});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgv.Location = new System.Drawing.Point(6, 38);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(749, 413);
            this.dgv.TabIndex = 7;
            // 
            // colCourse
            // 
            this.colCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCourse.HeaderText = "課程名稱";
            this.colCourse.Name = "colCourse";
            this.colCourse.ReadOnly = true;
            // 
            // colClass
            // 
            this.colClass.HeaderText = "開課年級";
            this.colClass.Name = "colClass";
            // 
            // colSGradeYear
            // 
            this.colSGradeYear.HeaderText = "學生年級";
            this.colSGradeYear.Name = "colSGradeYear";
            // 
            // colTeacher
            // 
            this.colTeacher.HeaderText = "授課老師";
            this.colTeacher.Name = "colTeacher";
            this.colTeacher.ReadOnly = true;
            this.colTeacher.Width = 155;
            // 
            // colExam1
            // 
            this.colExam1.HeaderText = "Midterm";
            this.colExam1.Name = "colExam1";
            this.colExam1.ReadOnly = true;
            // 
            // colExam2
            // 
            this.colExam2.HeaderText = "Final";
            this.colExam2.Name = "colExam2";
            this.colExam2.ReadOnly = true;
            // 
            // cboFilter
            // 
            this.cboFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFilter.DisplayMember = "Text";
            this.cboFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.FormattingEnabled = true;
            this.cboFilter.ItemHeight = 19;
            this.cboFilter.Items.AddRange(new object[] {
            this.comboItem4,
            this.comboItem5,
            this.comboItem6});
            this.cboFilter.Location = new System.Drawing.Point(467, 7);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(195, 25);
            this.cboFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboFilter.TabIndex = 11;
            this.cboFilter.Visible = false;
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "顯示全部";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "僅顯示Midterm未完成";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "僅顯示Final未完成";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "顯示全部";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "僅顯示Midterm未完成";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "僅顯示Final未完成";
            // 
            // labelX3
            // 
            this.labelX3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(412, 9);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(47, 21);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "篩　選";
            this.labelX3.Visible = false;
            // 
            // labelX4
            // 
            this.labelX4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(150, 459);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(208, 21);
            this.labelX4.TabIndex = 12;
            this.labelX4.Text = "僅統計1-6年級課程/修課學生資料";
            // 
            // SubjectConductStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 486);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(520, 300);
            this.Name = "SubjectConductStatusForm";
            this.Text = "Conduct輸入狀況(授課老師)";
            this.Load += new System.EventHandler(this.SubjectConductStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private DevComponents.DotNetBar.ButtonX btnQuery;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgv;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboFilter;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSGradeYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExam1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExam2;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}