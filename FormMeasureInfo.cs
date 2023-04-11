using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreCharger.Common;

namespace PreCharger
{
    public partial class FormMeasureInfo : Form
    {
        public int _iStage;
        public bool _bManualMode;
        Util util;
        private EquipProcess _EQProcess = null;

        #region Delegate
        public delegate void delegateReport_StartCharging(int stageno);
        public event delegateReport_StartCharging OnStartCharging = null;
        protected void RaiseOnStartCharging(int stageno)
        {
            if (OnStartCharging != null)
            {
                OnStartCharging(stageno);
            }
        }
        #endregion

        private static FormMeasureInfo measureinfoForm = new FormMeasureInfo();
        public static FormMeasureInfo GetInstance()
        {
            if (measureinfoForm == null) measureinfoForm = new FormMeasureInfo();
            return measureinfoForm;
        }
        public FormMeasureInfo()
        {
            InitializeComponent();

            this.Height = 940;
            measureinfoForm = this;

            util = new Util();
            _EQProcess = EquipProcess.GetInstance();

            makeGridView();
            initGridView();
            gridView.DoubleBuffered(true);
        }

        public void SetManualMode(bool bValue)
        {
            _bManualMode = bValue;
            if (_bManualMode == true) gbManualMode.Visible = true;
            else gbManualMode.Visible = false;
        }

        public void SetStage(int nIndex)
        {
            _iStage = nIndex;
            lblTitle.Text = "STAGE " + (_iStage + 1).ToString();
        }

        public void SetValueToGridView(int nIndex, string sVolt, string sCurr)
        {
            int rowIndex = nIndex / 16;
            int colIndex = nIndex % 16;

            gridView.Rows[rowIndex * 2].Cells[colIndex].Value = sVolt;
            gridView.Rows[rowIndex * 2 + 1].Cells[colIndex].Value = sCurr;
        }

