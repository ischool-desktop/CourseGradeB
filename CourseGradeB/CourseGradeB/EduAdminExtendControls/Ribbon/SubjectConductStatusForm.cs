using CourseGradeB.CourseExtendControls;
using FISCA.Data;
using FISCA.UDT;
using K12.Data;
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
    public partial class SubjectConductStatusForm : FISCA.Presentation.Controls.BaseForm
    {
        private int _schoolYear, _semester;

        //Key 皆為 Course ID
        private Dictionary<string, string> _teacherName;
        private Dictionary<string, int> _attendStudents;
        List<CourseObj> _courses;

        private QueryHelper _Q;
        private AccessHelper _A;
        private BackgroundWorker _BW;

        public SubjectConductStatusForm()
        {
            InitializeComponent();
            cboFilter.SelectedIndex = 0;

            _teacherName = new Dictionary<string, string>();
            _attendStudents = new Dictionary<string, int>();
            _courses = new List<CourseObj>();
            _Q = new QueryHelper();
            _A = new AccessHelper();

            _BW = new BackgroundWorker();
            _BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            _BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_Completed);

            int i;
            _schoolYear = int.TryParse(K12.Data.School.DefaultSchoolYear, out i) ? i : 97;
            _semester = int.TryParse(K12.Data.School.DefaultSemester, out i) ? i : 1;

            intSchoolYear.Value = _schoolYear;
            intSemester.Value = _semester;

            //Get Teacher Name
            DataTable dt = _Q.Select("SELECT tc_instruct.ref_course_id,teacher.teacher_name FROM tc_instruct JOIN teacher ON tc_instruct.ref_teacher_id=teacher.id ORDER BY tc_instruct.sequence DESC");
            foreach (DataRow row in dt.Rows)
            {
                string course_id = row["ref_course_id"] + "";
                string teacherName = row["teacher_name"] + "";

                if (!_teacherName.ContainsKey(course_id))
                    _teacherName.Add(course_id, string.Empty);

                _teacherName[course_id] = teacherName;
            }
        }

        private void BW_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            FormEnable(true);
            FillData();
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            _attendStudents.Clear();
            //取得學生相關資料
            string sql = "select course.id, ic.term, conduct from $ischool.conduct as ic";
            sql += " left join sc_attend on ic.ref_student_id=sc_attend.ref_student_id";
            sql += " left join student on sc_attend.ref_student_id=student.id";
            sql += " left join course on sc_attend.ref_course_id=course.id and ic.school_year=course.school_year and ic.semester=course.semester and ic.subject=course.subject ";
            sql += " where ic.school_year=" + _schoolYear + " and ic.semester=" + _semester + " and student.status = 1";
            DataTable dt = _Q.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                string id = row["id"] + "";
                string term = row["term"] + "";

                if (string.IsNullOrWhiteSpace(term))
                    term = "2";

                string key = id + "_" + term;

                //if (!_attendStudents.ContainsKey(key))
                //    _attendStudents.Add(key, 0);

                //_attendStudents[key]++;

                bool allFinished = true;
                try
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml("" + row["conduct"]);
                    foreach (System.Xml.XmlElement item in doc.SelectNodes("Conducts/Conduct/Item"))
                    {
                        if (item.GetAttribute("Grade") == "")
                            allFinished = false;
                    }
                }
                catch
                {
                    allFinished = false;
                }
                if (allFinished)
                {
                    if (!_attendStudents.ContainsKey(key))
                        _attendStudents.Add(key, 0);
                    _attendStudents[key]++;
                }
            }

            _courses.Clear();
            //取得課程相關資料
            sql = "select course.id,course.course_name,ice.grade_year,count(student.id) from course ";
            sql += "left join sc_attend on sc_attend.ref_course_id=course.id ";
            sql += "left join student on sc_attend.ref_student_id=student.id ";
            sql += "left join $ischool.course.extend as ice on ice.ref_course_id=course.id ";
            sql += "where course.school_year=" + _schoolYear + " and course.semester=" + _semester + " and student.status = 1";
            sql += " group by course.id,course.course_name,ice.grade_year";
            dt = _Q.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                CourseObj co = new CourseObj(row);
                co.TeacherName = _teacherName.ContainsKey(co.ID) ? _teacherName[co.ID] : "";
                _courses.Add(co);
            }

            //排序
            _courses.Sort(delegate(CourseObj x, CourseObj y)
            {
                string xx = (x.GradeYear + "").PadLeft(3, '0');
                xx += x.Name.PadLeft(50, '0');
                string yy = (y.GradeYear + "").PadLeft(3, '0');
                yy += y.Name.PadLeft(50, '0');
                return xx.CompareTo(yy);
            });
        }

        private void FillData()
        {
            dgv.Rows.Clear();
            foreach (CourseObj co in _courses)
            {
                if (co.GradeYear > 6)
                    continue;

                int total = co.Count;

                if (total == 0)
                    continue;

                string key1 = co.ID + "_1";
                string key2 = co.ID + "_2";
                int exam1 = _attendStudents.ContainsKey(key1) ? _attendStudents[key1] : 0;
                int exam2 = _attendStudents.ContainsKey(key2) ? _attendStudents[key2] : 0;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv);
                row.Cells[0].Value = co.Name;
                row.Cells[1].Value = co.GradeYear;
                row.Cells[2].Value = co.TeacherName;
                row.Cells[3].Value = exam1 + "/" + total;
                row.Cells[4].Value = exam2 + "/" + total;

                if (co.GradeYear > 4)
                    row.Cells[3].Value = "N/A";

                if (exam1 < total && "" + row.Cells[3].Value != "N/A")
                    row.Cells[3].Style.ForeColor = Color.Red;

                if (exam2 < total)
                    row.Cells[4].Style.ForeColor = Color.Red;

                //if (chkNotFinishedOnly.Checked)
                //{
                //    if (co.GradeYear <= 2 && (exam1 < total || exam2 < total))
                //        dgv.Rows.Add(row);
                //    else if (co.GradeYear > 2 && exam2 < total)
                //        dgv.Rows.Add(row);
                //    else
                //        continue;
                //}
                //else
                //    dgv.Rows.Add(row);
                if (cboFilter.SelectedIndex == 0)
                    dgv.Rows.Add(row);
                else if (cboFilter.SelectedIndex == 1 && exam1 < total && "" + row.Cells[3].Value != "N/A")
                    dgv.Rows.Add(row);
                else if (cboFilter.SelectedIndex == 2 && exam2 < total)
                    dgv.Rows.Add(row);
            }
            dgv.Refresh();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            _schoolYear = intSchoolYear.Value;
            _semester = intSemester.Value;

            if (_BW.IsBusy)
                MessageBox.Show("系統忙碌,請稍後再試...");
            else
            {
                FormEnable(false);
                _BW.RunWorkerAsync();
            }
        }

        private void FormEnable(bool b)
        {
            btnExport.Enabled = b;
            cboFilter.Enabled = b;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Worksheets[0].Name = "Conduct輸入狀況檢視清單";
            Aspose.Cells.Cells cs = wb.Worksheets[0].Cells;

            wb.Worksheets[0].Cells.Columns[0].Width = 40;
            wb.Worksheets[0].Cells.Columns[1].Width = 20;
            wb.Worksheets[0].Cells.Columns[2].Width = 20;
            wb.Worksheets[0].Cells.Columns[3].Width = 20;
            wb.Worksheets[0].Cells.Columns[4].Width = 20;

            cs[0, 0].PutValue("課程名稱");
            cs[0, 1].PutValue("開課年級");
            cs[0, 2].PutValue("授課老師");
            cs[0, 3].PutValue("Midterm");
            cs[0, 4].PutValue("Final");

            int index = 1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string course_name = "" + row.Cells[colCourse.Index].Value;
                string teacher_name = "" + row.Cells[colTeacher.Index].Value;
                string midterm = "" + row.Cells[colExam1.Index].Value;
                string final = "" + row.Cells[colExam2.Index].Value;

                cs[index, 0].PutValue(course_name);
                cs[index, 1].PutValue("" + row.Cells[colClass.Index].Value);
                cs[index, 2].PutValue(teacher_name);
                cs[index, 3].PutValue(midterm);
                cs[index, 4].PutValue(final);
                index++;
            }

            //wb.Worksheets[0].AutoFitColumns();

            SaveFileDialog save = new SaveFileDialog();
            save.Title = "另存新檔";
            save.FileName = "評量輸入狀況檢視清單(授課老師).xls";
            save.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";

            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    wb.Save(save.FileName, Aspose.Cells.SaveFormat.Excel97To2003);
                    System.Diagnostics.Process.Start(save.FileName);
                }
                catch
                {
                    MessageBox.Show("檔案儲存失敗");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        class CourseObj
        {
            public string ID;
            public string Name;
            public string TeacherName;
            public int GradeYear;
            public int Count;
            public CourseObj(DataRow row)
            {
                int i;
                this.ID = "" + row["id"];
                this.Name = "" + row["course_name"];
                this.GradeYear = int.TryParse(row["grade_year"] + "", out i) ? i : 0;
                this.Count = int.TryParse(row["count"] + "", out i) ? i : 0;
            }
        }
    }
}
