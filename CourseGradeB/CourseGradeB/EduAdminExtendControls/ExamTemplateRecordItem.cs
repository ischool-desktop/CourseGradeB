using JHSchool.Data;
using K12.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CourseGradeB.EduAdminExtendControls
{
    class ExamTemplateRecordItem
    {
        public int ExamID, Weight;
        public bool ExamNeed, DailyNeed, ConductNeed;
        public DateTime StartTime, EndTime;
        public ExamTemplateRecordItem(XmlElement elem)
        {
            ExamID = string.IsNullOrWhiteSpace(elem.GetAttribute("ExamID") + "") ? 0 : int.Parse(elem.GetAttribute("ExamID") + "");
            Weight = string.IsNullOrWhiteSpace(elem.GetAttribute("Weight") + "") ? 0 : int.Parse(elem.GetAttribute("Weight") + "");
            ExamNeed = (elem.GetAttribute("ExamNeed") + "") == "1" ? true : false;
            DailyNeed = (elem.GetAttribute("DailyNeed") + "") == "1" ? true : false;
            ConductNeed = (elem.GetAttribute("ConductNeed") + "") == "1" ? true : false;
            StartTime = DateToSaveFormat(elem.GetAttribute("StartTime") + "");
            EndTime = DateToSaveFormat(elem.GetAttribute("EndTime") + "");
        }

        private DateTime DateToSaveFormat(string source)
        {
            //Parse資料
            DateTime dt = new DateTime();
            DateTime.TryParse("" + source, out dt);
            
            //最後時間
            return dt;
        }

        public string ExamName
        {
            get
            {
                if(ExamID != 0)
                {
                    return JHExam.SelectByID(ExamID + "").Name;
                }

                return string.Empty;
            }
        }
    }
}
