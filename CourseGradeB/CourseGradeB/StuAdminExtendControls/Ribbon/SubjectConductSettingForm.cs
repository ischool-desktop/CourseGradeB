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
using System.Xml;

namespace CourseGradeB.StuAdminExtendControls
{
    public partial class SubjectConductSettingForm : BaseForm
    {
        private ConductSetting _setting;
        AccessHelper _A;
        ButtonItem _RunningItem;
        XmlDocument _doc;
        List<string> _subjects;

        public SubjectConductSettingForm(ConductSetting setting)
        {
            InitializeComponent();
            _setting = setting;
            _doc = new XmlDocument();
            _doc.LoadXml(_setting.Conduct);
            _subjects = new List<string>();

            _A = new AccessHelper();

            foreach (SubjectRecord s in _A.Select<SubjectRecord>())
            {
                ButtonItem item = new ButtonItem();
                item.OptionGroup = "subject";
                item.Text = s.Name;
                item.Click += new EventHandler(item_click);
                itemPanle1.Items.Add(item);

                if (!_subjects.Contains(s.Name))
                    _subjects.Add(s.Name);
            }
        }

        private void SubjectConductSettingForm_Load(object sender, EventArgs e)
        {
        }

        private void item_click(object sender, EventArgs e)
        {
            if (_RunningItem == null)
                _RunningItem = new ButtonItem();

            ButtonItem item = sender as ButtonItem;
            //判斷切換的item是否相同
            if (!_RunningItem.Equals(item))
            {
                //切換前將dgv.rows存到tag中
                _RunningItem.Tag = new ButtonTag(dgv);
                //切換
                _RunningItem = item;
                string subject = item.Text;

                dgv.Rows.Clear();

                //item.Tag == ""代表此item未被編輯過,做初始設定
                if (item.Tag + "" == "")
                {
                    foreach (XmlElement conduct in _doc.SelectNodes("//Conduct[@Subject=\"" + subject + "\"]"))
                    {
                        string group = conduct.GetAttribute("Group");

                        foreach (XmlElement itemx in conduct.SelectNodes("Item"))
                        {
                            string title = itemx.GetAttribute("Title");
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dgv, group, title);
                            dgv.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    //tag!=""就讀取tag
                    ButtonTag tag = item.Tag as ButtonTag;
                    foreach (DataGridViewRow row in tag.Rows)
                        dgv.Rows.Add(row);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //刪除科目不存在的ConductTemplate
            foreach (XmlNode node in _doc.SelectNodes("//Conduct[@Subject]"))
            {
                XmlElement elem = node as XmlElement;
                string subject = elem.GetAttribute("Subject");

                if(!_subjects.Contains(subject))
                    _doc.DocumentElement.RemoveChild(node);
            }

            //最後被編輯的item先將資料存到tag
            if (itemPanle1.SelectedItems.Count == 1)
                itemPanle1.SelectedItems[0].Tag = new ButtonTag(dgv);

            foreach (ButtonItem buttonItem in itemPanle1.Items)
            {
                //buttonItem.Tag == ""代表此item從未被編輯過
                if (buttonItem.Tag + "" != "")
                {
                    string subject = buttonItem.Text;

                    //刪除該subject的節點
                    foreach (XmlNode node in _doc.SelectNodes("//Conduct[@Subject=\"" + subject + "\"]"))
                        _doc.DocumentElement.RemoveChild(node);

                    ButtonTag tag = buttonItem.Tag as ButtonTag;

                    List<string> check_list = new List<string>();
                    //foreach (DataGridViewRow row in dgv.Rows)
                    foreach (DataGridViewRow row in tag.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string group = row.Cells[colGroup.Index].Value + "";
                        string title = row.Cells[colTitle.Index].Value + "";
                        string key = group + "_" + title;

                        XmlElement newElem = _doc.SelectSingleNode("//Conduct[@Group=\"" + group + "\"][@Subject=\"" + subject + "\"]") as XmlElement;
                        if (newElem == null)
                        {
                            newElem = _doc.CreateElement("Conduct");
                            newElem.SetAttribute("Group", group);
                            newElem.SetAttribute("Subject", subject);
                            _doc.DocumentElement.AppendChild(newElem);
                        }

                        //同group避免重複add相同item
                        if (!check_list.Contains(key))
                        {
                            XmlElement item = newElem.OwnerDocument.CreateElement("Item");
                            item.SetAttribute("Title", title);

                            //newElem.SetAttribute("Group", group);

                            newElem.AppendChild(item);

                            check_list.Add(key);
                        }
                    }
                }
            }

            _setting.Conduct = _doc.OuterXml;
            _setting.Save();
            this.Close();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
                dgv.EndEdit();
            else
                dgv.BeginEdit(true);
        }
    }

    internal class ButtonTag
    {
        private List<DataGridViewRow> rowList;
        public List<DataGridViewRow> Rows
        {
            get
            {
                return rowList;
            }
        }
        public ButtonTag(DataGridView dgv)
        {
            rowList = new List<DataGridViewRow>();
            foreach(DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                rowList.Add(row);
            }
        }
    }
}
