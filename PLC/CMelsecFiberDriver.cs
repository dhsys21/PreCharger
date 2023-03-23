using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreCharger.Common
{
    class CMelsecFiberDriver
    {
        CEquipmentData _system;
        public delegate void delegateConnectionState(CMelsecFiberDriver driver, enumConnectionState enumConnection);
        public delegate void delegateDriverErrorOccurred(CMelsecFiberDriver driver, int iErrorCode);
        public event delegateConnectionState OnConnectionStateChanged = null;
        protected void RaiseOnConnection(enumConnectionState enumConnection)
        {
            if (OnConnectionStateChanged != null)
            {
                OnConnectionStateChanged(this, enumConnection);
            }
        }

        public event delegateDriverErrorOccurred ErrorOccurred = null;
        protected void RaiseErrorOccurred(int iErrorCode)
        {
            try
            {
                if (ErrorOccurred != null)
                {
                    Control target = ErrorOccurred.Target as Control;
                    if (target != null && target.InvokeRequired)
                        target.Invoke(ErrorOccurred, new object[] { this, iErrorCode });
                    else
                        ErrorOccurred(this, iErrorCode);
                }
            }
            catch (Exception ex)
            {
                //SystemLogger.Log(ex);
            }
        }

        protected enumAddressingMode _enAddressingMode = enumAddressingMode.DEC;

        protected enumConnectionState _enConnectionState = enumConnectionState.Disconnected;
        protected enumConnectionState _ConnectionState;
        public enumConnectionState ConnectionState
        {
            get { return _ConnectionState; }
        }

        public bool Enabled
        {
            get { return this.ConnectionState != enumConnectionState.Disabled; }
        }

        public bool Connected
        {
            get { return ConnectionState == enumConnectionState.Connected; }
        }


        #region Properties
        protected enum enumAddressingMode
        {
            DEC,
            HEX
        }

        private CMelsecNetMemoryCollection _MonitorMemory = new CMelsecNetMemoryCollection();
        private const int _iMaxReadSize = 8192;

        /// <summary>
        /// MelsecNet Driver 사용을 위한 Minimum WorkingSet Size를 1MB로 설정
        /// </summary>
        private const int _iMinWorkingSetSize = 0x100000;
        /// <summary>
        /// MelsecNet Driver 사용을 위한 Maximum WorkingSet Size를 3MB로 설정
        /// </summary>
        private const int _iMaxWorkingSetSize = 0x300000;
        #endregion

        #region Constructor & Disposer
        static CMelsecFiberDriver()
        {
            // Working Set Size를 설정 한다.
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, _iMinWorkingSetSize, _iMaxWorkingSetSize);
        }

        public CMelsecFiberDriver()
        {
            _system = CEquipmentData.GetInstance();

            //_iChannelNo = 51;
            //_iStationNo = 0xFF;
            //_iNetworkNo = 11;

            _iChannelNo = _system.PLCCHANNELNO;
            _iStationNo = _system.PLCSTATIONNUMBER;
            _iNetworkNo = _system.PLCNETWORKNUMBER;

            _iMode = -1;
            _iPath = 0;
        }



        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected int RefreshDriverMemory()
        {
            if (!Connected) return 0;

            return RefreshMonitorMemory();
        }
        #endregion

        #region Monitoring
        private static string BIT_MONITOR = "BIT_MON";
        private static string WORD_MONITOR = "WORD_MON";
        private int RefreshMonitorMemory()
        {
            int iRet = 0;

            try
            {
                foreach (CMelsecNetMemory mem in _MonitorMemory.Values)
                {
                    switch (mem.MemoryType)
                    {
                        case enumMemoryType.Bit:
                            {
                                int iSize = mem.RawMemorySize * 2;

                                iRet = Receive(_iStationNo, mem.DeviceType, mem.StartAdderess, ref iSize, mem.RawMemory);

                                if (!IsSuccess(BIT_MONITOR, iRet))
                                {
                                    //SystemLogger.Log(Level.Exception, string.Format("Melsec-Net Driver Monitoring Failed!(mdReceive, ErrorCode = {0})", iRet), _DriverName);
                                    return iRet;
                                }
                            }
                            break;
                        case enumMemoryType.Word:
                            {
                                int i, j;
                                short[] sMemory = new short[_iMaxReadSize];
                                int iMemorySize = 0;
                                i = 0; j = 0;
                                while (true)
                                {
                                    if (mem.RawMemorySize <= _iMaxReadSize * i)
                                        break;

                                    if (mem.RawMemorySize >= (_iMaxReadSize * (i + 1)))
                                        iMemorySize = _iMaxReadSize * 2;
                                    else
                                        iMemorySize = (mem.RawMemorySize - (_iMaxReadSize * i)) * 2;


                                    iRet = Receive(_iStationNo, mem.DeviceType, mem.StartAdderess + _iMaxReadSize * i, ref iMemorySize, sMemory);

                                    if (!IsSuccess(WORD_MONITOR, iRet))
                                    {
                                        //SystemLogger.Log(Level.Exception, string.Format("Melsec-Net Driver Monitoring Failed!(mdReceive, ErrorCode = {0})", iRet), _DriverName);
                                        return iRet;
                                    }
                                    for (j = 0; j < iMemorySize / 2; j++)
                                    {
                                        mem.RawMemory[i * _iMaxReadSize + j] = sMemory[j];
                                    }
                                    i++;
                                }
                            }
                            break;
                    }

                }

                return 0;
            }
            catch (Exception ex)
            {
                //SystemLogger.Log(ex);
                return -1;
            }
        }

        /// <summary>
        /// MNet Function 실행 결과 값의 성공 여부 확인
        /// 
        /// - 성공
        ///    0      : Normal End
        ///    66     : Already Opend
        ///    -28150 : Data Link Error, Access the Own station's link device when data link not in progress.
        ///             Writing the data or reading are done. However, the data is not guaranteed.
        /// </summary>
        /// <param name="iRet"></param>
        /// <returns></returns>
        private bool IsSuccess(string strFrom, int iRet)
        {
            bool bIsSuccess = (iRet == 0) || (iRet == 66) || (iRet == -28150);
            if (!bIsSuccess)
            {
                RaiseErrorOccurred(iRet);
               // SystemLogger.Log(Level.Warning, string.Format("MelsecNet Driver Function Error!(From : {0}, Return Code : {1})", strFrom, iRet));
            }
            return bIsSuccess;
        }
        #endregion

        #region MelsecNet 관련 Function
        string _DriverName = string.Empty;
        public string DriverName
        {
            get { return _DriverName; }
            set { _DriverName = value; }
        }

        protected int _iChannelNo;
        public int ChannelNo
        {
            get { return _iChannelNo; }
            set { _iChannelNo = value; }
        }
        protected int _iNetworkNo;
        public int NetworkNo
        {
            get { return _iNetworkNo; }
            set { _iNetworkNo = value; }
        }
        protected int _iStationNo;
        public int StationNo
        {
            get { return _iStationNo; }
            set { _iStationNo = value; }
        }
        protected int _iMode;
        public int Mode
        {
            get { return _iMode; }
            set { _iMode = value; }
        }
        protected int _iPath;
        public int Path
        {
            get { return _iPath; }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int iMinimumWorkingSetSize, int iMaximumWorkingSetSize);
        [DllImport("mmscl32.dll")]
        private static extern ushort mdOpen(int sChan, int sMode, ref int iPath);
        [DllImport("mmscl32.dll")]
        private static extern ushort mdClose(int iPath);
        [DllImport("mmscl32.dll")]
        private static extern int mdDevSetEx(int iPath, int iNetNo, int iStNo, int iDevTyp, int iDevNo);
        [DllImport("mmscl32.dll")]
        private static extern int mdDevRstEx(int iPath, int iNetNo, int iStNo, int iDevtyp, int iDevNo);
        [DllImport("mmscl32.dll")]
        private static extern int mdSendEx(int iPath, int iNetNo, int iStNo, int iDevTyp, int iDevNo, ref int iSize, [In] short[] asData);
        [DllImport("mmscl32.dll")]
        private static extern int mdReceiveEx(int iPath, int iNetNo, int iStNo, int iDevTyp, int iDevNo, ref int iSize, [Out] short[] asData);

        protected virtual int Set(int iStationNo, enumDeviceType enDeviceType, int iDevNo)
        {
            return mdDevSetEx(_iPath, _iNetworkNo, iStationNo, (int)enDeviceType, iDevNo);
        }

        protected virtual int Reset(int iStationNo, enumDeviceType enDeviceType, int iDevNo)
        {
            return mdDevRstEx(_iPath, _iNetworkNo, iStationNo, (int)enDeviceType, iDevNo);
        }

        protected virtual int Send(int iStationNo, enumDeviceType enDeviceType, int iDevNo, ref int iSize, [In] short[] asData)
        {
            return mdSendEx(_iPath, _iNetworkNo, iStationNo, (int)enDeviceType, iDevNo, ref iSize, asData);
        }

        protected virtual int Receive(int iStationNo, enumDeviceType enDeviceType, int iDevNo, ref int iSize, [Out] short[] asData)
        {
            return mdReceiveEx(_iPath, _iNetworkNo, iStationNo, (int)enDeviceType, iDevNo, ref iSize, asData);
        }

        public bool SetBit(int iAddress, enumDeviceType DeviceType)
        {
            return SetBit(iAddress, _iStationNo, DeviceType);
        }

        public bool SetBit(int iAddress, int iStationNo, enumDeviceType DeviceType)
        {
            if (!this.Enabled) return false;

            int iRet = 0;

            iRet = Set(iStationNo, DeviceType, iAddress);

            return IsSuccess(iAddress.ToString(), iRet);
        }

        public bool ResetBit(int iAddress, enumDeviceType DeviceType)
        {
            return ResetBit(iAddress, _iStationNo, DeviceType);
        }

        public bool ResetBit(int iAddress, int iStationNo, enumDeviceType DeviceType)
        {
            if (!this.Enabled) return false;

            int iRet = 0;

            iRet = Reset(iStationNo, DeviceType, iAddress);

            return IsSuccess(iAddress.ToString(), iRet);
        }
        #endregion

        #region Read All Data From PLC
        public bool ReadAllWord(int iAddress, int iStationNo, enumDeviceType DeviceType, int iSize, out ushort[] asValue)
        {
            int iRet = 0;
            int nCount = iSize;

            asValue = new ushort[iSize];
            short[] sValue = new short[nCount];

            if (!this.Enabled) return false;

            iRet = Receive(iStationNo, DeviceType, iAddress, ref nCount, sValue);
            for (int i = 0; i < iSize; i++)
                asValue[i] = Convert.ToUInt16(sValue[i]);// Convert.ToUInt16(sValue[i * 2] + sValue[i * 2 + 1] * 256);
            return IsSuccess(iAddress.ToString(), iRet);
        }
        #endregion

        #region Read Data From PLC

        public bool ReadBit(int iAddress, int nIndex, int iStationNo, enumDeviceType DeviceType, out bool bValue)
        {
            bValue = false;
            if (!this.Enabled) return false;

            int iRet = 0;
            bool bRet = false;
            short[] asData = new short[1];
            int iSize = 2;

            switch (DeviceType)
            {
                case enumDeviceType.X:
                case enumDeviceType.Y:
                case enumDeviceType.B:
                case enumDeviceType.SB:
                case enumDeviceType.M:
                    {
                        int iStartAddress = (iAddress / 0x10) * 0x10;

                        iRet = Receive(iStationNo, DeviceType, iStartAddress, ref iSize, asData);
                        if (!IsSuccess(iAddress.ToString(), iRet))
                        {
                            bValue = false;
                            bRet = false;
                        }
                        else
                        {
                            bValue = ((asData[0] & (1 << (iAddress - iStartAddress))) != 0);
                            bRet = true;
                        }
                    }
                    break;
                case enumDeviceType.W:
                case enumDeviceType.SW:
                case enumDeviceType.WW:
                case enumDeviceType.WR:
                case enumDeviceType.D:
                    {

                        iRet = Receive(iStationNo, DeviceType, iAddress, ref iSize, asData);
                        if (!IsSuccess(iAddress.ToString(), iRet))
                        {
                            bValue = false;
                            bRet = false;
                        }
                        else
                        {
                            bValue = asData[0] == 1 ? true : false;// ((asData[0] & (1 << nIndex)) != 0);
                            bRet = true;
                        }
                    }
                    break;
            }
            return bRet;
        }

        public bool ReadWord(int iAddress, int iStationNo, enumDeviceType DeviceType, out ushort sValue)
        {
            sValue = 0;
            if (!this.Enabled) return false;

            int iRet = 0;
            short[] asData = new short[1];
            int iSize = 2;

            iRet = Receive(iStationNo, DeviceType, iAddress, ref iSize, asData);
            sValue = Convert.ToUInt16(asData[0]);
            return IsSuccess(iAddress.ToString(), iRet);
        }

        public bool ReadString(int iAddress, int iStationNo, enumDeviceType DeviceType, int iLength, out string strValue)
        {
            strValue = "";
            StringBuilder sb = new StringBuilder();
            if (!this.Enabled) return false;

            int iMemoryLength = (iLength + 1) / 2;
            int iRet = 0;
            bool bRet;
            short[] asData = new short[iMemoryLength];
            iMemoryLength *= 2;


            iRet = Receive(iStationNo, DeviceType, iAddress, ref iMemoryLength, asData);

            if (!IsSuccess(iAddress.ToString(), iRet))
            {
                strValue = "";
                return false;
            }
            else
            {
                for (int i = 0; i < iMemoryLength / 2; i++)
                {
                    sb.Append(Convert.ToChar(asData[i] & 0xFF));
                    sb.Append(Convert.ToChar((asData[i] >> 8) & 0xFF));
                }
                bRet = true;
            }

            strValue = sb.ToString();
            strValue = strValue.Substring(0, iLength);
            strValue = strValue.Trim(" \0".ToCharArray());

            return bRet;
        }
        #endregion

        #region Write Data to PLC
        public bool WriteBit(int iAddress, int nIndex, int iStationNo, enumDeviceType DeviceType, bool bValue)
        {
            if (!this.Enabled) return false;

            int iRet = -1;
            switch (DeviceType)
            {
                case enumDeviceType.X:
                case enumDeviceType.Y:
                case enumDeviceType.B:
                case enumDeviceType.SB:
                case enumDeviceType.M:
                    {
                        if (bValue)
                            iRet = Set(iStationNo, DeviceType, iAddress);
                        else
                            iRet = Reset(iStationNo, DeviceType, iAddress);
                    }
                    break;
                case enumDeviceType.W:
                case enumDeviceType.SW:
                case enumDeviceType.WW:
                case enumDeviceType.WR:
                case enumDeviceType.D:
                    {
                        short[] asData = new short[1];
                        int iSize = 2;

                        iRet = Receive(iStationNo, DeviceType, iAddress, ref iSize, asData);
                        if (IsSuccess(iAddress.ToString(), iRet))
                        {
                            if (bValue)
                                asData[0] = (short)((ushort)asData[0] | (1 << nIndex));
                            else
                                asData[0] = (short)((ushort)asData[0] & ~(1 << nIndex));

                            iRet = Send(iStationNo, DeviceType, iAddress, ref iSize, asData);
                        }
                    }
                    break;
            }

            return IsSuccess(iAddress.ToString(), iRet);
        }

        public bool WriteWord(int iAddress, int iStationNo, enumDeviceType DeviceType, int iData)
        {
            if (!this.Enabled) return false;

            int iRet = 0;
            short[] asData = new short[2];
            asData[0] = (short)(iData & 0x0000ffff);
            asData[1] = (short)((iData & 0xffff0000) >> 16);
            int iSize = 2;

            iRet = Send(iStationNo, DeviceType, iAddress, ref iSize, asData);

            return IsSuccess(iAddress.ToString(), iRet);
        }

        public bool WriteString(int iAddress, int iStationNo, enumDeviceType DeviceType, int iLength, string strData)
        {
            if (!this.Enabled) return false;

            int iMemoryLength = (iLength + 1) / 2;
            // WordLength 보다 큰 Data의 입력시 Length만큼 Data를 잘라냄
            if (strData.Length > iLength)
                strData = strData.Substring(0, iLength);

            strData = strData.PadRight(iMemoryLength * 2);

            short[] asData = new short[iMemoryLength];
            for (int i = 0; i < iMemoryLength; i++)
            {
                asData[i] = Convert.ToInt16(strData[i * 2]);
                asData[i] = Convert.ToInt16((ushort)asData[i] | (Convert.ToUInt16(strData[i * 2 + 1]) << 8));
            }

            int iRet = 0;
            iMemoryLength *= 2;

            iRet = Send(iStationNo, DeviceType, iAddress, ref iMemoryLength, asData);
            return IsSuccess(iAddress.ToString(), iRet);
        }

        #endregion

        

        #region Interfaces

        protected void UpdateConnectionInfo(int iCHANNEL_NO, int iMODE, int iSTATION_NO)
        {
            _iChannelNo = iCHANNEL_NO;
            _iMode = iMODE;
            _iStationNo = iSTATION_NO;
        }

        public int Open()
        {
            if (this.Connected)
                Close();

            int iRet = mdOpen(_iChannelNo, _iMode, ref _iPath);
            // 실제 보드가 장착된 PC에서 되는지 확인할것.

            bool bSuccess = (iRet == 0) || (iRet == 66);
            if (bSuccess)
                _ConnectionState = enumConnectionState.Connected;

            return bSuccess ? 0 : -1;
        }

        public int Open(int iCHANNEL_NO, int iMODE, int iSTATION_NO)
        {
            UpdateConnectionInfo(iCHANNEL_NO, iMODE, iSTATION_NO);

            int iRet = mdOpen(_iChannelNo, _iMode, ref _iPath);
            // 실제 보드가 장착된 PC에서 되는지 확인할것.

            bool bSuccess = (iRet == 0) || (iRet == 66);
            if (bSuccess)
            {
                _ConnectionState = enumConnectionState.Connected;
                RaiseOnConnection(enumConnectionState.Connected);
            }

            return bSuccess ? 0 : -1;
        }

        public int Close()
        {
            mdClose(_iPath);

            _ConnectionState = enumConnectionState.Disconnected;
            RaiseOnConnection(enumConnectionState.Disconnected);

            return 0;
        }
        #endregion
    }
}
