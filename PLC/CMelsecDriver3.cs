using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ACTETHERLib;
using ActUtlTypeLib;
using ActProgTypeLib;

namespace PreCharger.Common
{
    class CMelsecDriver3
    {
        // ActQNUDECPUTCP actPLC1;// = new ActQNUDECPUTCP();
        //ActQJ71E71TCPClass actPLC1;
        ActUtlType plc;
        ActProgType plc2;

        // PLC Station No 지정
        public string PLCStatNo = "1";
        // PLC Network No 지정
        public string PLCNetNo = "2";
        // PC Network No 지정(PLC Network No와 같아야됨)
        public string PCNetNo = "2";
        // PC Station No 지정(PLC와 다르게 지정)
        public string PCStatNo = "30";
        //PLC접속 Check 시간
        public string PLCTimeout = "100";
        // CPU Type
        public string CPUType = "112";

        private int _HeartBitCnt = 0;

        private string plcIPAddress;

        public delegate void delegateConnectionState(CMelsecDriver3 driver, enumConnectionState enumConnection);
        //public delegate void delegateDriverErrorOccurred(CMelsecDriver3 driver, int iErrorCode);
        public event delegateConnectionState OnConnectionStateChanged = null;
        protected void RaiseOnConnection(enumConnectionState enumConnection)
        {
            if (OnConnectionStateChanged != null)
            {
                OnConnectionStateChanged(this, enumConnection);
            }
        }

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

        private Thread[] _threadPLCScan = new Thread[_Constant.frmCount];
        private Thread _plcThread1, _plcThread2, _plcThread3, _plcThread4;
        private bool[] _bEnableScan = { false, false, false, false };
        private int _iScanInterval = 500;

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

        public string PLCIPADDRESS { get => plcIPAddress; set => plcIPAddress = value; }

        #region Show PLC Data To DataGridView
        //* nIndex => plc stage number
        public delegate void delegateReportShowData(int[] pc_iScanData, int[] plc_iScanData, int nIndex);
        public event delegateReportShowData OnShowData = null;
        protected void RaiseOnShowData(int[] pc_iScanData, int[] plc_iScanData, int nIndex)
        {
            if (OnShowData != null)
                OnShowData(pc_iScanData, plc_iScanData, nIndex);
        }
        #endregion

