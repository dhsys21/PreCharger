using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreCharger.Common;

namespace PreCharger
{
    class EquipProcess
    {
        public static EquipProcess equipprocess = null;
        Util util;
        CEquipmentData _system;
        PLCForm plcform = null;
        public int[] _deviceClearCount = new int[_Constant.frmCount];
        public Timer[] AutoInspectionTimer = new Timer[_Constant.frmCount];
        public Timer[] _tmrGetDataLog = new Timer[_Constant.frmCount];
        public Timer[] _tmrIDN = new Timer[_Constant.frmCount];

        private int[] CheckSetValueCount = new int[_Constant.frmCount];
        private DateTime[] dtChargingStart = new DateTime[_Constant.frmCount];
        private DateTime[] dtCharging = new DateTime[_Constant.frmCount];
        private TimeSpan[] tsChargingTime = new TimeSpan[_Constant.frmCount];

        public TotalForm[] nForm = new TotalForm[_Constant.frmCount];
        public FormMeasureInfo measureinfo;
        
        int AutoInspectionIndex = 0;
        public enumInspectionType enumInspType;

        #region Heart Bit
        public System.Windows.Forms.Timer SeqTimer = null;
        bool _PlcState = false;
        public bool PLC_STATE
        {
            get { return _PlcState; }
            set { _PlcState = value; }
        }

        bool State = false;
        #endregion

        #region delegate 정의
        public delegate void delegateDoWork();

        public delegate void delegateReport_AddPanel(DoubleBufferedPanel pnl);
        public event delegateReport_AddPanel OnAddPanel = null;
        protected void RaiseOnAddPanel(DoubleBufferedPanel pnl)
        {
            if (OnAddPanel != null)
            {
                OnAddPanel(pnl);
            }
        }

        public delegate void delegateReport_PrechargerDoWork(int nIndex, string sData);
        public event delegateReport_PrechargerDoWork OnPrechargerDoWork = null;
        protected void RaiseOnPrechargerDoWork(int nIndex, string sData)
        {
            if (OnPrechargerDoWork != null)
            {
                OnPrechargerDoWork(nIndex, sData);
            }
        }

        public delegate void delegateReport_StepInit(int nIndex);
        public event delegateReport_StepInit OnStepInit = null;
        protected void RaiseOnStepInit(int nIndex)
        {
            if (OnStepInit != null)
            {
                OnStepInit(nIndex);
            }
        }

        public delegate void delegateReport_StepTrayInfo(int nIndex);
        public event delegateReport_StepTrayInfo OnStepTrayInfo = null;
        protected void RaiseOnStepTrayInfo(int nIndex)
        {
            if (OnStepTrayInfo != null)
            {
                OnStepTrayInfo(nIndex);
            }
        }

        public delegate void delegateReport_StepCharging(int nIndex);
        public event delegateReport_StepCharging OnStepCharging = null;
        protected void RaiseOnStepCharging(int nIndex)
        {
            if (OnStepCharging != null)
            {
                OnStepCharging(nIndex);
            }
        }

        public delegate void delegateReport_StepStop(int nIndex);
        public event delegateReport_StepStop OnStepStop = null;
        protected void RaiseOnStepStop(int nIndex)
        {
            if (OnStepStop != null)
            {
                OnStepStop(nIndex);
            }
        }

        public delegate void delegateReport_StepFinish(int nIndex);
        public event delegateReport_StepFinish OnStepFinish = null;
        protected void RaiseOnStepFinish(int nIndex)
        {
            if (OnStepFinish != null)
            {
                OnStepFinish(nIndex);
            }
        }

        public delegate void delegateReport_PLCSignalStatus(int nIndex, int iAddress, int iValue);
        public event delegateReport_PLCSignalStatus OnPLCSignalStatus = null;
        protected void RaiseOnPLCSIgnalStatus(int nIndex, int iAddress, int iValue)
        {
            if (OnPLCSignalStatus != null)
            {
                OnPLCSignalStatus(nIndex, iAddress, iValue);
            }
        }
        #endregion

        #region PLC, Precharger 연결
        private CMelsecDriver3 _PLCDriver = null;
        
