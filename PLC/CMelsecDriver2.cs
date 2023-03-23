using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PreCharger.Common
{
    class CMelsecDriver2 : CMelsecEtherDriver
    {
        private CMelsecEtherDriver _Driver = null;

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

        private Thread[] _threadScan = new Thread[_Constant.frmCount];
        private bool[] _bEnableScan = { false, false, false, false };
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

        private int _pcEndAddress = 0;
        public int PCEndAddress
        {
            get { return _pcEndAddress; }
            set { _pcEndAddress = value; }
        }

        private int _plcEndAddress = 0;
        public int PLCEndAddress
        {
            get { return _plcEndAddress; }
            set { _plcEndAddress = value; }
        }

        private int _EndAddress2 = 0;
        public int EndAddress2
        {
            get { return _EndAddress2; }
            set { _EndAddress2 = value; }
        }

        #region Show PLC Data To DataGridView
        //* nIndex => plc stage number
        public delegate void delegateReportShowData(ushort[] pc_iScanData, ushort[] plc_iScanData, int nIndex);
        public event delegateReportShowData OnShowData = null;
        protected void RaiseOnShowData(ushort[] pc_iScanData, ushort[] plc_iScanData, int nIndex)
        {
            if (OnShowData != null)
                OnShowData(pc_iScanData, plc_iScanData, nIndex);
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


        public CMelsecDriver2(string sIPADDRESS, int iPORT)
        {
            _Driver = new CMelsecEtherDriver();
            _Driver.OnConnectionStateChanged += _Driver_OnConnectionStateChanged;

            _plcEndAddress = _Constant.PLC_D_LEN * 2;
            OldTrigger = new ushort[_plcEndAddress];

            _pcEndAddress = _Constant.PC_D_LEN * 2;
            OldTrigger = new ushort[_pcEndAddress];

            //Channel_N0, Mode, Station_No
            _Driver.Connect(sIPADDRESS, iPORT);

            if (_bPLcState == true)
            {
                StartScaning();
            }
        }

        private void _Driver_OnConnectionStateChanged(CMelsecEtherDriver driver, enumConnectionState enumConnection)
        {
            if (enumConnection == enumConnectionState.Connected)
                _bPLcState = true;
            else
                _bPLcState = false;
        }

        private void DoScan(object stagenumber)
        {
            int nIndex = Convert.ToInt32(stagenumber);
            while (_bEnableScan[nIndex])
            {
                //SystemLogger.Log(Level.Verbose, "SCAN");
                ScanTrigger();
                Thread.Sleep(_iScanInterval);
            }
        }

        public int StartScaning()
        {
            for(int i = 0; i < _Constant.frmCount; i++)
            {
                //_threadScan[i] = new Thread(new ThreadStart(DoScan));
                _threadScan[i] = new Thread(new ParameterizedThreadStart(DoScan));
                _threadScan[i].Priority = ThreadPriority.Highest;
                _threadScan[i].IsBackground = true;

                _bEnableScan[i] = true;
                _threadScan[i].Start(i);
            }
            

            return 0;
        }

        public int StopScan(int nIndex)
        {
            if (_threadScan[nIndex] != null)
            {
                _bEnableScan[nIndex] = false;
                _threadScan[nIndex].Join();
                _threadScan[nIndex] = null;
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
            for(int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            //for(int nIndex = 0; nIndex < 3; nIndex++)
            {
                ReadWord(_Constant.PC_D_START_NUM[nIndex], pc_count, out pc_iScan);
                ReadWord(_Constant.PLC_D_START_NUM[nIndex], plc_count, out plc_iScan);

                OnShowData(pc_iScan, plc_iScan, nIndex);

                //if (pc_iScan[_Constant.PC_HEART_BEAT] == 0)
                //    WriteWord(_Constant.PC_D_START_NUM[nIndex], 1);
                //else
                //    WriteWord(_Constant.PC_D_START_NUM[nIndex], 0);

                int nAddress = 0;
                for (int i = 0; i < _plcEndAddress; i++)
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

                    OldTrigger[i] = plc_iScan[i];
                }
            }
            #endregion


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
        }

        public void ReadWord(int iAddress, int iSize, out ushort[] asValue)
        {
            string sAddr;
            string sOutMsg = "";

            sAddr = "D*" + iAddress.ToString();
            _Driver.GetPlcValues_Bin(sAddr, iSize, out asValue, out sOutMsg);
        }

        public void ReadWord(int iAddress, out ushort asValue)
        {
            string sAddr;
            string sOutMsg = "";
            int iSize = 1;
            ushort[] asValues = new ushort[1];
            asValue = 0;
            asValues[0] = asValue;

            sAddr = "D*" + iAddress.ToString();
            _Driver.GetPlcValues_Bin(sAddr, iSize, out asValues, out sOutMsg);
            asValue = asValues[0];
        }

        public void ReadBit(int iAddress, int nIndex, out bool bValue)
        {
            bValue = false;
            string sAddr;
            string sOutMsg = "";
            int iSize = 1;
            ushort[] asValue = new ushort[1];

            sAddr = "D*" + iAddress.ToString();
            _Driver.GetPlcValues_Bin(sAddr, iSize, out asValue, out sOutMsg);
            if (asValue[nIndex] == 1) bValue = true;
            else bValue = false;
        }

        public void ReadString(int iAddress, int iSize, out string strValue)
        {
            StringBuilder sb = new StringBuilder();
            string sAddr;
            string sOutMsg = "";
            strValue = "";

            int iMemoryLength = (iSize + 1) / 2;
            ushort[] asValue = new ushort[iMemoryLength];
            iMemoryLength *= 2;


            sAddr = "D*" + iAddress.ToString();
            _Driver.GetPlcValues_Bin(sAddr, iMemoryLength, out asValue, out sOutMsg);

            for (int i = 0; i < iMemoryLength / 2; i++)
            {
                sb.Append(Convert.ToChar(asValue[i] & 0xFF));
                sb.Append(Convert.ToChar((asValue[i] >> 8) & 0xFF));
            }

            strValue = sb.ToString();
            strValue = strValue.Substring(0, iSize);
            strValue = strValue.Trim(" \0".ToCharArray());

        }

        public bool WriteWord(int iAddress, int iData)
        {
            try
            {
                string sMsg = "";
                string sAddr;
                string sWrites = iData.ToString();
                string[] sWrite = new string[1];

                sWrite[0] = sWrites;
                ushort[] abb = new ushort[sWrite.Length];

                for (int i = 0; i < sWrite.Length; i++)
                {
                    abb[i] = ushort.Parse(sWrite[i]);
                }

                sAddr = "D*" + iAddress.ToString();
                _Driver.SetPlcValues_Binary(sAddr, sWrite.Length, abb, out sMsg);

                return true;
            }
            catch (Exception ex)
            {
                // tsbMessage.Text = "PLC Communication Error : " + ex.Message;
                return false;
            }
        }

        public bool WriteBit(int iAddress, int nIndex, bool bValue)
        {
            try
            {
                string sMsg = "";
                string sAddr;
                bool[] abb = new bool[1];
                abb[0] = bValue;

                sAddr = "D*" + iAddress.ToString();
                //_Driver.SetPlcValues_Binary(sAddr, sWrite.Length, abb, out sMsg);
                _Driver.SetPlcValues_Binary(sAddr, nIndex, abb, out string errorMsg);

                return true;
            }
            catch (Exception ex)
            {
                // tsbMessage.Text = "PLC Communication Error : " + ex.Message;
                return false;
            }
        }


    }
}
