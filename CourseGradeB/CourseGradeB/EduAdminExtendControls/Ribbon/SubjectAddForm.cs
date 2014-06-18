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
    public partial class SubjectAddForm : BaseForm
    {
        AccessHelper _A = new AccessHelper();
        List<string> _SubjectCatch = new List<string>();
        public SubjectAddForm()
        {
            InitializeComponent();

            List<SubjectRecord> list = _A.Select<SubjectRecord>();

            foreach (SubjectRecord sr in list)
            {
                if (!_SubjectCatch.Contains(sr.Name))
                    _SubjectCatch.Add(sr.Name);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string name = txtSubjectName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!_SubjectCatch.Contains(name))
                {
                    SubjectRecord sr = new SubjectRecord();
                    sr.Name = name;
                    sr.Type = "Regular";

                    List<SubjectRecord> insert = new List<SubjectRecord>();
                    insert.Add(sr);
                    _A.InsertValues(insert);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("該科目名稱已存在");
                }
            }
            else
            {
                MessageBox.Show("請輸入科目名稱");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