        public CMelsecDriver3 PLCDRIVER
        {
            get { return _PLCDriver; }
        }

        private KeysightBT2202[] _PreCharger = new KeysightBT2202[_Constant.frmCount];
        public KeysightBT2202[] PRECHARGER
        {
            get { return _PreCharger; }
        }

        private CPrechargerData[] _PreChargerData = new CPrechargerData[_Constant.frmCount];
        public CPrechargerData[] PRECHARGERDATA { get => _PreChargerData; set => _PreChargerData = value; }

        private void GetIPAddress(int nIndex, out string ipaddress, out int port)
        {
            switch (nIndex)
            {
                case 0:
                    ipaddress = _system.SIPAddress01;
                    port = _system.IPort01;
                    break;
                case 1:
                    ipaddress = _system.SIPAddress02;
                    port = _system.IPort02;
                    break;
                case 2:
                    ipaddress = _system.SIPAddress03;
                    port = _system.IPort03;
                    break;
                case 3:
                    ipaddress = _system.SIPAddress04;
                    port = _system.IPort04;
                    break;
                default:
                    ipaddress = "192.168.0.1";
                    port = 10000;
                    break;
            }
        }
        #endregion

        public static EquipProcess GetInstance()
        {
            if (equipprocess == null) equipprocess = new EquipProcess();
            return equipprocess;
        }
        public EquipProcess()
        {
            equipprocess = this;
            enumInspType = enumInspectionType.Timer;
            util = new Util();
            _system = CEquipmentData.GetInstance();

            #region TotalForm, MeasureInfoForm
            string HOST;
            int PORT;
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                GetIPAddress(nIndex, out HOST, out PORT);

                //* FormTotal
                nForm[nIndex] = TotalForm.GetInstance(nIndex);
                nForm[nIndex].OnViewMeasureInfo += _TotalForm_OnViewMeasureInfo;

                //* 변수 초기화
                CheckSetValueCount[nIndex] = 0;
                dtChargingStart[nIndex] = new DateTime();
                dtCharging[nIndex] = new DateTime();
                tsChargingTime[nIndex] = new TimeSpan();

                //* PreCharger
                _PreCharger[nIndex] = KeysightBT2202.GetInstance(nIndex);
                _PreCharger[nIndex].Open(HOST, PORT.ToString(), nIndex);
                _PreCharger[nIndex].SetPrechargeParameter(_system.IPRETIME, _system.IPRECURRENT, _system.IPREVOLTAGE);
                _PreCharger[nIndex].SetChargeParameter(_system.ITime, _system.ICurrent, _system.IVoltage);
                _PreCharger[nIndex].SetDischargeParameter(_system.IDischargeTime, _system.IDischargeCurrent, _system.IDischargeVoltage);

                _PreCharger[nIndex].AUTOMODE = true;
                _PreCharger[nIndex].EQUIPSTATUS = enumEquipStatus.StepVacancy;
                _PreCharger[nIndex].OnReceived += _PreCharger_OnReceived;

                //* PreCharger Data
                // Precharger 수정필요
                _PreChargerData[nIndex] = new CPrechargerData();
                _PreChargerData[nIndex].InitData(nIndex);
                //* AutoInspectionTimer
                //* 다른 곳으로 이동 ? 또는 방법을 바꿔야 함.
                //* PLC 를 쓰레드로 처리 했기 때문에 이를 적용해야 함.
                AutoInspectionTimer[nIndex] = new Timer();
                AutoInspectionTimer[nIndex].Interval = 1000;
                AutoInspectionTimer[nIndex].Tag = nIndex;
                AutoInspectionTimer[nIndex].Tick += new EventHandler(AutoInspectionTimer_Tick);
                //* Get Data Log Timer
                _tmrGetDataLog[nIndex] = new Timer();
                _tmrGetDataLog[nIndex].Interval = 1000;
                _tmrGetDataLog[nIndex].Tag = nIndex;
                _tmrGetDataLog[nIndex].Tick += new EventHandler(_tmrGetDataLog_Tick);
                //* *IDN? - Keysight 장비 연결 확인용
                _tmrIDN[nIndex] = new Timer();
                _tmrIDN[nIndex].Interval = 3000;
                _tmrIDN[nIndex].Tag = nIndex;
                _tmrIDN[nIndex].Tick += new EventHandler(_tmrIDN_Tick);

                _deviceClearCount[nIndex] = 0;
            }

