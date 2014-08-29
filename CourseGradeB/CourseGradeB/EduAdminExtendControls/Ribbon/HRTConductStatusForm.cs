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
    public partial class HRTConductStatusForm : FISCA.Presentation.Controls.BaseForm
    {
        private int _schoolYear, _semester;

        Dictionary<string, int> _StudentCounts;
        List<ClassObj> _ClassList;

        private QueryHelper _Q;
        private AccessHelper _A;
        private BackgroundWorker _BW;

        public HRTConductStatusForm()
        {
            InitializeComponent();

            _StudentCounts = new Dictionary<string, int>();
            _ClassList = new List<ClassObj>();
            _Q = new QueryHelper();
            _A = new AccessHelper();

            _BW = new BackgroundWorker();
            _BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            _BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_Completed);

            int i;
            _schoolYear = int.TryParse(K12.Data.School.DefaultSchoolYear, out i) ? i : 97;
            _semester = int.TryParse(K12.Data.School.DefaultSemester, out i) ? i : 1;

            txtSYSM.Text = "現在學年度: " + _schoolYear + " 學期: " + _semester;
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
            _StudentCounts.Clear();
            //取得學生相關資料
            string sql = "select student.ref_class_id,term from $ischool.conduct";
            sql += " join student on student.id=$ischool.conduct.ref_student_id";
            sql += " where $ischool.conduct.subject is null and $ischool.conduct.school_year=" + _schoolYear + " and $ischool.conduct.semester=" + _semester;
            DataTable dt = _Q.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                string id = row["ref_class_id"] + "";
                string term = row["term"] + "";

                if (string.IsNullOrWhiteSpace(term))
                    term = "2";

                string key = id + "_" + term;

                if (!_StudentCounts.ContainsKey(key))
                    _StudentCounts.Add(key, 0);

                _StudentCounts[key]++;
            }

            _ClassList.Clear();
            //取得課程相關資料
            sql = "select class.id as class_id,class.class_name,class.grade_year,teacher.teacher_name,count(student.id) from class";
            sql += " left join student on student.ref_class_id=class.id";
            sql += " left join teacher on class.ref_teacher_id=teacher.id";
            sql += " group by class_id,class.grade_year,teacher.teacher_name,class.class_name";
            dt = _Q.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                _ClassList.Add(new ClassObj(row));
            }

            //排序
            _ClassList.Sort(delegate(ClassObj x, ClassObj y)
            {
                string xx = (x.GradeYear + "").PadLeft(3, '0');
                xx += x.Name.PadLeft(20, '0');
                string yy = (y.GradeYear + "").PadLeft(3, '0');
                yy += y.Name.PadLeft(20, '0');
                return xx.CompareTo(yy);
            });
        }

        private void FillData()
        {
            dgv.Rows.Clear();
            foreach (ClassObj co in _ClassList)
            {
                string courseName = co.Name;
                string courseTeacher = co.TeacherName;
                int total = co.Count;
                string key1 = co.ID + "_1";
                string key2 = co.ID + "_2";
                int exam1 = _StudentCounts.ContainsKey(key1) ? _StudentCounts[key1] : 0;
                int exam2 = _StudentCounts.ContainsKey(key2) ? _StudentCounts[key2] : 0;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv);
                row.Cells[0].Value = courseName;
                row.Cells[1].Value = courseTeacher;
                row.Cells[2].Value = exam1 + "/" + total;
                row.Cells[3].Value = exam2 + "/" + total;

                if (co.GradeYear > 2)
                    row.Cells[2].Value = "N/A";

                if (exam1 < total && row.Cells[2].Value + "" != "N/A")
                    row.Cells[2].Style.ForeColor = Color.Red;

                if (exam2 < total)
                    row.Cells[3].Style.ForeColor = Color.Red;

                if (chkNotFinishedOnly.Checked)
                {
                    if (co.GradeYear <= 2 && (exam1 < total || exam2 < total))
                        dgv.Rows.Add(row);
                    else if (co.GradeYear > 2 && exam2 < total)
                        dgv.Rows.Add(row);
                    else
                        continue;
                }
                else
                    dgv.Rows.Add(row);
            }
            dgv.Refresh();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
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
            chkNotFinishedOnly.Enabled = b;
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

            cs[0, 0].PutValue("班級名稱");
            cs[0, 1].PutValue("班導師");
            cs[0, 2].PutValue("Midterm");
            cs[0, 3].PutValue("Final");

            int index = 1;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string class_name = row.Cells[colClassName.Index].Value + "";
                string teacher_name = row.Cells[colTeacher.Index].Value + "";
                string midterm = row.Cells[colExam1.Index].Value + "";
                string final = row.Cells[colExam2.Index].Value + "";

                cs[index, 0].PutValue(class_name);
                cs[index, 1].PutValue(teacher_name);
                cs[index, 2].PutValue(midterm);
                cs[index, 3].PutValue(final);
                index++;
            }

            //wb.Worksheets[0].AutoFitColumns();

            SaveFileDialog save = new SaveFileDialog();
            save.Title = "另存新檔";
            save.FileName = "評量輸入狀況檢視清單(班導師).xls";
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

        class ClassObj
        {
            public string ID;
            public string Name;
            public string TeacherName;
            public int GradeYear;
            public int Count;
            public ClassObj(DataRow row)
            {
                int i;
                this.ID = row["class_id"] + "";
                this.Name = row["class_name"] + "";
                this.TeacherName = row["teacher_name"] + "";
                this.GradeYear = int.TryParse(row["grade_year"] + "", out i) ? i : 0;
                this.Count = int.TryParse(row["count"] + "", out i) ? i : 0;
            }
        }
    }
}
