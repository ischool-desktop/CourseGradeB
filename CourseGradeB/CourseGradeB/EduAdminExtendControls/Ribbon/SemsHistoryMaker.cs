using FISCA.Data;
using FISCA.Presentation.Controls;
using Framework;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    public partial class SemsHistoryMaker : BaseForm
    {
        private const string SchoolHodidayConfigString = "SCHOOL_HOLIDAY_CONFIG_STRING";
        private const string configString = "CONFIG_STRING";

        int _schoolYear, _semester;
        BackgroundWorker _BW;
        QueryHelper _Q;
        ConfigData _CD;
        Dictionary<string, int?> _GradeSchoolDays;

        public SemsHistoryMaker()
        {
            InitializeComponent();

            _schoolYear = int.Parse(K12.Data.School.DefaultSchoolYear);
            _semester = int.Parse(K12.Data.School.DefaultSemester);

            lblStatus.Text = "學年度: " + _schoolYear + " 學期: " + _semester;

            _BW = new BackgroundWorker();
            _BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            _BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_Completed);
            _Q = new QueryHelper();
            _CD = JHSchool.School.Configuration[SchoolHodidayConfigString];
            _GradeSchoolDays = new Dictionary<string, int?>();

            //取得上課天數設定
            XElement rootXml = null;
            string xmlContent = _CD[configString];

            if (!string.IsNullOrWhiteSpace(xmlContent))
                rootXml = XElement.Parse(xmlContent);
            else
                rootXml = new XElement("SchoolHolidays");

            //取得全校既有的年級並帶入設定
            DataTable dt = _Q.Select("select distinct grade_year from class where grade_year is not null order by grade_year");
            foreach (DataRow row in dt.Rows)
            {
                string grade = row["grade_year"] + "";
                string elemGrade = "//SchoolDayCountG" + grade;
                XElement elem = rootXml.XPathSelectElement(elemGrade);
                string value = elem == null ? string.Empty : elem.Value;

                if (!_GradeSchoolDays.ContainsKey(grade))
                    _GradeSchoolDays.Add(grade, null);

                int i;
                if (int.TryParse(value, out i))
                    _GradeSchoolDays[grade] = i;
            }
        }

        private void BW_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("學期歷程建立完成");
            picLoading.Visible = false;
            btnStart.Enabled = true;
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            //撈取全部學生
            Dictionary<string, StudentObj> student_obj_dic = new Dictionary<string, StudentObj>();
            string sqlcmd = "select student.id, student.seat_no,class.class_name,class.grade_year,teacher.teacher_name from student ";
            sqlcmd += "left join class on class.id=student.ref_class_id ";
            sqlcmd += "left join teacher on class.ref_teacher_id=teacher.id ";
            sqlcmd += "where student.status=1";

            DataTable dt = _Q.Select(sqlcmd);
            foreach (DataRow row in dt.Rows)
            {
                string id = row["id"] + "";
                if (!student_obj_dic.ContainsKey(id))
                    student_obj_dic.Add(id, new StudentObj(row));
            }

            //一般狀態學生SemesterHistoryRecord
            Dictionary<string, SemesterHistoryRecord> student_history_dic = new Dictionary<string, SemesterHistoryRecord>();
            foreach (SemesterHistoryRecord record in K12.Data.SemesterHistory.SelectByStudentIDs(student_obj_dic.Keys))
            {
                if (!student_history_dic.ContainsKey(record.RefStudentID))
                    student_history_dic.Add(record.RefStudentID, record);
            }

            //砍掉指定學年度學期的item
            foreach (SemesterHistoryRecord r in student_history_dic.Values)
            {
                foreach (SemesterHistoryItem item in r.SemesterHistoryItems.ToArray())
                {
                    if (item.SchoolYear == _schoolYear && item.Semester == _semester)
                        r.SemesterHistoryItems.Remove(item);
                }
            }

            //建立指定學年度學期的item
            foreach (SemesterHistoryRecord r in student_history_dic.Values)
            {
                string id = r.RefStudentID;
                SemesterHistoryItem item = new SemesterHistoryItem();
                item.SchoolYear = _schoolYear;
                item.Semester = _semester;
                item.GradeYear = student_obj_dic.ContainsKey(id) ? student_obj_dic[id].GradeYear : 0;
                item.SeatNo = student_obj_dic.ContainsKey(id) ? student_obj_dic[id].SeatNo : null;
                item.ClassName = student_obj_dic.ContainsKey(id) ? student_obj_dic[id].ClassName : "";
                item.Teacher = student_obj_dic.ContainsKey(id) ? student_obj_dic[id].TeacherName : "";
                item.SchoolDayCount = _GradeSchoolDays.ContainsKey(item.GradeYear + "") ? _GradeSchoolDays[item.GradeYear + ""] : null;

                r.SemesterHistoryItems.Add(item);
            }

            K12.Data.SemesterHistory.Update(student_history_dic.Values);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!_BW.IsBusy)
            {
                btnStart.Enabled = false;
                picLoading.Visible = true;
                _BW.RunWorkerAsync();
            }
            else
                MessageBox.Show("系統忙碌中,請稍後再試...");
        }

    }

    public class StudentObj
    {
        public string Id,ClassName, TeacherName;
        private string gradeYear,seatNo;
        public StudentObj(DataRow row)
        {
            Id = row["id"] + "";
            seatNo = row["seat_no"] + "";
            ClassName = row["class_name"] + "";
            gradeYear = row["grade_year"] + "";
            TeacherName = row["teacher_name"] + "";
        }

        public int GradeYear
        {
            get
            {
                int i = 0;
                if (int.TryParse(gradeYear, out i))
                    return i;
                else
                    return 0;
            }
        }

        public int? SeatNo
        {
            get
            {
                int i = 0;
                if (int.TryParse(seatNo, out i))
                    return i;
                else
                    return null;
            }
        }
    }
}