            measureinfo = FormMeasureInfo.GetInstance();
            measureinfo.OnStartCharging += _MeasureInfoForm_OnStartCharging;
            measureinfo.OnStartDischarging += _MeasureInfoForm_OnStartDischarging;
            measureinfo.OnStartMeasuring += _MeasureInfoForm_OnStartMeasuring;

            //measureinfo = new FormMeasureInfo();

            #endregion

            #region PLC 
            int mode_no = -1;
            try
            {
                _PLCDriver = new CMelsecDriver3(_system.PLCIPADDRESS, _system.PLCPORT);

                _PLCDriver.OnShowData += _PLCDriver_OnShowData;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            SeqTimer = new System.Windows.Forms.Timer();
            SeqTimer.Interval = 500;
            SeqTimer.Tick += new EventHandler(SeqTimer_Tick);
            //SeqTimer.Start();
            #endregion
            
            // makepanel();

            for(int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                if(_PreCharger[nIndex].AUTOMODE == true)
                    SetPCAutoManual(nIndex, 1);
            }
        }

        public void makepanel()
        {
            DoubleBufferedPanel[] nPanel = new DoubleBufferedPanel[_Constant.frmCount];
            int nx = 10, ny = 5;
            for (int i = 0; i < _Constant.frmCount; i++)
            {
                //nForm[i] = new TotalForm();
                nForm[i].Width = 470;
                nForm[i].Height = 910;
                nForm[i].stage = i;
                nForm[i].SetConnectInfo(0);

                nPanel[i] = new DoubleBufferedPanel();
                nPanel[i].Width = 475;
                nPanel[i].Height = 930;
                nPanel[i].Left = nx + i * (nPanel[i].Width + 1);
                nPanel[i].Top = ny;
                //BaseForm.BasePanel.Controls.Add(nPanel[i]);
                
                util.loadFormIntoPanel(nForm[i], nPanel[i]);
                RaiseOnAddPanel(nPanel[i]);
            }
        }

        #region AutoInspectionTimer
        private void AutoInspectionTimer_Tick(object sender, EventArgs e)
        {
            int AutoInspectionIndex = int.Parse(((Timer)sender).Tag.ToString());
            StartAutoInspection(AutoInspectionIndex);
        }

