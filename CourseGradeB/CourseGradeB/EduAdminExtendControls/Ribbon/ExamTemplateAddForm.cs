using FISCA.Presentation.Controls;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    public partial class ExamTemplateAddForm : BaseForm
    {
        AccessHelper _A = new AccessHelper();
        List<string> _Catch = new List<string>();

        public ExamTemplateAddForm()
        {
            InitializeComponent();

            List<ExamTemplateRecord> list = _A.Select<ExamTemplateRecord>();
            
            foreach (ExamTemplateRecord r in list)
            {
                if (!_Catch.Contains(r.Name))
                    _Catch.Add(r.Name);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!_Catch.Contains(name))
                {
                    ExamTemplateRecord record = new ExamTemplateRecord();
                    record.Name = name;
                    record.ExamScale = "100";

                    List<ExamTemplateRecord> insert = new List<ExamTemplateRecord>();
                    insert.Add(record);
                    _A.InsertValues(insert);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("該樣板名稱已存在");
                }
            }
            else
            {
                MessageBox.Show("請輸入樣板名稱");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
