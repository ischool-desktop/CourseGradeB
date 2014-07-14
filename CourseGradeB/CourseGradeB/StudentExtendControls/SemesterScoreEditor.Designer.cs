namespace CourseGradeB.StudentExtendControls
{
    partial class SemesterScoreEditor
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
            this.lblYearAndSemester = new DevComponents.DotNetBar.LabelX();
            this.lblClassAndName = new DevComponents.DotNetBar.LabelX();
            this.gpScore = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colSubjectName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSubjectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.gpScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // lblYearAndSemester
            // 
            this.lblYearAndSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblYearAndSemester.BackgroundStyle.Class = "";
            this.lblYearAndSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblYearAndSemester.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblYearAndSemester.Location = new System.Drawing.Point(13, 13);
            this.lblYearAndSemester.Name = "lblYearAndSemester";
            this.lblYearAndSemester.Size = new System.Drawing.Size(344, 34);
            this.lblYearAndSemester.TabIndex = 0;
            this.lblYearAndSemester.Text = "學年度學期";
            // 
            // lblClassAndName
            // 
            this.lblClassAndName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblClassAndName.BackgroundStyle.Class = "";
            this.lblClassAndName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblClassAndName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblClassAndName.Location = new System.Drawing.Point(494, 24);
            this.lblClassAndName.Name = "lblClassAndName";
            this.lblClassAndName.Size = new System.Drawing.Size(298, 23);
            this.lblClassAndName.TabIndex = 1;
            this.lblClassAndName.Text = "班級姓名";
            this.lblClassAndName.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // gpScore
            // 
            this.gpScore.BackColor = System.Drawing.Color.Transparent;
            this.gpScore.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpScore.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpScore.Controls.Add(this.dgv);
            this.gpScore.Location = new System.Drawing.Point(13, 54);
            this.gpScore.Name = "gpScore";
            this.gpScore.Size = new System.Drawing.Size(779, 337);
            // 
            // 
            // 
            this.gpScore.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpScore.Style.BackColorGradientAngle = 90;
            this.gpScore.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpScore.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpScore.Style.BorderBottomWidth = 1;
            this.gpScore.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpScore.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpScore.Style.BorderLeftWidth = 1;
            this.gpScore.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpScore.Style.BorderRightWidth = 1;
            this.gpScore.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpScore.Style.BorderTopWidth = 1;
            this.gpScore.Style.Class = "";
            this.gpScore.Style.CornerDiameter = 4;
            this.gpScore.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpScore.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpScore.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gpScore.StyleMouseDown.Class = "";
            this.gpScore.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpScore.StyleMouseOver.Class = "";
            this.gpScore.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpScore.TabIndex = 2;
            this.gpScore.Text = "學期科目成績";
            // 
            // dgv
            // 
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSubjectName,
            this.colSubjectType,
            this.colCredit,
            this.colScore});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgv.Location = new System.Drawing.Point(4, 4);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(766, 303);
            this.dgv.TabIndex = 0;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);
            // 
            // colSubjectName
            // 
            this.colSubjectName.HeaderText = "科目";
            this.colSubjectName.Name = "colSubjectName";
            this.colSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSubjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSubjectName.Width = 150;
            // 
            // colSubjectType
            // 
            this.colSubjectType.HeaderText = "科目類別";
            this.colSubjectType.Name = "colSubjectType";
            this.colSubjectType.ReadOnly = true;
            this.colSubjectType.Width = 150;
            // 
            // colCredit
            // 
            this.colCredit.HeaderText = "節數/權數";
            this.colCredit.Name = "colCredit";
            this.colCredit.Width = 150;
            // 
            // colScore
            // 
            this.colScore.HeaderText = "成績";
            this.colScore.Name = "colScore";
            this.colScore.Width = 150;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(717, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SemesterScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 432);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpScore);
            this.Controls.Add(this.lblClassAndName);
            this.Controls.Add(this.lblYearAndSemester);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SemesterScoreEditor";
            this.Text = "學期成績修改";
            this.Load += new System.EventHandler(this.SemesterScoreEditor_Load);
            this.gpScore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblYearAndSemester;
        private DevComponents.DotNetBar.LabelX lblClassAndName;
        private DevComponents.DotNetBar.Controls.GroupPanel gpScore;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgv;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubjectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
    }
}