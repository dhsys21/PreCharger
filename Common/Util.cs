using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using PreCharger.Common;
using System.Reflection;

namespace PreCharger
{
    class Util
    {
        IniFile ini = new IniFile();
        string STX = string.Format("{0}", (char)2);
        string ETX = string.Format("{0}", (char)3);
        Form form = null;
        Panel panel = null;
        public void loadFormIntoPanel(Form childform, Panel parentpanel)
        {
            form = null;

            form = childform;
            panel = parentpanel;

            if (form != null && panel != null)
            {
                form.TopLevel = false;
                form.AutoScroll = true;
                InsertForm(form, panel);
                form.Dock = DockStyle.Fill;

                panel.Controls.Add(form);
                form.Show();
            }
        }

        private void InsertForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;

                c.Controls.Add(f);
                f.Show();
            }
        }

        static public void RemoveForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.None;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;

                c.Controls.Remove(f);
                f = null;
            }
        }

        public int TryParseInt(string text, int nDefaultValue)
        {
            int res;
            int value;
            if (Int32.TryParse(text,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return value;
            }
            return nDefaultValue;
        }

        public double TryParseDouble(string text, double dDefaultValue)
        {
            double res;
            double value;
            if (Double.TryParse(text,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return value;
            }
            return dDefaultValue;
        }


        public void saveConfig(string filename, string main_title, string sub_title, string sValue)
        {
            
            // Network Setting
            ini[main_title][sub_title] = sValue;
           

            ini.Save(filename);
        }

        public string readConfig(string filename, string main_title, string sub_title)
        {
            string strValue = "";

            ini.Load(filename);
            strValue = ini[main_title][sub_title].ToString();

            return strValue;
        }

        public void FileWrite(string filePath, string strData)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.Write(strData);

                    //다쓴 StreamWriter 와 FileStream 닫기
                    streamWriter.Close();
                    fileStream.Close();
                }  
            }
        }
        public void FileWrite_old(string filePath, string strData)
        {
            FileStream fileStream = new FileStream(
                filePath,              //저장경로
                FileMode.Create,       //파일스트림 모드
                FileAccess.Write       //접근 권한
                );

            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.Write(strData);

            //다쓴 StreamWriter 와 FileStream 닫기
            streamWriter.Close();
            fileStream.Close();
        }
        public void FileAppend(string filePath, string timeData, double[,] spData, string forceData)
        {
            string strData = "";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    for (int i = 0; i < spData.GetLength(0); i++)
                    {
                        strData += spData[i, 0].ToString() + ";";
                    }
                    sw.WriteLine(timeData + ";" + strData + forceData);
                }
            }
        }

        public void FileAppend(string filePath, string strData)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        sw.WriteLine(strData);
                    }
                }
            }catch(Exception ex) { }
        }

        //private void saveConfig(string filename)
        //{
        //    IniFile ini = new IniFile();
        //    // Network Setting
        //    ini["COMMUNICATION"]["IP"] = cbSendText.Text;
        //    ini["COMMUNICATION"]["PORT"] = cbSendTimer.Text;

        //    // MAX_CONDITION
        //    ini["MAX_CONDITION"]["VOLTAGE"] = tbYAxisMax.Text;
        //    ini["MAX_CONDITION"]["CURRENT"] = tbYAxisMin.Text;
        //    ini["MAX_CONDITION"]["TIME"] = tbSwipeActivate.Text;

        //    // CELL_INFO
        //    ini["CELL_INFO"]["CELL_MODEL"] = tbYAxisMax.Text;
        //    ini["CELL_INFO"]["LOT_NUMBER"] = tbYAxisMin.Text;

        //    // CONDITION
        //    ini["CONDITION"]["1"] = tbYAxisMax.Text;
        //    ini["CONDITION"]["2"] = tbYAxisMin.Text;
        //    ini["CONDITION"]["3"] = tbSwipeActivate.Text;

        //    // MAIN
        //    ini["MAIN"]["REMEASURE_ALARM_COUNT"] = tbYAxisMax.Text;

        //    // AGING_TIME
        //    ini["AGING_TIME"]["USE"] = tbYAxisMax.Text;
        //    ini["AGING_TIME"]["TIME"] = tbYAxisMin.Text;

        //    ini.Save(filename);
        //}

        public string MakeCMD(string cmd)
        {
            return STX + CheckSum(cmd) + ETX;
        }

        public string CheckSum(string strData)
        {
            string sRtnVal = "";

            if (strData != null)
            {
                byte checksum = 0x00;
                byte[] aa = new byte[strData.Length];

                for (int i = 0; i < strData.Length; i++)
                    aa[i] = (byte)strData[i];

                for (int i = 0; i < strData.Length; i++)
                    checksum += (byte)strData[i];

                string kwon = checksum.ToString("X2");

                sRtnVal = strData + kwon;
            }

            return sRtnVal;
        }

        public void NGInformation(CPrechargerData CPreData)
        {
            string file = _Constant.TRAY_PATH + CPreData.TRAYID + ".Tray";

            IniFile ini = new IniFile();
            string ok_ng = "NG";

            for(int i = 0; i < 256; i++)
            {
                if (CPreData.MEASURERESULT[i] == true) ok_ng = "OK";
                else
                {
                    // acc_remeasure += 1;
                    ok_ng = "NG";
                }
                ini[i.ToString()]["PRECHARGER"] = ok_ng;
                ini[i.ToString()]["FINAL VOLT"] = CPreData.VOLT[i];
            }
            ini.Save(file);
        }

        public void SaveResultFile(int stageno, CPrechargerData CPreData)
        {
            string filename, dir, id, ir, ocv, ok_ng = string.Empty;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.DATA_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + "PreCharger_" + CPreData.TRAYID + "-" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file;
            file = "TRAY ID," + CPreData.TRAYID + "\r\n";
            file = file + "CELL MODEL," + CPreData.CELLMODEL + "\r\n";
            file = file + "LOT NUMBER," + CPreData.LOTNUMBER + "\r\n";
            file = file + "ARRIVE TIME," + CPreData.ARRIVETIME.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n";
            file = file + "FINISH TIME," + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n";
            file = file + "VOLTAGE," + CPreData.SETVOLTAGE + "\r\n";
            file = file + "CURRENT," + CPreData.SETCURRENT + "\r\n";
            file = file + "TIME," + CPreData.SETTIME + "\r\n";

            file += "CH,CELL ID,VOLT,CURR,RESULT\r\n";

            for (int i = 0; i < 256; ++i)
            {
                //		ir = real_data.volt[i];
                //		ocv = real_data.curr[i];
                id = CPreData.CELLSERIAL[i];
                ir = CPreData.VOLT[i];
                ocv = CPreData.CURR[i];

                if (CPreData.CELL[i] == true)
                {
                    if (CPreData.MEASURERESULT[i] == true) ok_ng = "OK";
                    else ok_ng = "NG";
                }
                else if (CPreData.CELL[i] == false)
                {
                    if (CPreData.MEASURERESULT[i] == true) ok_ng = "No Cell";
                    else ok_ng = "NG(No Cell)";
                }

                file = file + (i + 1).ToString() + "," + id + "," + ir + "," + ocv + "," + ok_ng + "\r\n";
            }

            FileWrite(filename, file);
        }

        public void SavePLCLog(int nIndex, string strMessage)
        {
            string dir = "";
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + "STAGE" + (nIndex + 1).ToString("D3") + "_PLC_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = "";

            strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }

        public void SaveLog(int nIndex, string strMessage, string type)
        {
            string dir = "";
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + "STAGE" + (nIndex + 1).ToString("D3") + "_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = "";

            if (type == "RX" && strMessage.Contains("MON"))
            {
                strMonitoring = strMessage.Substring(0, 33) + "\r\n";
                strMonitoring = strMonitoring + ParseMon2859(strMessage);
            }
            else
                strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + type + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }
        public void SaveLog(int nIndex, string strMessage)
        {
            string dir = string.Empty;
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + "STAGE" + (nIndex + 1).ToString("D3") + "_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = string.Empty;

            strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }

        //* MON 응답의 전체 길이가 2859
        private string ParseMon2859(string sMessage)
        {
            string sMon = "", code = "", tmpStr = "", msg = "";
            int nvolt = 0, ncurr = 0, ncapa = 0;
            sMon = sMessage.Substring(32);
            int iLength = sMon.Length - 10;
            int cnt = 1, nChannel = 1;
            while (iLength > cnt)
            {
                tmpStr = sMon.Substring(0, 11);
                sMon = sMon.Remove(0, 11);
                //sMon = sMon.Substring(11);
                cnt += 11;

                char[] charValues = tmpStr.ToCharArray();
                code = String.Format("{0:X}", Convert.ToInt32(charValues[0]));
                nvolt = (charValues[1] << 24) + (charValues[2] << 16) + (charValues[3] << 8) + charValues[4];
                ncurr = (charValues[5] << 24) + (charValues[6] << 16) + (charValues[7] << 8) + charValues[8];
                ncapa = (charValues[9] << 8) + (int)charValues[10];

                if (nChannel > 125) 
                    code = code + "";

                msg = msg + "CH-" + nChannel.ToString() + "-" + code + "-" + nvolt + "-" + ncurr + "-" + ncapa + "\t";
                nChannel++;
            }


            return msg;
        }

        /// <summary>
		/// 셀의 위치 변경 - ChangeMap
		/// </summary>
		/// <param name="nIndex">셀 번호 0 - 255</param>
		/// <param name="startposition">시작점 (left top, right top, left bottom, right bottom</param>
		/// <param name="rowIndex">row 번호</param>
		/// <param name="colIndex">column 번호</param>
		public void ChangeMapToGridView(int nIndex, int startposition, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;
            switch (startposition)
            {
                case 1: // start at left top to right top
                    rowIndex = nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 5: // start at left top to left bottom
                    rowIndex = nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 2: // start at right top to left top
                    rowIndex = nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 6: // start at right top to right bottom
                    rowIndex = nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                case 3: // start at left bottom to right bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 7: // start at left bottom to left top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 4: // start at right bottom to left bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 8: // start at right bottom to right top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                default:
                    break;
            }
        }

        public void ChangeMapToGridView(int nIndex, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;
            int startposition = _Constant.StartPosition;
            switch (startposition)
            {
                case 1: // start at left top to right top
                    rowIndex = nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 5: // start at left top to left bottom
                    rowIndex = nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 2: // start at right top to left top
                    rowIndex = nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 6: // start at right top to right bottom
                    rowIndex = nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                case 3: // start at left bottom to right bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 7: // start at left bottom to left top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 4: // start at right bottom to left bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 8: // start at right bottom to right top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                default:
                    break;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
