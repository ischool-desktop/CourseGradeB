using CourseGradeB.EduAdminExtendControls;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using JHSchool;
using JHSchool.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CourseGradeB.CourseExtendControls.Ribbon
{
    public partial class CourseScoreInputForm : BaseForm
    {
        private CourseRecord _course;
        private string _RunningExamId;
        private string _RunningExamName = string.Empty;
        QueryHelper _Q;

        private List<JHSCAttendRecord> _scAttendRecordList;
        Dictionary<string, SCETakeRecord> _SceTakeDic;
        Dictionary<string, JHSCAttendRecord> _ScAttendDic;
        private List<DataGridViewCell> _dirtyCellList;
        private object _cboSelectedItem;

        /// <summary>
        /// Constructor
        /// 傳入一個課程。
        /// </summary>
        /// <param name="course"></param>
        public CourseScoreInputForm(CourseRecord course)
        {
            InitializeComponent();
            _course = course;
            _Q = new QueryHelper();
            _SceTakeDic = new Dictionary<string, SCETakeRecord>();
            _ScAttendDic = new Dictionary<string, JHSCAttendRecord>();
            _dirtyCellList = new List<DataGridViewCell>();

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

        private void CourseScoreInputForm_Load(object sender, EventArgs e)
        {
            #region 取得修課學生

            //_scAttendRecordList = _course.GetAttends();
            _scAttendRecordList = JHSCAttend.SelectByCourseIDs(new string[] { _course.ID });

            _ScAttendDic.Clear();
            foreach (JHSCAttendRecord record in _scAttendRecordList)
            {
                if (!_ScAttendDic.ContainsKey(record.RefStudentID))
                    _ScAttendDic.Add(record.RefStudentID, record);
            }

            FillStudentsToDataGridView();

            #endregion

            #region 取得評量設定

            FillToComboBox(_course.ID);

            // 當沒有試別關閉
            if (cboExamList.Items.Count < 1)
                this.Close();
            else
                cboExamList.SelectedIndex = 0;
            #endregion
        }

        /// <summary>
        /// 將學生填入DataGridView。
        /// </summary>
        private void FillStudentsToDataGridView()
        {
            dgv.SuspendLayout();
            dgv.Rows.Clear();

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

            string aa = aClass == null ? (string.Empty).PadLeft(10, '0') : (aClass.ID).PadLeft(10, '0');
            aa += aStudent == null ? (string.Empty).PadLeft(3, '0') : (aStudent.SeatNo + "").PadLeft(3, '0');
            aa += aStudent == null ? (string.Empty).PadLeft(10, '0') : (aStudent.StudentNumber).PadLeft(10, '0');

            string bb = bClass == null ? (string.Empty).PadLeft(10, '0') : (bClass.ID).PadLeft(10, '0');
            bb += bStudent == null ? (string.Empty).PadLeft(3, '0') : (bStudent.SeatNo + "").PadLeft(3, '0');
            bb += bStudent == null ? (string.Empty).PadLeft(10, '0') : (bStudent.StudentNumber).PadLeft(10, '0');

            return aa.CompareTo(bb);
        }

        /// <summary>
        /// 將試別填入ComboBox。
        /// </summary>
        private void FillToComboBox(string id)
        {
            cboExamList.Items.Clear();

            //取得指定評分樣板內容
            DataTable dt = _Q.Select("SELECT * FROM $ischool.exam.template WHERE uid=(select ref_exam_template_id from $ischool.course.extend where ref_course_id=" + id + ")");
            if (dt.Rows.Count > 0)
            {
                string setting = dt.Rows[0]["setting"].ToString();

                if (!string.IsNullOrWhiteSpace(setting))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(setting);

                    foreach (XmlElement elem in doc.SelectNodes("//Item"))
                    {
                        ExamTemplateRecordItem item = new ExamTemplateRecordItem(elem);
                        cboExamList.Items.Add(new ExamComboBoxItem(item));
                    }
                }
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

            //系統歷程
            //Dictionary<int, DataGridViewRow> rowDic = new Dictionary<int, DataGridViewRow>();
            //StringBuilder insertLog = new StringBuilder();
            //StringBuilder updateLog = new StringBuilder();
            //StringBuilder deleteLog = new StringBuilder();
            
            //foreach (DataGridViewCell cell in _dirtyCellList)
            //{
            //    DataGridViewRow row = cell.OwningRow;

            //    if (!rowDic.ContainsKey(row.Index))
            //        rowDic.Add(row.Index, row);
            //    else
            //        continue;

            //    string strClassName = row.Cells[chClassName.Index].Value + "";
            //    string strSeatNo = row.Cells[chSeatNo.Index].Value + "";
            //    string strName = row.Cells[chName.Index].Value + "";
            //    string strStudentNumber = row.Cells[chStudentNumber.Index].Value + "";

            //    string before_examScore = row.Cells[chInputScore.Index].Tag + "";
            //    string after_examScore = row.Cells[chInputScore.Index].Value + "";
            //    string before_regularScore = row.Cells[chInputAssignmentScore.Index].Tag + "";
            //    string after_regularScore = row.Cells[chInputAssignmentScore.Index].Value + "";

            //    if(!string.IsNullOrWhiteSpace(before_examScore) || !string.IsNullOrWhiteSpace(before_regularScore))
            //        if (string.IsNullOrWhiteSpace(after_examScore) && string.IsNullOrWhiteSpace(after_regularScore))
            //        {
            //            if (deleteLog.ToString() == "")
            //                deleteLog.AppendLine("刪除成績:");

            //            deleteLog.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
            //            deleteLog.AppendLine("定期成績:" + before_examScore + ">>" + after_examScore);
            //            deleteLog.AppendLine("平時成績:" + before_regularScore + ">>" + after_regularScore);
            //            continue;
            //        }

            //     if(string.IsNullOrWhiteSpace(before_examScore) && string.IsNullOrWhiteSpace(before_regularScore))
            //         if (!string.IsNullOrWhiteSpace(after_examScore) || !string.IsNullOrWhiteSpace(after_regularScore))
            //         {
            //             if (insertLog.ToString() == "")
            //                 insertLog.AppendLine("新增成績:");

            //            insertLog.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
            //            insertLog.AppendLine("定期成績:" + before_examScore + ">>" + after_examScore);
            //            insertLog.AppendLine("平時成績:" + before_regularScore + ">>" + after_regularScore);
            //            continue;
            //         }

            //    if(before_examScore != after_examScore || before_regularScore != after_regularScore)
            //    {
            //        if (updateLog.ToString() == "")
            //            updateLog.AppendLine("修改成績:");

            //        updateLog.AppendLine("班級:" + strClassName + " 座號:" + strSeatNo + " 姓名:" + strName + " 學號:" + strStudentNumber);
            //        updateLog.AppendLine("定期成績:" + before_examScore + ">>" + after_examScore);
            //        updateLog.AppendLine("平時成績:" + before_regularScore + ">>" + after_regularScore);
            //        continue;
            //    }
            //}

            //if (deleteLog.ToString() != "")
            //    FISCA.LogAgent.ApplicationLog.Log("課程成績輸入", "成績輸入", deleteLog.ToString());
            //if (insertLog.ToString() != "")
            //    FISCA.LogAgent.ApplicationLog.Log("課程成績輸入", "成績輸入", insertLog.ToString());
            //if (updateLog.ToString() != "")
            //    FISCA.LogAgent.ApplicationLog.Log("課程成績輸入", "成績輸入", updateLog.ToString());

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
                    //如果該欄位不顯示就不更新資料
                    _SceTakeDic[sid].ExamScore = chInputScore.Visible ? exam_score : _SceTakeDic[sid].ExamScore;
                    _SceTakeDic[sid].RegularScore = chInputAssignmentScore.Visible ? regular_score : _SceTakeDic[sid].RegularScore;

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
                    record.RefExamID = _RunningExamId;
                    record.ExamScore = exam_score;
                    record.RegularScore = regular_score;
                    //兩種分數只要其中一個有值就加入新增清單
                    if (!string.IsNullOrWhiteSpace(record.ExamScore) || !string.IsNullOrWhiteSpace(record.RegularScore))
                        insert.Add(record.toJHSCETakeRecord());
                }
            }

            if (update.Count > 0)
                K12.Data.SCETake.Update(update);
            if (insert.Count > 0)
                K12.Data.SCETake.Insert(insert);
            if (delete.Count > 0)
                K12.Data.SCETake.Delete(delete);

            ReLoad();
        }

        private void cboExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cboSelectedItem == null)
                _cboSelectedItem = new object();

            if (!_cboSelectedItem.Equals(cboExamList.SelectedItem))
            {
                if (_dirtyCellList.Count > 0)
                {
                    if (MessageBox.Show("資料尚未儲存,確定切換?", "ischool", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        cboExamList.SelectedItem = _cboSelectedItem;
                        return;
                    }
                }

                _cboSelectedItem = cboExamList.SelectedItem;
                ReLoad();
            }
        }

        private void ReLoad()
        {
            _dirtyCellList.Clear();
            lblSave.Visible = false;
            _SceTakeDic.Clear();
            ExamComboBoxItem cboitem = cboExamList.SelectedItem as ExamComboBoxItem;
            ExamTemplateRecordItem item = cboitem.Item as ExamTemplateRecordItem;
            chInputScore.Visible = item.ExamNeed;
            chInputAssignmentScore.Visible = item.DailyNeed;
            _RunningExamId = item.ExamID + "";
            _RunningExamName = item.ExamName;

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
        /// <returns></returns>
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

        private class ExamComboBoxItem
        {
            public string DisplayText
            {
                get
                {
                    return _item.ExamName;
                }
            }

            private ExamTemplateRecordItem _item;

            public ExamTemplateRecordItem Item
            {
                get
                {
                    return _item;
                }
            }

            public ExamComboBoxItem(ExamTemplateRecordItem item)
            {
                _item = item;
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

            #region 檢查是否有變更過
            if ("" + cell.Tag != "" + cell.Value)
            {
                if (!_dirtyCellList.Contains(cell)) _dirtyCellList.Add(cell);
            }
            else
            {
                if (_dirtyCellList.Contains(cell)) _dirtyCellList.Remove(cell);
            }

            lblSave.Visible = _dirtyCellList.Count > 0;
            #endregion
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
    }
}
