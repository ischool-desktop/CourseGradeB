namespace ibsh.custom.blocker
{
    partial class BlockConfig
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
            this.MemotextBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.StartTime1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.EndTime1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.Save = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // MemotextBoxX
            // 
            this.MemotextBoxX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.MemotextBoxX.Border.Class = "TextBoxBorder";
            this.MemotextBoxX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.MemotextBoxX.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MemotextBoxX.Location = new System.Drawing.Point(9, 93);
            this.MemotextBoxX.Multiline = true;
            this.MemotextBoxX.Name = "MemotextBoxX";
            this.MemotextBoxX.Size = new System.Drawing.Size(567, 186);
            this.MemotextBoxX.TabIndex = 2;
            // 
            // StartTime1
            // 
            // 
            // 
            // 
            this.StartTime1.Border.Class = "TextBoxBorder";
            this.StartTime1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.StartTime1.Location = new System.Drawing.Point(113, 37);
            this.StartTime1.Name = "StartTime1";
            this.StartTime1.Size = new System.Drawing.Size(190, 25);
            this.StartTime1.TabIndex = 0;
            this.StartTime1.WatermarkText = "範例：2012/12/31 00:00:00";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(303, 39);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(17, 21);
            this.labelX5.TabIndex = 44;
            this.labelX5.Text = "~";
            // 
            // EndTime1
            // 
            // 
            // 
            // 
            this.EndTime1.Border.Class = "TextBoxBorder";
            this.EndTime1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.EndTime1.Location = new System.Drawing.Point(320, 37);
            this.EndTime1.Name = "EndTime1";
            this.EndTime1.Size = new System.Drawing.Size(190, 25);
            this.EndTime1.TabIndex = 1;
            this.EndTime1.WatermarkText = "範例：2012/12/31 00:00:00";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = "";
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(12, 39);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(101, 21);
            this.labelX6.TabIndex = 45;
            this.labelX6.Text = "禁止查詢時間：";
            // 
            // Save
            // 
            this.Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.BackColor = System.Drawing.Color.Transparent;
            this.Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Save.Location = new System.Drawing.Point(522, 285);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(54, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "儲存";
            this.Save.Click += new System.EventHandler(this.Save_Click);
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
            this.labelX1.Location = new System.Drawing.Point(12, 66);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 50;
            this.labelX1.Text = "顯示訊息：";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(543, 21);
            this.labelX2.TabIndex = 51;
            this.labelX2.Text = "在設定的時間區間內，關閉學生、家長網站系統的Student Info.功能，並顯示設定之訊息。";
            // 
            // BlockConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 320);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.MemotextBoxX);
            this.Controls.Add(this.StartTime1);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.EndTime1);
            this.Controls.Add(this.labelX6);
            this.DoubleBuffered = true;
            this.Name = "BlockConfig";
            this.Text = "關閉線上查詢";
            this.Load += new System.EventHandler(this.BlockConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX MemotextBoxX;
        private DevComponents.DotNetBar.Controls.TextBoxX StartTime1;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX EndTime1;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.ButtonX Save;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}