namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    partial class SetHoursOpeningForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvScore = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.gpScore = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.gpConduct = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dgvConduct = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMiddleBegin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMiddleEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinalBegin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinalEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScore)).BeginInit();
            this.gpScore.SuspendLayout();
            this.gpConduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConduct)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvScore
            // 
            this.dgvScore.AllowUserToAddRows = false;
            this.dgvScore.AllowUserToDeleteRows = false;
            this.dgvScore.BackgroundColor = System.Drawing.Color.White;
            this.dgvScore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScore.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGrade,
            this.colMiddleBegin,
            this.colMiddleEnd,
            this.colFinalBegin,
            this.colFinalEnd});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScore.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScore.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvScore.Location = new System.Drawing.Point(5, 3);
            this.dgvScore.Name = "dgvScore";
            this.dgvScore.RowTemplate.Height = 24;
            this.dgvScore.Size = new System.Drawing.Size(745, 144);
            this.dgvScore.TabIndex = 0;
            this.dgvScore.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScore_CellClick);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(701, 379);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gpScore
            // 
            this.gpScore.BackColor = System.Drawing.Color.Transparent;
            this.gpScore.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpScore.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpScore.Controls.Add(this.dgvScore);
            this.gpScore.Location = new System.Drawing.Point(13, 13);
            this.gpScore.Name = "gpScore";
            this.gpScore.Size = new System.Drawing.Size(763, 177);
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
            this.gpScore.Text = "成績開放設定";
            // 
            // gpConduct
            // 
            this.gpConduct.BackColor = System.Drawing.Color.Transparent;
            this.gpConduct.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpConduct.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpConduct.Controls.Add(this.dgvConduct);
            this.gpConduct.Location = new System.Drawing.Point(13, 196);
            this.gpConduct.Name = "gpConduct";
            this.gpConduct.Size = new System.Drawing.Size(763, 177);
            // 
            // 
            // 
            this.gpConduct.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpConduct.Style.BackColorGradientAngle = 90;
            this.gpConduct.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpConduct.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpConduct.Style.BorderBottomWidth = 1;
            this.gpConduct.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpConduct.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpConduct.Style.BorderLeftWidth = 1;
            this.gpConduct.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpConduct.Style.BorderRightWidth = 1;
            this.gpConduct.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpConduct.Style.BorderTopWidth = 1;
            this.gpConduct.Style.Class = "";
            this.gpConduct.Style.CornerDiameter = 4;
            this.gpConduct.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpConduct.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpConduct.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gpConduct.StyleMouseDown.Class = "";
            this.gpConduct.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gpConduct.StyleMouseOver.Class = "";
            this.gpConduct.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gpConduct.TabIndex = 3;
            this.gpConduct.Text = "指標開放設定";
            // 
            // dgvConduct
            // 
            this.dgvConduct.AllowUserToAddRows = false;
            this.dgvConduct.AllowUserToDeleteRows = false;
            this.dgvConduct.BackgroundColor = System.Drawing.Color.White;
            this.dgvConduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConduct.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvConduct.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvConduct.Location = new System.Drawing.Point(6, 3);
            this.dgvConduct.Name = "dgvConduct";
            this.dgvConduct.RowTemplate.Height = 24;
            this.dgvConduct.Size = new System.Drawing.Size(745, 144);
            this.dgvConduct.TabIndex = 1;
            this.dgvConduct.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConduct_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "年級區間";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "期中開始時間";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "期中結束時間";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "期末開始時間";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "期末結束時間";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.Width = 150;
            // 
            // colGrade
            // 
            this.colGrade.HeaderText = "年級區間";
            this.colGrade.Name = "colGrade";
            this.colGrade.ReadOnly = true;
            this.colGrade.Visible = false;
            // 
            // colMiddleBegin
            // 
            this.colMiddleBegin.HeaderText = "期中開始時間";
            this.colMiddleBegin.Name = "colMiddleBegin";
            this.colMiddleBegin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMiddleBegin.Width = 150;
            // 
            // colMiddleEnd
            // 
            this.colMiddleEnd.HeaderText = "期中結束時間";
            this.colMiddleEnd.Name = "colMiddleEnd";
            this.colMiddleEnd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMiddleEnd.Width = 150;
            // 
            // colFinalBegin
            // 
            this.colFinalBegin.HeaderText = "期末開始時間";
            this.colFinalBegin.Name = "colFinalBegin";
            this.colFinalBegin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFinalBegin.Width = 150;
            // 
            // colFinalEnd
            // 
            this.colFinalEnd.HeaderText = "期末結束時間";
            this.colFinalEnd.Name = "colFinalEnd";
            this.colFinalEnd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFinalEnd.Width = 150;
            // 
            // SetHoursOpeningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 407);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpConduct);
            this.Controls.Add(this.gpScore);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SetHoursOpeningForm";
            this.Text = "開放輸入時間";
            this.Load += new System.EventHandler(this.SetHoursOpeningForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScore)).EndInit();
            this.gpScore.ResumeLayout(false);
            this.gpConduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConduct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvScore;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.GroupPanel gpScore;
        private DevComponents.DotNetBar.Controls.GroupPanel gpConduct;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvConduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMiddleBegin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMiddleEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinalBegin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinalEnd;

    }
}