        private void StartAutoInspection(int stageno)
        {
            ErrorCheck();
            //if (_PreCharger[stageno].AUTOMODE == true 
            //    && _PreCharger[stageno].ConnectionState == true)
            //{
            //    switch (PRECHARGER[stageno].EQUIPSTATUS)
            //    {
            //        case enumEquipStatus.StepVacancy:
            //            AutoInspection_TrayIn(stageno); // check tray in, read tray id, steptrayin
            //            break;
            //        case enumEquipStatus.StepTrayIn:
            //            AutoInspection_LoadTrayInfo(stageno); // read trayinfo, stepready
            //            break;
            //        case enumEquipStatus.StepReady:
            //            AutoInspection_Charging(stageno);  // start charging, steprun
            //            break;
            //        case enumEquipStatus.StepRun:
            //            AutoInspection_Stop(stageno); // probe open, stepend
            //            break;
            //        case enumEquipStatus.StepEnd:
            //            AutoInspection_TrayOut(stageno); // tray out, stepfinish, write result, steptrayout
            //            break;
            //        case enumEquipStatus.StepTrayOut:
            //            AutoInspection_Wait(stageno); // plc init, to go stepvacancy
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void ErrorCheck()
        {

        }
        private void AutoInspection_Wait(int stageno)
        {
            ushort trayin = 0;
            //_PLCDriver.ReadWord(_Constant.PLC_D_START_NUM + _Constant.PLC_PRE_TRAY_IN[stageno], out trayin);

            if (trayin == 0)
            {
                PLCInit(stageno);
                PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;
            }
        }

        private void AutoInspection_TrayIn(int stageno, int[] PLCSCANDATA)
        {
            int trayin = 0;
            trayin = PLCSCANDATA[_Constant.PLC_D_START_NUM[stageno] + _Constant.PLC_PRE_TRAY_IN];
            if (trayin == 1)
            {
                //* Display Label State
                //* WriteLog();
                //RaiseOnStepInit(stageno);
                Initialization(stageno);
                System.Threading.Thread.Sleep(1000);
                PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayIn;
            }
            else
            {
                //* Display Label State
                PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;
            }
        }

        private void AutoInspection_LoadTrayInfo(int stageno)
        {
            /// Tray Id reading => load try info => probe close
            /// oldTrayIn != newTrayIn (tray in 신호 들어올 때 한번만 실행해야함)
            /// 초기화 시 oldTrayIn 신호 0으로 

            string trayid = string.Empty;

            if (ReadTrayId(stageno, out trayid) == false)
            {
                // BCR ERROR
                MessageBox.Show("TRAY ID ERROR", "Tray ID Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // BCR OK
                ReadCellInfo(stageno);
                if (LoadTrayInfo(stageno, trayid) == false)
                {
                    MessageBox.Show(trayid + ".tray File is no exist", "Tray Infomation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _PreCharger[stageno].EQUIPSTATUS = enumEquipStatus.StepReady;
                    SetProbeClose(stageno, 1);
                }
            }
        }

        private void AutoInspection_Charging(int stageno)
        {
            ushort probeclose = 0;
            //_PLCDriver.ReadWord(_Constant.PLC_D_START_NUM + _Constant.PLC_PRE_PROB_CLOSE[stageno], out probeclose);

            if (probeclose == 1)
                RaiseOnStepCharging(stageno);
        }

        private void AutoInspection_Stop(int stageno)
        {
            RaiseOnStepStop(stageno);
        }

        private void AutoInspection_TrayOut(int stageno)
        {
            ushort probeopen = 0;
            _PLCDriver.ReadWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PLC_PRE_PROB_OPEN, out probeopen);

            if (probeopen == 1)
                RaiseOnStepFinish(stageno);
            //BadInfomation();
            //ReadCellInfo();
            //WriteResultFile();
        }
        #endregion

        #region PreCharger Sub Working
        public void Initialization(int stageno)
        {
            util.SavePLCLog(stageno, "Init");
            PRECHARGERDATA[stageno].AMS = false;
            PRECHARGERDATA[stageno].AMF = false;
            PRECHARGERDATA[stageno].ENDCHARGING = false;
            PRECHARGERDATA[stageno].CHARGING = false;

            if (PRECHARGER[stageno].AUTOMODE == true)
                PRECHARGER[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;

            PLCInit(stageno);

            // trayid 초기화
            nForm[stageno].SetTrayId("");

            // display init
            InitDisplayInfo(stageno);
            //nForm[stageno].initGridView();
            //MeasureInfo[stageno].initGridView();
        }
        public void InitDisplayInfo(int stageno)
        {
            nForm[stageno].initGridView();
            measureinfo.initGridView(true);
        }
        #endregion

        #region PreCharger Command
        public async void StartCharging(object stageno)
        {
            int nStage = Convert.ToInt16(stageno.ToString());
            try
            {
                await PRECHARGER[nStage].SetStepChargeDef();
                //* Check Setting value and Step Definition
                if (await PRECHARGER[nStage].CheckStepChargeDef() == true)
                {
                    dtChargingStart[nStage] = DateTime.Now;
                    //* if equal, Start Charging
                    if (await PRECHARGER[nStage].StartCharging() == true)
                    {
                        //_tmrGetDataLog[stageno].Enabled = true;
                        PRECHARGER[nStage].ClearDataLog();
                        isRead = true;
                        //measureinfo = new FormMeasureInfo();
                        GetDateLogWhile(nStage);
                    }
                }
                else
                {
                    //* if not equal, Set Step Definition
                    await PRECHARGER[nStage].SetStepChargeDef().ConfigureAwait(false);
                    StartCharging(stageno);
                }
            }
            catch (Exception ex)
            {
                util.SaveLog(nStage, "StartPrecharging error => " + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        public async void StartDischarging(object stageno)
        {
            int nStage = Convert.ToInt16(stageno.ToString());
            try
            {
                await PRECHARGER[nStage].SetStepDischargeDef();
                //* Check Setting value and Step Definition
                if (await PRECHARGER[nStage].CheckStepDischargeDef() == true)
                {
                    dtChargingStart[nStage] = DateTime.Now;
                    //* if equal, Start Charging
                    if (await PRECHARGER[nStage].StartDischarging() == true)
                    {
                        //_tmrGetDataLog[stageno].Enabled = true;
                        PRECHARGER[nStage].ClearDataLog();
                        isRead = true;
                        //measureinfo = new FormMeasureInfo();
                        GetDateLogWhile(nStage);
                    }
                }
                else
                {
                    //* if not equal, Set Step Definition
                    await PRECHARGER[nStage].SetStepDischargeDef();
                    StartDischarging(stageno);
                }
            }
            catch (Exception ex)
            {
                util.SaveLog(nStage, "StartPrecharging error => " + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        public async void StartMeasuring(object stageno)
        {
            int nStage = Convert.ToInt16(stageno.ToString());
            try
            {
                isRead = true;
                //measureinfo = new FormMeasureInfo();
                GetMeasureInfoWhile(nStage);
            }
            catch (Exception ex)
            {
                util.SaveLog(nStage, "StartPrecharging error => " + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        bool isRead = false;
        private async void GetDateLogWhile(int stageno)
        {
            while (isRead)
            {
                dtCharging[stageno] = DateTime.Now;
                tsChargingTime[stageno] = (dtCharging[stageno] - dtChargingStart[stageno]).Duration();

                //*시간 표시
                measureinfo.SetChargingTime((int)tsChargingTime[stageno].TotalSeconds);
                nForm[stageno].SetChargingTime((int)tsChargingTime[stageno].TotalSeconds);

                //* stat:cell:rep? (@1001:1032)
                if (PRECHARGER[stageno].GetCellReports(8) == false)
                    StopCharging(stageno);

                //* data:log?
                double logCount = PRECHARGER[stageno].GetLogCount();
                if (logCount > 0)
                {
                    GgDataLogNamespace.GgBinData oDataLogQuery = PRECHARGER[stageno].GetDataLog();
                    PRECHARGERDATA[stageno].SetDataLog(oDataLogQuery);

                    measureinfo.DisplayChannelInfo(stageno, PRECHARGERDATA[stageno]);
                }

                await Task.Delay(100);
            }
        }
        private async void GetMeasureInfoWhile(int stageno)
        {
            while (isRead)
            {
                dtCharging[stageno] = DateTime.Now;
                tsChargingTime[stageno] = (dtCharging[stageno] - dtChargingStart[stageno]).Duration();

                double[] voltages = PRECHARGER[stageno].GetVoltage();
                PRECHARGERDATA[stageno].SetVoltages(voltages);
                await Task.Delay(100);

                double[] currents = PRECHARGER[stageno].GetCurrent();
                PRECHARGERDATA[stageno].SetCurrents(currents);
                await Task.Delay(100);

                measureinfo.DisplayChannelInfo(stageno, PRECHARGERDATA[stageno]);
            }
        }
        public void StopCharging(int stageno)
        {
            PRECHARGER[stageno].StopCharging();
            isRead = false;
            //_tmrGetDataLog[stageno].Enabled = false;
        }

        private void _tmrGetDataLog_Tick(object sender, EventArgs e)
        {
            int stageno = int.Parse(((Timer)sender).Tag.ToString());

            //* stat:cell:rep? (@1001:1032)
            
            //* data:log?
            double logCount = PRECHARGER[stageno].GetLogCount();
            if (logCount > 0)
                PRECHARGER[stageno].GetDataLog();
        }
        private async void _tmrIDN_Tick(object sender, EventArgs e)
        {
            int stageno = int.Parse(((Timer)sender).Tag.ToString());
            PRECHARGER[stageno].CONNECTIONSTATE = PRECHARGER[stageno].ConnectionCheck();
            if(PRECHARGER[stageno].CONNECTIONSTATE == false)
            {
                _deviceClearCount[stageno]++;
                if(_deviceClearCount[stageno] > 5)
                {
                    PRECHARGER[stageno].DeviceClear();
                    _deviceClearCount[stageno] = 0;
                    await Task.Delay(500);
                }
            }
        }
        #endregion

        #region Delegate Method

        int iState = 0;     // 1 = Normal, 2 = Error
        int oState = 0;
        private void _PreCharger_OnReceived(string strMessage, int stageno)
        {
            if (PRECHARGER[stageno].CONNECTIONSTATE == true)
            {
                iState = 1;
            }
            else
            {
                iState = 0;
            }

            util.SaveLog(stageno, strMessage);

            RaiseOnPrechargerDoWork(stageno, strMessage);
            //if (iState != oState)
            //    BaseForm.frmMain.nForm[0].OnConnectInfo(iState);
            oState = iState;
        }

        private void _TotalForm_OnViewMeasureInfo(int stageno, bool bManualMode)
        {
            measureinfo.SetStage(stageno);
            measureinfo.Visible = true; ;
            measureinfo.BringToFront();
            measureinfo.SetManualMode(bManualMode);

            InitDisplayInfo(stageno);
        }

        private void _MeasureInfoForm_OnStartCharging(int stageno)
        {
            SetTrayInfo(stageno);
            InitDisplayInfo(stageno);

            //* 일반호출
            //StartCharging(stageno);
            //* Task 사용
            Task.Factory.StartNew(new Action<object>(StartCharging), (object)stageno);
        }
        private void _MeasureInfoForm_OnStartDischarging(int stageno)
        {
            SetTrayInfo(stageno);
            InitDisplayInfo(stageno);

            //* 일반호출
            //StartCharging(stageno);
            //* Task 사용
            Task.Factory.StartNew(new Action<object>(StartDischarging), (object)stageno);
        }
        private void _MeasureInfoForm_OnStartMeasuring(int stageno)
        {
            Task.Factory.StartNew(new Action<object>(StartMeasuring), (object)stageno);
        }
        #endregion

        #region SET BIT (PROBE CLOSE, PROBE OPEN, TRAY OUT, PC ERROR, PC AUTO/MAN, CHARGING

        public void PLCInit(int stageno)
        {
            SetProbeOpen(stageno, 0);
            SetProbeClose(stageno, 0);
            SetTrayOut(stageno, 0);
            SetPCError(stageno, 0);
            SetCharging(stageno, 0);

            util.SavePLCLog(stageno, "PLC INIT (probe open, probe close, tray out, pc error, charging");
        }
        public void SetProbeClose(int stageno)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_OPEN, 0);
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_CLOSE, 1);

            util.SavePLCLog(stageno, 
                "ProbeClose(" + _Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_OPEN + " => 0, " + _Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_CLOSE + " => 1");
        }

        public void SetProbeClose(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_CLOSE, nValue);

            util.SavePLCLog(stageno,
                "ProbeClose(" + _Constant.PC_PRE_PROB_CLOSE + " => " + nValue.ToString());
        }

        public void SetProbeOpen(int stageno)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_CLOSE, 0);
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_OPEN, 1);

            util.SavePLCLog(stageno,
                "ProbeOpen(" + _Constant.PC_PRE_PROB_CLOSE + " => 0, " + _Constant.PC_PRE_PROB_OPEN + " => 1");
        }
        public void SetProbeOpen(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_PROB_OPEN, nValue);

            util.SavePLCLog(stageno,
                "ProbeOpen(" + _Constant.PC_PRE_PROB_OPEN + " => " + nValue.ToString());
        }
        public void SetTrayOut(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_TRAY_OUT, nValue);

            util.SavePLCLog(stageno,
                "TrayOut(" + _Constant.PC_PRE_TRAY_OUT + " => " + nValue.ToString());
        }

        public void SetPCError(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_ERROR, nValue);

            util.SavePLCLog(stageno,
                "PCError(" + _Constant.PC_PRE_ERROR + " => " + nValue.ToString());
        }

        public void SetPCAutoManual(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_STAGE_AUTO_READY, nValue);

            util.SavePLCLog(stageno,
                "AutoManual(" + _Constant.PC_PRE_STAGE_AUTO_READY + " => " + nValue.ToString());
        }
        public void SetCharging(int stageno, int nValue)
        {
            WriteWord(_Constant.PLC_D_START_NUM[stageno] + _Constant.PC_PRE_CHARGING, nValue);

            util.SavePLCLog(stageno,
                "Charging(" + _Constant.PC_PRE_CHARGING + " => " + nValue.ToString());
        }
        #endregion

        // Heart Beat
        #region Heart Beat
        void SeqTimer_Tick(object sender, EventArgs e)
        {
            //* PreCharger 1
            _PLCDriver.ReadBit(_Constant.PC_D_START_NUM[0] + _Constant.PC_HEART_BEAT, 0, out State);

            if (State)
            {
                State = false;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[0] + _Constant.PC_HEART_BEAT, 0, false);
            }
            else
            {
                State = true;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[0] + _Constant.PC_HEART_BEAT, 0, true);
            }

