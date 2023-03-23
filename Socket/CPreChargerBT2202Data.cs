using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreCharger
{
    class CPreChargerBT2202Data
    {
        private int _iStageNo;
        private bool _bAmf;
        private bool _bAms;
        private bool _bRst;
        private bool _bSet;
        private bool _bEndCharging;
        private bool _bCharging;
        private string _sTrayId;
        private bool _bTrayin;
        private int _iCellCount;
        private bool[] _bCell = new bool[256];
        private string[] _sCellSerial = new string[256];
        private bool[] _bMeasureResult = new bool[256];
        private string _sMdlName;
        private string _sBatchId;
        private string _sLotId;
        private string _sCellModel;
        private string _sLotNumber;
        private string[] _sVolt = new string[256];
        private string[] _sCurr = new string[256];

        public int STAGENO { get => _iStageNo; set => _iStageNo = value; }
        public bool AMF { get => _bAmf; set => _bAmf = value; }
        public bool AMS { get => _bAms; set => _bAms = value; }
        public bool RST { get => _bRst; set => _bRst = value; }
        public bool SET { get => _bSet; set => _bSet = value; }
        public bool ENDCHARGING { get => _bEndCharging; set => _bEndCharging = value; }
        public bool CHARGING { get => _bCharging; set => _bCharging = value; }
        public string TRAYID { get => _sTrayId; set => _sTrayId = value; }
        public bool TRAYIN { get => _bTrayin; set => _bTrayin = value; }
        public int CELLCOUNT { get => _iCellCount; set => _iCellCount = value; }
        public bool[] CELL { get => _bCell; set => _bCell = value; }
        public string[] CELLSERIAL { get => _sCellSerial; set => _sCellSerial = value; }
        public bool[] MEASURERESULT { get => _bMeasureResult; set => _bMeasureResult = value; }
        public string MDLNAME { get => _sMdlName; set => _sMdlName = value; }
        public string BATCHID { get => _sBatchId; set => _sBatchId = value; }
        public string LotId { get => _sLotId; set => _sLotId = value; }
        public string CellModel { get => _sCellModel; set => _sCellModel = value; }
        public string LotNumber { get => _sLotNumber; set => _sLotNumber = value; }
        public string[] Volt { get => _sVolt; set => _sVolt = value; }
        public string[] Curr { get => _sCurr; set => _sCurr = value; }

        public void InitData(int stageno)
        {
            _iStageNo = stageno;

            _bAmf = false;
            _bAms = false;
            _bRst = false;
            _bSet = false;
            _bEndCharging = false;
            _bCharging = false;

            _sTrayId = string.Empty;
            _bTrayin = false;
            _iCellCount = 0;
            System.Array.Clear(_bCell, 0, 256);
            System.Array.Clear(_sCellSerial, 0, 256);
            System.Array.Clear(_bMeasureResult, 0, 256);
            _sMdlName = string.Empty;
            _sBatchId = string.Empty;
            _sLotId = string.Empty;
            _sCellModel = string.Empty;
            _sLotNumber = string.Empty;

            for (int nIndex = 0; nIndex < 256; nIndex++)
            {
                _sVolt[nIndex] = "0";
                _sCurr[nIndex] = "0";
            }

        }
    }
}
