using CourseGradeB.EduAdminExtendControls;
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

namespace CourseGradeB.StudentExtendControls
{
    public partial class SemesterScoreEditor : BaseForm
    {
        AccessHelper _A = new AccessHelper();
        SemesterScoreRecord _ssr;
        Dictionary<string, string> _SubjectToTypeDic = new Dictionary<string, string>();
        Dictionary<string, string> _SubjectToGroupDic = new Dictionary<string, string>();
        Dictionary<string, string> _old = new Dictionary<string, string>();
        Dictionary<string, string> _new = new Dictionary<string, string>();
        string _sid;

        public SemesterScoreEditor(SemesterScoreRecord ssr, string id)
        {
            InitializeComponent();
            _ssr = ssr;
            _sid = id;

            //record舊資料for log
            foreach (SubjectScore ss in _ssr.Subjects.Values)
            {
                _old.Add(ss.Subject + "_節數", ss.Period + "");
                _old.Add(ss.Subject + "_權數", ss.Credit + "");
                _old.Add(ss.Subject + "_成績", ss.Score + "");
            }

            //科目對照
            foreach (SubjectRecord r in _A.Select<SubjectRecord>())
            {
                if (!_SubjectToTypeDic.ContainsKey(r.Name))
                    _SubjectToTypeDic.Add(r.Name, r.Type);

                if (!_SubjectToGroupDic.ContainsKey(r.Name))
                    _SubjectToGroupDic.Add(r.Name, r.Group);
            }

            //新增科目下拉清單
            colSubjectName.Items.Add(""); //空白預設項
            foreach (string name in _SubjectToTypeDic.Keys)
                colSubjectName.Items.Add(name);
        }

        private void SemesterScoreEditor_Load(object sender, EventArgs e)
        {
            lblYearAndSemester.Text = _ssr.SchoolYear + "學年度" + _ssr.Semester + "學期";

            ClassRecord cr = _ssr.Student.Class;
            string className = cr == null ? "" : cr.Name;
            lblClassAndName.Text = className + " " + _ssr.Student.Name;

            foreach (string subj in _ssr.Subjects.Keys)
            {
                SubjectScore ss = _ssr.Subjects[subj];
                DataGridViewRow row = new DataGridViewRow();

                string type = _SubjectToTypeDic.ContainsKey(subj) ? _SubjectToTypeDic[subj] : "";

                string period = ss.Period == ss.Credit ? ss.Period + "" : ss.Period + "/" + ss.Credit;

                string subj_name = colSubjectName.Items.Contains(subj) ? subj : "";

                row.CreateCells(dgv, subj_name, type, period, ss.Score);
                dgv.Rows.Add(row);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                Save();

                List<string> _ids = new List<string>() { _sid };
                Tool.SetCumulateGPA(_ids, _ssr.SchoolYear, _ssr.Semester);

                //record新資料for log
                foreach (SubjectScore ss in _ssr.Subjects.Values)
                {
                    _new.Add(ss.Subject + "_節數", ss.Period + "");
                    _new.Add(ss.Subject + "_權數", ss.Credit + "");
                    _new.Add(ss.Subject + "_成績", ss.Score + "");
                }

                StringBuilder sb = new StringBuilder();
                StudentRecord student = K12.Data.Student.SelectByID(_sid);
                sb.AppendLine("學年度:" + _ssr.SchoolYear + " 學期:" + _ssr.Semester);
                sb.AppendLine(Tool.GetStudentInfo(student));

                foreach (string key in _new.Keys)
                {
                    if (_old.ContainsKey(key))
                    {
                        if (_old[key] != _new[key])
                            sb.AppendLine(key + " 由 " + _old[key] + "改為 " + _new[key]);
                    }
                    else
                        sb.AppendLine(key + " 新增值 " + _new[key]);
                }

                foreach (string key in _old.Keys)
                {
                    if (!_new.ContainsKey(key))
                        sb.AppendLine(key + " 刪除值 " + _old[key]);
                }

                FISCA.LogAgent.ApplicationLog.Log("成績系統.學期成績", "修改學期成績", sb.ToString());
            }
            else
            {
                MessageBox.Show("資料錯誤,請確認後再儲存");
            }

        }

