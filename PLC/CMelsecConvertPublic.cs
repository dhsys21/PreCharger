using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCharger.Common
{
    public class CMelsecConvertPublic
    {
        /// <summary>
        /// Decimal -> Hex
        /// </summary>
        /// <param name="iParam"></param>
        /// <returns></returns>
        public string GetDecToHex(int iParam)
        {
            return iParam.ToString("X");
        }

        /// <summary>
        /// Char -> Hex
        /// </summary>
        /// <param name="asciiString"></param>
        /// <returns></returns>
        public string GetCharToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        /// <summary>
        /// 10진수 > 2진수
        /// </summary>
        /// <param name="asciiString"></param>
        /// <returns></returns>
        public string GetDecToBin(int iParam)
        {
            int Binary = 0;
            char[] BinaryHolder = new char[1];
            string BinaryResult = "";

            while (iParam > 0)
            {
                Binary = iParam % 2;
                BinaryResult += Binary;
                iParam = iParam / 2;
            }
            BinaryHolder = BinaryResult.ToCharArray();
            Array.Reverse(BinaryHolder);
            BinaryResult = new string(BinaryHolder);
            return BinaryResult;
        }

        /// <summary>
        /// Dec String -> Hex String
        /// </summary>
        /// <param name="sDecData">16진수로 변환할 10진수 값</param>
        /// <returns>변환된 16진수 값</returns>
        public string FromDecToHex(string sDecData)
        {
            string sDecToHex = "";

            try
            {
                long lDecData = Int64.Parse(sDecData);
                sDecToHex = Convert.ToString(lDecData, 16);
            }

            catch
            {
                sDecToHex = "";
            }

            return sDecToHex;

        }

        /// <summary>
        /// Hex String -> Dec String
        /// </summary>
        /// <param name="sDecData">10진수로 변환할 16진수 값</param>
        /// <returns>변환된 10진수 값</returns>
        public string FromHexToDec(string sHexData)
        {

            string sHexToDec = "";

            try
            {
                sHexToDec = Convert.ToInt64(sHexData, 16).ToString();
            }
            catch
            {
                sHexToDec = "";
            }

            return sHexToDec;
        }

        /// <summary>
        /// Tow Byte -> One Byte (두바이트의 스트링값은 16진수 -> 한바이트로 전환 (binary 프로토콜 response와 동일))
        /// </summary>
        /// <param name="iResponseByteLen">변환할 Byte 수</param>
        /// <param name="btSourceData">변환할 소스데이타</param>
        /// <returns>변환된 Byte값 </returns>
        public byte[] FromTByteToOByte(int iResponseByteLen, byte[] btSourceData, out string sErrorScript)
        {
            sErrorScript = "";

            int iBtCnt = 0;
            int iRem = 0;

            int iConvertBuffIndex = 0;
            int iConvertDataIndex = 0;
            byte[] btConvertBuff = null;      //해석을 위한 BYTE 임시버퍼
            byte[] btConvertData = null;

            try
            {
                btConvertData = new byte[btSourceData.Length / 2];
                btConvertBuff = new byte[iResponseByteLen];

                for (iBtCnt = 0; iBtCnt < btSourceData.Length; iBtCnt++)
                {
                    Math.DivRem((iBtCnt + 1), 2, out iRem);

                    if (iRem == 1)
                    {
                        btConvertBuff[iConvertBuffIndex] = btSourceData[iBtCnt];
                        iConvertBuffIndex = iConvertBuffIndex + 1;
                    }
                    else
                    {
                        btConvertBuff[iConvertBuffIndex] = btSourceData[iBtCnt];
                        btConvertData[iConvertDataIndex] = byte.Parse(FromHexToDec(System.Text.Encoding.ASCII.GetString(btConvertBuff).Replace("\0", "")));
                        btConvertBuff = new byte[iResponseByteLen];
                        iConvertBuffIndex = 0;
                        iConvertDataIndex = iConvertDataIndex + 1;
                    }

                }
            }
            catch (Exception ex)
            {
                sErrorScript = ex.Message;
                btConvertData = null;
            }

            return btConvertData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bASCII"></param>
        /// <param name="strConvData"></param>
        /// <param name="strConvDataLength"></param>
        /// <returns></returns>
        public string GetConvertToHexaData1Word1Char(bool bASCII, string[] strConvData)
        {
            string sReturnStr = "";

            try
            {

                if (bASCII)
                {
                    string[] sTurn = new string[2];
                    for (int i = 0; i < strConvData.Length; i++)
                    {
                        if (strConvData[i].Trim() == "")
                        {
                            for (int j = 0; j < strConvData[i].Length; j++)
                            {
                                sReturnStr += "00";
                            }
                        }
                        else
                        {
                            for (int j = 0; j < strConvData[i].Length; j++)
                            {
                                string sConvData = strConvData[i].Substring(j, 1);

                                int tmp = Convert.ToChar(sConvData);
                                sTurn[(j % 2)] = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));

                                if ((j % 2) == 1)
                                {
                                    // 앞 뒤 뒤집어 줌
                                    sReturnStr += sTurn[1] + sTurn[0];

                                    sTurn[0] = "00";
                                    sTurn[1] = "00";
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < strConvData.Length; i++)
                    {
                        string hexValue = int.Parse(strConvData[i]).ToString("X");
                        sReturnStr += hexValue.PadLeft(4, '0');
                    }
                }

            }
            catch
            {
                return "";
            }

            return sReturnStr;
        }

        /// <summary>
        /// PLC Add. 숫자부분 추출 [Melsec, Fuji]
        /// </summary>
        /// <param name="plcAddr"></param>
        /// <returns></returns>
        public int SelectAddrNormal(string plcAddr)
        {
            int i;
            int idx;
            int splitIdx;
            int ret;
            byte[] buff = new byte[80];
            string str;

            plcAddr = plcAddr.Trim();
            //splitIdx = plcAddr.IndexOf(':');
            splitIdx = plcAddr.IndexOf('*');

            if (!Char.IsLetter(plcAddr[0]) || splitIdx < 0)
                return -9999;

            idx = 0;
            for (i = splitIdx + 1; i < plcAddr.Length; i++)
            {
                if (plcAddr[i] < 0x20) break;
                if ((plcAddr[i] >= '0') && (plcAddr[i] <= '9'))
                    buff[idx++] = Convert.ToByte(plcAddr[i]);
            }

            if (idx == 0)
                return -9999;

            str = Encoding.Default.GetString(buff, 0, idx);
            ret = Convert.ToInt32(str);

            return ret;
        }

    }
}
