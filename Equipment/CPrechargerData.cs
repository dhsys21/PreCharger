﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using PreCharger.Common;
using System.Drawing;

namespace PreCharger
{
    public class CPrechargerData
    {
        Util util = new Util();
        CEquipmentData _system = CEquipmentData.GetInstance();

        private static CPrechargerData[] prechargerdata = new CPrechargerData[_Constant.frmCount];
        public static CPrechargerData GetInstance(int nIndex)
        {
            if (prechargerdata[nIndex] == null) prechargerdata[nIndex] = new CPrechargerData();
            return prechargerdata[nIndex];
        }

        #region precharger data
        private int _iStageNo;
        private string _sStageStatus;
        private int _iStepIndex;
        private int _iStepTime;
        private int _iTestTime;
        private int _iDataTime;
        private string[] _sVolt = new string[256];
        private string[] _sCurr = new string[256];
        private string[] _sCapa = new string[256];
        private string[] _sCode = new string[256];
        private Color[] _clrChannelColor = new Color[256];
        private string[] _sErrorMessage = new string[256];
        private bool[] _bMeasureResult = new bool[256];
        private int[] _iErrorTimeCount = new int[256];
        private string[] _sCellSerial = new string[256];
        private bool[] _bCell = new bool[256];

        private bool _bRst;
        private bool _bSet;
        private bool _bAms;
        private bool _bAmf;
        private bool _bTrayin;
        private bool _bCharging;
        private bool _bEndCharging;
        private string _sTrayId;
        private int _iCellCount;
        private string _sMdlName;
        private string _sBatchId;
        private string _sLotId;
        private string _sCellModel;
        private string _sLotNumber;
        private DateTime _dtArriveTime;
        private DateTime _dtFinishTime;
        #endregion

        #region SETTING VALUEs
        private string _sSetVoltage;
        private string _sSetCurrent;
        private string _sSetTime;
        private string _sParam;
        private double _dSetVoltage;
        private double _dSetCurrent;
        private int _iSetTime;
        public string SETVOLTAGE { get => _sSetVoltage; set => _sSetVoltage = value; }
        public string SETCURRENT { get => _sSetCurrent; set => _sSetCurrent = value; }
        public string SETTIME { get => _sSetTime; set => _sSetTime = value; }
        public string PARAM { get => _sParam; set => _sParam = value; }
        public double DSETVOLTAGE { get => _dSetVoltage; set => _dSetVoltage = value; }
        public double DSETCURRENT { get => _dSetCurrent; set => _dSetCurrent = value; }
        public int ISETTIME { get => _iSetTime; set => _iSetTime = value; }
        #endregion

        public int STAGENO { get => _iStageNo; set => _iStageNo = value; }
        public string STAGESTATUS { get => _sStageStatus; set => _sStageStatus = value; }
        public int STEPINDEX { get => _iStepIndex; set => _iStepIndex = value; }
        
        public int STEPTIME { get => _iStepTime; set => _iStepTime = value; }
        public int TESTTIME { get => _iTestTime; set => _iTestTime = value; }
        public int DATATIME { get => _iDataTime; set => _iDataTime = value; }
        public string[] VOLT { get => _sVolt; set => _sVolt = value; }
        public string[] CURR { get => _sCurr; set => _sCurr = value; }
        public string[] CAPA { get => _sCapa; set => _sCapa = value; }
        public string[] CODE { get => _sCode; set => _sCode = value; }
        public bool RST { get => _bRst; set => _bRst = value; }
        public bool SET { get => _bSet; set => _bSet = value; }
        public bool AMS { get => _bAms; set => _bAms = value; }
        public bool AMF { get => _bAmf; set => _bAmf = value; }
        public bool TRAYIN { get => _bTrayin; set => _bTrayin = value; }
        public bool CHARGING { get => _bCharging; set => _bCharging = value; }
        public bool ENDCHARGING { get => _bEndCharging; set => _bEndCharging = value; }
        public string TRAYID { get => _sTrayId; set => _sTrayId = value; }
        public bool[] CELL { get => _bCell; set => _bCell = value; }

