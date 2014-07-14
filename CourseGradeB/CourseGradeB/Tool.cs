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

    }
}
