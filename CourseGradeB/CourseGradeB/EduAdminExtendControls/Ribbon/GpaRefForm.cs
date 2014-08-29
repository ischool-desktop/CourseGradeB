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
    public partial class GpaRefForm : BaseForm
    {
        private AccessHelper _A;
        private int _SchoolYear;
        private int _Semester;
        private List<GpaRef> _Records;
        private Dictionary<string, string> _old;
        private Dictionary<string, string> _new;

        public GpaRefForm()
        {
            InitializeComponent();
            _A = new AccessHelper();
            _SchoolYear = int.Parse(K12.Data.School.DefaultSchoolYear);
            _Semester = int.Parse(K12.Data.School.DefaultSemester);
            _old = new Dictionary<string, string>();
            _new = new Dictionary<string, string>();
            LoadData();
        }

        private void LoadData()
        {
            _old.Clear();
            dgv.Rows.Clear();
            _Records = _A.Select<GpaRef>();
            _Records.Sort(delegate(GpaRef x, GpaRef y)
            {
                string xx = (x.SchoolYear + "").PadLeft(5, '0');
                xx += (x.Semester + "").PadLeft(2, '0');
                xx += (x.Grade + "").PadLeft(3, '0');

                string yy = (y.SchoolYear + "").PadLeft(5, '0');
                yy += (y.Semester + "").PadLeft(2, '0');
                yy += (y.Grade + "").PadLeft(3, '0');


                return xx.CompareTo(yy);
            });

            foreach (GpaRef gr in _Records)
            {
                string schoolYear = gr.SchoolYear + "";
                string semester = gr.Semester + "";
                string grade = gr.Grade + "";
                string max = gr.Max + "";
                string avg = gr.Avg + "";
                string key1 = schoolYear + "學年度" + semester + "學期" + grade + "年級最高GPA";
                string key2 = schoolYear + "學年度" + semester + "學期" + grade + "年級平均GPA";

                if (!_old.ContainsKey(key1))
                    _old.Add(key1, max);
                if (!_old.ContainsKey(key2))
                    _old.Add(key2, avg);

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv, schoolYear, semester, grade, max, avg);

                dgv.Rows.Add(row);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string msg = "將以預設的學年度: " + _SchoolYear + " 學期: " + _Semester + " 產生資料,相同時間點的資料將被覆蓋,確認繼續?";
            if (MessageBox.Show(msg, "ischool", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (!Tool.UpdateGPAref(_SchoolYear, _Semester))
                    MessageBox.Show("更新失敗,因為 學年度: " + _SchoolYear + " 學期: " + _Semester + " 以後的資料已經存在");
                else
                    MessageBox.Show("更新完成");

                LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _new.Clear();
            if (ValidateData())
            {
                List<GpaRef> insert = new List<GpaRef>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string schoolYear = row.Cells[colSchoolYear.Index].Value + "";
                    string semester = row.Cells[colSemester.Index].Value + "";
                    string grade = row.Cells[colGrade.Index].Value + "";
                    string max = row.Cells[colMax.Index].Value + "";
                    string avg = row.Cells[colAvg.Index].Value + "";
                    decimal d;

                    GpaRef gr = new GpaRef();
                    gr.SchoolYear = int.Parse(schoolYear);
                    gr.Semester = int.Parse(semester);
                    gr.Grade = int.Parse(grade);
                    gr.Max = decimal.TryParse(max, out d) ? d : 0;
                    gr.Avg = decimal.TryParse(avg, out d) ? d : 0;
                    insert.Add(gr);

                    string key1 = schoolYear + "學年度" + semester + "學期" + grade + "年級最高GPA";
                    string key2 = schoolYear + "學年度" + semester + "學期" + grade + "年級平均GPA";

                    if (!_new.ContainsKey(key1))
                        _new.Add(key1, max);
                    if (!_new.ContainsKey(key2))
                        _new.Add(key2, avg);
                }

                _A.DeletedValues(_Records);
                _A.InsertValues(insert);
                FiscaLogWriter();
                MessageBox.Show("儲存完成");
                LoadData();
            }
            else
            {
                MessageBox.Show("資料有誤,請確認後再儲存");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateData()
        {
            bool retVal = true;
            List<string> check = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string schoolYear = row.Cells[colSchoolYear.Index].Value + "";
                string semester = row.Cells[colSemester.Index].Value + "";
                string grade = row.Cells[colGrade.Index].Value + "";
                string max = row.Cells[colMax.Index].Value + "";
                string avg = row.Cells[colAvg.Index].Value + "";

                int i;
                decimal d;

                //row check
                row.ErrorText = "";
                string key = schoolYear + "_" + semester + "_" + grade;
                if (!check.Contains(key))
                {
                    check.Add(key);
                }
                else
                {
                    row.ErrorText = "資料重複,學年度學期年級的組合必須是唯一";
                    retVal = false;
                }

                //schoolYear
                row.Cells[colSchoolYear.Index].ErrorText = "";
                if (string.IsNullOrWhiteSpace(schoolYear))
                {
                    row.Cells[colSchoolYear.Index].ErrorText = "學年度不可為空白";
                    retVal = false;
                }
                else
                {
                    if(!int.TryParse(schoolYear,out i))
                    {
                        row.Cells[colSchoolYear.Index].ErrorText = "學年度必須為數字";
                        retVal = false;
                    }
                }

                //semester
                row.Cells[colSemester.Index].ErrorText = "";
                if (string.IsNullOrWhiteSpace(semester))
                {
                    row.Cells[colSemester.Index].ErrorText = "學期不可為空白";
                    retVal = false;
                }
                else
                {
                    if (!int.TryParse(semester, out i))
                    {
                        row.Cells[colSemester.Index].ErrorText = "學期必須為數字";
                        retVal = false;
                    }
                }

                //grade
                row.Cells[colGrade.Index].ErrorText = "";
                if (string.IsNullOrWhiteSpace(grade))
                {
                    row.Cells[colGrade.Index].ErrorText = "年級不可為空白";
                    retVal = false;
                }
                else
                {
                    if (int.TryParse(grade, out i))
                    {
                        if (i < 9 || i > 12)
                        {
                            row.Cells[colGrade.Index].ErrorText = "年級必須為9~12之間";
                            retVal = false;
                        }
                    }
                    else
                    {
                        row.Cells[colGrade.Index].ErrorText = "年級必須為數字";
                        retVal = false;
                    }
                }

                //max
                row.Cells[colMax.Index].ErrorText = "";
                if (!string.IsNullOrWhiteSpace(max))
                {
                    if (decimal.TryParse(max, out d))
                    {
                        row.Cells[colMax.Index].Value = Math.Round(d, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        row.Cells[colMax.Index].ErrorText = "MaxGPA必須為數字";
                        retVal = false;
                    }
                }

                //avg
                row.Cells[colAvg.Index].ErrorText = "";
                if (!string.IsNullOrWhiteSpace(avg))
                {
                    if (decimal.TryParse(avg, out d))
                    {
                        row.Cells[colAvg.Index].Value = Math.Round(d, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        row.Cells[colAvg.Index].ErrorText = "AvgGPA必須為數字";
                        retVal = false;
                    }
                }
            }
            return retVal;
        }

        private void FiscaLogWriter()
        {
            List<string> update = new List<string>();
            List<string> insert = new List<string>();
            List<string> delete = new List<string>();
            foreach (string key in _new.Keys)
            {
                if (_old.ContainsKey(key))
                {
                    if (_old[key] != _new[key])
                    {
                        update.Add(key + " 由 " + _old[key] + " 改為 " + _new[key]);
                    }
                }
                else
                {
                    insert.Add("新增 " + key + " 為 " + _new[key]);
                }
            }

            foreach (string key in _old.Keys)
            {
                if (!_new.ContainsKey(key))
                {
                    delete.Add("刪除 " + key + " 值為 " + _old[key]);
                }
            }

            if (update.Count > 0)
                FISCA.LogAgent.ApplicationLog.Log("GPA統計", "修改", string.Join("\r\n", update));

            if (insert.Count > 0)
                FISCA.LogAgent.ApplicationLog.Log("GPA統計", "新增", string.Join("\r\n", insert));

            if (delete.Count > 0)
                FISCA.LogAgent.ApplicationLog.Log("GPA統計", "刪除", string.Join("\r\n", delete));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string text = "自動統計功能說明:";
            text += "\r\n將以系統預設的學年度學期,針對現有9~12年級的班級計算出最高及平均GPA\r\n";
            text += "\r\n注意事項:";
            text += "\r\n1.若系統存有預設學年度學期之後的資料,為確保過去資料不被覆蓋,將不作計算";
            text += "\r\n2.使用時間點應該是在班級升級之前,才能正確保存當時的GPA計算資料";

            MessageBox.Show(text);
        }
    }
}