        private void makeGridView()
        {
            #region 행/열 제목, 갯수
            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                gridView.Columns.Add("CH" + nIndex.ToString("D2"), (nIndex + 1).ToString("D2"));
                gridView.Columns[nIndex].Width = 60;
                gridView.Columns[nIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            gridView.Rows.Clear();
            gridView.Rows.Add(32);
            gridView.RowHeadersWidth = 80;
            gridView.ColumnHeadersHeight = 40;
            //gridView.RowTemplate.Height = 30;

            for (int i = 0; i < 16; i++)
            {
                gridView.Rows[i * 2].HeaderCell.Value = (i + 1).ToString();
                gridView.Rows[i * 2 + 1].HeaderCell.Value = "-";
            }
            #endregion
        }
        public void initGridView()
        {
            gridView.RowsDefaultCellStyle.BackColor = _Constant.ColorVoltage;
            gridView.AlternatingRowsDefaultCellStyle.BackColor = _Constant.ColorCurrent;
            gridView.ClearSelection();
            gridView.DefaultCellStyle.Font = new Font("Times New Roman", 10);

            int nRow, nCol, nIdx;
            for(int nRowIndex = 0; nRowIndex < 16; nRowIndex++)
            {
                for(int nColIndex = 0; nColIndex < 16; nColIndex++)
                {
                    //nCol = nColIndex;
                    //nRow = nRowIndex * 2;
                    nIdx = nRowIndex * 16 + nColIndex;

                    util.ChangeMapToGridView(nIdx, out nRow, out nCol);
                    nRow = nRow * 2;

                    /// Voltage
                    gridView.Rows[nRow].Cells[nCol].Value = (nIdx + 1).ToString();
                    gridView.Rows[nRow].Cells[nCol].Style.BackColor = _Constant.ColorVoltage;
                    /// Current
                    gridView.Rows[nRow + 1].Cells[nCol].Value = (nIdx + 16) / 16 + " - " + ((nIdx % 16) + 1);
                    gridView.Rows[nRow + 1].Cells[nCol].Style.BackColor = _Constant.ColorCurrent;
                    /// Divider line
                    gridView.Rows[nRow + 1].DividerHeight = 2;

                    /// Row Height
                    gridView.Rows[nRow].Height = 24;
                    gridView.Rows[nRow + 1].Height = 24;
                }
            }
        }
        public void initGridView(bool bInitValue)
        {
            int nRow, nCol, nIdx;
            for (int nRowIndex = 0; nRowIndex < 16; nRowIndex++)
            {
                for (int nColIndex = 0; nColIndex < 16; nColIndex++)
                {
                    nIdx = nRowIndex * 16 + nColIndex;

                    util.ChangeMapToGridView(nIdx, out nRow, out nCol);
                    nRow = nRow * 2;

                    /// Voltage
                    gridView.Rows[nRow].Cells[nCol].Value = (nIdx + 1).ToString();
                    gridView.Rows[nRow].Cells[nCol].Style.BackColor = _Constant.ColorVoltage;
                    /// Current
                    gridView.Rows[nRow + 1].Cells[nCol].Value = (nIdx + 16) / 16 + " - " + ((nIdx % 16) + 1);
                }
            }
        }

        public void DisplayChannelInfo(int stageno, CPrechargerData CPreData)
        {
            int nIndex;
            int nRow_Volt, nRow_Curr, nCol;

            if (stageno != _iStage) return;

            this.Invoke((MethodInvoker)delegate{
                for (int rowIndex = 0; rowIndex < 16; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < 16; colIndex++)
                    {
                        nIndex = rowIndex * 16 + colIndex;
                        //nRow_Volt = rowIndex * 2;
                        //nRow_Curr = rowIndex * 2 + 1;
                        //nCol = colIndex;

                        util.ChangeMapToGridView(nIndex, out nRow_Volt, out nCol);
                        nRow_Volt = nRow_Volt * 2;
                        nRow_Curr = nRow_Volt + 1;

                        enumChannelStatus eChannelStatus = CPreData.ChannelStatus[nIndex];
                        Color clrChannelColor = CPreData.CHANNELCOLOR[nIndex];
                        string errormsg = CPreData.ERRORMSG[nIndex];
                        bool iCellExist = CPreData.CELL[nIndex];

                        if (iCellExist == false)
                        {
                            /// 1번
                            //gridView.Rows[nRow_Volt].Cells[nCol].Style.BackColor = clrChannelColor;
                            //gridView.Rows[nRow_Curr].Cells[nCol].Style.BackColor = clrChannelColor;

                            /// 2번
                            if (clrChannelColor == _Constant.ColorError || clrChannelColor == _Constant.ColorFlow)
                            {
                                gridView.Rows[nRow_Volt].Cells[nCol].Style.BackColor = clrChannelColor;
                                gridView.Rows[nRow_Curr].Cells[nCol].Style.BackColor = clrChannelColor;
                            }
                            else
                            {
                                gridView.Rows[nRow_Volt].Cells[nCol].Style.BackColor = _Constant.ColorNoCell;
                                gridView.Rows[nRow_Curr].Cells[nCol].Style.BackColor = _Constant.ColorNoCell;
                            }
                        }
                        else
                        {
                            if (clrChannelColor == _Constant.ColorReady || clrChannelColor == _Constant.ColorCharging || clrChannelColor == _Constant.ColorFinish)
                            {
                                gridView.Rows[nRow_Volt].Cells[nCol].Style.BackColor = _Constant.ColorVoltage;
                                gridView.Rows[nRow_Curr].Cells[nCol].Style.BackColor = _Constant.ColorCurrent;
                            }
                        }

                        if (errormsg != string.Empty && clrChannelColor == _Constant.ColorError)
                        {
                            gridView.DefaultCellStyle.Font = new Font("Times New Roman", 8);
                            gridView.Rows[nRow_Volt].Cells[nCol].Value = "ERROR";
                            gridView.Rows[nRow_Curr].Cells[nCol].Value = errormsg;
                            gridView.Rows[nRow_Volt].Cells[nCol].Style.BackColor = clrChannelColor;
                            gridView.Rows[nRow_Curr].Cells[nCol].Style.BackColor = clrChannelColor;
                        }
                        else if (clrChannelColor != _Constant.ColorNoCell)
                        {
                            gridView.DefaultCellStyle.Font = new Font("Times New Roman", 10);
                            gridView.Rows[nRow_Volt].Cells[nCol].Value = CPreData.VOLT[nIndex];
                            gridView.Rows[nRow_Curr].Cells[nCol].Value = CPreData.CURR[nIndex];
                        }

                    }

                }
            });
        }
        public void SetChargingTime(int nSeconds)
        {
            if (lblTestTime.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lblTestTime.BeginInvoke(new Action(() => lblTestTime.Text = nSeconds.ToString()));
            }
            else
            {
                // UI 쓰레드인 경우
                lblTestTime.Text = nSeconds.ToString();
            }
        }
        private void RunSTART()
        {
            
            
            _EQProcess.SetTrayInfo(this._iStage);
            //_EQProcess.InitDisplayInfo(this._iStage);
            //initGridView(true);
            _EQProcess.StartCharging(this._iStage);
        }

        private void RunSTOP()
        {
            _EQProcess.StopCharging(this._iStage);
           // BaseForm.frmMain.RunPreChargerCmd("AMF", this._iStage);
            RunProbeOpen();
            //BaseForm.frmMain.SetBitPLC(this._iStage, "PROBEOPEN", 1);
        }

        private void RunProbeOpen()
        {
            BaseForm.frmMain.SetBitPLC(this._iStage, "PROBEOPEN");
        }

        private void RunProbeClose()
        {
            BaseForm.frmMain.SetBitPLC(this._iStage, "PROBECLOSE");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnRunStart_Click(object sender, EventArgs e)
        {
            RaiseOnStartCharging(this._iStage);
            //RunSTART();
        }

        private void btnRunStop_Click(object sender, EventArgs e)
        {
            RunSTOP();
        }

        private void btnProbeOpen_Click(object sender, EventArgs e)
        {
            RunProbeOpen();
        }

        private void btnProbeClose_Click(object sender, EventArgs e)
        {
            RunProbeClose();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            initGridView();
        }
    }


}
