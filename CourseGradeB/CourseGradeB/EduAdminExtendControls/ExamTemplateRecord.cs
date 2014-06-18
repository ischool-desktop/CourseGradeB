using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB.EduAdminExtendControls
{
    [FISCA.UDT.TableName("ischool.exam.template")]
    class ExamTemplateRecord : ActiveRecord
    {
        [FISCA.UDT.Field(Field = "name")]
        public string Name { get; set; }

        [FISCA.UDT.Field(Field = "exam_scale")]
        public string ExamScale { get; set; }

        [FISCA.UDT.Field(Field = "setting")]
        public string Setting { get; set; }
    }
}
