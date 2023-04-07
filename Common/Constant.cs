using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreCharger.Common
{
    static class _Constant
    {
        public static readonly int frmCount =4;
        public static readonly int StartPosition = 4;
        public static readonly int ChannelCount = 256;

        #region PATH
        public static readonly string APP_PATH = @"D:\Precharger\\";
        public static readonly string BIN_PATH = APP_PATH + "Bin\\";
        public static readonly string DATA_PATH = APP_PATH + "Data\\";
        public static readonly string LOG_PATH = APP_PATH + "Log\\";
        public static readonly string TRAY_PATH = APP_PATH + "Tray\\";

        #endregion

        #region PLC - PC ADDRESS
        public static readonly int PLC_CELL_SERIAL_NUM_SIZE = 20;
        public static readonly int PLC_TRAY_ID_SIZE = 10;

        public static readonly int[] PLC_D_START_NUM = { 40000, 45000, 80000, 85000 };
        public static readonly int[] PC_D_START_NUM = { 42000, 47000, 82000, 87000 };

        public static readonly int PLC_D_LEN = 300;
        public static readonly int PC_D_LEN = 1200;

        public static readonly int PLC_HEART_BEAT = 0;
        public static readonly int PLC_PRE_ERROR = 2;
        public static readonly int PLC_PRE_ATUO_MANUAL = 1;
        public static readonly int PLC_PRE_TRAY_IN = 3;
        public static readonly int PLC_PRE_PROB_OPEN = 4;
        public static readonly int PLC_PRE_PROB_CLOSE = 5;
        public static readonly int PLC_PRE_TRAY_ID = 10;
        public static readonly int PLC_TRAY_CELL_DATA = 30;


        public static readonly int PC_HEART_BEAT = 0;
        public static readonly int PC_PRE_ERROR = 2;
        public static readonly int PC_PRE_STAGE_AUTO_READY = 1;
        public static readonly int PC_PRE_TRAY_OUT = 3;
        public static readonly int PC_PRE_PROB_OPEN = 4;
        public static readonly int PC_PRE_PROB_CLOSE = 5;
        public static readonly int PC_PRE_CHARGING = 6;
        public static readonly int PC_PRE_OKNG = 10;
        public static readonly int PC_PRE_VOLTAGE = 30;
        public static readonly int PC_PRE_CURRENT = 550;
        public static readonly int PC_PRE_NGCOUNT = 1100;
        public static readonly int PC_PRE_CURRENT_MIN = 1101;
        public static readonly int PC_PRE_CHARGE_VOLTAGE = 1102;
        public static readonly int PC_PRE_CHARGE_CURRENT = 1103;
        public static readonly int PC_PRE_CHARGE_TIME = 1104;

        #endregion

        #region Color Status
        public static readonly Color ColorReady = Color.LightBlue;
        public static readonly Color ColorCharging = Color.Orange;
        public static readonly Color ColorFinish = Color.LimeGreen;
        public static readonly Color ColorNoCell = Color.Silver;
        public static readonly Color ColorFlow = Color.Yellow;
        public static readonly Color ColorError = Color.Red;
        public static readonly Color ColorVoltage = Color.LightSkyBlue;
        public static readonly Color ColorCurrent = Color.LightPink;
        #endregion

        #region (PreCharger, Channel) Status
        //public static readonly string CHANNELSTATUS_OK = "00";
        //public static readonly string CHANNELSTATUS_OVERCHARGE = "03";
        //public static readonly string CHANNELSTATUS_FPNG = "04";
        //public static readonly string CHANNELSTATUS_NOCELL = "05";
        public static readonly int nNoAnswer = 0;
        public static readonly int nIdle = 1;
        public static readonly int nVacancy = 2;
        public static readonly int nIN = 3;
        public static readonly int nREADY = 4;
        public static readonly int nRUN = 5;
        public static readonly int nEND = 6;
        public static readonly int nFinish = 7;
        public static readonly int nManual = 8;
        public static readonly int nOpbox = 9;  // IMS 프로토콜 외 별도 생성
        public static readonly int nEmergency = 10;
        #endregion
    }

    public enum enumInspectionType
    {
        Thread = 0,
        Timer = 1
    }
    public enum enumConnectionState
    {
        Disabled = 0,
        Enabled = 1,
        Disconnected = 2,
        Connected = 3,
        Retry = 4,
        TimeOut = 5
    }
    public enum PLCDATATYPE
    {
        PLC = 0,
        PC = 1
    }
    public enum enumPreState
    {
        Manual = 0,
        Auto = 1
    }

    public enum enumEquipStatus
    {
        StepVacancy = 0,
        StepTrayIn = 1,
        StepReady = 2,
        StepRun = 3,
        StepEnd = 4,
        StepTrayOut = 5,
        StepManual = 6,
        StepNoAnswer = 7,
        StepEmergency = 8
    }

    public enum enumChannelStatus
    {
        OK = 0x00,
        OverCharge = 0x03,
        FPNG = 0x04,
        NoCell = 0x05,
        Disable = 0x06,
        EndTime = 0x10,
        EndVoltage = 0x11,
        EndCurrent = 0x12,
        EndCapa = 0x13,
        Stop = 0x30,
        Charging = 0x31,
        Discharging = 0x32,
        Rest = 0x33,
        NoStatue = 0x40,
        OCP = 0x41,
        PowerOVP = 0x42,
        PowerUVP = 0x43,
        PowerOTP = 0x44,
        SensingOverRange = 0x45,
        SensingNoRef = 0x46,
        SensingTimeOut = 0x47,
        DCUNErr = 0x48,
        LimitVoltage = 0xD0,
        UpperCurrent = 0xD9,
        LowerCurrent = 0xDA
    }

    public enum enumPrechargerStatus
    {
        Stop = 0x00,
        Contact = 0x01,
        ContactFinished = 0x02,
        Charging = 0x03,
        Discharging = 0x04,
        Rest = 0x05,
        StepFinished = 0x06,
        Calibration = 0x07,
        Ready = 0x08,
        CMuxComm = 0x10,
        CMuxStatus = 0x20,
        PowerOVP = 0x30,
        PowerUVP = 0x40,
        DTrip = 0x50
    }

    
}
