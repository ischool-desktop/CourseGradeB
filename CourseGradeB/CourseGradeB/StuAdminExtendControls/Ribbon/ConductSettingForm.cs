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
        ConductSetting _Grade1, _Grade2, _Grade3, _Grade4, _MediumGrade, _HighGrade;

        public ConductSettingForm()
        {
            InitializeComponent();
            _A = new AccessHelper();

            List<ConductSetting> allConductList = _A.Select<ConductSetting>();
            foreach (ConductSetting record in allConductList)
            {
                if (record.Grade == 1)
                {
                    _Grade1 = record;
                }
                else if (record.Grade == 2)
                {
                    _Grade2 = record;
                }
                else if (record.Grade == 3)
                {
                    _Grade3 = record;
                }
                else if (record.Grade == 4)
                {
                    _Grade4 = record;
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

            if (_Grade1 == null)
            {
                _Grade1 = new ConductSetting(1);
                _Grade1.Save();
            }

            if (_Grade2 == null)
            {
                _Grade2 = new ConductSetting(2);
                _Grade2.Save();
            }

            if (_Grade3 == null)
            {
                _Grade3 = new ConductSetting(3);
                _Grade3.Save();
            }

            if (_Grade4 == null)
            {
                _Grade4 = new ConductSetting(4);
                _Grade4.Save();
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
            LoadConduct(_Grade1, dgv1);
            LoadConduct(_Grade2, dgv2);
            LoadConduct(_Grade3, dgv3);
            LoadConduct(_Grade4, dgv4);

            LoadConduct(_MediumGrade, dgv6);
            LoadConduct(_HighGrade, dgv12);
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
            Save(_Grade1, dgv1);
            Save(_Grade2, dgv2);
            Save(_Grade3, dgv3);
            Save(_Grade4, dgv4);
            Save(_MediumGrade, dgv6);
            Save(_HighGrade, dgv12);
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
                XmlElement elem = doc.SelectSingleNode("//Conduct[@Group=" + Tool.XPathLiteral(group) + "][@Common]") as XmlElement;

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
            if (tabControl1.SelectedTab.Equals(ti6) || tabControl1.SelectedTab.Equals(ti12))
                linkLabel1.Enabled = false;
            else
                linkLabel1.Enabled = true;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConductSetting currentSetting = null;

            if (tabControl1.SelectedTab.Equals(ti1))
                currentSetting = _Grade1;
            if (tabControl1.SelectedTab.Equals(ti2))
                currentSetting = _Grade2;
            if (tabControl1.SelectedTab.Equals(ti3))
                currentSetting = _Grade3;
            if (tabControl1.SelectedTab.Equals(ti4))
                currentSetting = _Grade4;
            if (currentSetting != null)
            {
                SubjectConductSettingForm form = new SubjectConductSettingForm(currentSetting);
                form.ShowDialog();
            }
        }

        private void SelectTogether(DataGridView dgv, DataGridViewCellEventArgs e)
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

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == -1)
                dgv.EndEdit();
            else
                dgv.BeginEdit(true);
        }
        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == colCommon.Index)
                SelectTogether(dgv, e);
        }

        //private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == -1)
        //        dgv2.EndEdit();
        //    else
        //        dgv2.BeginEdit(true);
        //}

        //private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == -1)
        //        dgv6.EndEdit();
        //    else
        //        dgv6.BeginEdit(true);
        //}

        //private void dgv3_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == -1)
        //        dgv12.EndEdit();
        //    else
        //        dgv12.BeginEdit(true);
        //}

        //private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == colCommon.Index)
        //        SelectTogether(dgv2, e);
        //}

        //private void dgv1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == colCommon.Index)
        //        SelectTogether(dgv2, e);
        //}

        //private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == colCommon.Index)
        //        SelectTogether(dgv6, e);
        //}

        //private void dgv2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == colCommon.Index)
        //        SelectTogether(dgv6, e);
        //}


    }
}

