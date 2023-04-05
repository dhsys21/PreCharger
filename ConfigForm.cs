using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreCharger.Common;

namespace PreCharger
{
    public partial class ConfigForm : Form
    {
        Util util = new Util();

        CEquipmentData _system = null;
        CPrecharger[] _PreCharger = new CPrecharger[_Constant.frmCount];
        public ConfigForm()
        {
            InitializeComponent();
            _system = CEquipmentData.GetInstance();
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                _PreCharger[nIndex] = new CPrecharger();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void lblVoltage_Click(object sender, EventArgs e)
        {
            gbMaxSetting.Visible = !gbMaxSetting.Visible;
        }

        public void SaveConfigFile()
        {
            string filename1 = _Constant.BIN_PATH + "SystemInfo_0.inf";
            util.saveConfig(filename1, "CONDITION", "1", tbVoltage.Text);
            util.saveConfig(filename1, "CONDITION", "2", tbCurrent.Text);
            util.saveConfig(filename1, "CONDITION", "3", tbTime.Text);

            util.saveConfig(filename1, "PRE_CONDITION", "1", tbPreVoltage.Text);
            util.saveConfig(filename1, "PRE_CONDITION", "2", tbPreCurrent.Text);
            util.saveConfig(filename1, "PRE_CONDITION ", "3", tbPreTime.Text);

            util.saveConfig(filename1, "MAX_CONDITION", "volt", tbMaxVoltage.Text);
            util.saveConfig(filename1, "MAX_CONDITION", "curr", tbMaxCurrent.Text);
            util.saveConfig(filename1, "MAX_CONDITION", "time", tbMaxTime.Text);

            util.saveConfig(filename1, "COMMUNICATION", "IP01", tbIPAddress01.Text);
            util.saveConfig(filename1, "COMMUNICATION", "PORT01", tbPort01.Text);
            util.saveConfig(filename1, "COMMUNICATION", "IP02", tbIPAddress02.Text);
            util.saveConfig(filename1, "COMMUNICATION", "PORT02", tbPort02.Text);
            util.saveConfig(filename1, "COMMUNICATION", "IP03", tbIPAddress03.Text);
            util.saveConfig(filename1, "COMMUNICATION", "PORT03", tbPort03.Text);
            util.saveConfig(filename1, "COMMUNICATION", "IP04", tbIPAddress04.Text);
            util.saveConfig(filename1, "COMMUNICATION", "PORT04", tbPort04.Text);

            util.saveConfig(filename1, "CELL_INFO", "CELL_MODEL", tbCellModel.Text);
            util.saveConfig(filename1, "CELL_INFO", "LOT_NUMBER", tbLotNumber.Text);

            util.saveConfig(filename1, "LOG", "ALL_CHANNEL", chkSaveAllLog.Checked.ToString());

            util.saveConfig(filename1, "AGING_TIME", "USE", chkAgingUse.Checked.ToString());
            util.saveConfig(filename1, "AGING_TIME", "TIME", tbAgingTime.Text);

            util.saveConfig(filename1, "MAIN", "REMEASURE_ALARM_COUNT", tbRemeasureAlarmCount.Text);

            string filename2 = _Constant.BIN_PATH + "MainSystemInfo.inf";
            util.saveConfig(filename2, "STAGE", "LINE NO.", tbLineNo.Text);
            util.saveConfig(filename2, "PLC", "CHANNEL_NO", tbPLCChannel.Text);
            util.saveConfig(filename2, "PLC", "NETWORK NUMBER", tbPLCNetworkNumber.Text);
            util.saveConfig(filename2, "PLC", "STATION NUMBER", tbPLCStationNumber.Text);

            util.saveConfig(filename2, "PLC", "IPADDRESS", tbPLCIPAddress.Text);
            util.saveConfig(filename2, "PLC", "PORT", tbPLCPort.Text);
        }

        public void ReadConfigFile()
        {
            bool bValue = false;

            string filename1 = _Constant.BIN_PATH + "SystemInfo_0.inf";

            //* Max Value
            _system.IMaxVoltage = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "volt"), 4200);
            _system.IMaxCurrent = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "curr"), 1600);
            _system.IMaxTime = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "time"), 300);

            tbMaxVoltage.Text = _system.IMaxVoltage.ToString();
            tbMaxCurrent.Text = _system.IMaxCurrent.ToString();
            tbMaxTime.Text = _system.IMaxTime.ToString();

            //* Setting Value
            _system.IVoltage = util.TryParseInt(util.readConfig(filename1, "CONDITION", "1"), 4200);
            _system.ICurrent = util.TryParseInt(util.readConfig(filename1, "CONDITION", "2"), 1600);
            _system.ITime = util.TryParseInt(util.readConfig(filename1, "CONDITION", "3"), 180);

            tbVoltage.Text = _system.IVoltage.ToString();
            tbCurrent.Text = _system.ICurrent.ToString();
            tbTime.Text = _system.ITime.ToString();

            //* Precharge Setting Value
            _system.IPREVOLTAGE = util.TryParseInt(util.readConfig(filename1, "PRE_CONDITION", "1"), 2000);
            _system.IPRECURRENT = util.TryParseInt(util.readConfig(filename1, "PRE_CONDITION", "2"), 1000);
            _system.IPRETIME = util.TryParseInt(util.readConfig(filename1, "PRE_CONDITION", "3"), 60);

            tbPreVoltage.Text = _system.IPREVOLTAGE.ToString();
            tbPreCurrent.Text = _system.IPRECURRENT.ToString();
            tbPreTime.Text = _system.IPRETIME.ToString();

            //* PreCharger IP Address
            _system.SIPAddress01 = util.readConfig(filename1, "COMMUNICATION", "IP01");
            _system.IPort01 = util.TryParseInt(util.readConfig(filename1, "COMMUNICATION", "PORT01"), 45000);

            tbIPAddress01.Text = _system.SIPAddress01;
            tbPort01.Text = _system.IPort01.ToString();

            _system.SIPAddress02 = util.readConfig(filename1, "COMMUNICATION", "IP02");
            _system.IPort02 = util.TryParseInt(util.readConfig(filename1, "COMMUNICATION", "PORT02"), 45000);

            tbIPAddress02.Text = _system.SIPAddress02;
            tbPort02.Text = _system.IPort02.ToString();

            _system.SIPAddress03 = util.readConfig(filename1, "COMMUNICATION", "IP03");
            _system.IPort03 = util.TryParseInt(util.readConfig(filename1, "COMMUNICATION", "PORT03"), 45000);

            tbIPAddress03.Text = _system.SIPAddress03;
            tbPort03.Text = _system.IPort03.ToString();

            _system.SIPAddress04 = util.readConfig(filename1, "COMMUNICATION", "IP04");
            _system.IPort04 = util.TryParseInt(util.readConfig(filename1, "COMMUNICATION", "PORT04"), 45000);

            tbIPAddress04.Text = _system.SIPAddress04;
            tbPort04.Text = _system.IPort04.ToString();

            //* CELL INFO
            _system.SCell_Model = util.readConfig(filename1, "CELL_INFO", "CELL_MODEL");
            _system.SLot_Number = util.readConfig(filename1, "CELL_INFO", "LOT_NUMBER");

            tbCellModel.Text = _system.SCell_Model;
            tbLotNumber.Text = _system.SLot_Number;

            //* Log All Channel
            bValue = false;
            bool.TryParse(util.readConfig(filename1, "LOG", "ALL_CHANNEL"), out bValue);
            _system.BLOGALLCHANNEL = bValue;
            chkSaveAllLog.Checked = (_system.BLOGALLCHANNEL == true) ? true : false;

            //* Aging Time
            bValue = false;
            bool.TryParse(util.readConfig(filename1, "AGING_TIME", "USE"), out bValue);
            _system.BAgingUse = bValue;
            chkAgingUse.Checked = (_system.BAgingUse == true) ? true : false;

            _system.IAgingTime = util.TryParseInt(util.readConfig(filename1, "AGING_TIME", "TIME"), 100);
            tbAgingTime.Text = _system.IAgingTime.ToString();

            //* Remeasure Alarm Count
            _system.IRemeasureAlarmCount = util.TryParseInt(util.readConfig(filename1, "MAIN", "REMEASURE_ALARM_COUNT"), 3);
            tbRemeasureAlarmCount.Text = _system.IRemeasureAlarmCount.ToString();

            string filename2 = _Constant.BIN_PATH + "MainSystemInfo.inf";
            _system.SLINENO = util.readConfig(filename2, "STAGE", "LINE NO.");
            _system.PLCCHANNELNO = util.TryParseInt(util.readConfig(filename2, "PLC", "CHANNEL NO"), 151);
            _system.PLCNETWORKNUMBER = util.TryParseInt(util.readConfig(filename2, "PLC", "NETWORK NUMBER"), 11);
            _system.PLCSTATIONNUMBER = util.TryParseInt(util.readConfig(filename2, "PLC", "STATION NUMBER"), 1);

            tbLineNo.Text = _system.SLINENO;
            tbPLCChannel.Text = _system.PLCCHANNELNO.ToString();
            tbPLCNetworkNumber.Text = _system.PLCNETWORKNUMBER.ToString();
            tbPLCStationNumber.Text = _system.PLCSTATIONNUMBER.ToString();

            _system.PLCIPADDRESS = "192.168.0.1";
            _system.PLCIPADDRESS = util.readConfig(filename2, "PLC", "IPADDRESS");
            _system.PLCPORT = util.TryParseInt(util.readConfig(filename2, "PLC", "PORT"), 5005);
            tbPLCIPAddress.Text = _system.PLCIPADDRESS;
            tbPLCPort.Text = _system.PLCPORT.ToString();
        }

        public void SaveChargingConfigFile()
        {
            string filename1 = _Constant.BIN_PATH + "MainSystemInfo.inf";
            util.saveConfig(filename1, "CONDITION", "1", tbMaxVoltage.Text);
            util.saveConfig(filename1, "CONDITION", "2", tbMaxCurrent.Text);
            util.saveConfig(filename1, "CONDITION", "3", tbMaxTime.Text);
        }

        public void ReadChargingConfigFile()
        {
            string filename1 = _Constant.BIN_PATH + "MainSystemInfo.inf";

            //* Max Value
            _system.IMaxVoltage = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "volt"), 4200);
            _system.IMaxCurrent = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "curr"), 1600);
            _system.IMaxTime = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "time"), 180);
            
            tbMaxVoltage.Text = _system.IMaxVoltage.ToString();
            tbMaxCurrent.Text = _system.IMaxCurrent.ToString();
            tbMaxTime.Text = _system.IMaxTime.ToString();
        }

        public void SaveMaxConfigFile()
        {
            string filename1 = _Constant.BIN_PATH + "MainSystemInfo.inf";
            util.saveConfig(filename1, "MAX_CONDITION", "volt", tbMaxVoltage.Text);
            util.saveConfig(filename1, "MAX_CONDITION", "curr", tbMaxCurrent.Text);
            util.saveConfig(filename1, "MAX_CONDITION", "time", tbMaxTime.Text);
        }

        public void ReadMaxConfigFile()
        {
            int nValue = 0;
            bool bValue = false;

            string filename1 = _Constant.BIN_PATH + "MainSystemInfo.inf";

            //* Max Value
            _system.IMaxVoltage = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "volt"), 4200);
            _system.IMaxCurrent = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "curr"), 1600);
            _system.IMaxTime = util.TryParseInt(util.readConfig(filename1, "MAX_CONDITION", "time"), 300);

        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfigFile();
            ReadConfigFile();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            SaveChargingConfigFile();
            ReadChargingConfigFile();
        }

        private void btnMaxSet_Click(object sender, EventArgs e)
        {
            SaveMaxConfigFile();
            ReadMaxConfigFile();
        }
        private void OnDisconnectPreCharger(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int nIndex = int.Parse(btn.Tag.ToString());
            _PreCharger[nIndex] = CPrecharger.GetInstance(nIndex);
            _PreCharger[nIndex].Close();
            _PreCharger[nIndex].StopTimer();
        }

        private void OnConnectPreCharger(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int nIndex = int.Parse(btn.Tag.ToString());
            string sAddress = string.Empty;
            int iPort = 0;
            switch(nIndex)
            {
                case 0:
                    sAddress = tbIPAddress01.Text;
                    iPort = int.Parse(tbPort01.Text);
                    break;
                case 1:
                    sAddress = tbIPAddress02.Text;
                    iPort = int.Parse(tbPort02.Text);
                    break;
                case 2:
                    sAddress = tbIPAddress03.Text;
                    iPort = int.Parse(tbPort03.Text);
                    break;
                case 3:
                    sAddress = tbIPAddress04.Text;
                    iPort = int.Parse(tbPort04.Text);
                    break;
                default:
                    break;
            }

            //_PreCharger[nIndex] = null;
            _PreCharger[nIndex] = CPrecharger.GetInstance(nIndex);
            _PreCharger[nIndex].Open(sAddress, iPort, "ACTIVE");
            _PreCharger[nIndex].STAGE = nIndex;
            _PreCharger[nIndex].AUTOMODE = true;
            _PreCharger[nIndex].EQUIPSTATUS = enumEquipStatus.StepVacancy;
        }

        private void btnPLCConnect_Click(object sender, EventArgs e)
        {

        }
    }
}
