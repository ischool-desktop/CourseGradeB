using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CourseGradeB.ClassExtendControls.Ribbon
{
    public partial class HrtSelectSchoolYear : BaseForm
    {
        private int _schoolYear, _semester;
        string _classId;
        public HrtSelectSchoolYear(string id)
        {
            InitializeComponent();

            _schoolYear = int.Parse(K12.Data.School.DefaultSchoolYear);
            _semester = int.Parse(K12.Data.School.DefaultSemester);
            _classId = id;

            for (int i = -2; i <= 2; i++)
                cboSchoolYear.Items.Add(_schoolYear + i);

            cboSemester.Items.Add("1");
            cboSemester.Items.Add("2");

            cboSchoolYear.Text = _schoolYear + "";
            cboSemester.Text = _semester + "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _schoolYear = int.Parse(cboSchoolYear.Text);
            _semester = int.Parse(cboSemester.Text);
            new CourseGradeB.ClassExtendControls.Ribbon.HrtConductInputForm(_schoolYear,_semester,_classId).ShowDialog();
        }
    }
}
