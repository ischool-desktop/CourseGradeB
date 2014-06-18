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

namespace CourseGradeB.EduAdminExtendControls.Ribbon
{
    public partial class SubjectManager : BaseForm
    {
        AccessHelper _A = new AccessHelper();
        SubjectRecord _RunningItem;

        public SubjectManager()
        {
            InitializeComponent();

            cboType.Items.Add("Regular");
            cboType.Items.Add("Honor");

            ReLoad();
        }

        private void ReLoad()
        {
            itemPanel1.Items.Clear();
            _RunningItem = null;
            txtName.Text = "";
            txtEnName.Text = "";
            txtGroup.Text = "";
            cboType.Text = "";
            dgv.Rows.Clear();

            List<SubjectRecord> list = _A.Select<SubjectRecord>();

            list.Sort(delegate(SubjectRecord x, SubjectRecord y){
                string xx = x.Name.PadLeft(20,'0');
                string yy = y.Name.PadLeft(20, '0');
                return xx.CompareTo(yy);
            });

            foreach (SubjectRecord s in list)
            {
                ButtonItem item = new ButtonItem();
                item.OptionGroup = "subject";
                item.Text = s.Name;
                item.Tag = s;
                item.Click += new EventHandler(Item_Click);
                itemPanel1.Items.Add(item);
            }

            itemPanel1.RecalcLayout();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();

            ButtonItem item = sender as ButtonItem;
            SubjectRecord sr = item.Tag as SubjectRecord;
            _RunningItem = sr;


            txtName.Text = _RunningItem.Name;
            txtEnName.Text = _RunningItem.EnglishName;
            txtGroup.Text = _RunningItem.Group;
            cboType.Text = _RunningItem.Type;
            
            XmlDocument doc = new XmlDocument();

            if (!string.IsNullOrWhiteSpace(_RunningItem.Conduct))
                doc.LoadXml(_RunningItem.Conduct);

            foreach (XmlElement elem in doc.SelectNodes("//Conduct"))
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv, elem.InnerText);
                dgv.Rows.Add(row);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_RunningItem != null)
            {
                _RunningItem.Name = txtName.Text;
                _RunningItem.EnglishName = txtEnName.Text;
                _RunningItem.Group = txtGroup.Text;
                _RunningItem.Type = cboType.Text;

                XmlElement root = new XmlDocument().CreateElement("Conducts");

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if(row.IsNewRow) continue;

                    XmlElement elem = root.OwnerDocument.CreateElement("Conduct");
                    elem.InnerText = row.Cells[0].Value + "";
                    root.AppendChild(elem);
                }

                _RunningItem.Conduct = root.OuterXml;

                List<SubjectRecord> update = new List<SubjectRecord>();
                update.Add(_RunningItem);
                _A.UpdateValues(update);

                MessageBox.Show("科目: " + _RunningItem.Name + " 已儲存");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SubjectAddForm form = new SubjectAddForm();
            
            if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ReLoad();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_RunningItem != null)
            {
                DialogResult result = MessageBox.Show("確認刪除科目: " + _RunningItem.Name + " ?", "ischool", MessageBoxButtons.OKCancel);
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    List<SubjectRecord> delete = new List<SubjectRecord>();
                    delete.Add(_RunningItem);
                    _A.DeletedValues(delete);

                    ReLoad();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
