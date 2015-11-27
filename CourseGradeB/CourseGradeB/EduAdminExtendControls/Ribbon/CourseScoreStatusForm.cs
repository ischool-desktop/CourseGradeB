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
    public partial class CourseScoreStatusForm : FISCA.Presentation.Controls.BaseForm
    {
        private int _schoolYear, _semester;
        private List<string> _invalidCourseId;

        //Key 皆為 Course ID
        private Dictionary<string, CourseRecord> _courseRecordsDic;
        private Dictionary<string, int> _attendTotalDic;
        private Dictionary<string, int> _exam1CountDic;
        private Dictionary<string, int> _exam2CountDic;
        private Dictionary<string, string> _teacherName;

        private QueryHelper _Q;
        private AccessHelper _A;
        private BackgroundWorker _BW;

        public CourseScoreStatusForm()
        {
            InitializeComponent();
            cboFilter.SelectedIndex = 0;

            _invalidCourseId = new List<string>();

            _courseRecordsDic = new Dictionary<string, CourseRecord>();
            _attendTotalDic = new Dictionary<string, int>();
            _exam1CountDic = new Dictionary<string, int>();
            _exam2CountDic = new Dictionary<string, int>();
            _teacherName = new Dictionary<string, string>();
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
            DataTable dt = _Q.Select("SELECT tc_instruct.ref_course_id,teacher.teacher_name FROM tc_instruct LEFT OUTER JOIN teacher ON tc_instruct.ref_teacher_id=teacher.id ORDER BY tc_instruct.sequence DESC");
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

        private void FillData()
        {
            dgv.Rows.Clear();
            foreach (string id in _courseRecordsDic.Keys)
            {
                if (_invalidCourseId.Contains(id))
                    continue;

                string courseName = _courseRecordsDic[id].Name;
                string courseTeacher = _teacherName.ContainsKey(id) ? _teacherName[id] : string.Empty;
                int total = _attendTotalDic.ContainsKey(id) ? _attendTotalDic[id] : 0;
                int exam1 = _exam1CountDic.ContainsKey(id) ? _exam1CountDic[id] : 0;
                int exam2 = _exam2CountDic.ContainsKey(id) ? _exam2CountDic[id] : 0;

                DataGridViewRow row = new DataGridViewRow();
                //row.CreateCells(dgv, courseName, courseTeacher, exam1 + "/" + total, exam2 + "/" + total);
                row.CreateCells(dgv);
                row.Cells[0].Value = courseName;
                row.Cells[1].Value = courseTeacher;
                row.Cells[2].Value = exam1 + "/" + total;
                row.Cells[3].Value = exam2 + "/" + total;

                if (exam1 < total)
                    row.Cells[2].Style.ForeColor = Color.Red;

                if (exam2 < total)
                    row.Cells[3].Style.ForeColor = Color.Red;

                //if (chkNotFinishedOnly.Checked)
                //{
                //    if (exam1 < total || exam2 < total)
                //        dgv.Rows.Add(row);
                //    else
                //        continue;
                //}
                //else
                //    dgv.Rows.Add(row);
                if (cboFilter.SelectedIndex == 0)
                    dgv.Rows.Add(row);
                else if (cboFilter.SelectedIndex == 1 && exam1 < total)
                    dgv.Rows.Add(row);
                else if (cboFilter.SelectedIndex == 2 && exam2 < total)
                    dgv.Rows.Add(row);
            }
            dgv.Refresh();
        }

        private void QueryData()
        {
            //Get Course Record
            _courseRecordsDic.Clear();
            foreach (CourseRecord course in K12.Data.Course.SelectBySchoolYearAndSemester(_schoolYear, _semester))
            {
                if (!_courseRecordsDic.ContainsKey(course.ID))
                    _courseRecordsDic.Add(course.ID, course);
            }

            //Course IDs
            List<string> course_ids = _courseRecordsDic.Keys.ToList();

            _attendTotalDic.Clear();
            //Total Count
            Dictionary<string, string> attend_to_course = new Dictionary<string, string>();
            foreach (K12.Data.SCAttendRecord scr in K12.Data.SCAttend.SelectByCourseIDs(course_ids))
            {
                if (scr.Student.Status == StudentRecord.StudentStatus.一般)
                {
                    if (!_attendTotalDic.ContainsKey(scr.RefCourseID))
                        _attendTotalDic.Add(scr.RefCourseID, 0);

                    _attendTotalDic[scr.RefCourseID]++;

                    //for catch
                    if (!attend_to_course.ContainsKey(scr.ID))
                        attend_to_course.Add(scr.ID, scr.RefCourseID);
                }
            }

            _exam1CountDic.Clear();
            _exam2CountDic.Clear();
            if (course_ids.Count > 0)
            {
                //Exam1's Count
                foreach (K12.Data.SCETakeRecord sce in K12.Data.SCETake.SelectByCourseAndExam(course_ids, "1"))
                {
                    if (sce.Student.Status == StudentRecord.StudentStatus.一般)
                    {
                        string key = attend_to_course[sce.RefSCAttendID];

                        if (!_exam1CountDic.ContainsKey(key))
                            _exam1CountDic.Add(key, 0);

                        _exam1CountDic[key]++;
                    }
                }

                //Exam2's Count
                foreach (K12.Data.SCETakeRecord sce in K12.Data.SCETake.SelectByCourseAndExam(course_ids, "2"))
                {
                    if (sce.Student.Status == StudentRecord.StudentStatus.一般)
                    {
                        string key = attend_to_course[sce.RefSCAttendID];

                        if (!_exam2CountDic.ContainsKey(key))
                            _exam2CountDic.Add(key, 0);

                        _exam2CountDic[key]++;
                    }
                }

                //Get invalid course grade list
                string ids = string.Join(",", course_ids);
                _invalidCourseId.Clear();
                foreach (CourseExtendRecord cer in _A.Select<CourseExtendRecord>("ref_course_id in (" + ids + ") and grade_year <= 2"))
                {
                    string id = cer.Ref_course_id + "";
                    if (!_invalidCourseId.Contains(id))
                        _invalidCourseId.Add(id);
                }
            }
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
            wb.Worksheets[0].Name = "評量輸入狀況檢視清單";
            Aspose.Cells.Cells cs = wb.Worksheets[0].Cells;

            wb.Worksheets[0].Cells.Columns[0].Width = 40;
            wb.Worksheets[0].Cells.Columns[1].Width = 20;
            wb.Worksheets[0].Cells.Columns[2].Width = 10;
            wb.Worksheets[0].Cells.Columns[3].Width = 10;

            cs[0, 0].PutValue("課程名稱");
            cs[0, 1].PutValue("授課老師");
            cs[0, 2].PutValue("Midterm");
            cs[0, 3].PutValue("Final");

            int index = 1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string course_name = row.Cells[colCourse.Index].Value + "";
                string teacher_name = row.Cells[colTeacher.Index].Value + "";
                string midterm = row.Cells[colExam1.Index].Value + "";
                string final = row.Cells[colExam2.Index].Value + "";

                cs[index, 0].PutValue(course_name);
                cs[index, 1].PutValue(teacher_name);
                cs[index, 2].PutValue(midterm);
                cs[index, 3].PutValue(final);
                index++;
            }

            //wb.Worksheets[0].AutoFitColumns();

            SaveFileDialog save = new SaveFileDialog();
            save.Title = "另存新檔";
            save.FileName = "評量輸入狀況檢視清單.xls";
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
    }
}
