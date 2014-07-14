using DevComponents.AdvTree;
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
using System.Xml;

namespace CourseGradeB.StuAdminExtendControls
{
    public partial class ConductSettingForm : BaseForm
    {
        AccessHelper _A;
        ConductSetting _LowGrade, _MediumGrade, _HighGrade;

        public ConductSettingForm()
        {
            InitializeComponent();
            _A = new AccessHelper();

            List<ConductSetting> allConductList = _A.Select<ConductSetting>();
            foreach (ConductSetting record in allConductList)
            {
                if (record.Grade <= 2)
                {
                    _LowGrade = record;
                }
                else if (record.Grade <= 6)
                {
                    _MediumGrade = record;
                }
                else
                {
                    _HighGrade = record;
                }
            }

            if (_LowGrade == null)
            {
                _LowGrade = new ConductSetting(2);
                _LowGrade.Save();
            }

            if (_MediumGrade == null)
            {
                _MediumGrade = new ConductSetting(6);
                _MediumGrade.Save();
            }

            if (_HighGrade == null)
            {
                _HighGrade = new ConductSetting(12);
                _HighGrade.Save();
            }
        }

        private void ConductTitleManager_Load(object sender, EventArgs e)
        {
            tabControl1_SelectedTabChanged(null, null);
            LoadConduct(_LowGrade, dgv1);
            LoadConduct(_MediumGrade, dgv2);
            LoadConduct(_HighGrade, dgv3);
        }

        private void LoadConduct(ConductSetting setting, DataGridView dgv)
        {
            dgv.Rows.Clear();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setting.Conduct);

            //將具有Common屬性的node加入畫面
            foreach (XmlElement parent in doc.SelectNodes("//Conduct[@Common]"))
            {
                string group = parent.GetAttribute("Group");
                bool common = parent.GetAttribute("Common") == "True" ? true : false;

                foreach (XmlElement elem in parent.SelectNodes("Item"))
                {
                    string title = elem.GetAttribute("Title");

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgv, group, title, common);
                    dgv.Rows.Add(row);
                }
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Save(_LowGrade, dgv1);
            Save(_MediumGrade, dgv2);
            Save(_HighGrade, dgv3);
            this.Close();
        }

        private void Save(ConductSetting setting, DataGridView dgv)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setting.Conduct);

            //刪除帶有Common屬性的node
            foreach (XmlNode node in doc.SelectNodes("//Conduct[@Common]"))
                doc.DocumentElement.RemoveChild(node);

            List<string> check_list = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                string group = row.Cells[colGroup.Index].Value + "";
                string title = row.Cells[colTitle.Index].Value + "";
                string common = row.Cells[colCommon.Index].Value + "" == "True" ? "True" : "False";
                string key = group + "_" + title;

                //搜尋具有common屬性,且group名稱符合的node
                XmlElement elem = doc.SelectSingleNode("//Conduct[@Group='" + group + "'][@Common]") as XmlElement;

                //node不存在代表需新增並append
                if (elem == null)
                {
                    elem = doc.CreateElement("Conduct");
                    elem.SetAttribute("Group", group);
                    elem.SetAttribute("Common", common);
                    doc.DocumentElement.AppendChild(elem);
                }

                //同group避免重複add相同item
                if (!check_list.Contains(key))
                {
                    XmlElement item = doc.CreateElement("Item");
                    item.SetAttribute("Title", title);

                    elem.AppendChild(item);

                    check_list.Add(key);
                }
            }

            setting.Conduct = doc.OuterXml;
            setting.Save();
        }

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (tabControl1.SelectedTab.Equals(tabItem1))
                linkLabel1.Enabled = true;
            else
                linkLabel1.Enabled = false;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SubjectConductSettingForm form = new SubjectConductSettingForm(_LowGrade);
            form.ShowDialog();
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv1.BeginEdit(true);
        }

        private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv2.BeginEdit(true);
        }

        private void dgv3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv3.BeginEdit(true);
        }

        private void SelectTogether(DataGridView dgv,DataGridViewCellEventArgs e)
        {
            dgv.EndEdit();
            DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string group = dgv.Rows[e.RowIndex].Cells[colGroup.Index].Value + "";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells[colGroup.Index].Value + "" == group)
                    row.Cells[colCommon.Index].Value = cell.Value + "" == "True" ? true : false;
            }
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCommon.Index)
                SelectTogether(dgv1, e);
        }

        private void dgv1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCommon.Index)
                SelectTogether(dgv1, e);
        }

        private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCommon.Index)
                SelectTogether(dgv2, e);
        }

        private void dgv2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCommon.Index)
                SelectTogether(dgv2, e);
        }


    }
}