        public Color[] CHANNELCOLOR { get => _clrChannelColor; set => _clrChannelColor = value; }

        public string[] ERRORMSG { get => _sErrorMessage; set => _sErrorMessage = value; }
        public int CELLCOUNT { get => _iCellCount; set => _iCellCount = value; }
        public bool[] MEASURERESULT { get => _bMeasureResult; set => _bMeasureResult = value; }
        public int[] ERRORTIMECOUNT { get => _iErrorTimeCount; set => _iErrorTimeCount = value; }
        public string[] CELLSERIAL { get => _sCellSerial; set => _sCellSerial = value; }
        public string MDLNAME { get => _sMdlName; set => _sMdlName = value; }
        public string BATCHID { get => _sBatchId; set => _sBatchId = value; }
        public string LOTID { get => _sLotId; set => _sLotId = value; }
        public string CELLMODEL { get => _sCellModel; set => _sCellModel = value; }
        public string LOTNUMBER { get => _sLotNumber; set => _sLotNumber = value; }

        public DateTime FINISHTIME { get => _dtFinishTime; set => _dtFinishTime = value; }
        public DateTime ARRIVETIME { get => _dtArriveTime; set => _dtArriveTime = value; }

        


        #region PreCharger / Channel Status
        protected enumPrechargerStatus _PrechargerStatus = enumPrechargerStatus.Stop;
        public enumPrechargerStatus PrechargerStatus
        {
            get { return _PrechargerStatus; }
        }

        protected enumChannelStatus[] _ChannelStatus = new enumChannelStatus[256];
        public enumChannelStatus[] ChannelStatus
        {
            get { return _ChannelStatus; }
        }

        #endregion

        #region Delegate
        public delegate void delegateMainDisplayTestTime(int stageno, string sStep, string sTesttime);
        public event delegateMainDisplayTestTime OnDisplayTestTime = null;
        protected void RaiseOnDisplayTestTime(int stageno, string sStep, string sTesttime)
        {
            if (OnDisplayTestTime != null)
            {
                OnDisplayTestTime(stageno, sStep, sTesttime);
            }
        }
        #endregion

        public void InitData(int stageno)
        {
            _iStageNo = stageno;

            _bAmf = false;
            _bAms = false;
            _bRst = false;
            _bSet = false;
            _bEndCharging = false;
            _bCharging = false;

            _sStageStatus = string.Empty;
            _iTestTime = 0;
            _iStepIndex = 0;
            _iStepTime = 0;
            _iDataTime = 0;

            System.Array.Clear(_sCode, 0, 256);
            //System.Array.Clear(_sVolt, 0, 256);
            //System.Array.Clear(_sCurr, 0, 256);
            System.Array.Clear(_sCapa, 0, 256);

            _sTrayId = string.Empty;
            _bTrayin = false;
            _iCellCount = 0;
            System.Array.Clear(_bCell, 0, 256);
            System.Array.Clear(_sCellSerial, 0, 256);
            System.Array.Clear(_iErrorTimeCount, 0, 256);
            System.Array.Clear(_bMeasureResult, 0, 256);
            _sMdlName = string.Empty;
            _sBatchId = string.Empty;
            _sLotId = string.Empty;
            _sCellModel = string.Empty;
            _sLotNumber = string.Empty;

            for (int nIndex = 0; nIndex < 256; nIndex++)
            {
                _clrChannelColor[nIndex] = _Constant.ColorReady;
                _sErrorMessage[nIndex] = string.Empty;
                _sVolt[nIndex] = "0";
                _sCurr[nIndex] = "0";
            }

        }

