using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreCharger
{
    public partial class KeysightForm : Form
    {
        bool isReadDataLog;
        string _cmdtype;
        string precharge_time, charge_time, discharge_time;
        double precharge_current, precharge_voltage;
        double charge_current, charge_voltage;
        double discharge_current, discharge_voltage;

        #region Delegate - MainForm에서 사용
        public delegate void delegateReport_SendCommand(int stageno, string command);
        public event delegateReport_SendCommand OnSendCommand = null;
        protected void RaiseOnSendCommand(int stageno, string command)
        {
            if (OnSendCommand != null)
            {
                OnSendCommand(stageno, command);
            }
        }

        public delegate void delegateReport_SetCharging(int stageno, string[] prechargevalues, string[] chargevalues);
        public event delegateReport_SetCharging OnSetCharging = null;
        protected void RaiseOnSetCharging(int stageno, string[] prechargevalues, string[] chargevalues)
        {
            if (OnSetCharging != null)
            {
                OnSetCharging(stageno, prechargevalues, chargevalues);
            }
        }

        public delegate void delegateReport_SetDischarging(int stageno, string[] dischargevalues);
        public event delegateReport_SetDischarging OnSetDischarging = null;
        protected void RaiseOnSetDischarging(int stageno, string[] dischargevalues)
        {
            if (OnSetDischarging != null)
            {
                OnSetDischarging(stageno, dischargevalues);
            }
        }

        public delegate void delegateReport_StartSequence(int stageno);
        public event delegateReport_StartSequence OnStartSequence = null;
        protected void RaiseOnStartSequence(int stageno)
        {
            if (OnStartSequence != null)
            {
                OnStartSequence(stageno);
            }
        }

        public delegate void delegateReport_AbortSequence(int stageno);
        public event delegateReport_AbortSequence OnAbortSequence = null;
        protected void RaiseOnAbortSequence(int stageno)
        {
            if (OnAbortSequence != null)
            {
                OnAbortSequence(stageno);
            }
        }

        public delegate void delegateReport_GetDataLog(int stageno);
        public event delegateReport_GetDataLog OnGetDataLog = null;
        protected void RaiseOnGetDataLog(int stageno)
        {
            if (OnGetDataLog != null)
            {
                OnGetDataLog(stageno);
            }
        }

        public delegate void delegateReport_DeviceClear(int stageno);
        public event delegateReport_DeviceClear OnDeviceClear = null;
        protected void RaiseOnDeviceClear(int stageno)
        {
            if (OnDeviceClear != null)
            {
                OnDeviceClear(stageno);
            }
        }
        #endregion

        public KeysightForm()
        {
            InitializeComponent();

            isReadDataLog = false;
            _cmdtype = "0";
            precharge_current = precharge_voltage = charge_current = charge_voltage = discharge_current = discharge_voltage = 0.0;
            precharge_time = "0";
            charge_time = "0";
            discharge_time = "0";
        }

        #region Keysight Command
        private void SetCommand(int nCommandIndex, string command)
        {
            string commandtype = string.Empty;
            string commandtext = string.Empty;
            try
            {
                commandtype = command.Split('.')[0].Trim();
                commandtext = command.Split('.')[1].Trim();
                if (Convert.ToInt16(commandtype) < 20)
                    tbCommand.Text = commandtext;
                else
                    ShowChargingPanel(commandtype);
            }
            catch(Exception ex)
            {
                tbMsg.Text = "SetCommand Error : " + ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        private void ShowChargingPanel(string commandtype)
        {
            string commandset = string.Empty;
            try
            {
                precharge_time = tbPrechargeTime.Text;
                precharge_current = Convert.ToDouble(tbPrechargeCurrent.Text) / 1000.0;
                precharge_voltage = Convert.ToDouble(tbPrechargeVoltage.Text) / 1000.0;

                charge_time = tbChargeTime.Text;
                charge_current = Convert.ToDouble(tbChargeCurrent.Text) / 1000.0;
                charge_voltage = Convert.ToDouble(tbChargeVoltage.Text) / 1000.0;

                discharge_time = tbDischargeTime.Text;
                discharge_current = Convert.ToDouble(tbDischargeCurrent.Text) / 1000.0;
                discharge_voltage = Convert.ToDouble(tbDischargeVoltage.Text) / 1000.0;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            switch (commandtype)
            {
                case "21": // SET CHARGING
                    commandset = "*RST" + Environment.NewLine;
                    commandset += "SEQ:CLEar 1" + Environment.NewLine;
                    commandset += "SYST:PROB:LIM 2,0" + Environment.NewLine;
                    commandset += "CELL:DEF:QUICk 1" + Environment.NewLine;
                    commandset += "SEQ:STEP:DEF 1,1,PRECHARGE," + precharge_time
                        + "," + precharge_current.ToString() + "," + precharge_voltage.ToString() + Environment.NewLine;
                    commandset += "SEQ:STEP:DEF 1,2,CHARGE," + charge_time
                        + "," + charge_current.ToString() + "," + charge_voltage.ToString() + Environment.NewLine;
                    break;
                case "22": // SET DISCHARGING
                    commandset = "*RST" + Environment.NewLine;
                    commandset += "SEQ:CLEar 1" + Environment.NewLine;
                    commandset += "SYST:PROB:LIM 2,0" + Environment.NewLine;
                    commandset += "CELL:DEF:QUICk 1" + Environment.NewLine;
                    commandset += "SEQ:STEP:DEF 1,1,DISCHARGE," + discharge_time
                        + "," + discharge_current.ToString() + "," + discharge_voltage.ToString() + Environment.NewLine;
                    break;
                case "23": // START SEQUENCE
                    commandset = "CELL:ENABLE (@1001:8032),1" + Environment.NewLine;
                    commandset += "CELL:INIT (@1001:8032)" + Environment.NewLine;
                    break;
                case "24": // ABORT SEQUENCE
                    commandset = "SEQ:ABORT";
                    break;
                default:
                    break;
            }

            pnlCharging.Visible = true;
            tbCharging.Text = commandset;
            _cmdtype = commandtype;
        }
        private void SENDCOMMAND()
        {
            string CMD = tbCommand.Text;
            int stageno = cbKeysigt.SelectedIndex;
            RaiseOnSendCommand(stageno, CMD);
        }
        private void SENDCHARGINGCOMMAND()
        {
            int stageno = cbKeysigt.SelectedIndex;

            if (_cmdtype == "21") SETCHARGING(stageno);
            else if (_cmdtype == "22") SETDISCHARGING(stageno);
            else if (_cmdtype == "23") STARTSEQUENCE(stageno);
            else if (_cmdtype == "24") ABORTSEQUENCE(stageno);
        }
        private void SETCHARGING(int stageno)
        {
            string[] prechargevalues = new string[3];
            prechargevalues[0] = precharge_time;
            prechargevalues[1] = precharge_current.ToString();
            prechargevalues[2] = precharge_voltage.ToString();

            string[] chargevalues = new string[3];
            chargevalues[0] = charge_time;
            chargevalues[1] = charge_current.ToString();
            chargevalues[2] = charge_voltage.ToString();

            RaiseOnSetCharging(stageno, prechargevalues, chargevalues);
            //OnSetCharging.Invoke(prechargevalues, chargevalues);
        }
        private void SETDISCHARGING(int stageno)
        {
            string[] dischargevalues = new string[3];
            dischargevalues[0] = discharge_time;
            dischargevalues[1] = discharge_current.ToString();
            dischargevalues[2] = discharge_voltage.ToString();

            RaiseOnSetDischarging(stageno, dischargevalues);
        }
        private void STARTSEQUENCE(int stageno)
        {
            RaiseOnStartSequence(stageno);
        }
        private void ABORTSEQUENCE(int stageno)
        {
            RaiseOnAbortSequence(stageno);
        }
        private void GETDATALOG(int stageno)
        {
            GetDataLogWhile(stageno);
        }
        private void STOPDATALOG(int stageno)
        {
            isReadDataLog = false;
        }
        private void GetDataLogWhile(int stageno)
        {
            while(isReadDataLog)
            {
                RaiseOnGetDataLog(stageno);
            }
        }
        private void DEVICECLEAR(int stageno)
        {
            RaiseOnDeviceClear(stageno);
        }
        #endregion

        #region Event
        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            SENDCOMMAND();
        }
        private void lblCmd_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int nCommandIndex = int.Parse(lbl.Tag.ToString());
            SetCommand(nCommandIndex, lbl.Text);
        }
        private void lblCmd2_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int nCommandIndex = int.Parse(lbl.Tag.ToString());
            int stageno = cbKeysigt.SelectedIndex;
            if (nCommandIndex == 31) DEVICECLEAR(stageno);
            else if (nCommandIndex == 32) GETDATALOG(stageno);
            else if (nCommandIndex == 33) STOPDATALOG(stageno);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearResult();
        }
        private void btnCharging_Click(object sender, EventArgs e)
        {
            SENDCHARGINGCOMMAND();
        }
        private void btnCloseCharging_Click(object sender, EventArgs e)
        {
            CloseChargingPanel();
        }
        #endregion

        #region Method
        public void SetResult(string results)
        {
            if (tbMsg.InvokeRequired)
            {
                // 작업쓰레드인 경우
                tbMsg.BeginInvoke(new Action(() => tbMsg.AppendText(results + Environment.NewLine + Environment.NewLine)));
            }
            else
            {
                // UI 쓰레드인 경우
                tbMsg.AppendText(results + Environment.NewLine + Environment.NewLine);
            }
        }
        private void ClearResult()
        {
            tbMsg.Text = "";
        }
        private void CloseChargingPanel()
        {
            pnlCharging.Visible = false;
        }
        #endregion

        private void btnKeysightOpen_Click(object sender, EventArgs e)
        {

        }
    }
}
