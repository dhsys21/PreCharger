using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreCharger.Common;

namespace PreCharger
{
    class CEquipmentData
    {
        private int _iStageNo;

        private int _iPLCChannelNo;
        private int _iPLCNetworkNumber;
        private int _iPLCStationNumber;

        private string _sPLCIpAddress;
        private int _iPLCPort;

        private string _sIPAddress01;
        private int _iPort01;
        private string _sIPAddress02;
        private int _iPort02;
        private string _sIPAddress03;
        private int _iPort03;
        private string _sIPAddress04;
        private int _iPort04;

        private int _iMaxVoltage;
        private int _iMaxCurrent;
        private int _iMaxTime;
        private int _iVoltage;
        private int _iCurrent;
        private int _iTime;

        private string _sCell_Model;
        private string _sLot_Number;

        private int _iRemeasureAlarmCount;

        private bool _bLogAllChannel;

        private bool _bAgingUse;
        private int _iAgingTime;

        private string _sLineNo;

        public int IStageNo { get => _iStageNo; set => _iStageNo = value; }

        public int PLCCHANNELNO { get => _iPLCChannelNo; set => _iPLCChannelNo = value; }
        public int PLCNETWORKNUMBER { get => _iPLCNetworkNumber; set => _iPLCNetworkNumber = value; }
        public int PLCSTATIONNUMBER { get => _iPLCStationNumber; set => _iPLCStationNumber = value; }

        public string PLCIPADDRESS { get => _sPLCIpAddress; set => _sPLCIpAddress = value; }
        public int PLCPORT { get => _iPLCPort; set => _iPLCPort = value; }
        public string SIPAddress01 { get => _sIPAddress01; set => _sIPAddress01 = value; }
        public int IPort01 { get => _iPort01; set => _iPort01 = value; }
        public string SIPAddress02 { get => _sIPAddress02; set => _sIPAddress02 = value; }
        public int IPort02 { get => _iPort02; set => _iPort02 = value; }
        public string SIPAddress03 { get => _sIPAddress03; set => _sIPAddress03 = value; }
        public int IPort03 { get => _iPort03; set => _iPort03 = value; }
        public string SIPAddress04 { get => _sIPAddress04; set => _sIPAddress04 = value; }
        public int IPort04 { get => _iPort04; set => _iPort04 = value; }
        public int IMaxVoltage { get => _iMaxVoltage; set => _iMaxVoltage = value; }
        public int IMaxCurrent { get => _iMaxCurrent; set => _iMaxCurrent = value; }
        public int IMaxTime { get => _iMaxTime; set => _iMaxTime = value; }
        public int IVoltage { get => _iVoltage; set => _iVoltage = value; }
        public int ICurrent { get => _iCurrent; set => _iCurrent = value; }
        public int ITime { get => _iTime; set => _iTime = value; }
        public string SCell_Model { get => _sCell_Model; set => _sCell_Model = value; }
        public string SLot_Number { get => _sLot_Number; set => _sLot_Number = value; }
        public int IRemeasureAlarmCount { get => _iRemeasureAlarmCount; set => _iRemeasureAlarmCount = value; }

        public bool BLOGALLCHANNEL { get => _bLogAllChannel; set => _bLogAllChannel = value; }
        public bool BAgingUse { get => _bAgingUse; set => _bAgingUse = value; }
        public int IAgingTime { get => _iAgingTime; set => _iAgingTime = value; }

        public string SLINENO { get => _sLineNo; set => _sLineNo = value; }


        private static CEquipmentData equipmentdata;

        public static CEquipmentData GetInstance()
        {
            if (equipmentdata == null) equipmentdata = new CEquipmentData();
            return equipmentdata;
        }
    }
}
