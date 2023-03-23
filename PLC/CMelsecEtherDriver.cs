using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreCharger.Common
{
    class CMelsecEtherDriver
    {
        CEquipmentData _system;
        public Socket clientSocket;                     // PLC통신 Socket

        const int BUF_MAX = 2048;                       // 송수신 버퍼 크기
        const int RESPONSE_SIZE = 11;
        const int FRAME_SIZE = 21;

        CMelsecConvertPublic uConvert = new CMelsecConvertPublic();

        [Flags] //enumeration flags
        enum Command : int
        {
            DEV_READ = 0x0401,                          // 비트디바이스(X,Y,M등)를 1점 단위로 읽는다.
            // 비트디바이스(X,Y,M등)를 16점 단위로 읽는다.
            // 워드디바이스(D,R,T,C등)를 1점 단위로 읽는다.

            DEV_WRITE = 0x1401,                         // 비트디바이스(X,Y,M등)를 1점 단위로 쓴다.
            // 비트디바이스(X,Y,M등)를 16점 단위로 쓴다.
            // 워드디바이스(D,R,T,C등)를 1점 단위로 쓴다.

            RANDOM_WRITE = 0x1402,                      // 비트단위 or 워드단위의 랜덤 쓰기 

            RANDOM_READ = 0x0403                        // 워드단위의 랜덤 읽기
        }

        public delegate void delegateConnectionState(CMelsecEtherDriver driver, enumConnectionState enumConnection);
        public delegate void delegateDriverErrorOccurred(CMelsecEtherDriver driver, int iErrorCode);
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
        static CMelsecEtherDriver()
        {
        }

        public CMelsecEtherDriver()
        {
            _system = CEquipmentData.GetInstance();

            _sIpAddress = _system.PLCIPADDRESS;
            _iPort = _system.PLCPORT;
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
            return 0;
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
        string _sIpAddress = string.Empty;
        public string sIPADDRESS
        {
            get { return _sIpAddress; }
            set { _sIpAddress = value; }
        }

        protected int _iPort;
        public int iPORT
        {
            get { return _iPort; }
            set { _iPort = value; }
        }

        #endregion

        #region Read Data From PLC


        #endregion

        #region Write Data to PLC


        #endregion

        #region Binary 처리부

        public bool GetPlcValues_Bin(string stAddr, int wordCount, out ushort[] readWordSet, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[FRAME_SIZE];
            byte[] rcvBuf = new byte[BUF_MAX];

            readWordSet = new ushort[wordCount];
            short[] sValue = new short[wordCount];

            Array.Clear(readWordSet, 0, readWordSet.Length);
            errorMsg = "";

            try
            {
                sndSize = MakeReadFrame_Binary(stAddr, wordCount, sndBuf, true);
                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }
                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= RESPONSE_SIZE + wordCount * 2) break;
                }

                if (rcvSize > 0)
                {
                    if (rcvSize == RESPONSE_SIZE + wordCount * 2
                        && rcvBuf[0] == 0xD0 && rcvBuf[1] == 0x00           // rcvBuf[0], rcvBuf[1] : 서브헤더 (0xD0, 0x00)
                        && rcvBuf[9] == 0x00 && rcvBuf[10] == 0x00)         // rcvBuf[9], rcvBuf[10] : 종료코드
                    {
                        for (int i = 0; i < wordCount; i++)
                        {
                            readWordSet[i] = BitConverter.ToUInt16(rcvBuf, 11 + i * 2);
                        }
                    }
                    else if (rcvSize > RESPONSE_SIZE + wordCount * 2)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < RESPONSE_SIZE + wordCount * 2)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        public bool GetPlcValues_Binary(string stAddr, int wordCount, byte[] readWordSet, byte[] readErrorCodeSet, out string sRealData, out string sSendParam, out string errorMsg)
        {
            errorMsg = "";
            sRealData = "";
            sSendParam = "";

            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[FRAME_SIZE];
            byte[] rcvBuf = new byte[BUF_MAX];


            try
            {

                sndSize = MakeReadFrame_Binary(stAddr, wordCount, sndBuf, true);
                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }


                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);

                sRealData = ASCIIEncoding.ASCII.GetString(sndBuf);

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= RESPONSE_SIZE + wordCount * 2) break;
                }

                if (rcvSize > 0)
                {

                    if (rcvSize == RESPONSE_SIZE + wordCount * 2
                        && rcvBuf[0] == 0xD0 && rcvBuf[1] == 0x00           // rcvBuf[0], rcvBuf[1] : 서브헤더 (0xD0, 0x00)
                        && rcvBuf[9] == 0x00 && rcvBuf[10] == 0x00)         // rcvBuf[9], rcvBuf[10] : 종료코드
                    {
                        for (int i = 0; i < wordCount; i++)
                        {
                            readWordSet[i] = (byte)BitConverter.ToUInt16(rcvBuf, 11 + i * 2);
                        }
                    }
                    else if (rcvSize > RESPONSE_SIZE + wordCount * 2)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < RESPONSE_SIZE + wordCount * 2)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// FETCH Request Frame [READ/ BIT or WORD]
        /// </summary>
        /// <param name="stAddr"></param>
        /// <param name="pointCount"></param>
        /// <param name="frameData"></param>
        /// <param name="isWordRead"></param>
        /// <returns></returns>
        int MakeReadFrame_Binary(string stAddr, int pointCount, byte[] frameData, bool isWordRead)
        {
            byte devCode;
            int devNum;
            byte[] buf = new byte[2];

            Array.Clear(frameData, 0, frameData.Length);

            if (!ParseAddress(stAddr, out devCode, out devNum))
                return 0;

            frameData[0] = 0x50;						// 서브헤더
            frameData[1] = 0x00;

            frameData[2] = 0x00;						// 네트워크 번호 - 자국 Default

            frameData[3] = (byte)0xFF;			    // PLC 번호 - 자국 Default

            frameData[4] = (byte)0xFF;			    // 요구상대모듈 I/O번호
            frameData[5] = 0x03;

            frameData[6] = 0x00;						// 요구상대모듈 국번호

            frameData[7] = 0x0C;						// 요구데이터 길이 (CPU 감시타이머 ~ 끝까지) : 12 Byte
            frameData[8] = 0x00;

            frameData[9] = 0x10;						// CPU 감시타이머 (10*250ms = 2.5초) (* 단위 - 250ms)
            frameData[10] = 0x00;                     // - 설정범위<자국일 경우>: 0(무한대기), 1~40

            // 커맨드
            buf = BitConverter.GetBytes((int)Command.DEV_READ);
            frameData[11] = buf[0];   // L
            frameData[12] = buf[1];   // H

            //---------------------------------------------------------------------<데이터 부(캐릭터A부)>
            // 서브커맨드 (단위지정/ 모니터 조건 지정유무/ 디바이스 메모리 확장지정)
            if (isWordRead)
            {
                frameData[13] = 0x00;
                frameData[14] = 0x00;
            }
            else
            {
                frameData[13] = 0x01;
                frameData[14] = 0x00;
            }

            //++++++++++++++++++++++++++++++++++++++++++++++++++<요구 데이터 부>
            // 선두디바이스(3Byte)
            buf = BitConverter.GetBytes(devNum);
            frameData[15] = buf[0];   
            frameData[16] = buf[1];   
            frameData[17] = buf[2];

            // 디바이스 코드
            frameData[18] = devCode;

            // 디바이스점수
            buf = BitConverter.GetBytes(pointCount);
            frameData[19] = buf[0];   // L
            frameData[20] = buf[1];   // H
            //++++++++++++++++++++++++++++++++++++++++++++++++++
            //---------------------------------------------------------------------

            return FRAME_SIZE;
        }

        /// <summary>
        /// 임의 Address부터 N WORD Write
        /// </summary>
        /// <param name="stAddr"></param>
        /// <param name="wordCount"></param>
        /// <param name="writeWordSet"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SetPlcValues_Binary(string stAddr, int wordCount, ushort[] writeWordSet, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[BUF_MAX];
            byte[] rcvBuf = new byte[1024];

            errorMsg = "";

            try
            {
                sndSize = MakeWriteFrame_Binary(stAddr, wordCount, writeWordSet, sndBuf);
                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }
                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= RESPONSE_SIZE) break;
                }

                if (rcvSize > 0)
                {
                    if (rcvSize == RESPONSE_SIZE
                        && rcvBuf[0] == 0xD0 && rcvBuf[1] == 0x00           // rcvBuf[0], rcvBuf[1] : 서브헤더 (0xD0, 0x00)
                        && rcvBuf[9] == 0x00 && rcvBuf[10] == 0x00)         // rcvBuf[9], rcvBuf[10] : 종료코드
                    {
                        return true;
                    }
                    else if (rcvSize > RESPONSE_SIZE)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < RESPONSE_SIZE)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 임의 Address부터 N BIT Write
        /// </summary>
        /// <param name="stAddr"></param>
        /// <param name="bitCount"></param>
        /// <param name="writeBitSet"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SetPlcValues_Binary(string stAddr, int bitCount, bool[] writeBitSet, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[BUF_MAX];
            byte[] rcvBuf = new byte[1024];

            errorMsg = "";

            try
            {
                sndSize = MakeWriteFrame_Binary(stAddr, bitCount, writeBitSet, sndBuf);
                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }
                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= RESPONSE_SIZE) break;
                }

                if (rcvSize > 0)
                {
                    if (rcvSize == RESPONSE_SIZE
                        && rcvBuf[0] == 0xD0 && rcvBuf[1] == 0x00           // rcvBuf[0], rcvBuf[1] : 서브헤더 (0xD0, 0x00)
                        && rcvBuf[9] == 0x00 && rcvBuf[10] == 0x00)         // rcvBuf[9], rcvBuf[10] : 종료코드
                    {
                        return true;
                    }
                    else if (rcvSize > RESPONSE_SIZE)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < RESPONSE_SIZE)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// FETCH Request Frame [WRITE/ WORD]
        /// </summary>
        /// <param name="stAddr"></param>
        /// <param name="wordCount"></param>
        /// <param name="wordData"></param>
        /// <param name="frmData"></param>
        /// <returns></returns>
        int MakeWriteFrame_Binary(string stAddr, int wordCount, ushort[] wordData, byte[] frmData)
        {
            byte devCode;
            int devNum;
            byte[] buf = new byte[2];

            Array.Clear(frmData, 0, frmData.Length);

            if (!ParseAddress(stAddr, out devCode, out devNum))
                return 0;

            frmData[0] = 0x50;						// 서브헤더
            frmData[1] = 0x00;

            frmData[2] = 0x00;						// 네트워크 번호 - 자국 Default

            frmData[3] = (byte)0xFF;			    // PLC 번호 - 자국 Default

            frmData[4] = (byte)0xFF;			    // 요구상대모듈 I/O번호
            frmData[5] = 0x03;

            frmData[6] = 0x00;						// 요구상대모듈 국번호

            buf = BitConverter.GetBytes(0x0C + wordCount * 2);  // 요구데이터 길이 (CPU 감시타이머 ~ 끝까지) : +12 Byte
            frmData[7] = buf[0];    // L
            frmData[8] = buf[1];    // H

            frmData[9] = 0x10;						// CPU 감시타이머 (10*250ms = 2.5초) (* 단위 - 250ms)
            frmData[10] = 0x00;                     // - 설정범위<자국일 경우>: 0(무한대기), 1~40

            // 커맨드
            buf = BitConverter.GetBytes((int)Command.DEV_WRITE);
            frmData[11] = buf[0];   // L
            frmData[12] = buf[1];   // H

            //---------------------------------------------------------------------<데이터 부(캐릭터A부)>
            // 서브커맨드 (단위지정/ 모니터 조건 지정유무/ 디바이스 메모리 확장지정)
            frmData[13] = 0x00;     // word
            frmData[14] = 0x00;

            //++++++++++++++++++++++++++++++++++++++++++++++++++<요구 데이터 부>
            // 선두디바이스(3Byte)
            buf = BitConverter.GetBytes(devNum);
            frmData[15] = buf[0];   // L
            frmData[16] = buf[1];   // H
            frmData[17] = 0x00;

            // 디바이스 코드
            frmData[18] = devCode;

            // 디바이스점수
            buf = BitConverter.GetBytes(wordCount);
            frmData[19] = buf[0];   // L
            frmData[20] = buf[1];   // H

            // Write Data
            for (int i = 0; i < wordCount; i++)
            {
                buf = BitConverter.GetBytes(wordData[i]);
                frmData[i * 2 + 21] = buf[0];  // L
                frmData[i * 2 + 1 + 21] = buf[1];  // H
            }
            //++++++++++++++++++++++++++++++++++++++++++++++++++
            //---------------------------------------------------------------------

            return FRAME_SIZE + wordCount * 2;
        }

        /// <summary>
        /// FETCH Request Frame [WRITE/ BIT]
        /// - 2개의 ON/OFF Data를 1 Byte에 싣음
        /// </summary>
        /// <param name="stAddr"></param>
        /// <param name="bitCount"></param>
        /// <param name="bitData"></param>
        /// <param name="frmData"></param>
        /// <returns></returns>
        int MakeWriteFrame_Binary(string stAddr, int bitCount, bool[] bitData, byte[] frmData)
        {
            byte devCode;
            int devNum;
            byte[] buf = new byte[2];

            Array.Clear(frmData, 0, frmData.Length);

            if (!ParseAddress(stAddr, out devCode, out devNum))
                return 0;

            frmData[0] = 0x50;						// 서브헤더
            frmData[1] = 0x00;

            frmData[2] = 0x00;						// 네트워크 번호 - 자국 Default

            frmData[3] = (byte)0xFF;			    // PLC 번호 - 자국 Default

            frmData[4] = (byte)0xFF;			    // 요구상대모듈 I/O번호
            frmData[5] = 0x03;

            frmData[6] = 0x00;						// 요구상대모듈 국번호

            buf = BitConverter.GetBytes(0x0C + (bitCount + 1) / 2);   // 요구데이터 길이 (CPU 감시타이머 ~ 끝까지) : +12 Byte
            frmData[7] = buf[0];    // L
            frmData[8] = buf[1];    // H

            frmData[9] = 0x10;						// CPU 감시타이머 (10*250ms = 2.5초) (* 단위 - 250ms)
            frmData[10] = 0x00;                     // - 설정범위<자국일 경우>: 0(무한대기), 1~40

            // 커맨드
            buf = BitConverter.GetBytes((int)Command.DEV_WRITE);
            frmData[11] = buf[0];   // L
            frmData[12] = buf[1];   // H

            //---------------------------------------------------------------------<데이터 부(캐릭터A부)>
            // 서브커맨드 (단위지정/ 모니터 조건 지정유무/ 디바이스 메모리 확장지정)
            frmData[13] = 0x01;     // bit
            frmData[14] = 0x00;

            //++++++++++++++++++++++++++++++++++++++++++++++++++<요구 데이터 부>
            // 선두디바이스(3Byte)
            buf = BitConverter.GetBytes(devNum);
            frmData[15] = buf[0];   // L
            frmData[16] = buf[1];   // H
            frmData[17] = 0x00;

            // 디바이스 코드
            frmData[18] = devCode;

            // 디바이스점수
            buf = BitConverter.GetBytes(bitCount);
            frmData[19] = buf[0];   // L
            frmData[20] = buf[1];   // H

            // Write Data
            for (int i = 0; i < (bitCount + 1) / 2; i++)
            {
                buf[0] = 0;

                for (int j = 0; j < 2; j++)
                {
                    if (i * 2 + j + 1 > bitCount)
                        break;

                    // true만 Set시킴
                    if (bitData[i * 2 + j])
                        buf[0] |= (byte)(0x0001 << (4 - 4 * j));
                }

                frmData[i + 21] = buf[0];
            }
            //++++++++++++++++++++++++++++++++++++++++++++++++++
            //---------------------------------------------------------------------

            return FRAME_SIZE + (bitCount + 1) / 2;
        }


        # endregion

        # region Ascii 처리부

        public bool GetPlcValues_Ascii(string stAddr, int wordCount, byte[] readWordSet, byte[] readErrorCodeSet, out string sRealData, out string sSendParam, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[FRAME_SIZE * 2];
            //byte[] byteOutPLCRtnCode = new byte[4];

            string sOutMsg = "";

            Array.Clear(readWordSet, 0, readWordSet.Length);
            Array.Clear(readErrorCodeSet, 0, readErrorCodeSet.Length);

            errorMsg = "";

            sRealData = "";
            sSendParam = "";

            string sErrMsg = "";

            try
            {
                bool isWordReadPLC = true; // 워드일괄 읽기 이므로
                sndSize = MakeReadFrame_Ascii(stAddr, wordCount.ToString(), isWordReadPLC, out sndBuf, out sErrMsg);

                sSendParam = ASCIIEncoding.ASCII.GetString(sndBuf);

                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }

                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);

                int iMaxLength = (RESPONSE_SIZE * 2) + wordCount * 4;
                byte[] rcvBuf = new byte[iMaxLength];

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= iMaxLength) break;
                }

                if (rcvSize > 0)
                {

                    sRealData = ASCIIEncoding.ASCII.GetString(rcvBuf);

                    if (rcvSize == iMaxLength
                        && rcvBuf[00] == 68   // 0x44   // D
                        && rcvBuf[01] == 48   // 0x30   // 0       
                        && rcvBuf[02] == 48   // 0x30   // 0       
                        && rcvBuf[03] == 48   // 0x30   // 0       // 서브헤더(0~3)[D0000]
                        && rcvBuf[18] == 48   // 0x30   // 0
                        && rcvBuf[19] == 48   // 0x30   // 0
                        && rcvBuf[20] == 48   // 0x30   // 0
                        && rcvBuf[21] == 48   // 0x30   // 0       // 종료코드(18~21) [0000]
                        )
                    {
                        byte[] byteConvertRcvBuf = uConvert.FromTByteToOByte(2, rcvBuf, out sOutMsg);

                        for (int i = 0; i < readWordSet.Length; i++)
                        {
                            readWordSet[i] = byteConvertRcvBuf[11 + i];
                        }
                    }
                    else if (rcvSize > iMaxLength)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < iMaxLength)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        int MakeReadFrame_Ascii(string sStartAddress, string sReadAddLength, bool isWordRead, out byte[] frameData, out string sErrorScript)
        {
            string sTempCommand = "";
            string strPlcValue = "";
            sErrorScript = "";

            frameData = new byte[FRAME_SIZE * 2];

            try
            {
                // (a) ASCII코드로 교신할 경우 / page. 60
                strPlcValue += "5000";   // 서브헤드
                strPlcValue += "00";     // 네트워크 국번
                strPlcValue += "FF";     // PLC번호
                strPlcValue += "03FF";   // 요구상대모듈 I/O 번호
                strPlcValue += "00";     // 요구상대모듈 국번호
                strPlcValue += "0018";   // 요구데이터의 길이(요구 데이터 부 까지의 바이트 수를 말함[16진수]) > 24바이트
                strPlcValue += "0010";   // CPU감시 타이머

                // 3.3.5 워드단위의 일괄읽기 (커맨드：0401) / page. 125
                strPlcValue += "0401";   // 커맨드
                strPlcValue += "0000";   // 서브커맨드

                string sAddr = sStartAddress.Substring(2, sStartAddress.Length - 2);
                sTempCommand = sStartAddress.Substring(0, 1) + "*" + sAddr.PadLeft(6, '0').ToUpper();   // D*001000, R*001000, M*001000... > * 붙지만 TN은 * 없이 TN000100(얘는 적용안됐음)
                strPlcValue += sTempCommand;   // PLC주소(R*010000)

                strPlcValue += int.Parse(sReadAddLength).ToString("X").PadLeft(4, '0');   // 디바이스 점수(16진수)

                frameData = ASCIIEncoding.ASCII.GetBytes(strPlcValue);

                return frameData.Length;
            }
            catch (Exception ex)
            {
                sErrorScript = ex.Message;
                return 0;
            }

        }
        # endregion

        bool ParseAddress(string stAddr, out byte deviceCode, out int deviceNum)
        {
            /*
            Address 예:			R:901
            DEVICE CODE:		D, W, R, T, C, X, Y, M, B, F
            구현 DeviceCode:	D, W, R,       X, Y, M, B
            */

            deviceCode = 0x00;
            deviceNum = uConvert.SelectAddrNormal(stAddr);     // Device Add. No.부분 추출
            if (deviceNum < 0)
                return false;

            switch (Char.Parse(stAddr.Substring(0, 1)))
            {
                case 'D':	// 데이터 레지스터 - D*
                    deviceCode = 0xA8;
                    break;
                case 'W':	// 링크 레지스터 - W*
                    deviceCode = 0xB4;
                    break;
                case 'R':	// 화일 레지스터 - R*
                    deviceCode = 0xAF;
                    break;
                case 'M':	// 내부 릴레이 - M*
                    deviceCode = 0x90;
                    break;
                case 'B':	// 링크 릴레이 - B*
                    deviceCode = 0xA0;
                    break;
                case 'X':	// 입력 - X*
                    deviceCode = 0x9C;
                    break;
                case 'Y':	// 출력 - Y*
                    deviceCode = 0x9D;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public bool SetPlcValues_Ascii(bool bASCII, string stAddr, string[] sWriteData, int iWriteLength, out string strOutPLCRtnCode, out string errorMsg)
        {
            int sndSize;
            int rcvSize = 0;
            int bytesRec;
            byte[] sndBuf = new byte[BUF_MAX];
            byte[] rcvBuf = new byte[1024];
            byte[] byteOutPLCRtnCode = new byte[4];

            string sMsg = "";

            strOutPLCRtnCode = "";
            errorMsg = "";

            try
            {
                sndSize = MakeWriteFrame_Ascii(bASCII, stAddr, sWriteData, iWriteLength, out sndBuf, out sMsg);

                if (sndSize == 0)
                {
                    errorMsg = "Wrong PLC Device Address";
                    return false;
                }

                clientSocket.Send(sndBuf, sndSize, SocketFlags.None);
                string sSendMsg = System.Text.ASCIIEncoding.ASCII.GetString(sndBuf);

                while (true)
                {
                    bytesRec = clientSocket.Receive(rcvBuf, rcvSize, rcvBuf.Length - rcvSize, SocketFlags.None);

                    if (bytesRec == 0) break;
                    rcvSize += bytesRec;
                    if (rcvSize >= RESPONSE_SIZE * 2) break;
                }

                if (rcvSize > 0)
                {

                    string SSSS1 = System.Text.ASCIIEncoding.ASCII.GetString(rcvBuf);

                    if (rcvSize == RESPONSE_SIZE * 2
                        && rcvBuf[00] == 68   // 0x44   // D
                        && rcvBuf[01] == 48   // 0x30   // 0       
                        && rcvBuf[02] == 48   // 0x30   // 0       
                        && rcvBuf[03] == 48   // 0x30   // 0       // 서브헤더(0~3)[D0000]
                        && rcvBuf[18] == 48   // 0x30   // 0
                        && rcvBuf[19] == 48   // 0x30   // 0
                        && rcvBuf[20] == 48   // 0x30   // 0
                        && rcvBuf[21] == 48   // 0x30   // 0       // 종료코드(18~21) [0000]
                        )
                    {
                        return true;
                    }
                    else if (rcvSize > RESPONSE_SIZE * 2)
                    {
                        errorMsg = "Overflow Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                    else if (rcvSize < RESPONSE_SIZE * 2)
                    {
                        errorMsg = "Incomplete Data (" + rcvSize + " Bytes)";
                        return false;
                    }
                }
                else
                {
                    errorMsg = "No Received";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }

            return true;
        }

        int MakeWriteFrame_Ascii(bool bASCII, string sStartAddress, string[] sWriteData, int iWriteLength, out byte[] btReturnCmd, out string sErrorScript)
        {
            btReturnCmd = new byte[42 + (Convert.ToInt16(iWriteLength) * 4)];
            sErrorScript = "";

            int iTempCommand = 0;
            string sTempCommand = "";
            string strPlcValue = "";
            string strHexaData = uConvert.GetConvertToHexaData1Word1Char(bASCII, sWriteData);

            try
            {
                if (strHexaData == "")
                {
                    return 0;
                }

                strPlcValue += "5000";   // 서브헤더
                strPlcValue += "00";     // 네트워크번호
                strPlcValue += "FF";     // PLC번호
                strPlcValue += "03FF";   // 요구상대 모듈 I/O 번호
                strPlcValue += "00";     // 요구상대 모듈 국번호

                iTempCommand = 24 + (iWriteLength * 4);
                strPlcValue += iTempCommand.ToString("X").PadLeft(4, '0'); //"001C";   // [28->1C] DecToHex(요구데이터길이(24(001014010000R*0100010001) + (4 * LENGTH)))

                // 3.3.6 워드단위의 일괄쓰기 (커맨드：1401)
                strPlcValue += "0010";   // CPU감시타이머
                strPlcValue += "1401";   // 커맨드
                strPlcValue += "0000";   // 서브커맨드

                string sAddr = sStartAddress.Substring(2, sStartAddress.Length - 2);
                sTempCommand = sStartAddress.Substring(0, 1) + "*" + sAddr.PadLeft(6, '0').ToUpper();
                strPlcValue += sTempCommand;   // PLC주소(R*010000)

                iTempCommand = strHexaData.Length / 4;
                strPlcValue += iTempCommand.ToString("X").PadLeft(4, '0');   // 디바이스 점수(16진수)
                strPlcValue += strHexaData;   // 쓰기 데이터(변환데이터)

                btReturnCmd = ASCIIEncoding.ASCII.GetBytes(strPlcValue);

                return btReturnCmd.Length;

            }
            catch (Exception ex)
            {
                sErrorScript = ex.Message;
                return 0;
            }
        }


        #region Interfaces

        protected void UpdateConnectionInfo(string sIPADDRESS, int iPORT)
        {
            _sIpAddress = sIPADDRESS;
            _iPort = iPORT;
        }
        public int Connect(string sIPADDRESS, int iPORT)
        {
            // TCP socket Make
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 송수신 무응답 처리(5초 이내 Send/Receive 이루어져야...)
            clientSocket.Blocking = true;
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 500);
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 500);

            try
            {
                clientSocket.Connect(IPAddress.Parse(sIPADDRESS), iPORT);

                if (IsConnected() == true)
                {
                    _ConnectionState = enumConnectionState.Connected;
                    RaiseOnConnection(enumConnectionState.Connected);
                }
            }
            catch
            {
                return 0;
            }

            return 1;
        }

        public int Close()
        {
            if (clientSocket != null)
                clientSocket.Close();
            _ConnectionState = enumConnectionState.Disconnected;
            RaiseOnConnection(enumConnectionState.Disconnected);

            return 0;
        }

        public bool IsConnected()
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
        }
        #endregion
    }
}