        private void OnSatgeSatusChanged(string status)
        {
            switch (status)
            {
                case"0x00":
                    _PrechargerStatus = enumPrechargerStatus.Stop;
                    break;
                case"0x01":
                    _PrechargerStatus = enumPrechargerStatus.Contact;
                    break;
                case"0x02":
                    _PrechargerStatus = enumPrechargerStatus.ContactFinished;
                    break;
                case"0x03":
                    _PrechargerStatus = enumPrechargerStatus.Charging;
                    break;
                case"0x04":
                    _PrechargerStatus = enumPrechargerStatus.Discharging;
                    break;
                case"0x05":
                    _PrechargerStatus = enumPrechargerStatus.Rest;
                    break;
                case"0x06":
                    _PrechargerStatus = enumPrechargerStatus.StepFinished;
                    break;
                case"0x07":
                    _PrechargerStatus = enumPrechargerStatus.Calibration;
                    break;
                case"0x08":
                    _PrechargerStatus = enumPrechargerStatus.Ready;
                    break;
                case"0x10":
                    _PrechargerStatus = enumPrechargerStatus.CMuxComm;
                    break;
                case"0x20":
                    _PrechargerStatus = enumPrechargerStatus.CMuxStatus;
                    break;
                case"0x30":
                    _PrechargerStatus = enumPrechargerStatus.PowerOVP;
                    break;
                case"0x40":
                    _PrechargerStatus = enumPrechargerStatus.PowerUVP;
                    break;
                case"0x50":
                    _PrechargerStatus = enumPrechargerStatus.DTrip;
                    break;
                default:
                    break;
            }
        }

