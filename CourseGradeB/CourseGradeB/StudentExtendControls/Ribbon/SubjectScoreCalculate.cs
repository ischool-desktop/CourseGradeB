using FISCA.Data;
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

namespace CourseGradeB.StudentExtendControls.Ribbon
{
    public partial class SubjectScoreCalculate : BaseForm
    {
        private int _schoolYear, _semester;
        List<string> _ids;
        BackgroundWorker _BW;
        QueryHelper _Q;
        Dictionary<string, SemesterHistoryRecord> _Update_sems_history;
        List<string> _Student_schoolyear_sems;
        List<string> _logStr;


        public SubjectScoreCalculate(List<string> ids)
        {
            InitializeComponent();
            _ids = ids;
            _Update_sems_history = new Dictionary<string, SemesterHistoryRecord>();
            _Student_schoolyear_sems = new List<string>();
            _logStr = new List<string>();

            _Q = new QueryHelper();
            _BW = new BackgroundWorker();
            _BW.DoWork += new DoWorkEventHandler(BW_Dowork);
            _BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_Completed);


            _schoolYear = int.Parse(K12.Data.School.DefaultSchoolYear);
            _semester = int.Parse(K12.Data.School.DefaultSemester);

            for (int i = -2; i <= 2; i++)
                cboSchoolYear.Items.Add(_schoolYear + i);

            cboSemester.Items.Add("1");
            cboSemester.Items.Add("2");

            cboSchoolYear.Text = _schoolYear + "";
            cboSemester.Text = _semester + "";
        }

