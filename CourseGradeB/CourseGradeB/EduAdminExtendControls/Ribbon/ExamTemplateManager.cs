using DevComponents.DotNetBar;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
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
    public partial class ExamTemplateManager : BaseForm
    {
        AccessHelper _A = new AccessHelper();
        Dictionary<string, string> ExamIDToName;
        Dictionary<string, string> ExamNameToID;
        ExamTemplateRecord _RunningItem;
        bool _intDirty = false;
        bool _dgvDirty = false;
        bool _HasDelete = false;

        public ExamTemplateManager()
        {
            InitializeComponent();

            List<ExamRecord> exams = Exam.SelectAll();
            exams.Sort(delegate(ExamRecord x, ExamRecord y)
            {
                string xx = x.DisplayOrder.ToString().PadLeft(3, '0') + x.Name.PadLeft(20, '0');
                string yy = y.DisplayOrder.ToString().PadLeft(3, '0') + y.Name.PadLeft(20, '0');
                return xx.CompareTo(yy);
            });

            ExamIDToName = new Dictionary<string, string>();
            ExamNameToID = new Dictionary<string, string>();

            foreach (ExamRecord record in exams)
            {
                if (!ExamIDToName.ContainsKey(record.ID))
                    ExamIDToName.Add(record.ID, record.Name);

                if (!ExamNameToID.ContainsKey(record.Name))
                    ExamNameToID.Add(record.Name, record.ID);

                colName.Items.Add(record.Name);
            }

            ReLoad();
        }

        private void ReLoad()
        {
            _RunningItem = null;
            itemPanel1.Items.Clear();
            dgv.Rows.Clear();
            ReSetDirty();
            gp1.Text = "樣板名稱";
            intInput1.Enabled = false;
            btnSave.Enabled = false;

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
                item.OptionGroup = "item";
                item.Text = r.Name;
                item.Tag = r;
                item.Click += new EventHandler(Item_Click);
                itemPanel1.Items.Add(item);
            }

            itemPanel1.RecalcLayout();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            if (CanContinue())
            {
                btnSave.Enabled = true;
                dgv.Rows.Clear();

                ButtonItem item = sender as ButtonItem;
                ExamTemplateRecord record = item.Tag as ExamTemplateRecord;
                _RunningItem = record;

                gp1.Text = record.Name;
                intInput1.Value = int.Parse(_RunningItem.ExamScale);

                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrWhiteSpace(record.Setting))
                {
                    doc.LoadXml(record.Setting);
                }

                foreach (XmlElement elem in doc.SelectNodes("//Item"))
                {
                    string id = elem.GetAttribute("ExamID");
                    string weight = elem.GetAttribute("Weight");
                    bool examNeed = elem.GetAttribute("ExamNeed") == "1" ? true : false;
                    bool dailyNeed = elem.GetAttribute("DailyNeed") == "1" ? true : false;
                    bool conductNeed = elem.GetAttribute("ConductNeed") == "1" ? true : false;
                    string startTime = elem.GetAttribute("StartTime");
                    string endTime = elem.GetAttribute("EndTime");

                    DataGridViewRow row = new DataGridViewRow();
                    string name = ExamIDToName.ContainsKey(id) ? ExamIDToName[id] : "";
                    row.CreateCells(dgv, name, weight, examNeed, dailyNeed, conductNeed, startTime, endTime);
                    dgv.Rows.Add(row);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                    foreach (DataGridViewCell cell in row.Cells)
                        cell.Tag = cell.Value;

                ReSetDirty();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CanContinue())
            {
                ExamTemplateAddForm form = new ExamTemplateAddForm();
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ReLoad();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_RunningItem != null)
            {
                List<string> nameCheck = new List<string>();
                XmlElement root = new XmlDocument().CreateElement("Setting");

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue;

                    if(HasError(row))
                    {
                        MessageBox.Show("資料有錯誤,請確認後再儲存");
                        return;
                    }

                    string name = row.Cells[colName.Index].Value + "";
                    string weight = row.Cells[colWeight.Index].Value + "";
                    string examNeed = row.Cells[colExamNeed.Index].Value + "" == "True" ? "1" : "0" ;
                    string dailyNeed = row.Cells[colDailyNeed.Index].Value + "" == "True" ? "1" : "0";
                    string conductNeed = row.Cells[colConductNeed.Index].Value + "" == "True" ? "1" : "0";
                    string startTime = DateToSaveFormat(row.Cells[colStartTime.Index].Value + "");
                    string endTime = DateToSaveFormat(row.Cells[colEndTime.Index].Value + "");

                    if (nameCheck.Contains(name))
                    {
                        MessageBox.Show("考試名稱: " + name + "重覆,請確認後再儲存");
                        return;
                    }
                    else
                    {
                        nameCheck.Add(name);
                    }

                    XmlElement elem = root.OwnerDocument.CreateElement("Item");
                    elem.SetAttribute("ExamID", ExamNameToID.ContainsKey(name) ? ExamNameToID[name] : "");
                    elem.SetAttribute("Weight", weight);
                    elem.SetAttribute("ExamNeed", examNeed);
                    elem.SetAttribute("DailyNeed", dailyNeed);
                    elem.SetAttribute("ConductNeed", conductNeed);
                    elem.SetAttribute("StartTime", startTime);
                    elem.SetAttribute("EndTime", endTime);

                    root.AppendChild(elem);
                }

                _RunningItem.Setting = root.OuterXml;
                _RunningItem.ExamScale = intInput1.Value + "";

                List<ExamTemplateRecord> update = new List<ExamTemplateRecord>();
                update.Add(_RunningItem);
                _A.UpdateValues(update);

                ReSetDirty();
            }
        }

        private void intInput1_ValueChanged(object sender, EventArgs e)
        {
            lbVacancy.Text = (100 - intInput1.Value) + "%";

            if (_RunningItem != null)
            {
                if (_RunningItem.ExamScale == intInput1.Value.ToString())
                    _intDirty = false;
                else
                    _intDirty = true;

                lblIsDirty.Visible = _dgvDirty || _intDirty || _HasDelete;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_RunningItem != null)
            {
                if (MessageBox.Show("確認刪除: " + _RunningItem.Name + " ?", "ishcool", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    List<ExamTemplateRecord> delete = new List<ExamTemplateRecord>();
                    delete.Add(_RunningItem);
                    _A.DeletedValues(delete);

                    ReLoad();
                }
            }
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_RunningItem != null)
            {
                bool dirty = false;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue;

                    //考試名稱驗證
                    row.Cells[colName.Index].ErrorText = "";
                    if (string.IsNullOrWhiteSpace(row.Cells[colName.Index].Value + ""))
                        row.Cells[colName.Index].ErrorText = "考試名稱不可為空值";

                    //開始日期驗證
                    DateTime st = new DateTime();
                    row.Cells[colStartTime.Index].ErrorText = "";
                    if (!string.IsNullOrWhiteSpace(row.Cells[colStartTime.Index].Value + ""))
                    {
                        string time = DateToSaveFormat(row.Cells[colStartTime.Index].Value + "");
                        if (string.IsNullOrWhiteSpace(time))
                            row.Cells[colStartTime.Index].ErrorText = "開始日期格式錯誤(yyyy/MM/dd)";
                        else
                            DateTime.TryParse(time, out st);
                    }

                    //結束日期驗證
                    DateTime et = new DateTime();
                    row.Cells[colEndTime.Index].ErrorText = "";
                    if (!string.IsNullOrWhiteSpace(row.Cells[colEndTime.Index].Value + ""))
                    {
                        string time = DateToSaveFormat(row.Cells[colEndTime.Index].Value + "");
                        if (string.IsNullOrWhiteSpace(time))
                            row.Cells[colEndTime.Index].ErrorText = "結束日期格式錯誤(yyyy/MM/dd)";
                        else
                            DateTime.TryParse(time, out et);

                        if (st > et)
                        {
                            row.Cells[colStartTime.Index].ErrorText = "開始時間不可大於結束時間";
                            row.Cells[colEndTime.Index].ErrorText = "結束時間不可小於開始時間";
                        }
                    }

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Tag + "" != cell.Value + "")
                        {
                            dirty = true;
                        }
                    }
                }

                _dgvDirty = dirty;
                lblIsDirty.Visible = _dgvDirty || _intDirty || _HasDelete;
            }
        }

        private void dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_RunningItem != null)
            {
                _HasDelete = true;
                lblIsDirty.Visible = _dgvDirty || _intDirty || _HasDelete;
            }
        }

        private void ReSetDirty()
        {
            _intDirty = false;
            _dgvDirty = false;
            _HasDelete = false;
            lblIsDirty.Visible = false;
        }

        private bool CanContinue()
        {
            if (lblIsDirty.Visible == true)
                if (MessageBox.Show("資料尚未儲存,確定離開?", "ischool", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }

            return true;
        }

        private static string DateToSaveFormat(string source)
        {
            if (source == null || source == string.Empty) return string.Empty;

            try
            {
                DateTime? dt = DateTimeHelper.ParseGregorian(source, PaddingMethod.First);
                if (dt.HasValue)
                return dt.Value.ToString("yyyy/MM/dd HH:mm");
            }
            catch
            {
            }
            
            return string.Empty;
        }

        private bool HasError(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (!string.IsNullOrWhiteSpace(cell.ErrorText))
                    return true;
            }

            return false;
        }

        private void ExamTemplateManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lblIsDirty.Visible == true)
                if (MessageBox.Show("資料尚未儲存,確定關閉?", "ischool", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
        }
    }
}
