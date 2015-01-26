using CourseGradeB.EduAdminExtendControls;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    public class SubjectCompare : IComparer<string>
    {
        Dictionary<string, int> _subjOrder;

        public SubjectCompare()
        {
            //aaa
            AccessHelper a = new AccessHelper();
            List<SubjectRecord> list = a.Select<SubjectRecord>();
            list.Sort(delegate(SubjectRecord x, SubjectRecord y)
            {
                return x.UID.CompareTo(y.UID);
            });

            int order = 0;
            _subjOrder = new Dictionary<string, int>();

            foreach (SubjectRecord r in list)
            {
                if (!_subjOrder.ContainsKey(r.Name))
                {
                    _subjOrder.Add(r.Name, order);
                    order++;
                }
            }
        }

        public int Compare(string x, string y)
        {
            int xi = _subjOrder.ContainsKey(x) ? _subjOrder[x] : int.MaxValue -1;
            int yi = _subjOrder.ContainsKey(y) ? _subjOrder[y] : int.MaxValue -1;

            if (x == "Homeroom")
                xi = int.MaxValue;
            if (y == "Homeroom")
                yi = int.MaxValue;

            return xi.CompareTo(yi);
        }
    }
}
