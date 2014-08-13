namespace CourseGradeB.ClassExtendControls.Ribbon
{
    partial class HrtConductInputForm
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
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.intInput = new DevComponents.Editors.IntegerInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrade = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.lblSave = new DevComponents.DotNetBar.LabelX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cboTerm = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // itemPanel1
            // 
            this.itemPanel1.AutoScroll = true;
            this.itemPanel1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemPanel1.Location = new System.Drawing.Point(14, 42);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(192, 403);
            this.itemPanel1.TabIndex = 0;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.intInput);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.txtComment);
            this.groupPanel1.Controls.Add(this.dgv);
            this.groupPanel1.Controls.Add(this.lblSave);
            this.groupPanel1.Controls.Add(this.btnSave);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Location = new System.Drawing.Point(212, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(722, 430);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 1;
            this.groupPanel1.Text = "目前無選擇任何學生";
            // 
            // intInput
            // 
            // 
            // 
            // 
            this.intInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intInput.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intInput.Enabled = false;
            this.intInput.Location = new System.Drawing.Point(563, 3);
            this.intInput.MaxValue = 180;
            this.intInput.MinValue = 0;
            this.intInput.Name = "intInput";
            this.intInput.ShowUpDown = true;
            this.intInput.Size = new System.Drawing.Size(149, 25);
            this.intInput.TabIndex = 18;
            this.intInput.Leave += new System.EventHandler(this.intInput_Leave);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(499, 5);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(72, 23);
            this.labelX3.TabIndex = 17;
            this.labelX3.Text = "缺席天數";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(499, 63);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(213, 308);
            this.txtComment.TabIndex = 16;
            this.txtComment.Leave += new System.EventHandler(this.txtComment_Leave);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroup,
            this.colTitle,
            this.colGrade});
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
            this.dgv.Size = new System.Drawing.Size(489, 396);
            this.dgv.TabIndex = 0;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);
            // 
            // colGroup
            // 
            this.colGroup.HeaderText = "類別";
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            // 
            // colTitle
            // 
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTitle.HeaderText = "標題";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colGrade
            // 
            this.colGrade.DropDownHeight = 106;
            this.colGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colGrade.DropDownWidth = 121;
            this.colGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colGrade.HeaderText = "等第";
            this.colGrade.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colGrade.IntegralHeight = false;
            this.colGrade.ItemHeight = 17;
            this.colGrade.Name = "colGrade";
            this.colGrade.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colGrade.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // lblSave
            // 
            this.lblSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSave.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblSave.BackgroundStyle.Class = "";
            this.lblSave.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSave.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSave.ForeColor = System.Drawing.Color.Red;
            this.lblSave.Location = new System.Drawing.Point(575, 377);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(57, 23);
            this.lblSave.TabIndex = 13;
            this.lblSave.Text = "未儲存";
            this.lblSave.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblSave.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(638, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(499, 34);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(87, 23);
            this.labelX2.TabIndex = 15;
            this.labelX2.Text = "班導師評語";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(14, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "Term";
            // 
            // cboTerm
            // 
            this.cboTerm.DisplayMember = "DisplayText";
            this.cboTerm.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerm.FormattingEnabled = true;
            this.cboTerm.ItemHeight = 19;
            this.cboTerm.Location = new System.Drawing.Point(55, 10);
            this.cboTerm.Name = "cboTerm";
            this.cboTerm.Size = new System.Drawing.Size(151, 25);
            this.cboTerm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboTerm.TabIndex = 3;
            this.cboTerm.SelectedIndexChanged += new System.EventHandler(this.cboTerm_SelectedIndexChanged);
            // 
            // HrtConductInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 454);
            this.Controls.Add(this.cboTerm);
            this.Controls.Add(this.itemPanel1);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HrtConductInputForm";
            this.Text = "HrtConduct輸入";
            this.Load += new System.EventHandler(this.HrtConductInputForm_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgv;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboTerm;
        private DevComponents.DotNetBar.LabelX lblSave;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colGrade;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.IntegerInput intInput;
    }
}