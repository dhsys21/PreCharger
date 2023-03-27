using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PreCharger.Common;

namespace PreCharger
{
    public class CPrechargerDriver : CSocketDriver
    {
        private bool _bAutoMode = false;
        public bool AUTOMODE { get => _bAutoMode; set => _bAutoMode = value; }

        private int _stage = 0;
        public int STAGE{ get => _stage; set => _stage = value; }
        protected enumEquipStatus _EquipStatus = enumEquipStatus.StepVacancy;
        public enumEquipStatus EQUIPSTATUS { get => _EquipStatus; set => _EquipStatus = value; }
        string _strBuffData = string.Empty;
        string STX = string.Format("{0}", (char)2);
        string ETX = string.Format("{0}", (char)3);

        Util util = new Util();

        #region GetInstance()
        private static CPrechargerDriver[] PrechargerDriver = new CPrechargerDriver[_Constant.frmCount];

        public static CPrechargerDriver GetInstance(int nIndex)
        {
            if (PrechargerDriver[nIndex] == null) PrechargerDriver[nIndex] = new CPrechargerDriver();
            return PrechargerDriver[nIndex];
        }
        #endregion

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

        public CPrechargerDriver()
        {

        }
        public CPrechargerDriver(string _strIP, int _iPort, string _strType)
            : base()
        {
            //_Logger.Log(Level.Info, "********** LabelPrintDriver Initialize Start **********");
            //_Driver = new CMelsecNetDriver();
            //_Driver.OnConnectionStateChanged += _Driver_OnConnectionStateChanged;

            //_EndAddress = _PRE.PLC_D_LEN * 2;
            //OldTrigger = new ushort[_EndAddress];


            ////Channel_N0, Mode, Station_No
            //int iRet = _Driver.Open(iCHANNEL_NO, iMODE, iSTATION_NO);

            //if (iRet == 0)
            //{
            //    StartScaning();
            //}
            Open(_strIP, _iPort, _strType);

        }

        ~CPrechargerDriver()
        {
            CloseSocket();
        }

        public override int Send(string strMessage)
        {
           // _Logger.Log(Level.Info, "LabelPrint Message Send : " + strMessage);
            return base.Send(strMessage);
        }

        public void StopTimer()
        {
            StopReconnectTimer();
        }

        public void Close()
        {
            CloseSocket();
        }

        public void Open(string _strIP, int _iPort, string _strType)
        {
            try
            {
                InitConnectionString(_strIP, _iPort, _strType);
                int iRet = 0;
                iRet = OpenSocketPort();

                

                if (iRet != 0)
                {
                    //_Logger.Log(Level.Info, "********** LabelPrintDriver Open Error **********");
                }
                else
                {
                    //BaseForm.frmMain.timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
               // _Logger.Log(Level.Exception, "Label Print Driver Open Fail!!! : " + ex.ToString());
            }
        }

        public void PreChargerDoStart()
        {
            string sendCommand = MakePreChargerCommand("AMS", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerDoFinish()
        {
            string sendCommand = MakePreChargerCommand("AMF", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerDoSet(string param)
        {
            string sendCommand = MakePreChargerCommand("SET", param);
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerDoSen()
        {
            string sendCommand = MakePreChargerCommand("SEN", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerDoMonitoring()
        {
            string sendCommand = MakePreChargerCommand("MON", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerSetManual()
        {
            string sendCommand = MakePreChargerCommand("MAN", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        public void PreChargerSetAuto()
        {
            string sendCommand = MakePreChargerCommand("AUT", "");
            Send(sendCommand);
            util.SaveLog(this._stage, sendCommand, "TX");
        }

        protected override int ParseMessage(string strMessage)
        {
            string a = string.Empty;
            a = strMessage;
            //Format: $,< UNo > (,< SeqNo >),< Sts >,< Ackcd >,< Command >,< Parameter >,< Value > (,< Sum >)< CR >
            try
            {
                //_Logger.Log(Level.Info, "**** ParseMessage  **** " + strMessage);

                _strBuffData = _strBuffData + strMessage;

                int nlengthEtx = 0;
                int nlengthStx = 0;
                int socket_len = 0;
                string strData = string.Empty;
                string _strError = string.Empty;

                if (_strBuffData.Length > 2859) _strBuffData = _strBuffData.Substring(0, 2859);

                nlengthEtx = _strBuffData.IndexOf(""); //Etx = CR(\r)
                nlengthStx = _strBuffData.IndexOf(""); //Stx = ID 수정필요

                 
                //if (nlengthEtx >= 0)
                //{
                    if (nlengthStx >= 0)
                    {
                        strData = _strBuffData;//.Substring(nlengthStx + 1, nlengthEtx - 1);
                        if (strData.Substring(0, 1) == STX)
                        {
                            socket_len = Convert.ToInt32(strData.Substring(8, 5)) + 14;
                            if (strData.Substring(socket_len - 1, 1) == ETX)
                            {
                                _strBuffData = string.Empty;// _strBuffData.Substring(nlengthEtx + 1);
                                RaiseOnReceived(false, strData, _stage);
                            }
                        }
                    }
                //}
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void RunCommand(string cmd)//, string param)
        {
            //Send(MakeCommand(cmd, param));
            Send(cmd);
        }

        public string MakePreChargerCommand(string cmd, string param)
        {
            string command = "";
            if(cmd == "AMF" || cmd == "AMS" || cmd == "SEN" || cmd == "MON" || cmd == "AUT" || cmd == "MAN")
            {
                command = STX + "1" + cmd + "00100000" + ETX;
            }
            else if(cmd == "SET")
            {
                command = STX + "1" + cmd + "001" + Convert.ToInt32(param.Length).ToString("D5") + param + ETX;
            }

            return command;
        }
    }
}