        private void BW_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("成績計算完成");
            this.Close();
        }

        private void BW_Dowork(object sender, DoWorkEventArgs e)
        {
            //查詢成績
            string id = string.Join(",", _ids);
            string sqlcmd = "select student.name,student.student_number,student.seat_no,class.class_name,teacher.teacher_name,class.grade_year,course.id as course_id,$ischool.course.extend.grade_year as course_grade_year,course.period as period,course.credit as credit,course.subject as subject,$ischool.subject.list.group as group,$ischool.subject.list.type as type,sc_attend.ref_student_id as student_id,sce_take.ref_sc_attend_id as sc_attend_id,sce_take.ref_exam_id as exam_id,xpath_string(sce_take.extension,'//Score') as score ";
            sqlcmd += "from sc_attend ";
            sqlcmd += "join sce_take on sce_take.ref_sc_attend_id=sc_attend.id ";
            sqlcmd += "join course on course.id=sc_attend.ref_course_id ";
            sqlcmd += "join $ischool.course.extend on $ischool.course.extend.ref_course_id=course.id ";
            sqlcmd += "left join student on student.id=sc_attend.ref_student_id ";
            sqlcmd += "left join class on student.ref_class_id=class.id ";
            sqlcmd += "left join $ischool.subject.list on course.subject=$ischool.subject.list.name ";
            sqlcmd += "left join teacher on teacher.id=class.ref_teacher_id ";
            sqlcmd += "where sc_attend.ref_student_id in (" + id + ") and course.school_year=" + _schoolYear + " and course.semester=" + _semester + " and $ischool.course.extend.grade_year>2";

            DataTable dt = _Q.Select(sqlcmd);
            Dictionary<string, SubjectScoreObj> obj_dic = new Dictionary<string, SubjectScoreObj>();
            List<string> sc_attend_list = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string key = row["sc_attend_id"] + "";

                if (!sc_attend_list.Contains(key))
                    sc_attend_list.Add(key);

                if (!obj_dic.ContainsKey(key))
                    obj_dic.Add(key, new SubjectScoreObj(row));

                string exam_id = row["exam_id"] + "";
                string str_score = row["score"] + "";
                decimal score = 0;

                if (decimal.TryParse(str_score, out score))
                {
                    if (exam_id == "1")
                        obj_dic[key].exam_score_1 = score;

                    if (exam_id == "2")
                        obj_dic[key].exam_score_2 = score;
                }
            }

            //取得休課紀錄
            Dictionary<string, K12.Data.SCAttendRecord> sc_attend_dic = new Dictionary<string, K12.Data.SCAttendRecord>();
            foreach (var record in K12.Data.SCAttend.SelectByIDs(sc_attend_list))
            {
                if (!sc_attend_dic.ContainsKey(record.ID))
                    sc_attend_dic.Add(record.ID, record);
            }

            //取得學期成績紀錄
            Dictionary<string, K12.Data.SemesterScoreRecord> update_sems_score_dic = new Dictionary<string, SemesterScoreRecord>();
            Dictionary<string, K12.Data.SemesterScoreRecord> insert_sems_score_dic = new Dictionary<string, SemesterScoreRecord>();
            foreach (var record in K12.Data.SemesterScore.SelectBySchoolYearAndSemester(_ids, _schoolYear, _semester))
            {
                if (!update_sems_score_dic.ContainsKey(record.RefStudentID))
                    update_sems_score_dic.Add(record.RefStudentID, record);
            }

            //取得學期歷程紀錄
            _Update_sems_history.Clear();
            _Student_schoolyear_sems.Clear();
            foreach (SemesterHistoryRecord shr in K12.Data.SemesterHistory.SelectByStudentIDs(_ids))
            {
                if (!_Update_sems_history.ContainsKey(shr.RefStudentID))
                    _Update_sems_history.Add(shr.RefStudentID, shr);

                foreach (SemesterHistoryItem item in shr.SemesterHistoryItems)
                    _Student_schoolyear_sems.Add(shr.RefStudentID + "_" + item.SchoolYear + "_" + item.Semester);
            }

            _logStr.Clear();
            _logStr.Add("學年度:" + _schoolYear + " 學期:" + _semester);
            //遞迴每個休課紀錄
            foreach (SubjectScoreObj obj in obj_dic.Values)
            {
                //平均分數
                decimal score = (obj.exam_score_1 + obj.exam_score_2) / 2;
                score = Math.Round(score, 2, MidpointRounding.AwayFromZero);

                //設定課程成績
                if (sc_attend_dic.ContainsKey(obj.ScAttendId))
                    sc_attend_dic[obj.ScAttendId].Score = Math.Round(score, 2, MidpointRounding.AwayFromZero);

                //設定學期成績
                //學期成績-update清單
                if (update_sems_score_dic.ContainsKey(obj.StudentId))
                {
                    K12.Data.SemesterScoreRecord ssr = update_sems_score_dic[obj.StudentId];

                    if (!ssr.Subjects.ContainsKey(obj.SubjectName))
                        ssr.Subjects.Add(obj.SubjectName, new SubjectScore());

                    SetScore(ssr.Subjects[obj.SubjectName], score, obj);

                    CheckYearAndSemester(obj);
                }
                else
                {
                    //學期成績-insert清單
                    if (!insert_sems_score_dic.ContainsKey(obj.StudentId))
                        insert_sems_score_dic.Add(obj.StudentId, new SemesterScoreRecord(obj.StudentId, _schoolYear, _semester));

                    K12.Data.SemesterScoreRecord ssr = insert_sems_score_dic[obj.StudentId];

                    if (!ssr.Subjects.ContainsKey(obj.SubjectName))
                        ssr.Subjects.Add(obj.SubjectName, new SubjectScore());

                    SetScore(ssr.Subjects[obj.SubjectName], score, obj);

                    CheckYearAndSemester(obj);
                }

                //log
                string str = "姓名:" + obj.StudentName + " 學號:" + obj.StudentNumberID + " 座號:" + obj.SeatNo;
                if (!_logStr.Contains(str))
                    _logStr.Add(str);
            }

            if (sc_attend_dic.Count > 0)
                K12.Data.SCAttend.Update(sc_attend_dic.Values);

            if (update_sems_score_dic.Count > 0)
            {
                SetAvg(update_sems_score_dic);
                K12.Data.SemesterScore.Update(update_sems_score_dic.Values);
            }

            if (insert_sems_score_dic.Count > 0)
            {
                SetAvg(insert_sems_score_dic);
                K12.Data.SemesterScore.Insert(insert_sems_score_dic.Values);
            }

            if (_Update_sems_history.Count > 0)
                K12.Data.SemesterHistory.Update(_Update_sems_history.Values);

            if (_logStr.Count > 1)
            {
                string str = string.Join("\r\n", _logStr);
                FISCA.LogAgent.ApplicationLog.Log("學生學期科目成績計算(學生)", "計算學生學期科目成績", str);
            }

            //更新累計GPA平均
            Tool.SetCumulateGPA(_ids, _schoolYear, _semester);
        }

        private void SubjectScoreCalculate_Load(object sender, EventArgs e)
        {
            //K12.Data.SemesterScore.Delete(K12.Data.SemesterScore.SelectAll());
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _schoolYear = int.Parse(cboSchoolYear.Text);
            _semester = int.Parse(cboSemester.Text);

            if (!_BW.IsBusy)
                _BW.RunWorkerAsync();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetScore(SubjectScore ss, decimal score, SubjectScoreObj obj)
        {
            ss.Score = score;
            ss.Period = obj.Period;
            ss.Credit = obj.Credit;
            ss.Domain = obj.SubjectGroup;
            //ss.GPA = Tool.Instance.GetScoreGrade(score, obj.SubjectTypeEnum);

            if (obj.SubjectTypeEnum == Tool.SubjectType.Honor)
                ss.GPA = Tool.GPA.Eval(score).Honors;
            else
                ss.GPA = Tool.GPA.Eval(score).Regular;

            ss.Level = obj.CourseGradeYear;
        }

        private void SetAvg(Dictionary<string, K12.Data.SemesterScoreRecord> dic)
        {
            foreach (SemesterScoreRecord ssr in dic.Values)
            {
                decimal totalScore = 0;
                decimal totalGPA = 0;
                decimal count = 0;
                foreach (SubjectScore ss in ssr.Subjects.Values)
                {
                    totalScore += ss.Score.Value * ss.Credit.Value;
                    totalGPA += ss.GPA.Value * ss.Credit.Value;
                    count += ss.Credit.Value;
                }

                if (count > 0)
                {
                    ssr.AvgScore = Math.Round(totalScore / count, 2, MidpointRounding.AwayFromZero);
                    ssr.AvgGPA = Math.Round(totalGPA / count, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    ssr.AvgScore = null;
                    ssr.AvgGPA = null;
                }
            }
        }

        private void CheckYearAndSemester(SubjectScoreObj obj)
        {
            //若學生缺少該學年度學期的歷程,則自動建立
            string key = obj.StudentId + "_" + _schoolYear + "_" + _semester;
            if (!_Student_schoolyear_sems.Contains(key) && obj.GradeYear != 0)
            {
                _Student_schoolyear_sems.Add(key);
                SemesterHistoryItem item = new SemesterHistoryItem();
                item.SchoolYear = _schoolYear;
                item.Semester = _semester;
                item.GradeYear = obj.GradeYear;
                item.SeatNo = obj.SeatNo;
                item.ClassName = obj.ClassName;
                item.Teacher = obj.TeacherName;
                _Update_sems_history[obj.StudentId].SemesterHistoryItems.Add(item);
            }
        }

    }
}
