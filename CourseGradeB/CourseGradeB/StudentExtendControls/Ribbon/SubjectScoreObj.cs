using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CourseGradeB.StudentExtendControls.Ribbon
{
    class SubjectScoreObj
    {
        public string CourseId, StudentId, ScAttendId;
        public string SubjectName, SubjectGroup, SubjectType, StudentName, StudentNumberID, ClassName, TeacherName;
        private string _GradeYear, _StudentSeatNo,_CourseGradeYear;
        public decimal exam_score_1, exam_score_2;
        public decimal Period, Credit;

        public SubjectScoreObj(DataRow row)
        {
            CourseId = row["course_id"] + "";
            StudentId = row["student_id"] + "";
            ScAttendId = row["sc_attend_id"] + "";
            SubjectName = row["subject"] + "";
            SubjectGroup = row["group"] + "";
            SubjectType = row["type"] + "";
            _GradeYear = row["grade_year"] + "";
            StudentName = row["name"] + "";
            StudentNumberID = row["student_number"] + "";
            _StudentSeatNo = row["seat_no"] + "";
            ClassName = row["class_name"] + "";
            TeacherName = row["teacher_name"] + "";
            _CourseGradeYear = row["course_grade_year"] + "";
            exam_score_1 = 0;
            exam_score_2 = 0;

            Period = 0;
            decimal.TryParse(row["period"] + "", out Period);
            Credit = 0;
            decimal.TryParse(row["credit"] + "", out Credit);
        }

        public int GradeYear
        {
            get
            {
                int retVal = 0;
                int.TryParse(_GradeYear, out retVal);

                return retVal;
            }
        }

        public int? SeatNo
        {
            get
            {
                int retVal = 0;
                if (int.TryParse(_StudentSeatNo, out retVal))
                    return retVal;
                else
                    return null;
            }
        }

        public Tool.SubjectType SubjectTypeEnum
        {
            get
            {
                if (SubjectType == "Honor")
                    return Tool.SubjectType.Honor;
                else
                    return Tool.SubjectType.Regular;
            }
        }

        public int? CourseGradeYear
        {
            get
            {
                int retVal = 0;
                if (int.TryParse(_CourseGradeYear, out retVal))
                    return retVal;
                else
                    return null;
            }
        }
    }
}
