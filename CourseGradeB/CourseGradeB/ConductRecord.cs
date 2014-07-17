using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CourseGradeB
{
     [FISCA.UDT.TableName("ischool.conduct")]
    public class ConductRecord : ActiveRecord
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
         private string _conduct { get; set; }

         [FISCA.UDT.Field(Field = "comment")]
         public string Comment { get; set; }

         private static XmlDocument xdoc;
         private static XmlElement root;

         public string Conduct
         {
             get
             {
                 if (xdoc == null)
                     xdoc = new XmlDocument();

                 if (root == null)
                     root = new XmlDocument().CreateElement("Conducts");

                 //爆炸馬上修正
                 try
                 {
                     if (string.IsNullOrWhiteSpace(_conduct))
                         _conduct = root.OuterXml;

                     xdoc.LoadXml(_conduct);
                 }
                 catch
                 {
                     _conduct = root.OuterXml;
                 }

                 return _conduct;
             }

             set
             {
                 _conduct = value;
             }
         }
    }
}
