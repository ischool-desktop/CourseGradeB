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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblYearAndSemester = new DevComponents.DotNetBar.LabelX();
            this.lblClassAndName = new DevComponents.DotNetBar.LabelX();
            this.gpScore = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.colSubjectName = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colSubjectType = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colDomain = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.gpScore.Size = new System.Drawing.Size(859, 377);
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
            this.colDomain,
            this.colCredit,
            this.colScore,
            this.colLevel});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgv.Location = new System.Drawing.Point(4, 4);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(844, 338);
            this.dgv.TabIndex = 0;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(797, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // colSubjectName
            // 
            this.colSubjectName.DisplayMember = "Text";
            this.colSubjectName.DropDownHeight = 106;
            this.colSubjectName.DropDownWidth = 121;
            this.colSubjectName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colSubjectName.HeaderText = "科目";
            this.colSubjectName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colSubjectName.ItemHeight = 17;
            this.colSubjectName.Name = "colSubjectName";
            this.colSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSubjectName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colSubjectName.Width = 200;
            // 
            // colSubjectType
            // 
            this.colSubjectType.DisplayMember = "Text";
            this.colSubjectType.DropDownHeight = 106;
            this.colSubjectType.DropDownWidth = 121;
            this.colSubjectType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colSubjectType.HeaderText = "科目類別";
            this.colSubjectType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colSubjectType.ItemHeight = 17;
            this.colSubjectType.Name = "colSubjectType";
            this.colSubjectType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSubjectType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colSubjectType.Width = 140;
            // 
            // colDomain
            // 
            this.colDomain.DisplayMember = "Text";
            this.colDomain.DropDownHeight = 106;
            this.colDomain.DropDownWidth = 121;
            this.colDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colDomain.HeaderText = "群組";
            this.colDomain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colDomain.ItemHeight = 17;
            this.colDomain.Name = "colDomain";
            this.colDomain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDomain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colDomain.Width = 200;
            // 
            // colCredit
            // 
            this.colCredit.HeaderText = "節數/權數";
            this.colCredit.Name = "colCredit";
            // 
            // colScore
            // 
            this.colScore.HeaderText = "成績";
            this.colScore.Name = "colScore";
            this.colScore.Width = 80;
            // 
            // colLevel
            // 
            this.colLevel.HeaderText = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.Width = 80;
            // 
            // SemesterScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 477);
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
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colSubjectName;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colSubjectType;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
    }
}