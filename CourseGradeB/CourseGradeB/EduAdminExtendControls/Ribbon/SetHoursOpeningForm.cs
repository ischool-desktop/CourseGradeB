using CourseGradeB.StuAdminExtendControls;
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
    public partial class SetHoursOpeningForm : BaseForm
    {
        AccessHelper _A;
        ConductSetting _Grade1, _Grade2, _Grade3, _Grade4, _MediumGrade, _HighGrade;
        public SetHoursOpeningForm()
        {
            InitializeComponent();
            _A = new AccessHelper();

            foreach (ConductSetting cs in _A.Select<ConductSetting>())
            {
                if (cs.Grade == 1)
                {
                    _Grade1 = cs;
                }
                else if (cs.Grade == 2)
                {
                    _Grade2 = cs;
                }
                else if (cs.Grade == 3)
                {
                    _Grade3 = cs;
                }
                else if (cs.Grade == 4)
                {
                    _Grade4 = cs;
                }
                else if (cs.Grade <= 6)
                    _MediumGrade = cs;
                else
                    _HighGrade = cs;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidated(dgvScore) | !IsValidated(dgvConduct))
            {
                MessageBox.Show("資料錯誤,請確認後在儲存");
                return;
            }

            //dgvScore
            foreach (DataGridViewRow row in dgvScore.Rows)
            {
                if (row.IsNewRow) continue;


                _Grade1.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _Grade1.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _Grade1.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _Grade1.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                _Grade2.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _Grade2.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _Grade2.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _Grade2.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                _Grade3.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _Grade3.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _Grade3.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _Grade3.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                _Grade4.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _Grade4.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _Grade4.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _Grade4.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                _MediumGrade.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _MediumGrade.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _MediumGrade.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _MediumGrade.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                _HighGrade.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _HighGrade.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _HighGrade.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _HighGrade.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");
            }

            //dgvConduct
            foreach (DataGridViewRow row in dgvConduct.Rows)
            {
                if (row.IsNewRow) continue;

                string grade = row.Tag + "";
                if (grade == "1-2")
                {
                    _Grade1.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _Grade1.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _Grade1.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _Grade1.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                    _Grade2.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _Grade2.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _Grade2.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _Grade2.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");
                }
                else if (grade == "3-12")
                {
                    _Grade3.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _Grade3.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _Grade3.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _Grade3.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                    _Grade4.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _Grade4.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _Grade4.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _Grade4.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                    _MediumGrade.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _MediumGrade.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _MediumGrade.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _MediumGrade.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

                    _HighGrade.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _HighGrade.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _HighGrade.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _HighGrade.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");
                }
            }

            _Grade1.Save();
            _Grade2.Save();
            _Grade3.Save();
            _Grade4.Save();
            _MediumGrade.Save();
            _HighGrade.Save();
            this.Close();
        }

        private void SetHoursOpeningForm_Load(object sender, EventArgs e)
        {
            //dgvScore
            DataGridViewRow row_score = new DataGridViewRow();
            row_score.CreateCells(dgvScore, "1-12年級", _Grade2.MiddleBegin.ToShortDateString(), _Grade2.MiddleEnd.ToShortDateString(), _Grade2.FinalBegin.ToShortDateString(), _Grade2.FinalEnd.ToShortDateString());
            dgvScore.Rows.Add(row_score);

            //dgvConduct
            DataGridViewRow row_conduct1 = new DataGridViewRow();
            row_conduct1.CreateCells(dgvConduct, "1-2年級", _Grade2.MiddleBeginC.ToShortDateString(), _Grade2.MiddleEndC.ToShortDateString(), _Grade2.FinalBeginC.ToShortDateString(), _Grade2.FinalEndC.ToShortDateString());
            row_conduct1.Tag = "1-2";
            dgvConduct.Rows.Add(row_conduct1);

            DataGridViewRow row_conduct2 = new DataGridViewRow();
            row_conduct2.CreateCells(dgvConduct, "3-12年級", _MediumGrade.MiddleBeginC.ToShortDateString(), _MediumGrade.MiddleEndC.ToShortDateString(), _MediumGrade.FinalBeginC.ToShortDateString(), _MediumGrade.FinalEndC.ToShortDateString());
            row_conduct2.Tag = "3-12";
            dgvConduct.Rows.Add(row_conduct2);
        }

        private bool IsValidated(DataGridView dgv)
        {
            bool retVal = true;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                CheckDateTime(row.Cells[colMiddleBegin.Index], row.Cells[colMiddleEnd.Index]);
                CheckDateTime(row.Cells[colFinalBegin.Index], row.Cells[colFinalEnd.Index]);

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!string.IsNullOrWhiteSpace(cell.ErrorText))
                        retVal = false;
                }
            }

            return retVal;
        }

        private void CheckDateTime(DataGridViewCell c1, DataGridViewCell c2)
        {
            DateTime st;
            DateTime et;

            c1.ErrorText = "";
            c2.ErrorText = "";

            if (!DateTime.TryParse(c1.Value + "", out st))
                c1.ErrorText = "日期格式不正確(yyyy/MM/dd)";

            if (!DateTime.TryParse(c2.Value + "", out et))
                c2.ErrorText = "日期格式不正確(yyyy/MM/dd)";

            if (c1.ErrorText == "" && c2.ErrorText == "")
            {
                if(et <= st)
                {
                    c1.ErrorText = "開始日期不可大於結束日期";
                    c2.ErrorText = "結束日期不可小於開始日期";
                }
            }
        }

        private void dgvScore_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != colGrade.Index)
                dgvScore.BeginEdit(true);
        }

        private void dgvConduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != colGrade.Index)
                dgvConduct.BeginEdit(true);
        }
        
    }
}
