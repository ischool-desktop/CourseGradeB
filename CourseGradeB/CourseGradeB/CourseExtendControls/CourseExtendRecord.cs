using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB.CourseExtendControls
{
    [FISCA.UDT.TableName("ischool.course.extend")]
    class CourseExtendRecord : ActiveRecord
    {
        [FISCA.UDT.Field(Field = "ref_course_id")]
        public int Ref_course_id { get; set; }

        [FISCA.UDT.Field(Field = "grade_year")]
        public int GradeYear { get; set; }
    }
}
