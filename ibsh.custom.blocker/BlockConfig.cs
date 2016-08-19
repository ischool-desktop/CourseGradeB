using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ibsh.custom.blocker
{
    public partial class BlockConfig : FISCA.Presentation.Controls.BaseForm
    {
        public BlockConfig()
        {
            InitializeComponent();
        }

        private void BlockConfig_Load(object sender, EventArgs e)
        {
            var target = BlockConfigRecord.Instance;
            this.StartTime1.Text = target.StartTime == null ? "" : target.StartTime.ToString("yyyy/M/d HH:mm:ss");
            this.EndTime1.Text = target.EndTime == null ? "" : target.EndTime.ToString("yyyy/M/d HH:mm:ss");
            this.MemotextBoxX.Text = target.Memo;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var target = BlockConfigRecord.Instance;
            DateTime dt1, dt2;
            if (!DateTime.TryParse(StartTime1.Text, out dt1))
            {
                MessageBox.Show("開始時間格式不正確。");
            }

            if (!DateTime.TryParse(EndTime1.Text, out dt2))
            {
                MessageBox.Show("結束時間格式不正確。");
            }
            if (dt1 != null && dt2 != null)
            {
                target.StartTime = dt1;
                target.EndTime = dt2;
                target.Memo = MemotextBoxX.Text;
                target.Save();
                Close();
            }
        }
    }
}
