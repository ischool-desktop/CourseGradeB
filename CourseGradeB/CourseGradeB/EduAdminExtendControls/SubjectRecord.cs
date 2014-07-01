using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB.EduAdminExtendControls
{
    [FISCA.UDT.TableName("ischool.subject.list")]
    class SubjectRecord : ActiveRecord
    {
        [FISCA.UDT.Field(Field = "name")]
        public string Name { get; set; }

        [FISCA.UDT.Field(Field = "english_name")]
        public string EnglishName { get; set; }

        [FISCA.UDT.Field(Field = "group")]
        public string Group { get; set; }

        [FISCA.UDT.Field(Field = "type")]
        public string Type { get; set; }

        
    }
}
