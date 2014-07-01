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
            //_A.DeletedValues(allConductList);
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
                _LowGrade = CreatNewRecord(2);

            if (_MediumGrade == null)
                _MediumGrade = CreatNewRecord(6);

            if (_HighGrade == null)
                _HighGrade = CreatNewRecord(12);
        }

        private ConductSetting CreatNewRecord(int grade)
        {
            XmlElement root = new XmlDocument().CreateElement("Conducts");
            ConductSetting setting = new ConductSetting();
            setting.Grade = grade;
            setting.Conduct = root.OuterXml;
            setting.Save();
            return setting;
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
        }

        private void Save(ConductSetting setting, DataGridView dgv)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setting.Conduct);

            //刪除帶有Common屬性的node
            foreach (XmlNode node in doc.SelectNodes("//Conduct[@Common]"))
                doc.DocumentElement.RemoveChild(node);

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                string group = row.Cells[colGroup.Index].Value + "";
                string title = row.Cells[colTitle.Index].Value + "";
                string common = row.Cells[colCommon.Index].Value + "" == "True" ? "True" : "False";

                //搜尋具有common屬性,且group名稱符合的node
                XmlElement elem = doc.SelectSingleNode("//Conduct[@Group='" + group + "'][@Common]") as XmlElement;

                //node不存在代表需新增並append
                if (elem == null)
                {
                    elem = doc.CreateElement("Conduct");
                    elem.SetAttribute("Group", group);
                    doc.DocumentElement.AppendChild(elem);
                }

                //會複寫成最後一個item設定的Group跟Common
                elem.SetAttribute("Common", common);

                XmlElement item = doc.CreateElement("Item");
                item.SetAttribute("Title", title);

                elem.AppendChild(item);
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

        
    }
}