        private void Save()
        {
            _ssr.Subjects.Clear();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string name = row.Cells[colSubjectName.Index].Value + "";
                string type = row.Cells[colSubjectType.Index].Value + "";
                Tool.SubjectType stype = type == "Honor" ? Tool.SubjectType.Honor : Tool.SubjectType.Regular;

                string period_credit = row.Cells[colCredit.Index].Value + "";
                string period, credit;
                if (period_credit.Contains("/"))
                {
                    period = period_credit.Split('/')[0];
                    credit = period_credit.Split('/')[1];
                }
                else
                {
                    period = credit = period_credit;
                }

                string score = row.Cells[colScore.Index].Value + "";

                string group = _SubjectToGroupDic.ContainsKey(name) ? _SubjectToGroupDic[name] : "";

                if (!_ssr.Subjects.ContainsKey(name))
                {
                    _ssr.Subjects.Add(name, new SubjectScore());
                    SubjectScore ss = _ssr.Subjects[name];
                    ss.Subject = name;
                    ss.Period = decimal.Parse(period);
                    ss.Credit = decimal.Parse(credit);
                    ss.Domain = group;
                    ss.Score = decimal.Parse(score);
                    //ss.GPA = Tool.Instance.GetScoreGrade(ss.Score.Value, stype);

                    if (stype == Tool.SubjectType.Honor)
                        ss.GPA = Tool.GPA.Eval(ss.Score.Value).Honors;
                    else
                        ss.GPA = Tool.GPA.Eval(ss.Score.Value).Regular;
                }
            }

            decimal totalScore = 0;
            decimal totalGPA = 0;
            decimal count = 0;
            foreach (SubjectScore ss in _ssr.Subjects.Values)
            {
                totalScore += ss.Score.Value * ss.Credit.Value;
                totalGPA += ss.GPA.Value * ss.Credit.Value;
                count += ss.Credit.Value;
            }

            if (count > 0)
            {
                _ssr.AvgScore = Math.Round(totalScore / count, 2, MidpointRounding.AwayFromZero);
                _ssr.AvgGPA = Math.Round(totalGPA / count, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                _ssr.AvgScore = null;
                _ssr.AvgGPA = null;
            }

            K12.Data.SemesterScore.Update(_ssr);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
                dgv.EndEdit();
            else
                dgv.BeginEdit(true);
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgv.Rows[e.RowIndex];

            if (e.ColumnIndex == colSubjectName.Index)
            {
                string subj_name = row.Cells[colSubjectName.Index].Value + "";
                row.Cells[colSubjectType.Index].Value = _SubjectToTypeDic.ContainsKey(subj_name) ? _SubjectToTypeDic[subj_name] : "";
            }
        }

        private bool isValid()
        {
            bool retVal = true;
            List<string> subj_list = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                //檢查科目名稱
                string subj_name = row.Cells[colSubjectName.Index].Value + "";

                if (string.IsNullOrWhiteSpace(subj_name))
                {
                    row.Cells[colSubjectName.Index].ErrorText = "科目名稱不可為空白";
                    retVal = false;
                }
                else
                {
                    if (!subj_list.Contains(subj_name))
                    {
                        subj_list.Add(subj_name);
                        row.Cells[colSubjectName.Index].ErrorText = "";
                    }
                    else
                    {
                        row.Cells[colSubjectName.Index].ErrorText = "科目名稱重複";
                        retVal = false;
                    }
                }

                

                //檢查科目type
                string type = row.Cells[colSubjectType.Index].Value + "";
                if (type == "Honor" || type == "Regular")
                {
                    row.Cells[colSubjectType.Index].ErrorText = "";
                }
                else
                {
                    row.Cells[colSubjectType.Index].ErrorText = "科目類別不可為空白";
                    retVal = false;
                }

                //檢查權數
                string pc = row.Cells[colCredit.Index].Value + "";
                decimal d = 0;
                if (pc.Contains("/"))
                {
                    if (decimal.TryParse(pc.Split('/')[0], out d) && decimal.TryParse(pc.Split('/')[1], out d))
                    {
                        row.Cells[colCredit.Index].ErrorText = "";
                    }
                    else
                    {
                        row.Cells[colCredit.Index].ErrorText = "節權數格式錯誤";
                        retVal = false;
                    }
                }
                else
                {
                    if (decimal.TryParse(pc, out d))
                    {
                        row.Cells[colCredit.Index].ErrorText = "";
                    }
                    else
                    {
                        row.Cells[colCredit.Index].ErrorText = "節權數格式錯誤";
                        retVal = false;
                    }
                }

                //檢查成績
                string score = row.Cells[colScore.Index].Value + "";
                if (decimal.TryParse(score, out d))
                {
                    row.Cells[colScore.Index].ErrorText = "";
                    if(d < 0 || d > 100)
                    {
                        row.Cells[colScore.Index].ErrorText = "成績範圍需在0~100之間";
                        retVal = false;
                    }
                }
                else
                {
                    row.Cells[colScore.Index].ErrorText = "成績應該為數字";
                    retVal = false;
                }
            }
            return retVal;
        }
    }
}
