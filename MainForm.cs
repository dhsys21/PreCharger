using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PreCharger.Common;
using System.Diagnostics;

namespace PreCharger
{
    public partial class BaseForm : Form
    {
        bool[] bFlag = { true, true, true, true };
        bool[] bSend = { false, false, false, false };
        int[] nLastSendTime = { 0, 0, 0, 0 };
        public int[] nFlag = { 0, 0, 0, 0 };
        public static BaseForm frmMain = null;

        int DeleteDay;
        int DeleteIndex;

        private CPrechargerData[] _PREData = new CPrechargerData[_Constant.frmCount];
        
        private EquipProcess _EQProcess = null;
        private CEquipmentData _System = null;
        Util util;
        PLCForm plcForm;
        ConfigForm configForm;
        //public FormMeasureInfo[] MeasureInfo = new FormMeasureInfo[_Constant.frmCount];
        
        public System.Windows.Forms.Timer[] SendTimer = new Timer[_Constant.frmCount];
        private Timer StatusTimer1 = null;

        public delegate void delegateReport_OnLabelTrayId(int nIndex, string trayid);
        public event delegateReport_OnLabelTrayId OnLabelTrayId = null;
        protected void RaiseOnOnLabelTrayId(int nIndex, string trayid)
        {
            if (OnLabelTrayId != null)
            {
                OnLabelTrayId(nIndex, trayid);
            }
        }
        public BaseForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            frmMain = this;
            util = new Util();
            plcForm = PLCForm.GetInstance();
            configForm = new ConfigForm();
            _System = CEquipmentData.GetInstance();

            StatusTimer1 = new Timer();
            StatusTimer1.Interval = 1000;
            StatusTimer1.Tick += new EventHandler(StatusTimer1_Tick);

            //* ReadSystemInfo
            configForm.ReadConfigFile();

            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                //* Measure Info Form
                //MeasureInfo[nIndex] = new FormMeasureInfo();

                //* PreCharger Data
                #region Precharger 수정필요
                _PREData[nIndex] = new CPrechargerData();
                _PREData[nIndex].InitData(nIndex);
                _PREData[nIndex].OnDisplayTestTime += _CPrechargerData_OnDisplayTestTime;

                //CmdSet(nIndex, _System.IVoltage.ToString(), _System.ICurrent.ToString(), _System.ITime.ToString());
                #endregion
            }

            //makepanel();
            makefolder();

