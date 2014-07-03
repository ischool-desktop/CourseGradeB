using CourseGradeB.EduAdminExtendControls;
using DevComponents.DotNetBar;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using JHSchool;
using JHSchool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CourseGradeB.CourseExtendControls.Ribbon
{
    public partial class CourseScoreInputForm : BaseForm
    {
        private const string InsertMode = "insert";
        private const string DeleteMode = "delete";
        private const string UpdateMode = "update";

        private CourseRecord _course;
        private string _RunningExamId;
        private string _RunningExamName = string.Empty;

        private List<JHSCAttendRecord> _scAttendRecordList;
        private Dictionary<string, SCETakeRecord> _SceTakeDic;
        private Dictionary<string, JHSCAttendRecord> _ScAttendDic;
        private List<DataGridViewCell> _dirtyCellList;
        private object _cboSelectedItem;
        private Dictionary<string, DataGridViewRow> _studentRow;
        AccessHelper _A;
        QueryHelper _Q;
        BackgroundWorker _BW;
        K12.Data.UpdateHelper _U = new K12.Data.UpdateHelper();

        /// <summary>
        /// Constructor
        /// 傳入一個課程。
        /// </summary>
        /// <param name="course"></param>
        public CourseScoreInputForm(CourseRecord course)
        {
            InitializeComponent();
            _course = course;
            _SceTakeDic = new Dictionary<string, SCETakeRecord>();
            _ScAttendDic = new Dictionary<string, JHSCAttendRecord>();
            _dirtyCellList = new List<DataGridViewCell>();
            _studentRow = new Dictionary<string, DataGridViewRow>();
            _BW = new BackgroundWorker();
            _BW.DoWork += new DoWorkEventHandler(_BW_DoWork);
            _BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_BW_Completed);
            _A = new AccessHelper();
            _Q = new QueryHelper();

            #region 設定小標題
            TeacherRecord first = course.GetFirstTeacher();
            TeacherRecord second = course.GetSecondTeacher();
            TeacherRecord third = course.GetThirdTeacher();

            StringBuilder builder = new StringBuilder("");
            if (first != null) builder.Append(first.Name + ",");
            if (second != null) builder.Append(second.Name + ",");
            if (third != null) builder.Append(third.Name + ",");

            string teachers = builder.ToString();
            if (!string.IsNullOrEmpty(teachers))
                teachers = teachers.Substring(0, teachers.Length - 1);

            lblCourseName.Text = course.Name + (!string.IsNullOrEmpty(teachers) ? " (" + teachers + ")" : "");

            #endregion
        }

        private void _BW_DoWork(object sender, DoWorkEventArgs e)
        {
            Save();
        }

        private void _BW_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ReLoad();
        }

        private void CourseScoreInputForm_Load(object sender, EventArgs e)
        {
            //確認考試ID有1和2
            CheckExamIDExist();

            //取得課程開課年級
            int course_grade_year = -1;
            List<CourseExtendRecord> list = _A.Select<CourseExtendRecord>("ref_course_id=" + _course.ID);
            if (list.Count > 0)
                course_grade_year = list[0].GradeYear;

            if (course_grade_year == -1)
            {
                MessageBox.Show("開課年級不正確,無法輸入成績...");
                this.Close();
            }
            else if (course_grade_year == 1 || course_grade_year == 2)
            {
                MessageBox.Show("1或2年級的課程無法輸入成績...");
                this.Close();
            }
            else
            {
                //取得修課學生
                FillStudentsToDataGridView();

                cboExamList.Items.Clear();
                cboExamList.Items.Add(new ExamComboBoxItem("Middle Term", 1));
                cboExamList.Items.Add(new ExamComboBoxItem("Final Term", 2));

                cboExamList.SelectedIndex = 0;
            }
        }

        private void CheckExamIDExist()
        {
            try
            {
                DataTable dt = _Q.Select("SELECT id From exam");
                List<int> exam_ids = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    exam_ids.Add(int.Parse(row[0] + ""));
                }

                if (!exam_ids.Contains(1))
                    _U.Execute("insert into exam (id,exam_name,display_order) values (1,'Middle',0)");
                if (!exam_ids.Contains(2))
                    _U.Execute("insert into exam (id,exam_name,display_order) values (2,'Final',1)");
            }
            catch (Exception e)
            {
                MessageBox.Show("考試名稱檢查發生錯誤,原因:" + e.Message);
                this.Close();
            }
        }

        /// <summary>
        /// 將學生填入DataGridView。
        /// </summary>
        private void FillStudentsToDataGridView()
        {
            dgv.SuspendLayout();
            dgv.Rows.Clear();

            _scAttendRecordList = JHSCAttend.SelectByCourseIDs(new string[] { _course.ID });

            _ScAttendDic.Clear();
            foreach (JHSCAttendRecord record in _scAttendRecordList)
            {
                if (!_ScAttendDic.ContainsKey(record.RefStudentID))
                    _ScAttendDic.Add(record.RefStudentID, record);
            }
            _scAttendRecordList.Sort(SCAttendComparer);

            foreach (var record in _scAttendRecordList)
            {
                JHStudentRecord student = record.Student;
                if (student.StatusStr != "一般") continue;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv,
                    (student.Class != null) ? student.Class.Name : "",
                    student.SeatNo,
                    student.Name,
                    student.StudentNumber
                );
                row.Tag = student.ID;
                dgv.Rows.Add(row);

                if (!_studentRow.ContainsKey(student.ID))
                    _studentRow.Add(student.ID, row);
            }

            dgv.ResumeLayout();
        }

        /// <summary>
        /// Comparision
        /// 依班級、座號、學號排序修課學生。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int SCAttendComparer(JHSCAttendRecord a, JHSCAttendRecord b)
        {
            JHStudentRecord aStudent = a.Student;
            JHClassRecord aClass = a.Student.Class;
            JHStudentRecord bStudent = b.Student;
            JHClassRecord bClass = b.Student.Class;

            string aa = aClass == null ? (string.Empty).PadLeft(10, '0') : (aClass.Name).PadLeft(10, '0');
            aa += aStudent == null ? (string.Empty).PadLeft(3, '0') : (aStudent.SeatNo + "").PadLeft(3, '0');
            aa += aStudent == null ? (string.Empty).PadLeft(10, '0') : (aStudent.StudentNumber).PadLeft(10, '0');

            string bb = bClass == null ? (string.Empty).PadLeft(10, '0') : (bClass.Name).PadLeft(10, '0');
            bb += bStudent == null ? (string.Empty).PadLeft(3, '0') : (bStudent.SeatNo + "").PadLeft(3, '0');
            bb += bStudent == null ? (string.Empty).PadLeft(10, '0') : (bStudent.StudentNumber).PadLeft(10, '0');

            return aa.CompareTo(bb);
        }

        /// <summary>
        /// 更新畫面
        /// </summary>
        private void ReLoad()
        {
            _dirtyCellList.Clear();
            lblSave.Visible = false;
            _SceTakeDic.Clear();

            ExamComboBoxItem cboitem = _cboSelectedItem as ExamComboBoxItem;
            _RunningExamId = cboitem.Value + "";
            _RunningExamName = cboitem.DisplayText;
            
            List<JHSCETakeRecord> records = JHSCETake.SelectByCourseAndExam(_course.ID, _RunningExamId);
            foreach (JHSCETakeRecord record in records)
            {
                if (!_SceTakeDic.ContainsKey(record.RefStudentID))
                    _SceTakeDic.Add(record.RefStudentID, new SCETakeRecord(record));
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                //學生ID
                string sid = row.Tag + "";

                //Cell初值設定
                row.Cells[chInputScore.Index].Value = "";
                row.Cells[chInputAssignmentScore.Index].Value = "";

                //有紀錄則填值
                if (_SceTakeDic.ContainsKey(sid))
                {
                    row.Cells[chInputScore.Index].Value = _SceTakeDic[sid].ExamScore;
                    row.Cells[chInputAssignmentScore.Index].Value = _SceTakeDic[sid].RegularScore;
                }

                //將Cell的值填入tag作dirty判斷
                row.Cells[chInputScore.Index].Tag = row.Cells[chInputScore.Index].Value;
                row.Cells[chInputAssignmentScore.Index].Tag = row.Cells[chInputAssignmentScore.Index].Value;

                //文字顏色修正
                FixTextColor(row.Cells[chInputScore.Index]);
                FixTextColor(row.Cells[chInputAssignmentScore.Index]);
            }

            pictureBox1.Visible = false;
        }

        /// <summary>
        /// 驗證每個欄位是否正確。
        /// 有錯誤訊息表示不正確。
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            bool valid = true;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (chInputScore.Visible == true && !string.IsNullOrEmpty(row.Cells[chInputScore.Index].ErrorText))
                    valid = false;
                if (chInputAssignmentScore.Visible == true && !string.IsNullOrEmpty(row.Cells[chInputAssignmentScore.Index].ErrorText))
                    valid = false;
            }

            return valid;
        }

        /// <summary>
        /// 驗證分數是否超出正常範圍(0~100)。
        /// </summary>
        private bool HasGreenText()
        {
            bool retVal = false;
            //只驗證dirtyCell,找到一個就不繼續驗證了
            foreach (DataGridViewCell cell in _dirtyCellList)
            {
                if (cell.Style.ForeColor == Color.Green)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// 修正數字顏色
        /// </summary>
        private void FixTextColor(DataGridViewCell cell)
        {
            cell.Style.ForeColor = Color.Black;

            cell.ErrorText = "";
            if (!string.IsNullOrEmpty("" + cell.Value))
            {
                decimal d;
                if (decimal.TryParse("" + cell.Value, out d))
                {
                    if (d < 60)
                        cell.Style.ForeColor = Color.Red;
                    if (d > 100 || d < 0)
                        cell.Style.ForeColor = Color.Green;
                }
                else
                {
                    cell.ErrorText = "分數必須是數字";
                }
            }
        }

        /// <summary>
        /// 紀錄系統歷程
        /// </summary>
        private void FiscaLogWriter(List<JHSCETakeRecord> list, string action)
        {
            bool mustWrite = false;
            string title = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (action == InsertMode)
            {
                title = "成績新增";
                mustWrite = true;
            }
            if (action == DeleteMode)
            {
                title = "成績刪除";
                mustWrite = true;
            }
            if (action == UpdateMode)
            {
                title = "成績修改";
            }

            sb.AppendLine(_course.SchoolYear + "學年度第" + _course.Semester + "學期 " + _course.Name + " 試別:" + _RunningExamName);

            foreach (JHSCETakeRecord r in list)
            {
                if (_studentRow.ContainsKey(r.RefStudentID))
                {
                    DataGridViewRow row = _studentRow[r.RefStudentID];
                    string strClassName = row.Cells[chClassName.Index].Value + "";
                    string strSeatNo = row.Cells[chSeatNo.Index].Value + "";
                    string strName = row.Cells[chName.Index].Value + "";
                    string strStudentNumber = row.Cells[chStudentNumber.Index].Value + "";

                    string before_examScore = row.Cells[chInputScore.Index].Tag + "";
                    string after_examScore = row.Cells[chInputScore.Index].Value + "";
                    string before_regularScore = row.Cells[chInputAssignmentScore.Index].Tag + "";
                    string after_regularScore = row.Cells[chInputAssignmentScore.Index].Value + "";

                    switch (action)
                    {
                        case InsertMode:
                            sb.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
                            if (string.IsNullOrWhiteSpace(before_examScore) && !string.IsNullOrWhiteSpace(after_examScore))
                                sb.AppendLine("定期成績新增分數「" + after_examScore + "」");
                            if (string.IsNullOrWhiteSpace(before_regularScore) && !string.IsNullOrWhiteSpace(after_regularScore))
                                sb.AppendLine("平時成績新增分數「" + after_regularScore + "」");
                            break;
                        case DeleteMode:
                            sb.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
                            if (!string.IsNullOrWhiteSpace(before_examScore) && string.IsNullOrWhiteSpace(after_examScore))
                                sb.AppendLine("定期成績刪除分數「" + before_examScore + "」");
                            if (!string.IsNullOrWhiteSpace(before_regularScore) && string.IsNullOrWhiteSpace(after_regularScore))
                                sb.AppendLine("平時成績刪除分數「" + before_regularScore + "」");
                            break;
                        case UpdateMode:
                            string str1 = string.Empty;
                            string str2 = string.Empty;
                            bool needAppend = false;
                            if (before_examScore != after_examScore)
                            {
                                str1 = "定期成績由「" + before_examScore + "」改為「" + after_examScore + "」";
                                mustWrite = true;
                                needAppend = true;
                            }
                            if (before_regularScore != after_regularScore)
                            {
                                str2 = "平時成績由「" + before_regularScore + "」改為「" + after_regularScore + "」";
                                mustWrite = true;
                                needAppend = true;
                            }

                            if (needAppend)
                            {
                                sb.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
                                if (!string.IsNullOrWhiteSpace(str1))
                                    sb.AppendLine(str1);
                                if (!string.IsNullOrWhiteSpace(str2))
                                    sb.AppendLine(str2);
                            }
                            break;
                        default:
                            //do nothing...
                            break;
                    }
                }
            }

            if (mustWrite)
                FISCA.LogAgent.ApplicationLog.Log("課程成績輸入", title, sb.ToString());
        }

        private void Save()
        {
            List<JHSCETakeRecord> update = new List<JHSCETakeRecord>();
            List<JHSCETakeRecord> insert = new List<JHSCETakeRecord>();
            List<JHSCETakeRecord> delete = new List<JHSCETakeRecord>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                string sid = row.Tag + "";
                string exam_score = row.Cells[chInputScore.Index].Value + "";
                string regular_score = row.Cells[chInputAssignmentScore.Index].Value + "";

                if (_SceTakeDic.ContainsKey(sid))
                {
                    _SceTakeDic[sid].ExamScore = exam_score;
                    _SceTakeDic[sid].RegularScore = regular_score;

                    //兩種分數皆無資料則加入刪除清單,否則就更新
                    if (string.IsNullOrWhiteSpace(_SceTakeDic[sid].ExamScore) && string.IsNullOrWhiteSpace(_SceTakeDic[sid].RegularScore))
                        delete.Add(_SceTakeDic[sid].toJHSCETakeRecord());
                    else
                        update.Add(_SceTakeDic[sid].toJHSCETakeRecord());
                }
                else if (_ScAttendDic.ContainsKey(sid))
                {
                    SCETakeRecord record = new SCETakeRecord(new JHSCETakeRecord());
                    record.RefSCAttendID = _ScAttendDic[sid].ID;
                    record.RefStudentID = sid;
                    record.RefExamID = _RunningExamId;
                    record.ExamScore = exam_score;
                    record.RegularScore = regular_score;
                    //兩種分數只要其中一個有值就加入新增清單
                    if (!string.IsNullOrWhiteSpace(record.ExamScore) || !string.IsNullOrWhiteSpace(record.RegularScore))
                        insert.Add(record.toJHSCETakeRecord());
                }
            }

            if (update.Count > 0)
            {
                K12.Data.SCETake.Update(update);
                FiscaLogWriter(update, UpdateMode);
            }

            if (insert.Count > 0)
            {
                K12.Data.SCETake.Insert(insert);
                FiscaLogWriter(insert, InsertMode);
            }

            if (delete.Count > 0)
            {
                K12.Data.SCETake.Delete(delete);
                FiscaLogWriter(delete, DeleteMode);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
                dgv.EndEdit();

                if (_RunningExamId == null)
                {
                    FISCA.Presentation.Controls.MsgBox.Show("沒有試別無法儲存.");
                    return;
                }
                if (!IsValid())
                {
                    MessageBox.Show("儲存資料有誤,無法儲存");
                    return;
                }
                if (HasGreenText())
                {
                    if (MessageBox.Show("資料存在非0~100分的成績,確定要儲存?", "ischool", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                pictureBox1.Visible = true;
                _BW.RunWorkerAsync();
        }

        private void cboExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cboSelectedItem == null)
                _cboSelectedItem = new object();

            //選擇與上次相同Item則不做畫面ReLoad
            if (!_cboSelectedItem.Equals(cboExamList.SelectedItem))
            {
                if (_dirtyCellList.Count > 0)
                {
                    if (MessageBox.Show("資料尚未儲存,確定切換?", "ischool", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        //不切換則選回上次的Item
                        cboExamList.SelectedItem = _cboSelectedItem;
                        return;
                    }
                }

                _cboSelectedItem = cboExamList.SelectedItem;
                ReLoad();
            }
        }

        /// <summary>
        /// 當欄位結束編輯，進行驗證。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != chInputScore.Index && e.ColumnIndex != chInputAssignmentScore.Index) return;

            DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            //驗證分數及分數顏色修正
            FixTextColor(cell);

            //Cell變更檢查
            if ("" + cell.Tag != "" + cell.Value)
            {
                if (!_dirtyCellList.Contains(cell)) _dirtyCellList.Add(cell);
            }
            else
            {
                if (_dirtyCellList.Contains(cell)) _dirtyCellList.Remove(cell);
            }

            lblSave.Visible = _dirtyCellList.Count > 0;
        }

        /// <summary>
        /// 點欄位立即進入編輯。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == chInputScore.Index || e.ColumnIndex == chInputAssignmentScore.Index)
                dgv.BeginEdit(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CourseScoreInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgv.EndEdit();
            if (_dirtyCellList.Count > 0)
            {
                if (MessageBox.Show("資料尚未儲存,確定關閉?", "ischool", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private class ExamComboBoxItem
        {
            private int _value;
            private string _key;
            public string DisplayText
            {
                get
                {
                    return _key;
                }
            }

            public int Value
            {
                get
                {
                    return _value;
                }
            }

            public ExamComboBoxItem(string key, int value)
            {
                _key = key;
                _value = value;
            }
        }
    }
}
