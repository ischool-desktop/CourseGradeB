using CourseGradeB.EduAdminExtendControls;
using DevComponents.DotNetBar;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CourseGradeB.CourseExtendControls.Ribbon
{
    public partial class GiveRefExamTemplateForm : BaseForm
    {
        private List<string> _Course;
        AccessHelper _A = new AccessHelper();
        public GiveRefExamTemplateForm(List<string> ids)
        {
            InitializeComponent();
            _Course = ids;
        }

        private void GiveRefExamTemplateForm_Load(object sender, EventArgs e)
        {
            ButtonItem emptyIitem = new ButtonItem();
            emptyIitem.OptionGroup = "items";
            emptyIitem.Text = "不指定";
            emptyIitem.Tag = -1;
            itemPanel1.Items.Add(emptyIitem);

            List<ExamTemplateRecord> list = _A.Select<ExamTemplateRecord>();
            list.Sort(delegate(ExamTemplateRecord x, ExamTemplateRecord y)
            {
                string xx = x.Name.PadLeft(20, '0');
                string yy = y.Name.PadLeft(20, '0');
                return xx.CompareTo(yy);
            });

            foreach (ExamTemplateRecord r in list)
            {
                ButtonItem item = new ButtonItem();
                item.OptionGroup = "items";
                item.Text = r.Name;
                item.Tag = r.UID;
                itemPanel1.Items.Add(item);
            }

            itemPanel1.RecalcLayout();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            EventHandler eh;
            string EventCode = "Res_CourseExt";
            eh = FISCA.InteractionService.PublishEvent(EventCode);

            if (itemPanel1.SelectedItems.Count == 1)
            {
                string ref_exam_template_id = itemPanel1.SelectedItems[0].Tag + "";

                string course_ids = string.Join(",", _Course);

                List<CourseExtendRecord> list = _A.Select<CourseExtendRecord>("ref_course_id in (" + course_ids + ")");
                Dictionary<int, CourseExtendRecord> dic = new Dictionary<int, CourseExtendRecord>();
                foreach (CourseExtendRecord r in list)
                {
                    if (!dic.ContainsKey(r.Ref_course_id))
                        dic.Add(r.Ref_course_id, r);
                }

                List<CourseExtendRecord> insert = new List<CourseExtendRecord>();
                List<CourseExtendRecord> update = new List<CourseExtendRecord>();
                List<CourseExtendRecord> delete = new List<CourseExtendRecord>();
                foreach (string sid in _Course)
                {
                    int id = int.Parse(sid);
                    if (dic.ContainsKey(id))
                    {
                        if (ref_exam_template_id == "-1")
                        {
                            delete.Add(dic[id]);
                        }
                        else
                        {
                            //dic[id].Ref_exam_template_id = int.Parse(ref_exam_template_id);
                            update.Add(dic[id]);
                        }
                    }
                    else
                    {
                        if (ref_exam_template_id != "-1")
                        {
                            CourseExtendRecord record = new CourseExtendRecord();
                            record.Ref_course_id = id;
                            //record.Ref_exam_template_id = int.Parse(ref_exam_template_id);
                            insert.Add(record);
                        }
                    }
                }

                if (insert.Count > 0)
                    _A.InsertValues(insert);

                if (update.Count > 0)
                    _A.UpdateValues(update);

                if (delete.Count > 0)
                    _A.DeletedValues(delete);

                eh(null, EventArgs.Empty);

                this.Close();
            }
            else
            {
                MessageBox.Show("請選擇一個評分樣板");
            }
        }
    }
}