            //MeasureInfo = new FormMeasureInfo();
            //MeasureInfo = FormMeasureInfo.GetInstance();
            lblLineNo.Text = "#" + _System.SLINENO;
        }


        private void BaseForm_Load(object sender, EventArgs e)
        {
            //_EQProcess = new EquipProcess();
            _EQProcess = EquipProcess.GetInstance();
            _EQProcess.OnAddPanel += _EQProcess_OnAddPanel;
            _EQProcess.OnPLCSignalStatus += _EQProcess_OnPLCStatus;
           // _EQProcess.OnStepInit += _EQProcess_OnStepInit;
            _EQProcess.OnStepTrayInfo += _EQProcess_OnStepTrayInfo;
            _EQProcess.OnStepCharging += _EQProcess_OnStepCharging;
            _EQProcess.OnStepStop += _EQProcess_OnStepStop;
            _EQProcess.OnStepFinish += _EQProcess_OnStepFinish;
            _EQProcess.OnShowData += _EQProcess_OnShowData;
            _EQProcess.makepanel();
            StatusTimer1.Enabled = true;

        }

        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_EQProcess != null) _EQProcess.Close();
            Application.Exit();
        }
        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you want to exit PRECHARGER?", "EXIT ACS", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //* Thread stop
                Process.GetCurrentProcess().Kill();
                // application stop
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void makefolder()
        {
            if (Directory.Exists(_Constant.APP_PATH) == false) Directory.CreateDirectory(_Constant.APP_PATH);
            if (Directory.Exists(_Constant.BIN_PATH) == false) Directory.CreateDirectory(_Constant.BIN_PATH);
            if (Directory.Exists(_Constant.DATA_PATH) == false) Directory.CreateDirectory(_Constant.DATA_PATH);
            if (Directory.Exists(_Constant.LOG_PATH) == false) Directory.CreateDirectory(_Constant.LOG_PATH);
            if (Directory.Exists(_Constant.TRAY_PATH) == false) Directory.CreateDirectory(_Constant.TRAY_PATH);
        }

        #region EQProcess Event
        private void _EQProcess_OnAddPanel(DoubleBufferedPanel pnl)
        {
            BasePanel.Controls.Add(pnl);
        }
        private void _EQProcess_OnPLCStatus(int nIndex, int iAddress, int iValue)
        {
            //if (iAddress == _Constant.PLC_PRE_ATUO_MANUAL)
            //    nForm[nIndex].OnAutoManual(iValue);
            //else if (iAddress == _Constant.PLC_PRE_ERROR)
            //    nForm[nIndex].OnPLCError(iValue);
            //else if (iAddress == _Constant.PLC_PRE_TRAY_IN)
            //    nForm[nIndex].OnTrayIN(iValue);
            //else if (iAddress == _Constant.PLC_PRE_PROB_OPEN)
            //    nForm[nIndex].OnProbeOpen(iValue);
            //else if (iAddress == _Constant.PLC_PRE_PROB_CLOSE)
            //    nForm[nIndex].OnProbeClose(iValue);
        }

        /// <summary>
        /// Display PreCharger Data to MeasureInfoForm
        /// </summary>
        private void _EQProcess_OnShowData(int nIndex, CPrechargerData cData)
        {

            //MeasureInfo.DisplayChannelInfo2(cData);
        }

        /// <summary>
        /// STEP Ready, TRAYIN, TrayInfo, PROBECLOSE, Charging, finish, PROBEOPEN, TrayOut
        /// Tray In = 0 & Ams = false                   : Ready                     : Wait
        /// Tray In = 1                                 : Barcode -> Probe Close    : initialize -> get tray info
        /// Tray In = 1 & Probe Close = 1               : Charging                  : ams
        /// Tray In = 1 & Probe Close = 1 & amf = true  : Finish -> Probe Open      : amf
        /// Tray In = 1 & Probe Open = 1                : Tray Out                  : write result
        /// </summary>
        private void _EQProcess_OnStepTrayInfo(int nIndex)
        {


        }

        private void _EQProcess_OnStepCharging(int nIndex)
        {
            //if(_PREData[nIndex].AMS == false)
            //{
            //    if (_PREData[nIndex].SET == false)
            //    {
            //        if (_PREData[nIndex].PARAM == null) nForm[nIndex].CmdSet();
            //        CmdSet(nIndex);
            //        global::System.Threading.Thread.Sleep(1000);
            //    }
            //    else
            //    {
            //        CmdStart(nIndex);
            //        _EQProcess.PRECHARGER[nIndex].EQUIPSTATUS = enumEquipStatus.StepRun;
            //        global::System.Threading.Thread.Sleep(1000);
            //    }

            //}
        }

        private void _EQProcess_OnStepStop(int nIndex)
        {
            if (_EQProcess.PRECHARGER[nIndex].EQUIPSTATUS == enumEquipStatus.StepRun
                && _PREData[nIndex].ENDCHARGING == true && _PREData[nIndex].AMF == true)
                CmdStop(nIndex, _PREData[nIndex]);
        }

        private void _EQProcess_OnStepFinish(int nIndex)
        {
            if (_EQProcess.PRECHARGER[nIndex].EQUIPSTATUS == enumEquipStatus.StepEnd)
            {
                util.NGInformation(_PREData[nIndex]);
                util.SaveResultFile(nIndex, _PREData[nIndex]);

                _EQProcess.PRECHARGER[nIndex].EQUIPSTATUS = enumEquipStatus.StepTrayOut;
                _EQProcess.SetTrayOut(nIndex, 1);
            }
        }
        #endregion


        #region PreCharger 명령보내기 / 받기 / 상태정보
        private void StatusTimer1_Tick(object sender, EventArgs e)
        {
            #region PreCharger 상태
            int iState = 0;
            for (int i = 0; i < _Constant.frmCount; i++)
            {
                if (_EQProcess.PRECHARGER[i].CONNECTIONSTATE == true)
                {
                    iState = 1;
                    //_EQProcess.AutoInspectionTimer[i].Enabled = true;
                    if(_EQProcess.PRECHARGER[i].AUTOMODE == true && _EQProcess.PRECHARGER[i].EQUIPSTATUS == enumEquipStatus.StepNoAnswer)
                        _EQProcess.PRECHARGER[i].EQUIPSTATUS = enumEquipStatus.StepVacancy;
                }
                else
                {
                    iState = 0;
                    //_EQProcess.AutoInspectionTimer[i].Enabled = false;
                    if (_EQProcess.PRECHARGER[i].AUTOMODE == true) 
                        _EQProcess.PRECHARGER[i].EQUIPSTATUS = enumEquipStatus.StepNoAnswer;
                        //nForm[i].SetDisplayStatus("NoAnswer");
                }
                //nForm[i].SetConnectInfo(iState);

                SetEquiStatus(i, _EQProcess.PRECHARGER[i].EQUIPSTATUS);
            }
            #endregion

            #region PLC 연결 상태
            if (_EQProcess.PLC_STATE == true) btnPLC.BackColor = Color.LimeGreen;
            else btnPLC.BackColor = Color.Red;
            #endregion
        }

        private void SetEquiStatus(int stageno, enumEquipStatus _EquiStatus)
        {
            //switch(_EquiStatus)
            //{
            //    case enumEquipStatus.StepVacancy:
            //        nForm[stageno].SetDisplayStatus("Vacancy");
            //        break;
            //    case enumEquipStatus.StepTrayIn:
            //        nForm[stageno].SetDisplayStatus("TrayIn");
            //        break;
            //    case enumEquipStatus.StepReady:
            //        nForm[stageno].SetDisplayStatus("Ready");
            //        break;
            //    case enumEquipStatus.StepRun:
            //        nForm[stageno].SetDisplayStatus("Run");
            //        break;
            //    case enumEquipStatus.StepEnd:
            //        nForm[stageno].SetDisplayStatus("End");
            //        break;
            //    case enumEquipStatus.StepTrayOut:
            //        nForm[stageno].SetDisplayStatus("TrayOut");
            //        break;
            //    case enumEquipStatus.StepManual:
            //        nForm[stageno].SetDisplayStatus("Manual");
            //        break;
            //    case enumEquipStatus.StepNoAnswer:
            //        nForm[stageno].SetDisplayStatus("NoAnswer");
            //        break;
            //    case enumEquipStatus.StepEmergency:
            //        nForm[stageno].SetDisplayStatus("EmergencyStop");
            //        break;
            //    default:
            //        break;
            //}
            
        }
        #endregion

        #region PreCharger Command

        public void CmdAuto(int stageno)
        {
            _EQProcess.PRECHARGER[stageno].AUTOMODE = true;
            RunPreChargerCmd("AUT", stageno);
        }

        public void CmdManual(int stageno)
        {
            _EQProcess.PRECHARGER[stageno].AUTOMODE = false;
            RunPreChargerCmd("MAN", stageno);
        }

        public void CmdStop(int stageno)
        {
            CmdStop(stageno, null);
        }

        public void CmdStop(int stageno, CPrechargerData CPreData)
        {
            RunPreChargerCmd("AMF", stageno);
            _EQProcess.SetProbeOpen(stageno, 1);
            enumEquipStatus _equipstatus = _EQProcess.PRECHARGER[stageno].EQUIPSTATUS;

            if (_EQProcess.PRECHARGER[stageno].AUTOMODE == true && _equipstatus == enumEquipStatus.StepRun)
                _EQProcess.PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepEnd;
            else if (_EQProcess.PRECHARGER[stageno].AUTOMODE == true && _equipstatus == enumEquipStatus.StepManual)
                _EQProcess.PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;
            else if (_EQProcess.PRECHARGER[stageno].AUTOMODE == false)
                _EQProcess.PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepManual;

            _PREData[stageno].AMS = false;
            _PREData[stageno].AMF = true;
            _PREData[stageno].ENDCHARGING = false;
            _PREData[stageno].CHARGING = false;
            if (CPreData != null)
                CPreData.SetResultData(stageno, _PREData[stageno]);

        }
        public void RunPreChargerCmd(string cmd, int stageno)
        {
            //switch(cmd)
            //{
            //    case "AMS":
            //        _EQProcess.PRECHARGER[stageno].PreChargerDoStart();
            //        break;
            //    case "AMF":
            //        _EQProcess.PRECHARGER[stageno].PreChargerDoFinish();
            //        break;
            //    case "SET":
            //        if(_PREData[stageno].PARAM != null && _PREData[stageno].PARAM.Length > 0)
            //            _EQProcess.PRECHARGER[stageno].PreChargerDoSet(_PREData[stageno].PARAM);
            //        break;
            //    case "SEN":
            //        _EQProcess.PRECHARGER[stageno].PreChargerDoSen();
            //        break;
            //    case "MON":
            //        _EQProcess.PRECHARGER[stageno].PreChargerDoMonitoring();
            //        break;
            //    default:
            //        break;
            //}
        }
        #endregion

        #region PLC Do Work / SET BIT
        public void PLCInit(int stageno)
        {
            _EQProcess.PLCInit(stageno);
            //_EQProcess.SetProbeOpen(stageno, 0);
            //_EQProcess.SetProbeClose(stageno, 0);
            //_EQProcess.SetTrayOut(stageno, 0);
            //_EQProcess.SetPCError(stageno, 0);
            //_EQProcess.SetCharging(stageno, 0);

            
        }

        public void SetPCError(int stageno, int nValue)
        {
            _EQProcess.SetPCError(stageno, nValue);
        }
        

        public void OnTrayIn(int stageno, int iValue)
        {

        }
        public void SetTrayId(int stageno, string trayid)
        {
           // nForm[stageno].OnTrayId(trayid);
        }

        public void SetTrayOut(int stageno)
        {
            SetBitPLC(stageno, "TRAYOUT", 1);
        }
        public void SetBitPLC(int stageno, string sAddresssName)
        {
            SetBitPLC(stageno, sAddresssName, 1);
        }

        public void SetBitPLC(int stageno, string sAddressName, int nValue)
        {
            switch(sAddressName)
            {
                case "PROBECLOSE":
                    _EQProcess.SetProbeClose(stageno);
                    break;
                case "PROBEOPEN":
                    _EQProcess.SetProbeOpen(stageno);
                    break;
                case "TRAYOUT":
                    _EQProcess.SetTrayOut(stageno, nValue);
                    break;
                case "PCERROR":
                    _EQProcess.SetPCError(stageno, nValue);
                    break;
                case "AUTOMANUAL":
                    _EQProcess.SetPCAutoManual(stageno, nValue);
                    break;
                case "CHARGING":
                    _EQProcess.SetCharging(stageno, nValue);
                    break;
                default:
                    break;
            }
        }

        public void WriteWord(int iAddress, int iData)
        {
            _EQProcess.WriteWord(iAddress, iData);
        }
        #endregion

        #region Display TotalForm
        private void _CPrechargerData_OnDisplayTestTime(int stageno, string sStep, string sTesttime)
        {
           // nForm[stageno].SetTestTimeInfo(sStep, sTesttime);
        }
        #endregion

        private void btnPLC_Click(object sender, EventArgs e)
        {
            plcForm.ShowDialog();
        }
        /// <summary>
        /// BIT 쓰기 테스트 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteBit_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            //configForm.Visible = true;
            configForm.ShowDialog();
        }

        
    }
}
