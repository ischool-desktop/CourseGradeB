using CourseGradeB.EduAdminExtendControls;
using FISCA.UDT;
using K12.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    public class Tool
    {
        private AccessHelper _A;
        private static Tool _instance;
        private Dictionary<string, string> _SubjectDic;
        private Dictionary<string, string> _CourseIdToSubjectType;
        public List<CourseRecord> Courses;

        private Tool()
        {
            _A = new AccessHelper();
            _SubjectDic = new Dictionary<string, string>();
            _CourseIdToSubjectType = new Dictionary<string, string>();
            Courses = new List<CourseRecord>();
        }

        public static Tool Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Tool();

                return _instance;
            }
        }

        public void Refresh()
        {
            _SubjectDic.Clear();
            _CourseIdToSubjectType.Clear();

            //取得科目資料
            foreach (SubjectRecord sr in _A.Select<SubjectRecord>())
            {
                if (!_SubjectDic.ContainsKey(sr.Name))
                    _SubjectDic.Add(sr.Name, sr.Type);
            }

            //遞迴指定的課程清單
            foreach (CourseRecord cr in Courses)
            {
                //建立課程ID對照key
                if (!_CourseIdToSubjectType.ContainsKey(cr.ID))
                    _CourseIdToSubjectType.Add(cr.ID, "");

                //指定課程ID對照value
                if (_SubjectDic.ContainsKey(cr.Subject))
                    _CourseIdToSubjectType[cr.ID] = _SubjectDic[cr.Subject];
            }
        }

        public string GetSubjectType(string id)
        {
            if (_CourseIdToSubjectType.ContainsKey(id))
                return _CourseIdToSubjectType[id];

            return string.Empty;
        }

        public enum SubjectType
        {
            Honor = 0,
            Regular = 1
        }

        public decimal GetScoreGrade(decimal score, SubjectType type)
        {
            decimal retVal = 0;
            switch (type)
            {
                case SubjectType.Honor:
                    if (score >= 97 && score <= 100)
                        retVal = 4.8M;
                    else if (score >= 93 && score < 97)
                        retVal = 4.5M;
                    else if (score >= 90 && score < 93)
                        retVal = 4.2M;
                    else if (score >= 87 && score < 90)
                        retVal = 3.8M;
                    else if (score >= 83 && score < 87)
                        retVal = 3.5M;
                    else if (score >= 80 && score < 83)
                        retVal = 3.2M;
                    else if (score >= 77 && score < 80)
                        retVal = 2.8M;
                    else if (score >= 73 && score < 77)
                        retVal = 2.5M;
                    else if (score >= 70 && score < 73)
                        retVal = 2.2M;
                    else if (score >= 67 && score < 70)
                        retVal = 1.8M;
                    else if (score >= 63 && score < 67)
                        retVal = 1.5M;
                    else if (score >= 60 && score < 63)
                        retVal = 1.2M;
                    else if (score >= 0 && score < 60)
                        retVal = 0;
                    break;
                default:
                    if (score >= 97 && score <= 100)
                        retVal = 4.3M;
                    else if (score >= 93 && score < 97)
                        retVal = 4.0M;
                    else if (score >= 90 && score < 93)
                        retVal = 3.7M;
                    else if (score >= 87 && score < 90)
                        retVal = 3.3M;
                    else if (score >= 83 && score < 87)
                        retVal = 3.0M;
                    else if (score >= 80 && score < 83)
                        retVal = 2.7M;
                    else if (score >= 77 && score < 80)
                        retVal = 2.3M;
                    else if (score >= 73 && score < 77)
                        retVal = 2.0M;
                    else if (score >= 70 && score < 73)
                        retVal = 1.7M;
                    else if (score >= 67 && score < 70)
                        retVal = 1.3M;
                    else if (score >= 63 && score < 67)
                        retVal = 1.0M;
                    else if (score >= 60 && score < 63)
                        retVal = 0.7M;
                    else if (score >= 0 && score < 60)
                        retVal = 0;
                    break;
            }
            return retVal;
        }

        public string GetStudentInfo(StudentRecord student)
        {
            ClassRecord cr = student.Class;
            string className = cr == null ? "" : cr.Name;

            string str = "學生:" + student.Name + " 班級:" + className + " 座號:" + student.SeatNo + " 學號:" + student.StudentNumber + "\r\n";
            return str;
        }

        public void SetCumulateGPA(List<string> ids,int schoolYear, int semester)
        {
            //學期歷程
            Dictionary<string, string> sems_history = new Dictionary<string, string>();
            foreach (SemesterHistoryRecord shr in K12.Data.SemesterHistory.SelectByStudentIDs(ids))
            {
                foreach (SemesterHistoryItem item in shr.SemesterHistoryItems)
                {
                    //學生年級對照
                    string key = item.RefStudentID + "_" + item.SchoolYear + "_" + item.Semester;
                    if (!sems_history.ContainsKey(key))
                    {
                        if (item.Semester == 1)
                            sems_history.Add(key, item.GradeYear + "上");
                        else
                            sems_history.Add(key, item.GradeYear + "下");
                    }

                }
            }

            //取得學期成績紀錄
            Dictionary<string, K12.Data.SemesterScoreRecord> update = new Dictionary<string, SemesterScoreRecord>();
            foreach (var record in K12.Data.SemesterScore.SelectBySchoolYearAndSemester(ids, schoolYear, semester))
            {
                if (!update.ContainsKey(record.RefStudentID))
                    update.Add(record.RefStudentID, record);
            }

            //結算累計GPA
            Dictionary<string, Dictionary<string, SemesterScoreRecord>> student_gpa_dic = new Dictionary<string, Dictionary<string, SemesterScoreRecord>>();
            foreach (SemesterScoreRecord r in K12.Data.SemesterScore.SelectByStudentIDs(ids))
            {
                //學年度學期判斷排除
                if (r.SchoolYear > schoolYear)
                    continue;
                else if (r.SchoolYear == schoolYear)
                {
                    if (r.Semester > semester)
                        continue;
                }

                string key = r.RefStudentID + "_" + r.SchoolYear + "_" + r.Semester;

                if (!student_gpa_dic.ContainsKey(r.RefStudentID))
                    student_gpa_dic.Add(r.RefStudentID, new Dictionary<string, SemesterScoreRecord>());

                if (sems_history.ContainsKey(key))
                {
                    string grade = sems_history[key];
                    if (grade == "9上" || grade == "9下" || grade == "10上" || grade == "10下" || grade == "11上" || grade == "11下" || grade == "12上" || grade == "12下")
                    {
                        if (!student_gpa_dic[r.RefStudentID].ContainsKey(grade))
                            student_gpa_dic[r.RefStudentID].Add(grade, r);

                        SemesterScoreRecord ssr = student_gpa_dic[r.RefStudentID][grade];

                        //如果有相同grade的record,以較新的為主
                        if (r.SchoolYear > ssr.SchoolYear)
                            student_gpa_dic[r.RefStudentID][grade] = r;
                        else if (r.SchoolYear == ssr.SchoolYear)
                        {
                            if (r.Semester > ssr.Semester)
                                student_gpa_dic[r.RefStudentID][grade] = r;
                        }
                    }
                }
            }

            foreach (string id in update.Keys)
            {
                if (!student_gpa_dic.ContainsKey(id))
                    continue;

                SemesterScoreRecord ssr = update[id];

                decimal total = 0;
                int count = 0;

                foreach (SemesterScoreRecord r in student_gpa_dic[id].Values)
                {
                    if (r.AvgGPA.HasValue)
                    {
                        total += r.AvgGPA.Value;
                        count++;
                    }
                }

                if (count > 0)
                    ssr.CumulateGPA = Math.Round(total / count, 2, MidpointRounding.AwayFromZero);
            }

            K12.Data.SemesterScore.Update(update.Values);
        }

        public struct Domain
        {
            public string Name;
            public decimal Hours;
            public int DisplayOrder;
        }

        public static Dictionary<int, List<Domain>> DomainDic = new Dictionary<int, List<Domain>>()
        {
            {6,new List<Domain>()
            {
                {new Domain{Name="Chinese",Hours=6,DisplayOrder=1}},
                {new Domain{Name="Humanities",Hours=8,DisplayOrder=2}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3}},
                {new Domain{Name="Science",Hours=4,DisplayOrder=4}},
                {new Domain{Name="Chinese Social Studies",Hours=3,DisplayOrder=5}},
                {new Domain{Name="Physical Education",Hours=0,DisplayOrder=6}},
                {new Domain{Name="Humanity",Hours=8,DisplayOrder=7}},
                {new Domain{Name="Art",Hours=2,DisplayOrder=8}},
                {new Domain{Name="Music",Hours=2,DisplayOrder=9}},
                {new Domain{Name="Computer",Hours=1,DisplayOrder=10}},
                {new Domain{Name="Chinese Culture",Hours=1,DisplayOrder=11}}
            }},
            {8,new List<Domain>(){
                {new Domain{Name="Chinese",Hours=5,DisplayOrder=1}},
                {new Domain{Name="English",Hours=6,DisplayOrder=2}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3}},
                {new Domain{Name="Science",Hours=5,DisplayOrder=4}},
                {new Domain{Name="Chinese Social Studies",Hours=2,DisplayOrder=5}},
                {new Domain{Name="Western Social Studies",Hours=2,DisplayOrder=6}},
                {new Domain{Name="Computer",Hours=1,DisplayOrder=7}},
                {new Domain{Name="Music",Hours=1,DisplayOrder=8}},
                {new Domain{Name="Physical Education",Hours=0,DisplayOrder=9}},
                {new Domain{Name="Scouting",Hours=1,DisplayOrder=10}},
                {new Domain{Name="Art",Hours=1,DisplayOrder=11}}
            }},
            {12,new List<Domain>(){
                {new Domain{Name="Chinese",Hours=5,DisplayOrder=1}},
                {new Domain{Name="English",Hours=6,DisplayOrder=2}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3}},
                {new Domain{Name="Science",Hours=5,DisplayOrder=4}},
                {new Domain{Name="Social Studies",Hours=4,DisplayOrder=5}},
                {new Domain{Name="Physical Education",Hours=2,DisplayOrder=6}},
                {new Domain{Name="Elective",Hours=4,DisplayOrder=7}}
            }}
        };
    }
}
