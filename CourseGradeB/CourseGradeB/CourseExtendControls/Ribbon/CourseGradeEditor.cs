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

namespace CourseGradeB.CourseExtendControls.Ribbon
{
    public partial class CourseGradeEditor : BaseForm
    {
        List<string> _courses;
        AccessHelper _A;

        public CourseGradeEditor(List<string> courses)
        {
            InitializeComponent();
            _A = new AccessHelper();

            _courses = courses;

            for (int i = 1; i <= 12; i++)
                cboGrade.Items.Add(i);

            cboGrade.Text = "1";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int i;
            int grade = 1;

            if (int.TryParse(cboGrade.Text, out i))
                grade = i;

            string ids = string.Join(",", _courses);

            Dictionary<string, CourseExtendRecord> dic = new Dictionary<string, CourseExtendRecord>();

            foreach (CourseExtendRecord cer in _A.Select<CourseExtendRecord>("ref_course_id in (" + ids + ")"))
            {
                if (!dic.ContainsKey(cer.Ref_course_id + ""))
                    dic.Add(cer.Ref_course_id + "", cer);
            }

            List<CourseExtendRecord> update = new List<CourseExtendRecord>();
            List<CourseExtendRecord> insert = new List<CourseExtendRecord>();

            foreach (string id in _courses)
            {
                int temp;
                int ref_course_id = 0;

                if (int.TryParse(id, out temp))
                    ref_course_id = temp;

                if (dic.ContainsKey(id))
                {
                    dic[id].GradeYear = grade;
                    update.Add(dic[id]);
                }
                else
                {
                    if (ref_course_id > 0)
                    {
                        CourseExtendRecord cer = new CourseExtendRecord();
                        cer.Ref_course_id = ref_course_id;
                        cer.GradeYear = grade;

                        insert.Add(cer);
                    }
                }
            }

            if (update.Count > 0)
                _A.UpdateValues(update);

            if (insert.Count > 0)
                _A.InsertValues(insert);

            MessageBox.Show("修改完成");
            this.Close();
        }
    }
}
