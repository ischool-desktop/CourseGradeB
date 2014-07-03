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
        ConductSetting _LowGrade, _MediumGrade, _HighGrade;
        public SetHoursOpeningForm()
        {
            InitializeComponent();
            _A = new AccessHelper();

            foreach (ConductSetting cs in _A.Select<ConductSetting>())
            {
                if (cs.Grade <= 2)
                    _LowGrade = cs;
                else if (cs.Grade <= 6)
                    _MediumGrade = cs;
                else
                    _HighGrade = cs;
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

                _LowGrade.MiddleBegin = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                _LowGrade.MiddleEnd = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                _LowGrade.FinalBegin = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                _LowGrade.FinalEnd = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");

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
                    _LowGrade.MiddleBeginC = DateTime.Parse(row.Cells[colMiddleBegin.Index].Value + "");
                    _LowGrade.MiddleEndC = DateTime.Parse(row.Cells[colMiddleEnd.Index].Value + "");
                    _LowGrade.FinalBeginC = DateTime.Parse(row.Cells[colFinalBegin.Index].Value + "");
                    _LowGrade.FinalEndC = DateTime.Parse(row.Cells[colFinalEnd.Index].Value + "");
                }
                else if (grade == "3-12")
                {
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

            _LowGrade.Save();
            _MediumGrade.Save();
            _HighGrade.Save();
            this.Close();
        }

        private void SetHoursOpeningForm_Load(object sender, EventArgs e)
        {
            //dgvScore
            DataGridViewRow row_score = new DataGridViewRow();
            row_score.CreateCells(dgvScore, "1-12年級", _LowGrade.MiddleBegin.ToShortDateString(), _LowGrade.MiddleEnd.ToShortDateString(), _LowGrade.FinalBegin.ToShortDateString(), _LowGrade.FinalEnd.ToShortDateString());
            dgvScore.Rows.Add(row_score);

            //dgvConduct
            DataGridViewRow row_conduct1 = new DataGridViewRow();
            row_conduct1.CreateCells(dgvConduct, "1-2年級", _LowGrade.MiddleBeginC.ToShortDateString(), _LowGrade.MiddleEndC.ToShortDateString(), _LowGrade.FinalBeginC.ToShortDateString(), _LowGrade.FinalEndC.ToShortDateString());
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
