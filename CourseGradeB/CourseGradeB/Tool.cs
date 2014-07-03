using CourseGradeB.EduAdminExtendControls;
using FISCA.UDT;
using K12.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    class Tool
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
    }
}
