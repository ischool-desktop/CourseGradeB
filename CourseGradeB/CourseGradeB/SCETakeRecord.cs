using JHSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CourseGradeB
{
    class SCETakeRecord
    {
        private JHSCETakeRecord _record;
        private string _examScore, _regularScore;

        public SCETakeRecord(JHSCETakeRecord record)
        {
            _record = record;

            //try to read score
            XmlElement root = _record.ToXML();
            XmlNode node1 = root.SelectSingleNode("Extension/Extension/Score");
            XmlNode node2 = root.SelectSingleNode("Extension/Extension/AssignmentScore");
            _examScore = node1 == null ? string.Empty : node1.InnerText;
            _regularScore = node2 == null ? string.Empty : node2.InnerText;
        }

        public JHSCETakeRecord toJHSCETakeRecord()
        {
            XmlElement root = _record.ToXML();
            XmlNode node1 = root.SelectSingleNode("Extension/Extension/Score");
            XmlNode node2 = root.SelectSingleNode("Extension/Extension/AssignmentScore");

            if (node1 == null)
            {
                node1 = root.OwnerDocument.CreateElement("Score");
                root.SelectSingleNode("Extension/Extension").AppendChild(node1);
            }

            if (node2 == null)
            {
                node2 = root.OwnerDocument.CreateElement("AssignmentScore");
                root.SelectSingleNode("Extension/Extension").AppendChild(node2);
            }

            node1.InnerText = _examScore;
            node2.InnerText = _regularScore;

            _record.Load(root);
            return _record;
        }

        public string ExamScore
        {
            get
            {
                return _examScore;
            }
            set
            {
                decimal score = 0;
                _examScore = decimal.TryParse(value + "", out score) ? score + "" : string.Empty;
            }
        }

        public string RegularScore
        {
            get
            {
                return _regularScore;
            }
            set
            {
                decimal score = 0;
                _regularScore = decimal.TryParse(value + "", out score) ? score + "" : string.Empty;
            }
        }

        public string RefSCAttendID
        {
            get
            {
                return _record.RefSCAttendID;
            }
            set
            {
                _record.RefSCAttendID = value;
            }
        }

        public string RefExamID
        {
            get
            {
                return _record.RefExamID;
            }
            set
            {
                _record.RefExamID = value;
            }
        }
    }
}