        private void OnChannelSatusChanged(int nIndex, string status)
        {
            _sErrorMessage[nIndex] = string.Empty;
            switch (status)
            {
                case "0x00":
                    _ChannelStatus[nIndex] = enumChannelStatus.OK;
                    break;
                case "0x03":
                    _ChannelStatus[nIndex] = enumChannelStatus.OverCharge;
                    break;
                case "0x04":
                    _ChannelStatus[nIndex] = enumChannelStatus.FPNG;
                    break;
                case "0x05":
                    _ChannelStatus[nIndex] = enumChannelStatus.NoCell;
                    if (_bCell[nIndex] == true) _clrChannelColor[nIndex] = _Constant.ColorError;
                    else _clrChannelColor[nIndex] = _Constant.ColorNoCell;
                    _sErrorMessage[nIndex] = "NO CELL";
                    break;
                case "0x06":
                    _ChannelStatus[nIndex] = enumChannelStatus.Disable;
                    _sErrorMessage[nIndex] = "DISABLE";
                    break;
                case "0x30":
                    _ChannelStatus[nIndex] = enumChannelStatus.Stop;
                    if (_bCell[nIndex] == true) _clrChannelColor[nIndex] = _Constant.ColorReady;
                    else _clrChannelColor[nIndex] = _Constant.ColorNoCell;
                    break;
                case "0x31":
                    _ChannelStatus[nIndex] = enumChannelStatus.Charging;
                    if (_bCell[nIndex] == false) _clrChannelColor[nIndex] = _Constant.ColorFlow;
                    else if (double.Parse(_sVolt[nIndex]) == 0) _clrChannelColor[nIndex] = _Constant.ColorError;
                    else _clrChannelColor[nIndex] = _Constant.ColorCharging;
                    break;
                case "0x32":
                    _ChannelStatus[nIndex] = enumChannelStatus.Discharging;
                    break;
                case "0x33":
                    _ChannelStatus[nIndex] = enumChannelStatus.Rest;
                    break;
                case "0x40":
                    _ChannelStatus[nIndex] = enumChannelStatus.NoStatue;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "COMM";
                    break;
                case "0x41":
                    _ChannelStatus[nIndex] = enumChannelStatus.OCP;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "OCP";
                    break;
                case "0x42":
                    _ChannelStatus[nIndex] = enumChannelStatus.PowerOVP;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "OVP";
                    break;
                case "0x43":
                    _ChannelStatus[nIndex] = enumChannelStatus.PowerUVP;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    break;
                case "0x44":
                    _ChannelStatus[nIndex] = enumChannelStatus.PowerOTP;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "OTP";
                    break;
                case "0x45":
                    _ChannelStatus[nIndex] = enumChannelStatus.SensingOverRange;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    break;
                case "0x46":
                    _ChannelStatus[nIndex] = enumChannelStatus.SensingNoRef;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "SNR";
                    break;
                case "0x47":
                    _ChannelStatus[nIndex] = enumChannelStatus.SensingTimeOut;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "STO";
                    break;
                case "0x48":
                    _ChannelStatus[nIndex] = enumChannelStatus.DCUNErr;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "DCUN";
                    break;
                case "0xD0":
                    _ChannelStatus[nIndex] = enumChannelStatus.LimitVoltage;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "LIMIT";
                    break;
                case "0xD9":
                    _ChannelStatus[nIndex] = enumChannelStatus.UpperCurrent;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "UC";
                    break;
                case "0xDA":
                    _ChannelStatus[nIndex] = enumChannelStatus.LowerCurrent;
                    _clrChannelColor[nIndex] = _Constant.ColorError;
                    _sErrorMessage[nIndex] = "LC";
                    break;
                default:
                    break;
            }
        }
        public void SetTrayInfo(bool[] bExist)
        {
            for (int i = 0; i < bExist.Length; i++)
                _bCell[i] = bExist[i];
        }
        public void SetTrayInfo(int iChannel, bool bExist)
        {
            _bCell[iChannel] = bExist;
        }
        public void SetDataLog(GgDataLogNamespace.GgBinData oDataLogQuery)
        {
            for (int cIndex = 0; cIndex < 256; cIndex++)
            {
                _sCurr[cIndex] = (oDataLogQuery.IMon[cIndex] * 1000.0).ToString("F1");
                _sVolt[cIndex] = (oDataLogQuery.VSense[cIndex] * 1000.0).ToString("F2");
            }
        }
        public void SetVoltages(double[] voltages)
        {
            for(int cIndex = 0; cIndex < 256; cIndex++)
            {
                _sVolt[cIndex] = (voltages[cIndex] * 1000.0).ToString("F1");
            }
        }
        public void SetCurrents(double[] currents)
        {
            for (int cIndex = 0; cIndex < 256; cIndex++)
            {
                _sCurr[cIndex] = (currents[cIndex] * 1000.0).ToString("F2");
            }
        }

