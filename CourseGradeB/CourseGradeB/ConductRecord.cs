using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
     [FISCA.UDT.TableName("ischool.conduct")]
    class ConductRecord : ActiveRecord
    {
         [FISCA.UDT.Field(Field = "ref_student_id")]
         public int RefStudentId { get; set; }

         [FISCA.UDT.Field(Field = "term")]
         public string Term { get; set; }

         [FISCA.UDT.Field(Field = "subject")]
         public string Subject { get; set; }

         [FISCA.UDT.Field(Field = "school_year")]
         public int SchoolYear { get; set; }

         [FISCA.UDT.Field(Field = "semester")]
         public int Semester { get; set; }

         [FISCA.UDT.Field(Field = "conduct")]
         public string Conduct { get; set; }

         [FISCA.UDT.Field(Field = "comment")]
         public string Comment { get; set; }

    }
}
