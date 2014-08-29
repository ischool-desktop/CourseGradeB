using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB.EduAdminExtendControls
{
    [FISCA.UDT.TableName("ischool.gparef")]
    public class GpaRef : ActiveRecord
    {
        [FISCA.UDT.Field(Field = "school_year")]
        public int SchoolYear { get; set; }

        [FISCA.UDT.Field(Field = "semester")]
        public int Semester { get; set; }

        [FISCA.UDT.Field(Field = "grade")]
        public int Grade { get; set; }

        [FISCA.UDT.Field(Field = "max")]
        public decimal Max { get; set; }

        [FISCA.UDT.Field(Field = "avg")]
        public decimal Avg { get; set; }
    }
}
