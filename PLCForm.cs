using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreCharger.Common;

namespace PreCharger
{
    public partial class PLCForm : Form
    {
        private static PLCForm plcform;
        DataGridView[] dgvPCs = new DataGridView[_Constant.frmCount];
        DataGridView[] dgvPLCs = new DataGridView[_Constant.frmCount];
        public static PLCForm GetInstance()
        {
            if (plcform == null) plcform = new PLCForm();
            return plcform;
        }
        public PLCForm()
        {
            InitializeComponent();
            plcform = this;

            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                dgvPCs[nIndex] = new DataGridView();
                dgvPLCs[nIndex] = new DataGridView();
            }

            dgvPCs[0] = dgvPC1;   dgvPCs[1] = dgvPC2;   dgvPCs[2] = dgvPC3;   dgvPCs[3] = dgvPC4;
            dgvPLCs[0] = dgvPLC1; dgvPLCs[1] = dgvPLC2; dgvPLCs[2] = dgvPLC3; dgvPLCs[3] = dgvPLC4;

            MakeGridView();
        }

        private void MakeGridView()
        {
            for(int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                #region PLC
                //* 열 추가
                dgvPLCs[nIndex].Columns.Add("001", "PLC Address");
                dgvPLCs[nIndex].Columns.Add("002", "PLC Name");
                dgvPLCs[nIndex].Columns.Add("003", "PLC Value");

                dgvPLCs[nIndex].Columns[0].Width = 120;
                dgvPLCs[nIndex].Columns[1].Width = 170;
                dgvPLCs[nIndex].Columns[2].Width = 140;

                //* 행 추가
                dgvPLCs[nIndex].Rows.Add(300);

                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_HEART_BEAT), "PLC HEART BEAT", 0);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_ERROR), "PLC ERROR", 2);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_ATUO_MANUAL), "PLC AUTO/MANUAL", 1);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_TRAY_IN), "PLC TRAY IN", 3);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_PROB_OPEN), "PLC PROB OPEN", 4);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_PROB_CLOSE), "PLC PROB CLOSE", 5);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_PRE_TRAY_ID), "PLC TRAY ID", 6);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_CELL_DATA), "PLC TRAY CELL DATA", 7);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                {
                    AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_CELL_DATA + cellIndex), "TRAY CELL DATA #" + (cellIndex + 1), cellIndex + 8);
                }

                dgvPLCs[nIndex].ScrollBars = ScrollBars.Both;
                dgvPLCs[nIndex].PerformLayout();
                #endregion

                #region PC
                dgvPCs[nIndex].Columns.Add("001", "PC Address");
                dgvPCs[nIndex].Columns.Add("002", "PC Name");
                dgvPCs[nIndex].Columns.Add("003", "PC Value");

                dgvPCs[nIndex].Columns[0].Width = 120;
                dgvPCs[nIndex].Columns[1].Width = 170;
                dgvPCs[nIndex].Columns[2].Width = 130;

                dgvPCs[nIndex].Rows.Add(544);

                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_HEART_BEAT, "PC HEART BEAT", 0);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_ERROR, "PC ERROR", 2);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_STAGE_AUTO_READY, "PC AUTO MANUAL", 1);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_TRAY_OUT, "PC TRAY OUT", 3);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_PROB_OPEN, "PC PROB OPEN", 4);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_PROB_CLOSE, "PC PROB CLOSE", 5);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_CHARGING, "PC CHARGING", 6);

                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_NGCOUNT, "PRECHARGER NG COUNT", 8);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_CURRENT_MIN, "CURRENT MIN. VALUE", 9);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_CHARGE_VOLTAGE, "CHARGE VOLTAGE", 10);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_CHARGE_CURRENT, "CHARGE CURRENT", 11);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_CHARGE_TIME, "CHARGE TIME", 12);

                for (int cellIndex = 0; cellIndex < 16; cellIndex++)
                    AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_PRE_OKNG + cellIndex, "PRECHARGE OK/NG #" + (cellIndex + 1), cellIndex + 14);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                    AddTitleGridView(dgvPCs[nIndex], nIndex, +_Constant.PC_PRE_VOLTAGE + cellIndex * 2, "PRECHARGE VOLTAGE #" + (cellIndex + 1), cellIndex + 31);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                    AddTitleGridView(dgvPCs[nIndex], nIndex, +_Constant.PC_PRE_CURRENT + cellIndex * 2, "PRECHARGE CURRENT #" + (cellIndex + 1), cellIndex + 288);

                dgvPCs[nIndex].ScrollBars = ScrollBars.Both;
                dgvPCs[nIndex].PerformLayout();
                #endregion
            }
        }

        private void MakeGridView2()
        {
            #region PLC
            //* 열 추가
            dgvPLC4.Columns.Add("001", "PLC Address");
            dgvPLC4.Columns.Add("002", "PLC Name");
            dgvPLC4.Columns.Add("003", "PLC Value");

            dgvPLC4.Columns[0].Width = 120;
            dgvPLC4.Columns[1].Width = 170;
            dgvPLC4.Columns[2].Width = 130;

            //* 행 추가
            dgvPLC4.Rows.Add(300);

            for(int i = 0; i < _Constant.frmCount; i++)
            {
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_HEART_BEAT), "PLC HEART BEAT", 0);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_ERROR), "PLC ERROR", 2);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_ATUO_MANUAL), "PLC AUTO/MANUAL", 1);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_TRAY_IN), "PLC TRAY IN", 3);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_PROB_OPEN), "PLC PROB OPEN", 4);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_PROB_CLOSE), "PLC PROB CLOSE", 5);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_PRE_TRAY_ID), "PLC TRAY ID", 6);
                AddTitleGridView(dgvPLC4, i, (_Constant.PLC_TRAY_CELL_DATA), "PLC TRAY CELL DATA", 7);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                {
                    AddTitleGridView(dgvPLC4, i, (_Constant.PLC_TRAY_CELL_DATA + cellIndex), "TRAY CELL DATA #" + (cellIndex + 1), cellIndex + 8);
                }
            }

            dgvPLC4.ScrollBars = ScrollBars.Both;
            dgvPLC4.PerformLayout();
            #endregion

            #region PC
            dgvPC4.Columns.Add("001", "PC Address");
            dgvPC4.Columns.Add("002", "PC Name");
            dgvPC4.Columns.Add("003", "PC Value");

            dgvPC4.Columns[0].Width = 120;
            dgvPC4.Columns[1].Width = 170;
            dgvPC4.Columns[2].Width = 130;

            dgvPC4.Rows.Add(540);

            for(int i = 0; i < _Constant.frmCount; i++)
            {
                AddTitleGridView(dgvPC4, i, _Constant.PC_HEART_BEAT, "PC HEART BEAT", 0);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_ERROR, "PC ERROR", 2);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_STAGE_AUTO_READY, "PC AUTO MANUAL", 1);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_TRAY_OUT, "PC TRAY OUT", 3);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_PROB_OPEN, "PC PROB OPEN", 4);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_PROB_CLOSE, "PC PROB CLOSE", 5);
                AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_CHARGING, "PC CHARGING", 6);

                for (int cellIndex = 0; cellIndex < 16; cellIndex++)
                    AddTitleGridView(dgvPC4, i, _Constant.PC_PRE_OKNG + cellIndex, "PRECHARGE OK/NG #" + (cellIndex + 1), cellIndex + 7);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                    AddTitleGridView(dgvPC4, i, + _Constant.PC_PRE_VOLTAGE + cellIndex * 2, "PRECHARGE VOLTAGE #" + (cellIndex + 1), cellIndex + 24);

                for (int cellIndex = 0; cellIndex < 256; cellIndex++)
                    AddTitleGridView(dgvPC4, i, + _Constant.PC_PRE_CURRENT + i * 2, "PRECHARGE CURRENT #" + (cellIndex + 1), cellIndex + 280);
            }
            


            dgvPC4.ScrollBars = ScrollBars.Both;
            dgvPC4.PerformLayout();
            #endregion
        }

        private void MakeGridViewPLC(int nIndex)
        {
            DataGridView dgv;
            if (nIndex == 0) dgv = dgvPLC1;
            else if(nIndex == 1) dgv = dgvPLC2;
            else if (nIndex == 2) dgv = dgvPLC3;
            else if (nIndex == 3) dgv = dgvPLC4;


        }

        private void MakeGridViewPC(int nIndex)
        {
            DataGridView dgv;
            if (nIndex == 0) dgv = dgvPC1;
            else if (nIndex == 1) dgv = dgvPC2;
            else if (nIndex == 2) dgv = dgvPC3;
            else if (nIndex == 3) dgv = dgvPC4;


        }

        private void AddTitleGridView(DataGridView dgv, int frmIndex, int iAddress, string sName, int nRow)
        {
            string sAddress = "";
            if (dgv.Name.Contains("dgvPLC")) sAddress = (_Constant.PLC_D_START_NUM[frmIndex] + iAddress).ToString();
            else if(dgv.Name.Contains("dgvPC")) sAddress = (_Constant.PC_D_START_NUM[frmIndex] + iAddress).ToString();

            dgv.Rows[nRow].Cells[0].Value = sAddress;
            dgv.Rows[nRow].Cells[1].Value = sName;
        }

        private void AddDataGridView(DataGridView dgv, int iAddress, int iSize, int[] pData, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = GetStringFromWord(iAddress, iSize, pData);
        }
        private void AddDataGridView(DataGridView dgv, int iAddress, int[] pData, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = pData[iAddress].ToString();
        }

        public void SetDataToGrid(int[] pcData, int[] plcData, int nIndex)
        {
            #region PLC DATA VIEW
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_HEART_BEAT, plcData, 0);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_ERROR, plcData, 2);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_ATUO_MANUAL, plcData, 1);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_TRAY_IN, plcData, 3);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_PROB_OPEN, plcData, 4);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_PROB_CLOSE, plcData, 5);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_PRE_TRAY_ID, _Constant.PLC_TRAY_ID_SIZE * 2, plcData, 6);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_CELL_DATA, plcData, 7);

            #endregion

            #region PC DATA VIEW
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_HEART_BEAT, pcData, 0);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_ERROR, pcData, 2);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_STAGE_AUTO_READY, pcData, 1);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_TRAY_OUT, pcData, 3);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_PROB_OPEN, pcData, 4);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_PROB_CLOSE, pcData, 5);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_PRE_CHARGING, pcData, 6);


            #endregion

            //for(int i = 0; i < 16; i++)
            //    dgvPC.Rows[120 + i].Cells[2].Value = GetBitFromWord(120 + i, pcData);
        }

        public string GetStringFromWord(int start_index, int size, int[] iScanData)
        {
            string strValue = "";
            StringBuilder sb = new StringBuilder();

            int iLength = (size + 1) / 2;

            for (int i = 0; i < iLength; i++)
            {
                sb.Append(Convert.ToChar(iScanData[start_index + i] & 0xFF));
                sb.Append(Convert.ToChar((iScanData[start_index + i] >> 8) & 0xFF));
            }

            strValue = sb.ToString();
            strValue = strValue.Substring(0, iLength);
            strValue = strValue.Trim(" \0".ToCharArray());

            return strValue;
            
        }

        public string GetBitFromWord(int nIndex, ushort[] iScanData)
        {
            string strValue = "";
            for(int i = 0; i < 16; i++)
            {
                strValue += ((iScanData[nIndex] & (1 << i)) != 0) ? "1" : "0";
            }

            return strValue;
        }

        private void PLCForm_Load(object sender, EventArgs e)
        {
            
        }

        private void PLCForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
