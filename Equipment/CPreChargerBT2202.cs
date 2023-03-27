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
    public class CPreChargerBT2202
    {
        Util util = new Util();
        public string VISA_ADDRESS;
        private ResourceManager resourceManager;
        //private FormattedIO488Class ioObject;
        private FormattedIO488 ioObject;
        private string IPADDRESS = string.Empty;
        private string PORT = string.Empty;
        private string _voltage;
        private string _current;
        private string _time;
        private int STAGENO = 0;
        private bool _ConnectionState;
        private bool _bAutoMode = false;
        protected enumEquipStatus _EquipStatus = enumEquipStatus.StepVacancy;
        public bool AUTOMODE { get => _bAutoMode; set => _bAutoMode = value; }
        public bool ConnectionState { get => _ConnectionState; set => _ConnectionState = value; }
        public string Voltage { get => _voltage; set => _voltage = value; }
        public string Current { get => _current; set => _current = value; }
        public string Time { get => _time; set => _time = value; }
        public enumEquipStatus EQUIPSTATUS { get => _EquipStatus; set => _EquipStatus = value; }

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
        private static CPreChargerBT2202[] PrechargerDriver = new CPreChargerBT2202[_Constant.frmCount];

        public static CPreChargerBT2202 GetInstance(int nIndex)
        {
            if (PrechargerDriver[nIndex] == null) PrechargerDriver[nIndex] = new CPreChargerBT2202();
            return PrechargerDriver[nIndex];
        }
        #endregion
        public CPreChargerBT2202()
        {
           // _tmrReconnect.Elapsed += new System.Timers.ElapsedEventHandler(_tmrReconnect_Elapsed);
            //_tmrReconnect.Enabled = true;
        }
        public CPreChargerBT2202(string ipaddress, string port, int stageno)
        {
            STAGENO = stageno;
            IPADDRESS = ipaddress;
            PORT = port;
            VISA_ADDRESS = "TCPIP::" + IPADDRESS + "::" + PORT + "::SOCKET";
            resourceManager = new ResourceManager();
            ioObject = new FormattedIO488();
            ioObject.IO = (IMessage)resourceManager.Open(VISA_ADDRESS, AccessMode.NO_LOCK, 0, "");


            if (connect().Contains("Keysight Technologies")) _ConnectionState = true;
            else _ConnectionState = false;
        }

        public bool Connected
        {
            get { return ConnectionState == true; }
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
            }
            catch(Exception ex) { }

            if (connect().Contains("Keysight Technologies")) _ConnectionState = true;
            else _ConnectionState = false;
        }
        private string connect()
        {
            string idnResponse = string.Empty;
            try
            {
                ioObject.WriteString("*IDN?");
                idnResponse = ioObject.ReadString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return idnResponse;
        }
        public void disconnect()
        {
            try
            {
                ioObject.IO.Close();
            }
            catch { }
            try
            {
#pragma warning disable CA1416 // 플랫폼 호환성 유효성 검사
                Marshal.ReleaseComObject(ioObject);
#pragma warning restore CA1416 // 플랫폼 호환성 유효성 검사
            }
            catch { }
        }
        public void SETPARAMETER(string voltage, string current, string time)
        {
            _voltage = voltage;
            _current = current;
            _time = time;
        }
        private string RUNCOMMAND(string cmd)
        {
            string cmdResponse = string.Empty;
            try
            { 
                if (ioObject != null)
                {
                    ioObject.WriteString(cmd, true);
                    
                    util.SaveLog(STAGENO, "Send> " + cmd);
                    cmdResponse = ioObject.ReadString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if(cmdResponse != string.Empty)
                util.SaveLog(STAGENO, "Recv> " + cmdResponse);
            return cmdResponse;

        }
        //* RUNCOMMANDWITHNORESULT
        private void RUNCOMMANDONLY(string cmd)
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
            }
        }

        #region BT2202A COMMANDS
        string CMD = string.Empty;
        private void runRST()
        {
            CMD = "*RST";
            RUNCOMMANDONLY(CMD);
        }
        private void runABORT()
        {
            CMD = "SEQ:ABORT";
            RUNCOMMANDONLY(CMD);
        }
        private void runCLEAR()
        {
            CMD = "SEQ:CLEar 1";
            RUNCOMMANDONLY(CMD);
        }
        private void setDEFQuick()
        {
            CMD = "CELL:DEF:QUICk 1";
            RUNCOMMANDONLY(CMD);
        }
        private void setPROBELIMIT(string resistance)
        {
            //CMD = "SYST:PROB:LIM 2,0";
            CMD = "SYST:PROB:LIM " + resistance + ",0";
            RUNCOMMANDONLY(CMD);
        }
        private bool CHECKSTEPDEFINITION(string seq_id, string step_id, string voltage, string current, string time)
        {
            CMD = "SEQ:STEP:DEF ? " + seq_id + "," + step_id;
            string[] results = RUNCOMMAND(CMD).Split(',');
            //0 : type, 1 : time, 2 : current, 3 : voltage
            if (step_id == "1" && results[0] == "PRECHARGE" && results[1] == time && results[2] == current && results[3] == voltage)
                return true;
            else if (step_id == "2" && results[0] == "CHARGE" && results[1] == time && results[2] == current && results[3] == voltage)
                return true;
            else
                return false;
        }
        private void SETCHARGE(string command, string step_id, string voltage, string current, string time)
        {
            CMD = "SEQ:STEP:DEF 1," + step_id + "," + command + "," + time + "," + current + "," + voltage;
            //Command = "SEQ:STEP:DEF 1,1,CHARGE,30,2.0000,4.200";
            RUNCOMMANDONLY(CMD);
        }
        private void SETCHARGECONDITION(string step_id, string test_id, string condition1, string condition2, string ba, string time)
        {
            CMD = "SEQ:TEST:DEF 1," + step_id + "," + test_id + "," + condition1 + "," + condition2 + "," + ba + "," + time + ", FAIL";
            RUNCOMMANDONLY(CMD);
        }
        private void SETPRECHARGE()
        {
            runCLEAR();
            setDEFQuick();
            SETCHARGE("PRECHARGE", "1", _voltage, _current, _time);
            SETCHARGE("CHARGE", "2", _voltage, _current, _time);

            //* 전압으로 판단할 때 애매한 경우가 있음
//            if (chkCondition.Checked == true)
//            {
//                SETCHARGECONDITION("2", "1", "VOLT_LE", voltCondition.ToString(), "BEFORE", "90");
//            }
        }
        private void STARTCHARGING()
        {
            SETENABLE();
            SETINIT();
        }
        private void SETENABLE()
        {
            for(int boardindex = 1; boardindex < 9; boardindex++)
            {
                CMD = "CELL:ENABLE (@" + boardindex + "001:" + boardindex + "032),1";
                RUNCOMMANDONLY(CMD);
            }

            //CMD = "CELL:ENABLE (@1001:1032),1";
            //RUNCOMMAND(CMD);
        }

        private void SETINIT()
        {
            for(int boardindex = 1; boardindex < 9; boardindex++)
            {
                CMD = "CELL:INIT (@" + boardindex + "001:" + boardindex + "032)";
                RUNCOMMANDONLY(CMD);
            }

            //CMD = "CELL:INIT (@1001:1032)";
            //RUNCOMMAND(CMD);
        }
        private string GETVERBOSE(string channel)
        {
            //CMD = "STAT:CELL:VERBose? 1001";
            CMD = "STAT:CELL:VERBose? " + channel;
                //# Returns IDLE|RUNNING,<seq>,<step>,<Vsense>,<Imon>,NONE|OK|FAIL|ABORTED,<testId>,<testType>,<expected limit>,<measured limit>
            string[] result = RUNCOMMAND(CMD).Split(',');
            if (result.Length > 5)
                return result[0] + "," + result[3] + "," + result[4] + "," + result[5];
            else
                return ",,,";
        }
        private string GETVOLTAGE()
        {
            //Voltage
            CMD = "MEAS:VOLT? 0000";
            return RUNCOMMAND(CMD);
        }
        private string GETCURRENT()
        {
            //Current
            CMD = "MEAS:CURR? 0000";
            return RUNCOMMAND(CMD);
        }
        #endregion
    }
}
