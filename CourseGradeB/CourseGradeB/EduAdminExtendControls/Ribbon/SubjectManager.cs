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

namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    public partial class SubjectManager : BaseForm
    {
        AccessHelper _A;

        public SubjectManager()
        {
            InitializeComponent();
            _A = new AccessHelper();

            ReLoad();
        }

        private void ReLoad()
        {
            List<SubjectRecord> list = _A.Select<SubjectRecord>();

            list.Sort(delegate(SubjectRecord x, SubjectRecord y)
            {
                string xx = x.Name.PadLeft(20, '0');
                string yy = y.Name.PadLeft(20, '0');
                return xx.CompareTo(yy);
            });

            foreach (SubjectRecord s in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                string name = s.Name;
                string enName = s.EnglishName;
                string group = s.Group;
                string type = s.Type;
                row.Tag = s;
                row.CreateCells(dgv, name, enName, group, type);

                dgv.Rows.Add(row);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SubjectRecord> insert = new List<SubjectRecord>();
            List<string> existName = new List<string>();
            bool pass = true;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells[colName.Index].ErrorText = "";
                row.Cells[colType.Index].ErrorText = "";

                string name = row.Cells[colName.Index].Value + "";
                string type = row.Cells[colType.Index].Value + "";

                if (string.IsNullOrWhiteSpace(name))
                {
                    row.Cells[colName.Index].ErrorText = "科目名稱不可為空白";
                    pass = false;
                }
                else
                {
                    if (existName.Contains(name))
                    {
                        row.Cells[colName.Index].ErrorText = "科目名稱不可重複";
                        pass = false;
                    }
                    else
                    {
                        existName.Add(name);
                    }
                }

                if (string.IsNullOrWhiteSpace(type))
                {
                    row.Cells[colType.Index].ErrorText = "組別不可為空白";
                    pass = false;
                }

                SubjectRecord record = new SubjectRecord();
                record.Name = row.Cells[colName.Index].Value + "";
                record.EnglishName = row.Cells[colEnName.Index].Value + "";
                record.Group = row.Cells[colGroup.Index].Value + "";
                record.Type = row.Cells[colType.Index].Value + "";
                insert.Add(record);
            }

            if (pass)
            {
                List<SubjectRecord> allRecord = _A.Select<SubjectRecord>();
                    _A.DeletedValues(allRecord);

                if (insert.Count > 0)
                    _A.InsertValues(insert);
            }
            else
            {
                MessageBox.Show("資料有誤,請確認後再儲存");
            }
        }
    }
}