            //* PreCharger 2
            _PLCDriver.ReadBit(_Constant.PC_D_START_NUM[1] + _Constant.PC_HEART_BEAT, 0, out State);

            if (State)
            {
                State = false;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[1] + _Constant.PC_HEART_BEAT, 0, false);
            }
            else
            {
                State = true;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[1] + _Constant.PC_HEART_BEAT, 0, true);
            }

            //* PreCharger 3
            _PLCDriver.ReadBit(_Constant.PC_D_START_NUM[2] + _Constant.PC_HEART_BEAT, 0, out State);

            if (State)
            {
                State = false;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[2] + _Constant.PC_HEART_BEAT, 0, false);
            }
            else
            {
                State = true;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[2] + _Constant.PC_HEART_BEAT, 0, true);
            }

            //* PreCharger 4
            _PLCDriver.ReadBit(_Constant.PC_D_START_NUM[3] + _Constant.PC_HEART_BEAT, 0, out State);

            if (State)
            {
                State = false;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[3] + _Constant.PC_HEART_BEAT, 0, false);
            }
            else
            {
                State = true;
                _PLCDriver.WriteBit(_Constant.PC_D_START_NUM[3] + _Constant.PC_HEART_BEAT, 0, true);
            }
        }
        #endregion

        #region PLCDriver Show Data in DataGridView
        //* nIndex => plc - stage number
        private void _PLCDriver_OnShowData(int[] pc_iScanData, int[] plc_iScanData, int nIndex)
        {
            try
            {
                plcform = PLCForm.GetInstance();
                if(plcform != null)
                    plcform.SetDataToGrid(pc_iScanData, plc_iScanData, nIndex);

                if (_PreCharger[nIndex].AUTOMODE == true && _PreCharger[nIndex].CONNECTIONSTATE == true)
                {
                    switch (PRECHARGER[nIndex].EQUIPSTATUS)
                    {
                        case enumEquipStatus.StepVacancy:
                            AutoInspection_TrayIn(nIndex, plc_iScanData); // check tray in, read tray id, steptrayin
                            break;
                        case enumEquipStatus.StepTrayIn:
                            AutoInspection_LoadTrayInfo(nIndex); // read trayinfo, stepready
                            break;
                        case enumEquipStatus.StepReady:
                            AutoInspection_Charging(nIndex);  // start charging, steprun
                            break;
                        case enumEquipStatus.StepRun:
                            AutoInspection_Stop(nIndex); // probe open, stepend
                            break;
                        case enumEquipStatus.StepEnd:
                            AutoInspection_TrayOut(nIndex); // tray out, stepfinish, write result, steptrayout
                            break;
                        case enumEquipStatus.StepTrayOut:
                            AutoInspection_Wait(nIndex); // plc init, to go stepvacancy
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //_Logger.Log(Level.Exception, "Error! OnTrayOuput Fail!!: " + ex.ToString());
            }
            finally
            {
                //_PLCDriver.WriteBit(0x1501, true);
                //LogEvent("PLC Driver OnTrayOuput [B1501 ON]");
                //if (TriggerTimeOut(1, 100, 5))
                //{
                //    LogEvent("PLC Driver OnTrayOuput Time Out [B0401 ON]", true);
                //}
                //_PLCDriver.WriteBit(0x1501, false);
                //LogEvent("PLC Driver OnTrayOuput [B1501 OFF]");
            }
        }
        #endregion

        #region TRAY Info
        public void SetTrayInfo(int stageno)
        {
            bool[] bCell = new bool[_Constant.ChannelCount];
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                bCell[nIndex] = true;
            PRECHARGERDATA[stageno].SetTrayInfo(bCell);

            //for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            //    PRECHARGERDATA[stageno].SetTrayInfo(nIndex, true);
        }

        public void SetTrayInfo(int stageno, string filename)
        {
            try
            {
                _PreChargerData[stageno].CELLCOUNT = 0;
                IniFile ini = new IniFile();
                ini.Load(filename);
                string data = string.Empty;
                for (int i = 0; i < 256; i++)
                {
                    data = ini[i.ToString()]["CELL_SERIAL"].ToString();
                    if (data == string.Empty)
                    {
                        _PreChargerData[stageno].CELL[i] = false;
                        _PreChargerData[stageno].CELLSERIAL[i] = "-";
                        _PreChargerData[stageno].CHANNELCOLOR[i] = _Constant.ColorNoCell;
                        //MeasureInfo[stageno].SetValueToGridView(i, "No", "Cell");
                    }
                    else
                    {
                        _PreChargerData[stageno].CELL[i] = true;
                        _PreChargerData[stageno].CELLSERIAL[i] = data;
                        _PreChargerData[stageno].CHANNELCOLOR[i] = _Constant.ColorReady;
                        _PreChargerData[stageno].CELLCOUNT++;
                    }
                }
                _PreChargerData[stageno].ARRIVETIME = DateTime.Now;
                //nForm[stageno].DisplayChannelInfo(_PREData[stageno], _EQProcess.PRECHARGER[stageno].EQUIPSTATUS);
                //MeasureInfo[stageno].DisplayChannelInfo(_PREData[stageno]);
            }
            catch (Exception ex)
            {

            }
        }

        public bool LoadTrayInfo(int nIndex, string trayid)
        {
            string fileName = _Constant.TRAY_PATH + trayid + ".Tray";

            if (File.Exists(fileName))
            {
                SetTrayInfo(nIndex, fileName);

                return true;
            }
            else return false;
        }
        public bool ReadTrayId(int nIndex, out string trayid)
        {
            ReadTrayIdFromPLC(nIndex, out trayid);
            //* 2023 04 06 아래 코드는 대체하여 구현해야 함
            //RaiseOnOnLabelTrayId(nIndex, trayid);

            if (trayid == string.Empty || trayid.Length == 0)
                return false;

            return true;
        }



        public void ReadCellInfo(int stageno)
        {
            string file = string.Empty;
            file = _Constant.BIN_PATH + "SystemInfo_" + stageno.ToString() + ".inf";
            IniFile ini = new IniFile();
            ini.Load(file);
            _PreChargerData[stageno].CELLMODEL = ini["CELL_INFO"]["CELL_MODEL"].ToString();
            _PreChargerData[stageno].LOTNUMBER = ini["CELL_INFO"]["LOT_NUMBER"].ToString();
        }
        #endregion

        #region PLC Command
        public void ReadTrayIdFromPLC(int nIndex, out string trayid)
        {
            _PLCDriver.ReadString(_Constant.PLC_D_START_NUM[nIndex] + _Constant.PLC_PRE_TRAY_ID, _Constant.PLC_TRAY_ID_SIZE * 2, out trayid);
        }
        public void WriteBit(int nAddress, int nIndex, bool bValue)
        {
            _PLCDriver.WriteBit(nAddress, nIndex, bValue);
        }
        public void WriteWord(int nAddress, int iData)
        {
            _PLCDriver.WriteWord(nAddress, iData);
        }

        public void ReadWord(int nAddress, out ushort nValue)
        {
            _PLCDriver.ReadWord(nAddress, out nValue);
        }
        #endregion

        public void Close()
        {
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                if(_PreCharger[nIndex] != null) _PreCharger[nIndex].Disconnect();
        }
    }
}
