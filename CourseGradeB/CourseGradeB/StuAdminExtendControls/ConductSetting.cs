using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB.StuAdminExtendControls
{
    [FISCA.UDT.TableName("ischool.eval.config")]
    public class ConductSetting : ActiveRecord
    {
        [FISCA.UDT.Field(Field = "grade")]
        public int Grade { get; set; }
        	
        [FISCA.UDT.Field(Field = "middle_begin")]
        public DateTime MiddleBegin { get; set; }

        [FISCA.UDT.Field(Field = "middle_end")]
        public DateTime MiddleEnd { get; set; }

        [FISCA.UDT.Field(Field = "final_begin")]
        public DateTime FinalBegin { get; set; }

        [FISCA.UDT.Field(Field = "final_end")]
        public DateTime FinalEnd { get; set; }

        [FISCA.UDT.Field(Field = "middle_begin_c")]
        public DateTime MiddleBeginC { get; set; }

        [FISCA.UDT.Field(Field = "middle_end_c")]
        public DateTime MiddleEndC { get; set; }

        [FISCA.UDT.Field(Field = "final_begin_c")]
        public DateTime FinalBeginC { get; set; }

        [FISCA.UDT.Field(Field = "final_end_c")]
        public DateTime FinalEndC { get; set; }

        [FISCA.UDT.Field(Field = "conduct")]
        public string Conduct { get; set; }
    }
}
