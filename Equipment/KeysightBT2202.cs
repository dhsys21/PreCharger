using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ivi.Visa;
using Keysight.Visa;
using PreCharger.Common;

namespace PreCharger
{
    public class KeysightBT2202
    {
        Util util = new Util();
        public string VISA_ADDRESS;
        private ResourceManager manager;
        private TcpipSession session;
        private IMessageBasedFormattedIO GG;
        private IMessageBasedRawIO GGraw;
        private string IPADDRESS = string.Empty;
        private string PORT = string.Empty;
        private int TIMEOUT = 5000;
        private double _preVoltage;
        private double _preCurrent;
        private int _preTime;
        private double _voltage;
        private double _current;
        private int _time;
        private int _stageno = 0;
        private bool _ConnectionState;
        private bool _bAutoMode = false;
        protected enumEquipStatus _EquipStatus = enumEquipStatus.StepVacancy;
        public bool AUTOMODE { get => _bAutoMode; set => _bAutoMode = value; }
        public bool CONNECTIONSTATE { get => _ConnectionState; set => _ConnectionState = value; }
        public double VOLTAGE { get => _voltage; set => _voltage = value; }
        public double CURRENT { get => _current; set => _current = value; }
        public int TIME { get => _time; set => _time = value; }
        public double PREVOLTAGE { get => _preVoltage; set => _preVoltage = value; }
        public double PRECURRENT { get => _preCurrent; set => _preCurrent = value; }
        public int PRETIME { get => _preTime; set => _preTime = value; }
        public enumEquipStatus EQUIPSTATUS { get => _EquipStatus; set => _EquipStatus = value; }
        public int STAGENO { get => _stageno; set => _stageno = value; }

        protected System.Timers.Timer _tmrReconnect = new System.Timers.Timer(2000);

        #region delegate
        //Connection State는 공통적으로 사용
        public delegate void delegateDoWork();
        public delegate void delegateConnectionState(bool enumConnect);
        public event delegateConnectionState OnConnection = null;
        protected void RaiseOnConnectionState(bool enumConnect)
        {
            if (OnConnection != null)
            {
                new delegateDoWork(delegate ()
                {
                    OnConnection(enumConnect);
                }).BeginInvoke(null, null);
            }
        }

        public delegate void delegateReport_OnReceived(string strMessage, int stageno);
        public event delegateReport_OnReceived OnReceived = null;
        protected void RaiseOnReceived(bool bAsync, string strMessage, int stageno)
        {
            if (OnReceived != null)
            {
                OnReceived(strMessage, stageno);
            }
        }

        #endregion

        #region GetInstance()
        private static KeysightBT2202[] PrechargerDriver = new KeysightBT2202[_Constant.frmCount];

        public static KeysightBT2202 GetInstance(int nIndex)
        {
            if (PrechargerDriver[nIndex] == null) PrechargerDriver[nIndex] = new KeysightBT2202();
            return PrechargerDriver[nIndex];
        }
        #endregion

        public KeysightBT2202()
        {
            // _tmrReconnect.Elapsed += new System.Timers.ElapsedEventHandler(_tmrReconnect_Elapsed);
            //_tmrReconnect.Enabled = true;
        }
        public KeysightBT2202(string ipaddress, string port, int stageno)
        {
            Open(ipaddress, port, stageno);

            //* time, current (A), voltage (V)
            //* 30초, 1000mA, 1000mV
            SetPrechargeParameter(30, 1.0, 1.0);
        }

