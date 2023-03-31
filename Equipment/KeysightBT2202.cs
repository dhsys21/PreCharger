using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ivi.Visa.Interop;
using PreCharger.Common;

namespace PreCharger
{
    public class KeysightBT2202
    {
        Util util = new Util();
        public string VISA_ADDRESS;
        private ResourceManager resourceManager;
        //private FormattedIO488Class ioObject;
        private FormattedIO488 ioObject;
        private string IPADDRESS = string.Empty;
        private string PORT = string.Empty;
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

        public void Open(string ipaddress, string port, int stageno)
        {
            STAGENO = stageno;
            IPADDRESS = ipaddress;
            PORT = port;
            VISA_ADDRESS = "TCPIP::" + IPADDRESS + "::" + PORT + "::SOCKET";
            resourceManager = new ResourceManager();
            ioObject = new FormattedIO488();
            try
            {
                ioObject.IO = (IMessage)resourceManager.Open(VISA_ADDRESS, AccessMode.NO_LOCK, 0, "");
                ioObject.IO.Timeout = 5 * 1000;
            }
            catch (Exception ex) { }

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
            try
            {
                ioObject.IO.Close();
            }
            catch { }
            try
            {
                Marshal.ReleaseComObject(ioObject);
            }
            catch { }
        }

        public void SetChargeParameter(int time, double current, double voltage)
        {
            _voltage = (voltage / 1000.0);
            _current = (current / 1000.0);
            _time = time;
        }
        public void SetPrechargeParameter(int time, double current, double voltage)
        {
            _preVoltage = voltage;
            _preCurrent = current;
            _preTime = time;
        }

        #region BT2202A COMMANDS
        string CMD = string.Empty;
        public string RunCommand(string cmd)
        {
            string cmdResponse = string.Empty;
            try
            {
                if (ioObject != null)
                {
                    //ioObject.WriteString(cmd, true);
                    util.SaveLog(STAGENO, "Send> " + cmd);
                    ioObject.WriteString(cmd);
                    cmdResponse = ioObject.ReadString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                util.SaveLog(STAGENO, "Command Error (" + cmd + ") > " + ex.ToString());
            }

            if (cmdResponse != string.Empty)
                util.SaveLog(STAGENO, "Recv> " + cmdResponse);
            return cmdResponse;

        }
        private void RunCommandOnly(string cmd)
        {
            try
            {
                if (ioObject != null)
                {
                    ioObject.WriteString(cmd, true);

                    util.SaveLog(STAGENO, "Send> " + cmd);
                }
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
        private void setPROBELIMIT(string resistance)
        {
            //CMD = "SYST:PROB:LIM 2,0";
            CMD = "SYST:PROB:LIM " + resistance + ",0";
            RunCommandOnly(CMD);
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
            await Task.Delay(500);
            setDEFQuick();
            await Task.Delay(500);
            SetStepDefinition("PRECHARGE", "1", _preTime, _preCurrent, _preVoltage);
            await Task.Delay(500);
            SetStepDefinition("CHARGE", "2", _time, _current, _voltage);
            await Task.Delay(500);

            //* 전압으로 판단할 때 애매한 경우가 있음
            //            if (chkCondition.Checked == true)
            //            {
            //                SETCHARGECONDITION("2", "1", "VOLT_LE", voltCondition.ToString(), "BEFORE", "90");
            //            }
        }
        #endregion

        #region Charging
        public bool StartCharging()
        {
            SetEnable();
            SetInit();

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
        public byte[] GetDataLog()
        {
            CMD = "DATA:LOG?";
            string cmdResponse = string.Empty;
            try
            {
                if (ioObject != null)
                {
                    ioObject.WriteString(CMD, true);

                    util.SaveLog(STAGENO, "Send> " + CMD);
                    //cmdResponse = ioObject.ReadString();

                    byte[] header = ioObject.IO.Read(11);
                    Int32 dataCount = Int32.Parse(System.Text.Encoding.ASCII.GetString(header).Substring(2));

                    byte[] ResultsArray = ioObject.IO.Read(dataCount);

                    return ResultsArray;
                }
            }
            catch (Exception ex)
            {
                util.SaveLog(STAGENO, "GetDataLog Error > " + ex.ToString());
                Console.WriteLine(ex.ToString());
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
        private string[] GetVoltage()
        {
            //Voltage
            CMD = "MEAS:VOLT? 0000";
            string[] results = RunCommand(CMD).Split(',');
            return results;
        }
        private string[] GetCurrent()
        {
            //Current
            CMD = "MEAS:CURR? 0000";
            string[] results = RunCommand(CMD).Split(',');
            return results;
        }
        #endregion

    }
}
