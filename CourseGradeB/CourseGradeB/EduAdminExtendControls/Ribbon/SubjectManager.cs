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
        List<string> _groupList;

        public SubjectManager()
        {
            InitializeComponent();
            _A = new AccessHelper();
            _groupList = new List<string>();

            Dictionary<int, List<Tool.Domain>> dic = Tool.DomainDic;
            foreach (List<Tool.Domain> domains in dic.Values)
            {
                foreach (Tool.Domain domain in domains)
                {
                    if (!_groupList.Contains(domain.Name))
                        _groupList.Add(domain.Name);
                }
            }

            _groupList.Sort();

            foreach (string group in _groupList)
                colGroup.Items.Add(group);

            colType.Items.Add("Regular");
            colType.Items.Add("Honor");

            ReLoad();
        }

        private void ReLoad()
        {
            List<SubjectRecord> list = _A.Select<SubjectRecord>();

            list.Sort(delegate(SubjectRecord x, SubjectRecord y) 
            {
                return x.UID.CompareTo(y.UID);
            });

            foreach (SubjectRecord s in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                string name = s.Name;
                //string enName = s.EnglishName;
                string chName = s.ChineseName;
                string group = s.Group;
                string type = s.Type;
                row.Tag = s;
                //row.CreateCells(dgv, name, enName, group, Type);
                row.CreateCells(dgv, name, chName, group, type);

                if (!colGroup.Items.Contains(group))
                    colGroup.Items.Add(group);

                if (!colType.Items.Contains(type))
                    colType.Items.Add(type);

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
                row.Cells[colGroup.Index].ErrorText = "";

                string name = row.Cells[colName.Index].Value + "";
                string type = row.Cells[colType.Index].Value + "";
                string group = row.Cells[colGroup.Index].Value + "";

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

                if (string.IsNullOrWhiteSpace(group))
                {
                    row.Cells[colGroup.Index].ErrorText = "群組名稱不可為空白";
                    pass = false;
                }
                else if (!_groupList.Contains(group))
                {
                    row.Cells[colGroup.Index].ErrorText = "群組名稱有誤,請選擇預設群組";
                    pass = false;
                }

                if (string.IsNullOrWhiteSpace(type))
                {
                    row.Cells[colType.Index].ErrorText = "組別不可為空白";
                    pass = false;
                }

                row.ErrorText = "";
                List<string> errors = new List<string>();
                if (!string.IsNullOrWhiteSpace(row.Cells[colGroup.Index].ErrorText))
                    errors.Add(row.Cells[colGroup.Index].ErrorText);

                if (!string.IsNullOrWhiteSpace(row.Cells[colType.Index].ErrorText))
                    errors.Add(row.Cells[colType.Index].ErrorText);

                row.ErrorText = string.Join(",", errors);

                SubjectRecord record = new SubjectRecord();
                record.Name = row.Cells[colName.Index].Value + "";
                //record.EnglishName = row.Cells[colEnName.Index].Value + "";
                record.ChineseName = row.Cells[colChName.Index].Value + "";
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

                //EventHandler eh = FISCA.InteractionService.PublishEvent("SubjectChange");
                //eh(null, EventArgs.Empty);

                this.Close();
            }
            else
            {
                MessageBox.Show("資料有誤,請確認後再儲存");
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
                dgv.EndEdit();
            else
                dgv.BeginEdit(true);
        }
    }
}
