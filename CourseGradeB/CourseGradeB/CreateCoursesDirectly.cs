using CourseGradeB.ClassExtendControls;
using JHSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseGradeB
{
    class CreateCoursesDirectly
    {
        public CreateCoursesDirectly()
        {
            List<JHClassRecord> list = JHClass.SelectByIDs(K12.Presentation.NLDPanels.Class.SelectedSource);
            CreateClassCourseForm form = new CreateClassCourseForm(list);
            form.ShowDialog();
        }
    }
}