        public void SetStatusData(string sData)
        {
            string param = "";
            param = sData.Remove(0, 13);
            
            //* default 값. _sSetTime 이 null 일 경우 대비

            _sStageStatus = "0x" + String.Format("{0:X2}", Convert.ToInt32(param.Substring(0, 2)));
            OnSatgeSatusChanged(_sStageStatus);
            _iTestTime = Convert.ToInt32(param.Substring(2, 8));
            _iStepTime = Convert.ToInt32(param.Substring(10, 6));
            _iStepIndex = Convert.ToInt32(param.Substring(16, 3));

            string sTestTime = _iTestTime / 60 + " : " + _iTestTime % 60;
            RaiseOnDisplayTestTime(this._iStageNo, GetPreChargerStatus(_sStageStatus), sTestTime);

            if (_bAmf == false && _bCharging == true
                && ((_PrechargerStatus == enumPrechargerStatus.StepFinished  /*_sStageStatus == "06" && _iTestTime > iChargingTime*/)
                || _bEndCharging == true
                || (_PrechargerStatus == enumPrechargerStatus.Stop /* _sStageStatus == "00"*/ && _iTestTime > _system.ITime && _bAms == true)))
            {
                _bCharging = false;
                _bEndCharging = true;
                _bAmf = true;
                _bAms = false;
            }
            else if(_PrechargerStatus == enumPrechargerStatus.Charging /*_sStageStatus == "03"*/ && _iTestTime == 1)
            {
                _bCharging = true;
                _bEndCharging = false;
                _bAms = true;
                _bAmf = false;
            }

            param = param.Remove(0, 19);

            int index = 0, mapping_index = 0;
            string code = "", tmpStr = "";
            int ivolt = 0, icurr = 0, icapa = 0;
            double dVolt;
            int iLength = param.Length - 10;
            int cnt = 1;
            while (iLength > cnt)
            {
                tmpStr = param.Substring(0, 11);
                param = param.Remove(0, 11);

                cnt += 11;

                //Mapping
                mapping_index = index;
                char[] charValues = tmpStr.ToCharArray();
                code = "0x" + String.Format("{0:X2}", Convert.ToInt32(charValues[0]));
                ivolt = (charValues[1] << 24) + (charValues[2] << 16) + (charValues[3] << 8) + charValues[4];
                icurr = (charValues[5] << 24) + (charValues[6] << 16) + (charValues[7] << 8) + charValues[8];
                icapa = (charValues[9] << 8) + charValues[10];

                if(icapa == mapping_index + 1 && _bAms == true)
                {
                    _sCode[mapping_index] = code;
                    OnChannelSatusChanged(mapping_index, code);
                    _sVolt[mapping_index] = String.Format("{0:0.0}", ivolt / 10.0);
                    _sCurr[mapping_index] = String.Format("{0:0.0}", icurr / 10.0);
                    _sCapa[mapping_index] = String.Format("{0:0}", icapa);
                }
                dVolt = ivolt / 10.0;// String.Format("{0.0}", nvolt);
                index += 1;
            }
        }

        public void SetResultData(int stageno, CPrechargerData CPreData)
        {
            int pos = 0;
            double dTempCurr = 0.0;

            for (int nIndex = 0; nIndex < 256; nIndex++)
            {
                //pos = chMap[nIndex + 1];
                pos = nIndex;
                double.TryParse(CPreData.CURR[nIndex], out dTempCurr);
                if (CPreData.CELL[nIndex] == true)
                {
                    if ( dTempCurr <= 50 || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorError || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorFlow)
                    {
                        CPreData.MEASURERESULT[nIndex] = false;
                    }
                    else
                    {
                        CPreData.MEASURERESULT[nIndex] = true;
                    }
                }
                else if(CPreData.CELL[nIndex] == false)
                {
                    //* 셀이 없으면 colorerror 가 될 수 없음.(???)
                    if(CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorError || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorFlow)
                    {
                        CPreData.MEASURERESULT[nIndex] = false;
                    }
                    else
                    {
                        CPreData.MEASURERESULT[nIndex] = true;
                    }
                }
            }
        }

        

        public void SetParms()
        {
            string param;
            param = "1";
            param = param + Convert.ToInt32(_sSetVoltage).ToString("D4");
            param = param + Convert.ToInt32(_sSetCurrent).ToString("D4");
            param = param + Convert.ToInt32(_sSetTime).ToString("D4");
            param = param + Convert.ToInt32(50).ToString("D4");

            _sParam = param;
        }

        private string GetPreChargerStatus(string status)
        {
            switch(status)
            {
                case "0x00": return "Wait(00)";
                case "0x01": return "Contact(01)";
                case "0x02": return "ContactFinished(02)";
                case "0x03": return "Charging(03)";
                case "0x04": return "Discharging(04)";
                case "0x05": return "Rest(05)";
                case "0x06": return "StepFinished(06)";
                case "0x07": return "Calibration(07)";
                case "0x08": return "Ready(08)";
                case "0x10": return "CmuxComm(10)";
                case "0x20": return "CmuxStatus(20)";
                case "0x30": return "PowerOVP Error(30)";
                case "0x40": return "PowerUVP Error(40)";
                case "0x50": return "DTri Error(50)";
                default: return status;
            }
        }

    }
}
