using CourseGradeB.CourseExtendControls;
using CourseGradeB.EduAdminExtendControls;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    class Global
    {
        public AccessHelper _A;
        private static Global _instance;
        public Dictionary<int, string> ExamTemplateCatch;
        public Dictionary<int, int> CourseExtendCatch;

        private Global()
        {
            _A = new AccessHelper();
            ExamTemplateCatch = new Dictionary<int, string>();
            CourseExtendCatch = new Dictionary<int, int>();
            Refresh();
        }

        public static Global Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Global();

                return _instance;
            }
        }

        public void Refresh()
        {
            GetExamTemplateDic();
            GetCourseExtendDic();
        }

        private void GetExamTemplateDic()
        {
            ExamTemplateCatch.Clear();
            List<ExamTemplateRecord> list = _A.Select<ExamTemplateRecord>();
            foreach (ExamTemplateRecord record in list)
            {
                int key = int.Parse(record.UID);
                if (!ExamTemplateCatch.ContainsKey(key))
                    ExamTemplateCatch.Add(key, record.Name);
            }
        }

        private void GetCourseExtendDic()
        {
            CourseExtendCatch.Clear();
            List<CourseExtendRecord> list = _A.Select<CourseExtendRecord>();
            foreach (CourseExtendRecord record in list)
            {
                if (!CourseExtendCatch.ContainsKey(record.Ref_course_id))
                    CourseExtendCatch.Add(record.Ref_course_id, record.Ref_exam_template_id);
            }
        }

        public string GetExamTemplateName(int key)
        {
            if (CourseExtendCatch.ContainsKey(key))
                if (ExamTemplateCatch.ContainsKey(CourseExtendCatch[key]))
                    return ExamTemplateCatch[CourseExtendCatch[key]];

            return string.Empty;
        }
    }
}
