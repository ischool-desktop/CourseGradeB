using FISCA.Presentation;
using Framework;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FCode = Framework.Security.FeatureCodeAttribute;

namespace CourseGradeB.StudentExtendControls
{
    [FCode("JHSchool.Student.Detail.SemsScore", "學期成績")]
    internal partial class SemsSubjScoreItem : JHSchool.Legacy.PalmerwormItem
    {
        List<SemesterScoreRecord> _records;
        bool _CanEdit;
        public SemsSubjScoreItem()
        {
            InitializeComponent();
            Title = "學期成績";
            _records = new List<SemesterScoreRecord>();
            _CanEdit = User.Acl["JHSchool.Student.Detail.SemsScore"].Editable;
            btnAdd.Enabled = btnEdit.Enabled = btnDelete.Enabled = _CanEdit;
        }

        protected override object OnBackgroundWorkerWorking()
        {
            //取得學生學期成績紀錄
            _records = K12.Data.SemesterScore.SelectByStudentID(RunningID);
            //排序
            _records.Sort(delegate(SemesterScoreRecord x, SemesterScoreRecord y)
            {
                string xx = x.SchoolYear.ToString().PadLeft(4, '0');
                xx += x.Semester.ToString().PadLeft(2, '0');
                string yy = y.SchoolYear.ToString().PadLeft(4, '0');
                yy += y.Semester.ToString().PadLeft(2, '0');

                return yy.CompareTo(xx);
            });
            return RunningID;
        }

        protected override void OnBackgroundWorkerCompleted(object result)
        {
            listView.Items.Clear();
            listView.Columns.Clear();

            //建立學年度欄位
            ColumnHeader ch_schoolyear = new ColumnHeader();
            ch_schoolyear.Text = "學年度";
            ch_schoolyear.Width = GetColumnHeaderWidth(ch_schoolyear.Text);
            listView.Columns.Add(ch_schoolyear);
            //建立學期欄位
            ColumnHeader ch_semester = new ColumnHeader();
            ch_semester.Text = "學期";
            ch_semester.Width = GetColumnHeaderWidth(ch_semester.Text);
            listView.Columns.Add(ch_semester);

            //整理科目名稱聯集
            List<string> subj_name = new List<string>();
            foreach (SemesterScoreRecord record in _records)
            {
                foreach (string subj in record.Subjects.Keys)
                    if (!subj_name.Contains(subj))
                        subj_name.Add(subj);
            }

            //建立科目欄位
            foreach (string name in subj_name)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = name;
                ch.Width = GetColumnHeaderWidth(ch.Text);
                listView.Columns.Add(ch);
            }

            //建立AvgScore欄位
            ColumnHeader ch_AvgScore = new ColumnHeader();
            ch_AvgScore.Text = "平均成績";
            ch_AvgScore.Width = GetColumnHeaderWidth(ch_AvgScore.Text);
            listView.Columns.Add(ch_AvgScore);
            //建立AvgGPA欄位
            ColumnHeader ch_AvgGPA = new ColumnHeader();
            ch_AvgGPA.Text = "平均GPA";
            ch_AvgGPA.Width = GetColumnHeaderWidth(ch_AvgGPA.Text);
            listView.Columns.Add(ch_AvgGPA);

            //插入item
            foreach (SemesterScoreRecord record in _records)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = record;
                item.Text = record.SchoolYear + "";
                listView.Items.Add(item);

                //填入學期
                item.SubItems.Add(record.Semester + "");

                //填入學期科目成績
                foreach (string name in subj_name)
                {
                    if (record.Subjects.ContainsKey(name))
                        item.SubItems.Add(record.Subjects[name].Score + "");
                    else
                        item.SubItems.Add("");
                }

                //填入AvgScore跟AvgGPA
                item.SubItems.Add(record.AvgScore + "");
                item.SubItems.Add(record.AvgGPA + "");
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            SemesterScoreAddForm form = new SemesterScoreAddForm(_records, RunningID);
            if (form.ShowDialog() == DialogResult.OK)
            {
                StudentRecord student = K12.Data.Student.SelectByID(RunningID);
                FISCA.LogAgent.ApplicationLog.Log("成績系統.學期成績", "新增學期成績", Tool.Instance.GetStudentInfo(student));
                _bgWorker.RunWorkerAsync();
            }
        }

        private int GetColumnHeaderWidth(string text)
        {
            return (text.Length - 1) * 13 + 31; //神奇的欄位寬度計算…
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1 && _CanEdit)
            {
                SemesterScoreRecord record = listView.SelectedItems[0].Tag as SemesterScoreRecord;
                SemesterScoreEditor edit = new SemesterScoreEditor(record, RunningID);
                if (edit.ShowDialog() == DialogResult.OK)
                    _bgWorker.RunWorkerAsync();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                SemesterScoreRecord record = listView.SelectedItems[0].Tag as SemesterScoreRecord;
                SemesterScoreEditor edit = new SemesterScoreEditor(record, RunningID);
                if (edit.ShowDialog() == DialogResult.OK)
                    _bgWorker.RunWorkerAsync();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                SemesterScoreRecord record = listView.SelectedItems[0].Tag as SemesterScoreRecord;

                if (MessageBox.Show("確認刪除「" + record.SchoolYear + "學年度 第" + record.Semester + "學期」的學期成績?", "ischool", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StudentRecord student = record.Student;
                    string str = "學年度:" + record.SchoolYear + " 學期:" + record.Semester;
                    str += Tool.Instance.GetStudentInfo(record.Student);
                    str += GetSemesterScoreInfo(record);
                    FISCA.LogAgent.ApplicationLog.Log("成績系統.學期成績", "刪除學期成績", str);

                    K12.Data.SemesterScore.Delete(record);
                    _bgWorker.RunWorkerAsync();
                }
            }
        }

        private string GetSemesterScoreInfo(SemesterScoreRecord record)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("平均成績:" + record.AvgScore);
            sb.AppendLine("平均GPA:" + record.AvgGPA);

            foreach (SubjectScore ss in record.Subjects.Values)
            {
                sb.AppendLine("科目:" + ss.Subject + " 節數:" + ss.Period + " 權數:" + ss.Credit+ " 成績:" + ss.Score);
            }

            return sb.ToString();
        }
    }
}