        public async void Open(string ipaddress, string port, int stageno)
        {
            STAGENO = stageno;
            IPADDRESS = ipaddress;
            PORT = port;
            //VISA_ADDRESS = "TCPIP0::" + IPADDRESS + "::" + PORT + "::SOCKET";
            VISA_ADDRESS = "TCPIP0::" + IPADDRESS + "::5025::SOCKET";
            VISA_ADDRESS = "TCPIP0::" + IPADDRESS + "::inst0::INSTR";
            manager = new ResourceManager();
            

            try
            {
                session = (TcpipSession)manager.Open(VISA_ADDRESS, Ivi.Visa.AccessModes.None, TIMEOUT);
                GG = session.FormattedIO;
                GGraw = session.RawIO;
                //timeout does not seem to stick when opening so set it explicitly
                session.TimeoutMilliseconds = 3000;
                
                runRST();
                await Task.Delay(5000);

                runCLEAR();
                await Task.Delay(1000);
            }
            catch (Exception ex) {
                util.SaveLog(STAGENO, "Connection Error : " + VISA_ADDRESS);
            }

            string strResult = Connect();
            if (strResult.Contains("Keysight Technologies")) CONNECTIONSTATE = true;
            else CONNECTIONSTATE = false;
        }
        private string Connect()
        {
            //string idnResponse = string.Empty;
            //try
            //{
            //    ioObject.WriteString("*IDN?");
            //    idnResponse = ioObject.ReadString();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

            string idnResponse;
            CMD = "*IDN?";
            idnResponse = RunCommand(CMD);

            return idnResponse;
        }
        public void Disconnect()
        {
            session.Dispose();
            session = null;
            //connectionOpened = true;
        }

        public void SetChargeParameter(int time, double current, double voltage)
        {
            _voltage = (voltage / 1000.0);
            _current = (current / 1000.0);
            _time = time;
        }
        public void SetPrechargeParameter(int time, double current, double voltage)
        {
            _preVoltage = (voltage / 1000.0);
            _preCurrent = (current / 1000.0);
            _preTime = time;
        }

