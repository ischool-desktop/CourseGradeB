using CourseGradeB.EduAdminExtendControls;
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

namespace CourseGradeB.StuAdminExtendControls
{
    public partial class SubjectConductAddForm : BaseForm
    {
        public string SubjectName;
        public SubjectConductAddForm()
        {
            InitializeComponent();

            SubjectName = "";
            AccessHelper _A = new AccessHelper();
            List<SubjectRecord> list = _A.Select<SubjectRecord>();

            if (list.Count == 0)
            {
                MessageBox.Show("沒有任何科目可以新增,請確認該科目資料已被建立");
                this.Close();
            }
                
            foreach (SubjectRecord record in _A.Select<SubjectRecord>())
            {
                if (!cboSubject.Items.Contains(record.Name))
                    cboSubject.Items.Add(record.Name);
            }

            cboSubject.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboSubject.Text))
            {
                MessageBox.Show("科目不可為空白");
                return;
            }

            SubjectName = cboSubject.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
