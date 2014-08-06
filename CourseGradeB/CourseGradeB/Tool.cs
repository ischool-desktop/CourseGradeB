using CourseGradeB.EduAdminExtendControls;
using FISCA.UDT;
using K12.Data;
using System;
using System.Collections;
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

                //_instance.Refresh();
                return _instance;
            }
        }

        /// <summary>
        /// 取得科目排序(每次呼叫都會更新資料)
        /// </summary>
        /// <returns></returns>
        public static SubjectCompare GetSubjectCompare()
        {
            return new SubjectCompare();
        }

        /// <summary>
        /// 重新取得科目清單
        /// </summary>
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

        //public static string GetKey(params string[] keys)
        //{
        //    return null;
        //}

        public static string GetStudentInfo(StudentRecord student)
        {
            ClassRecord cr = student.Class;
            string className = cr == null ? "" : cr.Name;

            string str = "學生:" + student.Name + " 班級:" + className + " 座號:" + student.SeatNo + " 學號:" + student.StudentNumber + "\r\n";
            return str;
        }

        /// <summary>
        /// 計算指定學生在該學年度學期的平均分數
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="schoolYear"></param>
        /// <param name="semester"></param>
        public static void SetAverage(List<string> ids, int schoolYear, int semester)
        {
            List<SemesterScoreRecord> list = K12.Data.SemesterScore.SelectBySchoolYearAndSemester(ids, schoolYear, semester);

            foreach (SemesterScoreRecord ssr in list)
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

            if (list.Count > 0)
                K12.Data.SemesterScore.Update(list);
        }

        /// <summary>
        /// 計算指定學生在該學年度學期的累計GPA
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="schoolYear"></param>
        /// <param name="semester"></param>
        public static void SetCumulateGPA(List<string> ids, int schoolYear, int semester)
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
            public string ShortName;
        }

        public static Dictionary<int, List<Domain>> DomainDic
        {
            get
            {
                return Tool.Instance.GetDoaminDic();
            }
        }

        private Dictionary<int, List<Domain>> GetDoaminDic()
        {
            Dictionary<int, List<Domain>> dic = new Dictionary<int, List<Domain>>()
            {
            {6,new List<Domain>()
            {
                {new Domain{Name="Chinese",Hours=6,DisplayOrder=1,ShortName="Chinese"}},
                {new Domain{Name="Humanities",Hours=8,DisplayOrder=2,ShortName="Humanities"}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3,ShortName="Math"}},
                {new Domain{Name="Science",Hours=4,DisplayOrder=4,ShortName="Science"}},
                {new Domain{Name="Chinese Social Studies",Hours=3,DisplayOrder=5,ShortName="C.S.S"}},
                {new Domain{Name="Physical Education",Hours=0,DisplayOrder=6,ShortName="P.E."}},
                {new Domain{Name="Humanity",Hours=8,DisplayOrder=7,ShortName="Humanity"}},
                {new Domain{Name="Art",Hours=2,DisplayOrder=8,ShortName="Art"}},
                {new Domain{Name="Music",Hours=2,DisplayOrder=9,ShortName="Music"}},
                {new Domain{Name="Computer",Hours=1,DisplayOrder=10,ShortName="Computer"}},
                {new Domain{Name="Chinese Culture",Hours=1,DisplayOrder=11,ShortName="C.C."}}
            }},
            {8,new List<Domain>(){
                {new Domain{Name="Chinese",Hours=5,DisplayOrder=1,ShortName="Chinese"}},
                {new Domain{Name="English",Hours=6,DisplayOrder=2,ShortName="English"}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3,ShortName="Math"}},
                {new Domain{Name="Science",Hours=5,DisplayOrder=4,ShortName="Science"}},
                {new Domain{Name="Chinese Social Studies",Hours=2,DisplayOrder=5,ShortName="C.S.S"}},
                {new Domain{Name="Western Social Studies",Hours=2,DisplayOrder=6,ShortName="WSS"}},
                {new Domain{Name="Computer",Hours=1,DisplayOrder=7,ShortName="Computer"}},
                {new Domain{Name="Music",Hours=1,DisplayOrder=8,ShortName="Music"}},
                {new Domain{Name="Physical Education",Hours=0,DisplayOrder=9,ShortName="P.E."}},
                {new Domain{Name="Scouting",Hours=1,DisplayOrder=10,ShortName="Scouting"}},
                {new Domain{Name="Art",Hours=1,DisplayOrder=11,ShortName="Art"}}
            }},
            {12,new List<Domain>(){
                {new Domain{Name="Chinese",Hours=5,DisplayOrder=1,ShortName="Chinese"}},
                {new Domain{Name="English",Hours=6,DisplayOrder=2,ShortName="English"}},
                {new Domain{Name="Mathematics",Hours=5,DisplayOrder=3,ShortName="Math"}},
                {new Domain{Name="Science",Hours=5,DisplayOrder=4,ShortName="Science"}},
                {new Domain{Name="Social Studies",Hours=4,DisplayOrder=5,ShortName="S.S."}},
                {new Domain{Name="Physical Education",Hours=2,DisplayOrder=6,ShortName="P.E."}},
                {new Domain{Name="Elective",Hours=4,DisplayOrder=7,ShortName="Elective"}}
            }}
        };

            return dic;
        }

        public class GPA
        {
            private static List<GPA> gpalist = new List<GPA>();

            static GPA()
            {
                gpalist.Add(new GPA(97, 4.3m, 4.8m, "A+"));
                gpalist.Add(new GPA(93, 4.0m, 4.5m, "A"));
                gpalist.Add(new GPA(90, 3.7m, 4.2m, "A-"));
                gpalist.Add(new GPA(87, 3.3m, 3.8m, "B+"));
                gpalist.Add(new GPA(83, 3.0m, 3.5m, "B"));
                gpalist.Add(new GPA(80, 2.7m, 3.2m, "B-"));
                gpalist.Add(new GPA(77, 2.3m, 2.8m, "C+"));
                gpalist.Add(new GPA(73, 2.0m, 2.5m, "C"));
                gpalist.Add(new GPA(70, 1.7m, 2.2m, "C-"));
                gpalist.Add(new GPA(67, 1.3m, 1.8m, "D+"));
                gpalist.Add(new GPA(63, 1.0m, 1.5m, "D"));
                gpalist.Add(new GPA(60, 0.7m, 1.2m, "D-"));
                gpalist.Add(new GPA(59, 0m, 0m, "F"));
            }

            public static GPA Eval(decimal score)
            {
                foreach (GPA gpa in gpalist)
                {
                    if (score >= gpa.Limit)
                        return gpa;
                }

                return gpalist[gpalist.Count - 1]; //最低那個。
            }

            public GPA(decimal limit, decimal regular, decimal honors, string letter)
            {
                Limit = limit;
                Regular = regular;
                Honors = honors;
                Letter = letter;
            }

            public decimal Limit { get; private set; }
            public decimal Regular { get; private set; }

            public decimal Honors { get; private set; }

            public string Letter { get; private set; }

            public override string ToString()
            {
                return string.Format("Regular:{0}, Honors:{1}, Letter:{2}, Limit:{3}", Regular, Honors, Letter, Limit);
            }
        }

        public static string XPathLiteral(string value)
        {
            // if the value contains only single or double quotes, construct
            // an XPath literal
            if (!value.Contains("\""))
            {
                return "\"" + value + "\"";
            }
            if (!value.Contains("'"))
            {
                return "'" + value + "'";
            }

            // if the value contains both single and double quotes, construct an
            // expression that concatenates all non-double-quote substrings with
            // the quotes, e.g.:
            //
            //    concat("foo", '"', "bar")
            StringBuilder sb = new StringBuilder();
            sb.Append("concat(");
            string[] substrings = value.Split('\"');
            for (int i = 0; i < substrings.Length; i++)
            {
                bool needComma = (i > 0);
                if (substrings[i] != "")
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("\"");
                    sb.Append(substrings[i]);
                    sb.Append("\"");
                    needComma = true;
                }
                if (i < substrings.Length - 1)
                {
                    if (needComma)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("'\"'");
                }

            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