        #region BT2202A COMMANDS
        string CMD = string.Empty;
        public bool sendSCPI(string scpi, bool checkError = true)
        {
            try
            {
                bool ret = true;

                util.SaveLog(STAGENO, "Send> " + scpi);
                GG.WriteLine(scpi);

                //errorCheck();
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine("sendSCPI(): " + scpi + ". Caught exception " + e.Message);
                return false;
            }
        }
        public byte[] readBinaryBlock()
        {

            DateTime oStart = DateTime.Now;

            // wait for the the # char.
            int iPoundChar = 0;
            do
            {
                iPoundChar = GGraw.Read(1)[0];
            }
            while (iPoundChar != 35);

            // Now read the number of digits that indicate the size
            int iDigitsForBlockSize = readAsInt(1);
            // Message.Generate("Digits in Block Size = " + iDigitsForBlockSize);

            // Now read the size of the block
            int iBlockSize = readAsInt(iDigitsForBlockSize);
            // Message.Generate("BlockSize = " + iBlockSize);

            // Read the Block with all the data
            int iChunkSize = 3 * 1024;
            int iWaitBetweenChuckReadsMs = 2;
            byte[] bBlock = new byte[iBlockSize];

            int iBlockIndex = 0;
            int iBlockSizeLeft = iBlockSize;
            while (iBlockSizeLeft > 0)
            {
                System.Threading.Thread.Sleep(iWaitBetweenChuckReadsMs);
                int iWantedReadSize = (iChunkSize < iBlockSizeLeft) ? iChunkSize : iBlockSizeLeft;
                // Message.Generate("iChunkSize = " + iChunkSize + " iBlockSizeLeft = " + iBlockSizeLeft);
                byte[] bChunk = GGraw.Read(iWantedReadSize);
                int iChunkSizeRead = bChunk.GetLength(0);
                iBlockSizeLeft -= iChunkSizeRead;
                for (int i = 0; i < iChunkSizeRead; i++)
                    bBlock[iBlockIndex++] = bChunk[i];
            }


            // Message.Generate("bBlock Size = " + bBlock.GetLength(0));
            if (bBlock.GetLength(0) != iBlockSize)
            {
                util.SaveLog(STAGENO, "readBinaryBlock: BlockSize is incoreect");
                Console.WriteLine("readBinaryBlock: BlockSize is incoreect");
            }

            //throw away a byte
            byte[] bThrowAway = GGraw.Read(1);
            TimeSpan oSpan = DateTime.Now - oStart;
            // Message.Generate("Time to read block(ms) = " + oSpan.Milliseconds);
            return bBlock;
        }
        private int readAsInt(int iSize)
        {
            byte[] bDigits = GGraw.Read(iSize);
            System.Text.ASCIIEncoding oAsciiEncoding = new System.Text.ASCIIEncoding();
            String sDigits = oAsciiEncoding.GetString(bDigits);
            int iDigits = Int32.Parse(sDigits);
            return iDigits;
        }
        public string RunCommand(string cmd)
        {
            string cmdResponse = string.Empty;
            try
            {
                util.SaveLog(STAGENO, "Send> " + cmd);
                if (GG != null)
                {
                    GG.WriteLine(cmd);
                    cmdResponse = GG.ReadLine();
                }
                else
                    util.SaveLog(STAGENO, "RunCommand Error : Keysight BT2200 is not connected!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                util.SaveLog(STAGENO, "Command Error (" + cmd + ") > " + ex.ToString());
            }

            if (cmdResponse != string.Empty)
                util.SaveLog(STAGENO, "Recv> " + cmdResponse.Replace("\n", ""));
            else
                util.SaveLog(STAGENO, "Recv> " + "No Resut.");
            return cmdResponse;

        }
        public double GetDoubleResult(string cmd)
        {
            string strResult = RunCommand(cmd);
            double d = Double.Parse(strResult);
            return d;
        }
        private void RunCommandOnly(string cmd)
        {
            try
            {
                util.SaveLog(STAGENO, "Send> " + cmd);
                if(GG != null)
                    GG.PrintfAndFlush(cmd);
                else
                    util.SaveLog(STAGENO, "RunCommandOnly Error : Keysight BT2200 is not connected!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                util.SaveLog(STAGENO, "Command Error (" + cmd + ") > " + ex.ToString());
            }
        }
        private void runRST()
        {
            CMD = "*RST";
            RunCommandOnly(CMD);
        }
        private void runABORT()
        {
            CMD = "SEQ:ABORT";
            RunCommandOnly(CMD);
        }
        private void runCLEAR()
        {
            CMD = "SEQ:CLEar 1";
            RunCommandOnly(CMD);
        }
        private void setDEFQuick()
        {
            CMD = "CELL:DEF:QUICk 1";
            RunCommandOnly(CMD);
        }
        private void setPROBELIMIT(int resistance)
        {
            //CMD = "SYST:PROB:LIM 2,0";
            CMD = "SYST:PROB:LIM " + resistance.ToString() + ",0";
            RunCommandOnly(CMD);
        }
        public bool errorCheck()
        {
            string errorMessage = string.Empty;
            try
            {
                util.SaveLog(STAGENO, "Send> SYST:ERR?");
                GG.WriteLine("SYST:ERR?");
                errorMessage = GG.ReadLine();
                util.SaveLog(STAGENO, "Recv> (SYST:ERR?) " + errorMessage.ToString());
            }
            catch (Exception e)
            {
                util.SaveLog(STAGENO, "errorCheck(SYST:ERR?): Caught exception " + e.ToString());
                //Console.WriteLine("errorCheck(): Caught exception " + e.Message);
            }

            if (errorMessage.Contains("No error"))
                return true;

            else
                return false;
        }
        public bool ConnectionCheck()
        {
            string strResult = Connect();
            if (strResult.Contains("Keysight Technologies")) return true;
            else return false;
        }
        public bool DeviceClear()
        {
            try
            {
                util.SaveLog(STAGENO, "Send> Device Clear : " + VISA_ADDRESS);
                session.Clear();
                return true;
            }
            catch (Exception ex)
            {
                util.SaveLog(STAGENO, "Device Clear Error : " + VISA_ADDRESS);
            }
            return false;
        }
        #endregion

        #region Step Definition
        public async Task<bool> CheckStepDefinition()
        {
            bool bPrecharge = false;
            bool bCharge = false;
            //* Precharge sequence -> 1, step -> 1
            //* Charge sequence -> 1, step -> 2
            string[] def_precharge = GetStepDefinition("1", "1");
            await Task.Delay(500);
            string[] def_charge = GetStepDefinition("1", "2");
            await Task.Delay(500);

            if (CheckPrechargeDefinition(def_precharge) == true && CheckChargeDefinition(def_charge) == true)
                return true;

            return false;
        }
        public bool CheckPrechargeDefinition(string[] def_values)
        {
            if (def_values[0] == "PRECHARGE" && Convert.ToDouble(def_values[1]) == PRETIME
                && Convert.ToDouble(def_values[2]) == PRECURRENT && Convert.ToDouble(def_values[3]) == PREVOLTAGE)
                return true;

            return false;
        }
        public bool CheckChargeDefinition(string[] def_values)
        {
            if (def_values[0] == "CHARGE" && Convert.ToDouble(def_values[1]) == TIME
                && Convert.ToDouble(def_values[2]) == CURRENT && Convert.ToDouble(def_values[3]) == VOLTAGE)
                return true;

            return false;
        }
        /// <summary>
        /// splitData[0] - type (PRECHARGE, CHARGE)
        /// splitData[1] - time
        /// splitData[2] - current
        /// splitData[3] - voltage
        /// </summary>
        public string[] GetStepDefinition(string seq_id, string step_id)
        {
            CMD = "SEQ:STEP:DEF? " + seq_id + "," + step_id;
            var strData = RunCommand(CMD);

            string[] splitData = strData.Split(',');
            return splitData;
        }
        private void SetStepDefinition(string type, string step_id, int time, double current, double voltage)
        {
            //* Ex Command : "SEQ:STEP:DEF 1,1,PRECHARGE,30,1.0,1.0";
            //* Ex Command : "SEQ:STEP:DEF 1,2,CHARGE,180,2.0,4.2";
            CMD = "SEQ:STEP:DEF 1," + step_id + "," + type + "," + time.ToString() + "," + current.ToString() + "," + voltage.ToString();
            RunCommandOnly(CMD);
        }
        public async Task SetStepDefinition()
        {
            runCLEAR();
            await Task.Delay(100);
            setPROBELIMIT(2);
            await Task.Delay(100);
            setDEFQuick();
            await Task.Delay(100);
            SetStepDefinition("PRECHARGE", "1", _preTime, _preCurrent, _preVoltage);
            await Task.Delay(200);
            SetStepDefinition("CHARGE", "2", _time, _current, _voltage);
            await Task.Delay(200);

            //* 전압으로 판단할 때 애매한 경우가 있음
            //            if (chkCondition.Checked == true)
            //            {
            //                SETCHARGECONDITION("2", "1", "VOLT_LE", voltCondition.ToString(), "BEFORE", "90");
            //            }
        }
        #endregion

        #region Charging
        public void StopCharging()
        {
            CMD = "SEQ:ABORT";
            RunCommandOnly(CMD);
        }
        public async Task<bool> StartCharging()
        {
            //SetEnable();
            //SetInit();

            string enableCMD = string.Empty;
            string initCMD = string.Empty;

            //for (int boardindex = 1; boardindex < 9; boardindex++)
            //{
            //    enableCMD = "CELL:ENABLE (@" + boardindex + "001:" + boardindex + "032),1";
            //    RunCommandOnly(enableCMD);

            //    initCMD = "CELL:INIT (@" + boardindex + "001:" + boardindex + "032)";
            //    RunCommandOnly(initCMD);
            //}

            enableCMD = "CELL:ENABLE (@1001:8032),1";
            RunCommandOnly(enableCMD);
            await Task.Delay(100);
            
            initCMD = "CELL:INIT (@1001:8032)";
            RunCommandOnly(initCMD);
            //await Task.Delay(1000);

            return true;
        }
        private async void SetEnable()
        {
            for(int boardindex = 1; boardindex < 9; boardindex++)
            {
                //* Ex Command : "CELL:ENABLE (@1001:1032),1";
                CMD = "CELL:ENABLE (@" + boardindex + "001:" + boardindex + "032),1";
                RunCommandOnly(CMD);
                await Task.Delay(100);
            }
        }

        private async void SetInit()
        {
            for(int boardindex = 1; boardindex < 9; boardindex++)
            {
                //* Ex Command : "CELL:INIT (@1001:1032)";
                CMD = "CELL:INIT (@" + boardindex + "001:" + boardindex + "032)";
                RunCommandOnly(CMD);
                await Task.Delay(500);
            }
        }
        #endregion

        #region Get Data Command
        public bool GetCellVerbose(int cellIndex)
        {
            string[] results;
            int iBoardno = cellIndex / 32 + 1;
            int iChannelno = cellIndex % 32;
            CMD = "STAT:CELL:VERBOSE? " + iBoardno.ToString() + "0" + iChannelno.ToString("D2");
            try
            {
                util.SaveLog(STAGENO, "Send> " + CMD);
                GG.WriteLine(CMD);
                string cmdResponse = GG.ReadLine();
                results = cmdResponse.Split(','); ;
                util.SaveLog(STAGENO, "Recv> " + cmdResponse);
                return true;
            }
            catch(Exception ex)
            {
                util.SaveLog(STAGENO, "GetCellVerbose Error : " + ex.ToString());
            }
            return false;
        }
        public string[] GetCellReport(string cmd)
        {
            string[] errorresult = new string[1];
            try
            {
                GG.WriteLine(cmd);
                string[] cmdResponse = GG.ReadLine().Split(',');
                return cmdResponse;
            }catch(Exception ex)
            {
                util.SaveLog(STAGENO, "GetCellReport Error. : " + ex.ToString());
            }

            return errorresult;
        }
        public bool GetCellReports(int BOARDCOUNT)
        {
            bool bCharging = false;
            string[] cmdResponse;
            string[] results = new string[BOARDCOUNT * 32];
            string strString = string.Empty;
            util.SaveLog(STAGENO, "SEND> STAT:CELL:REP? (@1001:" + BOARDCOUNT + "032)");
            for(int boardIndex = 0; boardIndex < BOARDCOUNT; boardIndex++)
            {
                CMD = "STAT:CELL:REP? (@" + (boardIndex + 1) + "001:" + (boardIndex + 1) + "032)";
                //GG.WriteLine(CMD);
                //cmdResponse = GG.ReadLine().Split(',');
                cmdResponse = GetCellReport(CMD);

                if (cmdResponse.Length == 32)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        if (cmdResponse[i].Trim() == "1") bCharging = true;
                        results[boardIndex * 32 + i] = cmdResponse[i];
                        strString += (boardIndex * 32 + i + 1).ToString("D3") + "-";
                        strString += cmdResponse[i] + "\t";
                    }
                }
                else
                {
                    for (int i = 0; i < 32; i++)
                    {
                        results[boardIndex * 32 + i] = "0";
                        strString += (boardIndex * 32 + i + 1).ToString("D3") + "-";
                        strString += "0\t";
                    }
                }
            }
            util.SaveLog(STAGENO, "RECV> " + strString.Replace("\n", ""));
            return bCharging;
        }
        public void ClearDataLog()
        {
            sendSCPI("DATA:LOG:CLE");
        }
        public double GetLogCount()
        {
            double logCount = GetDoubleResult("Data:log:records:available?");
            return logCount;
        }
        public GgDataLogNamespace.GgBinData GetDataLog()
        {
            List<GgDataLogNamespace.GgBinData> oDataLogQuery = null;

            CMD = "DATA:LOG?";
            string cmdResponse = string.Empty;
            byte[] ResultsArray = null;
            try
            {
                sendSCPI(CMD);
                byte[] bBlock = readBinaryBlock();

                oDataLogQuery = GgDataLogNamespace.DataLogQueryClass.dataLogQuery(bBlock);
                string strString = string.Empty;
                strString = "Recv (log:data?) [CellId-IMon-VSense-VLocal-Dcir1-Dcir2-SeqState]\n";
                for(int i = 0; i < oDataLogQuery.Count; i++)
                {
                    for(int cIndex = 0; cIndex < 256; cIndex++)
                    {
                        //strString += (cIndex + 1).ToString("D3") + "-";
                        strString += oDataLogQuery[i].CellId[cIndex] + "-";
                        //strString += oDataLogQuery[i].TimeStamp + "-";
                        strString += (oDataLogQuery[i].IMon[cIndex] * 1000.0).ToString("F3") + "-";
                        strString += (oDataLogQuery[i].VSense[cIndex] * 1000.0).ToString("F5") + "-";
                        strString += (oDataLogQuery[i].VLocal[cIndex] * 1000.0).ToString("F5") + "-";
                        strString += oDataLogQuery[i].Dcir1[cIndex] > 1000 ? "999-" : oDataLogQuery[i].Dcir1[cIndex] + "-";
                        strString += oDataLogQuery[i].Dcir2[cIndex] > 1000 ? "999-" : oDataLogQuery[i].Dcir2[cIndex] + "-";
                        strString += oDataLogQuery[i].SequenceState[cIndex] + "\t";
                    }
                    util.SaveLog(STAGENO, strString);
                    strString = string.Empty;
                }
                return oDataLogQuery[0];
            }
            catch (Exception ex)
            {
                util.SaveLog(STAGENO, "GetDataLog Error > " + ex.ToString());
                Console.WriteLine(ex.ToString());
                sendSCPI("DATA:LOG:CLE");
            }

            return null;
        }
        private string GetVerbose(string channel)
        {
            //CMD = "STAT:CELL:VERBose? 1001";
            CMD = "STAT:CELL:VERBose? " + channel;
                //# Returns IDLE|RUNNING,<seq>,<step>,<Vsense>,<Imon>,NONE|OK|FAIL|ABORTED,<testId>,<testType>,<expected limit>,<measured limit>
            string[] result = RunCommand(CMD).Split(',');
            if (result.Length > 5)
                return result[0] + "," + result[3] + "," + result[4] + "," + result[5];
            else
                return ",,,";
        }
        public async void GetMeas()
        {
            string[] cmdResponse;
            string strString = string.Empty;
            try
            {
                CMD = "MEAS:VOLT? 0000";
                util.SaveLog(STAGENO, "Send> " + CMD);
                GG.WriteLine(CMD);
                cmdResponse = GG.ReadLine().Split(',');
                for (int i = 0; i < cmdResponse.Length; i++)
                    strString += i.ToString("D3") + "-" + cmdResponse[i];
                util.SaveLog(STAGENO, "Recv> " + strString);
                await Task.Delay(100);

                CMD = "MEAS:CURR? 0000";
                util.SaveLog(STAGENO, "Send> " + CMD);
                GG.WriteLine(CMD);
                cmdResponse = GG.ReadLine().Split(',');
                for (int i = 0; i < cmdResponse.Length; i++)
                    strString += i.ToString("D3") + "-" + cmdResponse[i];
                util.SaveLog(STAGENO, "Recv> " + strString);
            }
            catch (Exception ex)
            {
                util.SaveLog(STAGENO, "GetCellReport Error. : " + ex.ToString());
            }

        }
        public string[] GetVoltage()
        {
            //Voltage
            string strString = string.Empty;
            CMD = "MEAS:VOLT? 0000";
            string[] results = RunCommand(CMD).Split(',');
            
            for (int i = 0; i < results.Length; i++)
                strString += (i+1).ToString("D3") + "-" + results[i] + "\t";
            util.SaveLog(STAGENO, "Recv> " + strString);
            return results;
        }
        public string[] GetCurrent()
        {
            //Current
            string strString = string.Empty;
            CMD = "MEAS:CURR? 0000";
            string[] results = RunCommand(CMD).Split(',');
            
            for (int i = 0; i < results.Length; i++)
                strString += (i + 1).ToString("D3") + "-" + results[i] + "\t";
            util.SaveLog(STAGENO, "Recv> " + strString);
            return results;
        }
        #endregion

    }
}
