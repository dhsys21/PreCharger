using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PreCharger.Common
{
    class CMelsecDriver : CMelsecFiberDriver
    {
        private CMelsecFiberDriver _Driver = null;

        private int _HeartBitCnt = 0;

        string TrayID = string.Empty;
        string CellID = string.Empty;
        string DMCID = string.Empty;
        ushort SlotNo = 0;

        public delegate void delegateDoorOpen(string strUnit);
        public event delegateDoorOpen OnDoorOpen = null;
        protected void RaiseOnDoorOpen(string strunit)
        {
            if (OnDoorOpen != null)
                OnDoorOpen(strunit);
        }

        private bool _bPLcState = false;
        public bool PLCSTATE
        {
            get { return _bPLcState; }
        }

        private Thread _threadScan = null;
        private bool _bEnableScan = false;
        private int _iScanInterval = 200;

        private ushort[] OldTrigger = null;
        public ushort[] DriverTrigger
        {
            get { return OldTrigger; }
        }

        //ScanTrigger 범위(Start, End)
        private int _StartAddress = 0;
        public int StartAddress
        {
            get { return _StartAddress; }
            set { _StartAddress = value; }
        }

        private int _EndAddress = 0;
        public int EndAddress
        {
            get { return _EndAddress; }
            set { _EndAddress = value; }
        }

        private int _EndAddress2 = 0;
        public int EndAddress2
        {
            get { return _EndAddress2; }
            set { _EndAddress2 = value; }
        }

        #region Show PLC Data To DataGridView
        public delegate void delegateReportShowData(ushort[] pc_iScanData, ushort[] plc_iScanData);
        public event delegateReportShowData OnShowData = null;
        protected void RaiseOnShowData(ushort[] pc_iScanData, ushort[] plc_iScanData)
        {
            if (OnShowData != null)
                OnShowData(pc_iScanData, plc_iScanData);
        }
        #endregion


        public delegate void delegateReportPLCStatus(int nIndex, int nValue, string type);
        public event delegateReportPLCStatus OnPLCStatus = null;
        protected void RaiseOnPLCStatus(int nIndex, int nValue, string type)
        {
            if (OnPLCStatus != null)
                OnPLCStatus(nIndex, nValue, type);
        }
        //Tray IN
        public delegate void delegateReportPLCTrayIN(int nIndex);
        public event delegateReportPLCTrayIN OnPLCTrayIN = null;
        protected void RaiseOnPLCTrayIN(int nIndex)
        {
            if (OnPLCTrayIN != null)
                OnPLCTrayIN(nIndex);
        }

        //tray id
        public delegate void delegateReportPLCTrayID(int nIndex);
        public event delegateReportPLCTrayID OnPLCTrayID = null;
        protected void RaiseOnPLCTrayID(int nIndex)
        {
            if (OnPLCTrayID != null)
                OnPLCTrayID(nIndex);
        }

        // Probe Open
        public delegate void delegateReportPLCProbeOpen(int nIndex);
        public event delegateReportPLCProbeOpen OnPLCProbeOpen = null;
        protected void RaiseOnPLCProbeOpen(int nIndex)
        {
            if (OnPLCProbeOpen != null)
                OnPLCProbeOpen(nIndex);
        }

        //Probe Close
        public delegate void delegateReportPLCProbeClose(int nIndex);
        public event delegateReportPLCProbeClose OnPLCProbeClose = null;
        protected void RaiseOnPLCProbeClose(int nIndex)
        {
            if (OnPLCProbeClose != null)
                OnPLCProbeClose(nIndex);
        }

        //PLC Error
        public delegate void delegateReportPLCError(int nIndex);
        public event delegateReportPLCError OnPLCError = null;
        protected void RaiseOnPLCError(int nIndex)
        {
            if (OnPLCError != null)
                OnPLCError(nIndex);
        }

        //PLC Auto Manual
        public delegate void delegateReportPLCAutoManual(int nIndex);
        public event delegateReportPLCAutoManual OnPLCAutoManual = null;
        protected void RaiseOnPLCAutoManual(int nIndex)
        {
            if (OnPLCAutoManual != null)
                OnPLCAutoManual(nIndex);
        }

        #region Taping #1
        //Taping 검사 완료
        public delegate void delegateReportTaping1EdgeInspection(string DMCID);
        public event delegateReportTaping1EdgeInspection OnTaping1EdgeInspection = null;
        protected void RaiseOnTaping1EdgeInspection(string DMCID)
        {
            if (OnTaping1EdgeInspection != null)
                OnTaping1EdgeInspection(DMCID);
        }

        // DMC 라벨 Matching
        public delegate void delegateReportLine1DMCLabelMatching();
        public event delegateReportLine1DMCLabelMatching OnLine1DMCLabelMatching = null;
        protected void RaiseOnLine1DMCLabelMatching()
        {
            if (OnLine1DMCLabelMatching != null)
                OnLine1DMCLabelMatching();
        }
        #endregion

        public CMelsecDriver(int iCHANNEL_NO, int iMODE, int iSTATION_NO)
        {
            _Driver = new CMelsecFiberDriver();
            _Driver.OnConnectionStateChanged += _Driver_OnConnectionStateChanged;

            _EndAddress = _Constant.PLC_D_LEN * 2;
            OldTrigger = new ushort[_EndAddress];
            

            //Channel_N0, Mode, Station_No
            int iRet = _Driver.Open( iCHANNEL_NO, iMODE, iSTATION_NO);

            if (iRet == 0)
            {
                StartScaning();
            }
        }

        private void _Driver_OnConnectionStateChanged(CMelsecFiberDriver driver, enumConnectionState enumConnection)
        {
            if (enumConnection == enumConnectionState.Connected)
                _bPLcState = true;
            else
                _bPLcState = false;
        }

        private void DoScan()
        {
            while (_bEnableScan)
            {
                //SystemLogger.Log(Level.Verbose, "SCAN");
                ScanTrigger();
                Thread.Sleep(_iScanInterval);
            }
        }

        public int StartScaning()
        {
            _threadScan = new Thread(new ThreadStart(DoScan));
            _threadScan.Priority = ThreadPriority.Highest;
            _threadScan.IsBackground = true;

            _bEnableScan = true;
            _threadScan.Start();

            return 0;
        }

        public int StopScan()
        {
            if (_threadScan != null)
            {
                _bEnableScan = false;
                _threadScan.Join();
                _threadScan = null;
            }
            return 0;
        }

        private void ScanTrigger()
        {
            int pc_count = _Constant.PC_D_LEN * 2;
            ushort[] pc_iScan = new ushort[pc_count];

            int plc_count = _Constant.PLC_D_LEN * 2;
            ushort[] plc_iScan = new ushort[plc_count];

            #region Show PLC Data to DataGridView
            ReadAllData(_Constant.PC_D_START_NUM[0], enumDeviceType.D, pc_count, out pc_iScan);
            ReadAllData(_Constant.PLC_D_START_NUM[0], enumDeviceType.D, plc_count, out plc_iScan);
            OnShowData(pc_iScan, plc_iScan);
            #endregion

            #region 수정
            int nAddress = 0;
            for(int i = 0; i < _EndAddress; i++)
            {
                //if (plc_iScan[i] != OldTrigger[i])
                //{
                    for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                    {
                        nAddress = _Constant.PLC_PRE_TRAY_IN;
                        if (i == nAddress) RaiseOnPLCTrayIN(nIndex);

                        nAddress = _Constant.PLC_PRE_TRAY_ID;
                        if (i == nAddress) RaiseOnPLCTrayID(nIndex);

                        nAddress = _Constant.PLC_PRE_PROB_OPEN;
                        if (i == nAddress) RaiseOnPLCProbeOpen(nIndex);

                        nAddress = _Constant.PLC_PRE_PROB_CLOSE;
                        if (i == nAddress) RaiseOnPLCProbeClose(nIndex);

                        nAddress = _Constant.PLC_PRE_ERROR;
                        if (i == nAddress) RaiseOnPLCError(nIndex);

                        nAddress = _Constant.PLC_PRE_ATUO_MANUAL;
                        if (i == nAddress) RaiseOnPLCAutoManual(nIndex);

                    }
                //}

                OldTrigger[i] = plc_iScan[i];
            }
            
            /*
            for (int i = 0; i < _EndAddress; i++)
            {

                if (iScan[i] == true && Convert.ToBoolean(OldTrigger[i]) == false)
                {
                    switch (i)
                    {

                        /// [ Taping #1 ]
                        /// Cell 투입 완료
                        /// Taping 엣지 검사 완료
                        /// Taping 높이 검사 완료
                        /// Cell 배출 완료

                        /// [ Taping #2 ]
                        /// Cell 투입 완료
                        /// Taping 엣지 검사 완료
                        /// Taping 높이 검사 완료
                        /// Cell 배출 완료

                        ///</summary>
                        case 1:
                            ReadWordA(0x0600, TrayIDNum, out TrayID);
                            RaiseOnTrayOuput(TrayID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0401]");
                            break;
                        case 16:
                            ReadWordA(0x0500, TrayIDNum, out TrayID);
                            RaiseOnOnTrayBCRReadComplete(TrayID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0410]");
                            break;
                        case 64:
                            ReadWordA(0x0520, CellIDNum, out CellID);
                            ReadWord(0x052B, out SlotNo);
                            RaiseOnCellIDReadComplete(1, CellID, SlotNo);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0440]");
                            break;
                        case 65:
                            ReadWordA(0x0540, CellIDNum, out CellID);
                            RaiseOnT3CheckComplete(1, CellID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0441]");
                            break;
                        case 66:
                            WriteBit(0x1522, false);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0442]");
                            break;
                        case 67:
                            ReadWordA(0x05A0, CellIDNum, out CellID);
                            RaiseOnDMCLabelCheckComplete(1, CellID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0443]");
                            break;
                        case 68:
                            ReadWordA(0x05D0, DMCIDNum, out DMCID);
                            RaiseOnCellOuput(1, DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B0444]");
                            break;
                        case 80:
                            RaiseOnTaping1DMCLabelPrintReq();
                            _Logger.Log(Level.Info, "PLC BIT ON [B0450]");
                            break;
                        case 96:
                            RaiseOnLine1DMCLabelMatching();
                            _Logger.Log(Level.Info, "PLC BIT ON [B0460]");
                            break;
                        case 160:
                            ReadWordA(0x0620, CellIDNum, out CellID);
                            ReadWord(0x062B, out SlotNo);
                            RaiseOnCellIDReadComplete(2, CellID, SlotNo);
                            _Logger.Log(Level.Info, "PLC BIT ON [B04A0]");
                            break;
                        case 161:
                            ReadWordA(0x0640, CellIDNum, out CellID);
                            RaiseOnT3CheckComplete(2, CellID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B04A1]");
                            break;
                        case 162:
                            WriteBit(0x1552, false);
                            _Logger.Log(Level.Info, "PLC BIT ON [B04A2]");
                            break;
                        case 163:
                            ReadWordA(0x06A0, CellIDNum, out CellID);
                            RaiseOnDMCLabelCheckComplete(2, CellID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B04A3]");
                            break;
                        case 164:
                            ReadWordA(0x06D0, DMCIDNum, out DMCID);
                            RaiseOnCellOuput(2, DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B04A4]");
                            break;
                        case 176:
                            RaiseOnTaping2DMCLabelPrintReq();
                            _Logger.Log(Level.Info, "PLC BIT ON [B04B0]");
                            break;
                        case 192:
                            RaiseOnLine2DMCLabelMatching();
                            _Logger.Log(Level.Info, "PLC BIT ON [B04C0]");
                            break;
                        case 3392:
                            ReadWordA(0x1400, DMCIDNum, out DMCID);
                            RaiseOnTaping1SetInputComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1140]");
                            break;
                        case 3408:
                            ReadWordA(0x1440, DMCIDNum, out DMCID);
                            RaiseOnTaping1EdgeInspection(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1150]");
                            break;
                        case 3409:
                            ReadWordA(0x1480, DMCIDNum, out DMCID);
                            RaiseOnTaping1CheckComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1151]");
                            break;
                        case 3424:
                            ReadWordA(0x14C0, DMCIDNum, out DMCID);
                            RaiseOnTaping1SetOutputComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1160]");
                            break;
                        case 3648:
                            ReadWordA(0x2400, DMCIDNum, out DMCID);
                            RaiseOnTaping2SetInputComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1240]");
                            break;
                        case 3664:
                            ReadWordA(0x2440, DMCIDNum, out DMCID);
                            RaiseOnTaping2EdgeInspection(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1250]");
                            break;
                        case 3665:
                            ReadWordA(0x2480, DMCIDNum, out DMCID);
                            RaiseOnTaping2CheckComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1251]");
                            break;
                        case 3680:
                            ReadWordA(0x24C0, DMCIDNum, out DMCID);
                            RaiseOnTaping2SetOutputComplete(DMCID);
                            _Logger.Log(Level.Info, "PLC BIT ON [B1260]");
                            break;
                    }
                }
                OldTrigger[i] = Convert.ToInt16(iScan[i]);
            }
            */
            #endregion
        }

        public ushort[] ReadAllData(int iAddress, enumDeviceType DeviceType, int iSize, out ushort[] asValue)
        {
            _Driver.ReadAllWord(iAddress, 1, DeviceType, iSize, out asValue);
            return asValue;
        }

        public void ReadBit(int iAddress, int nIndex, out bool bValue)
        {
            bValue = false;
            _Driver.ReadBit(iAddress, nIndex, 1, enumDeviceType.D, out bValue);

        }

        public void ReadWord(int iAddress, out ushort asValue )
        {
            _Driver.ReadWord(iAddress, 1, enumDeviceType.D, out asValue);
        }

        public void ReadString(int iAddress, int iLength, out string strValue)
        {
            _Driver.ReadString(iAddress, 1, enumDeviceType.D, iLength,out strValue);
        }

        public void WriteBit(int iAddress, int nIndex, bool bValue)
        {
            _Driver.WriteBit(_Constant.PC_D_START_NUM[0] + iAddress, nIndex, 1, enumDeviceType.D, bValue);
        }

        public void WriteWord(int iAddress, int iData)
        {
            _Driver.WriteWord(_Constant.PC_D_START_NUM[0] + iAddress, 1, enumDeviceType.D, iData);
        }

    }
}
