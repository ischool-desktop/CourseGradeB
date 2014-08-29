using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    /// <summary>
    /// 升學輔導老師Tag
    /// </summary>
    public class Tagging
    {
        public static void Main()
        {
            Customization.Tagging.SystemTag.Define("Teacher", "升學輔導老師", Color.Gold, "CareersCounselor", "升學輔導老師", "教師>系統類別");
        }
    }
}