        public CMelsecDriver3(string sIPADDRESS, int iPORT)
        {
            try
            {
                plc = new ActUtlType();
                plc.ActLogicalStationNumber = 1;
                //plc.ActHostAddress = "192.168.1.21";

                PLCIPADDRESS = sIPADDRESS;
                plc2 = new ActProgType();
                Open(PLCIPADDRESS);

                int rtn1 = plc.Open();
                if (rtn1 == 0) _bPLcState = true;
                else _bPLcState = false;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            
            _plcEndAddress = _Constant.PLC_D_LEN * 2;
            _pcEndAddress = _Constant.PC_D_LEN * 2;

            //Channel_N0, Mode, Station_No
            // _Driver.Connect(sIPADDRESS, iPORT);

            if (_bPLcState == true)
            {
                StartScaning();
            }
        }
        public int Open(string sIPADDRESS)
        {
            plc2.ActNetworkNumber = int.Parse(PLCNetNo);
            plc2.ActStationNumber = int.Parse(PLCStatNo);
            plc2.ActSourceNetworkNumber = int.Parse(PCNetNo);
            plc2.ActSourceStationNumber = int.Parse(PCStatNo);
            plc2.ActTimeOut = int.Parse(PLCTimeout);
            plc2.ActCpuType = int.Parse(CPUType);
            plc2.ActHostAddress = sIPADDRESS;

            int rtn = 0;
            try
            {
                rtn = plc2.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                rtn = -1;
            }

            return rtn;
        }
        public int StartScaning()
        {
            //for (int i = 0; i < _Constant.frmCount; i++)
            //{
            //    //_threadScan[i] = new Thread(new ThreadStart(DoScan));
            //    _threadPLCScan[i] = new Thread(new ParameterizedThreadStart(DoScan));
            //    _threadPLCScan[i].Priority = ThreadPriority.Highest;
            //    _threadPLCScan[i].IsBackground = true;

            //    _bEnableScan[i] = true;
            //}

            //for (int i = 0; i < _Constant.frmCount; i++)
            //{
            //    _threadPLCScan[i].Start(i);
            //}

            //for (int i = 0; i < _Constant.frmCount; i++)
            //{
            //    _threadPLCScan[i].Join();
            //}
            _bEnableScan[0] = true;
            _threadPLCScan[0] = new Thread(() => DoScan(0));
            _threadPLCScan[0].Start();

            _bEnableScan[1] = true;
            _threadPLCScan[1] = new Thread(() => DoScan(1));
            _threadPLCScan[1].Start();

            _bEnableScan[2] = true;
            _threadPLCScan[2] = new Thread(() => DoScan(2));
            _threadPLCScan[2].Start();

            _bEnableScan[3] = true;
            _threadPLCScan[3] = new Thread(() => DoScan(3));
            _threadPLCScan[3].Start();

            return 0;
        }
        private void DoScan(int stagenumber)
        {
            //int nIndex = Convert.ToInt32(stagenumber);

            while (_bEnableScan[stagenumber])
            {
                //SystemLogger.Log(Level.Verbose, "SCAN");
                ScanTrigger(stagenumber);
                Thread.Sleep(_iScanInterval);
            }
        }
        private void ScanTrigger(int stageno)
        {
            int pc_count = _Constant.PC_D_LEN * 2;
            int[] pc_iScan = new int[pc_count];

            int plc_count = _Constant.PLC_D_LEN * 2;
            int[] plc_iScan = new int[plc_count];

            #region PLC CONNECTION 
            int rtn = 0;
            try
            {
                rtn = plc2.Open();

                if (rtn != 0)
                {
                    plc2.Close();
                    Thread.Sleep(500);
                    rtn = plc2.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                rtn = -1;
            }
            #endregion

            #region Show PLC Data to DataGridView
            int rtn1 = plc2.ReadDeviceBlock(_Constant.PLC_D_START_NUM[stageno].ToString(), 300, out plc_iScan[0]);
            int rtn2 = plc2.ReadDeviceBlock(_Constant.PC_D_START_NUM[stageno].ToString(), 200, out pc_iScan[0]);

            RaiseOnShowData(pc_iScan, plc_iScan, stageno);
            #endregion
        }
        public int StopScan(int nIndex)
        {
            if (_threadPLCScan[nIndex] != null)
            {
                _bEnableScan[nIndex] = false;
                _threadPLCScan[nIndex].Join();
                _threadPLCScan[nIndex] = null;
            }
            return 0;
        }

        

        public void ReadWord(int iAddress, int iSize, out ushort[] asValue)
        {
            string sAddr;
            ushort[] asValues = new ushort[1];
            //asValue[0] = asValues[0];
            asValue = asValues;
            sAddr = "D*" + iAddress.ToString();
           // _Driver.GetPlcValues_Bin(sAddr, iSize, out asValue, out sOutMsg);
        }

        public void ReadWord(int iAddress, out ushort asValue)
        {
            string sAddr;
            int iSize = 1;
            ushort[] asValues = new ushort[1];
            asValue = 0;
            asValues[0] = asValue;

            sAddr = "D*" + iAddress.ToString();
           // _Driver.GetPlcValues_Bin(sAddr, iSize, out asValues, out sOutMsg);
            asValue = asValues[0];
        }

        public void ReadBit(int iAddress, int nIndex, out bool bValue)
        {
            bValue = false;
            string sAddr;
            int iSize = 1;
            ushort[] asValue = new ushort[1];

            sAddr = "D*" + iAddress.ToString();
           // _Driver.GetPlcValues_Bin(sAddr, iSize, out asValue, out sOutMsg);
            if (asValue[nIndex] == 1) bValue = true;
            else bValue = false;
        }

        public void ReadString(int iAddress, int iSize, out string strValue)
        {
            StringBuilder sb = new StringBuilder();
            string sAddr;
            strValue = "";

            int iMemoryLength = (iSize + 1) / 2;
            ushort[] asValue = new ushort[iMemoryLength];
            iMemoryLength *= 2;


            sAddr = "D*" + iAddress.ToString();
            //_Driver.GetPlcValues_Bin(sAddr, iMemoryLength, out asValue, out sOutMsg);

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
               // _Driver.SetPlcValues_Binary(sAddr, sWrite.Length, abb, out sMsg);

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
                string sAddr;
                bool[] abb = new bool[1];
                abb[0] = bValue;

                sAddr = "D*" + iAddress.ToString();
                //_Driver.SetPlcValues_Binary(sAddr, sWrite.Length, abb, out sMsg);
                //_Driver.SetPlcValues_Binary(sAddr, nIndex, abb, out string errorMsg);

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
