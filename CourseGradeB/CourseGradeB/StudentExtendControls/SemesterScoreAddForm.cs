using FISCA.Presentation.Controls;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CourseGradeB.StudentExtendControls
{
    public partial class SemesterScoreAddForm : BaseForm
    {
        List<string> _list;
        string _id;
        private int _schoolYear, _semester;

        public SemesterScoreAddForm(List<SemesterScoreRecord> records,string id)
        {
            InitializeComponent();
            _list = new List<string>();
            _id = id;

            foreach (SemesterScoreRecord r in records)
            {
                string key = r.SchoolYear + "_" + r.Semester;
                _list.Add(key);
            }

            _schoolYear = int.Parse(K12.Data.School.DefaultSchoolYear);
            _semester = int.Parse(K12.Data.School.DefaultSemester);

            for (int i = -2; i <= 2; i++)
                cboSchoolYear.Items.Add(_schoolYear + i);

            cboSemester.Items.Add("1");
            cboSemester.Items.Add("2");

            cboSchoolYear.Text = _schoolYear + "";
            cboSemester.Text = _semester + "";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(cboSchoolYear.Text);
            int semester = int.Parse(cboSemester.Text);
            string key = schoolYear + "_" + semester;

            if(_list.Contains(key))
            {
                MessageBox.Show("該學年度學期已存在,無法新增");
            }
            else
            {
                SemesterScoreRecord record = new SemesterScoreRecord(_id, schoolYear,semester);
                K12.Data.SemesterScore.Insert(record);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
