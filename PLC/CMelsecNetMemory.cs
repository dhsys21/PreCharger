using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreCharger.Common
{
    public enum enumDeviceType
    {
        X = 1,
        Y = 2,
        M = 4,
        SB = 5,
        D = 13,
        SW = 14,
        B = 23,
        W = 24,
        WW = 36,
        WR = 37,
        R = 22
    }

    public enum enumMemoryType
    {
        Bit,
        Word
    }

    #region CMelsecNetMemory
    /// <summary>
    /// CMelsecNetMemory에 대한 요약 설명입니다.
    /// </summary>
    public class CMelsecNetMemory
    {
        private int _nMemorySize;
        public int RawMemorySize
        {
            get { return _nMemorySize; }
        }
        private short[] _asRawMemory;
        public short[] RawMemory
        {
            get { return _asRawMemory; }
        }

        private enumDeviceType _DeviceType;
        public enumDeviceType DeviceType
        {
            get { return _DeviceType; }
        }

        private enumMemoryType _MemoryType;
        public enumMemoryType MemoryType
        {
            get { return _MemoryType; }
        }

        private int _iStartAddress;
        public int StartAdderess
        {
            get { return _iStartAddress; }
        }
        private int _iEndAddress;
        public int EndAdderess
        {
            get { return _iEndAddress; }
        }

        public CMelsecNetMemory()
        {
            _DeviceType = enumDeviceType.B;
            _MemoryType = enumMemoryType.Bit;
            _iStartAddress = 0;
            _iEndAddress = 0;
            _nMemorySize = 0;
            _asRawMemory = null;
        }

        public CMelsecNetMemory(enumDeviceType sDeviceType, int iStartAddress, int iEndAddress)
        {
            _DeviceType = sDeviceType;
            switch (_DeviceType)
            {
                case enumDeviceType.X:
                case enumDeviceType.Y:
                case enumDeviceType.B:
                case enumDeviceType.SB:
                case enumDeviceType.M:
                    _MemoryType = enumMemoryType.Bit;
                    break;
                case enumDeviceType.W:
                case enumDeviceType.SW:
                case enumDeviceType.WR:
                case enumDeviceType.WW:
                case enumDeviceType.D:
                case enumDeviceType.R:
                    _MemoryType = enumMemoryType.Word;
                    break;
            }

            _iStartAddress = iStartAddress;
            _iEndAddress = iEndAddress;


            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    _nMemorySize = (_iEndAddress - _iStartAddress) / 0x10 + 1;
                    break;
                case enumMemoryType.Word:
                    _nMemorySize = _iEndAddress - _iStartAddress + 1;
                    break;
            }
            _asRawMemory = new short[_nMemorySize];
            for (int i = 0; i < _nMemorySize; i++)
            {
                _asRawMemory[i] = 0;
            }
        }

        public static enumMemoryType GetMemoryType(enumDeviceType deviceType)
        {
            enumMemoryType memoryType = enumMemoryType.Bit;
            switch (deviceType)
            {
                case enumDeviceType.X:
                case enumDeviceType.Y:
                case enumDeviceType.B:
                case enumDeviceType.SB:
                    memoryType = enumMemoryType.Bit;
                    break;
                case enumDeviceType.W:
                case enumDeviceType.SW:
                case enumDeviceType.WR:
                case enumDeviceType.WW:
                    memoryType = enumMemoryType.Word;
                    break;
            }

            return memoryType;
        }

        public bool ReadBit(int iAddress, int nIndex, out bool bValue)
        {
            bValue = false;
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int nStartAddress = (iAddress - _iStartAddress) / 0x10;
                        bValue = ((_asRawMemory[nStartAddress] & (1 << (iAddress & 0x000F))) != 0);
                    }
                    break;
                case enumMemoryType.Word:
                    bValue = ((_asRawMemory[iAddress - _iStartAddress] & (1 << nIndex)) != 0);
                    break;
            }

            return true;
        }

        public bool WriteBit(int iAddress, int nIndex, bool bValue)
        {
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int nStartAddress = (iAddress - _iStartAddress) / 0x10;
                        if (bValue)
                        {
                            _asRawMemory[nStartAddress] |= (short)(1 << (iAddress & 0x000F));
                        }
                        else
                        {
                            _asRawMemory[nStartAddress] &= (short)~(1 << (iAddress & 0x000F));
                        }
                    }
                    break;
                case enumMemoryType.Word:
                    if (bValue)
                        _asRawMemory[iAddress - _iStartAddress] = (short)((ushort)_asRawMemory[iAddress - _iStartAddress] | (1 << nIndex));
                    else
                        _asRawMemory[iAddress - _iStartAddress] = (short)((ushort)_asRawMemory[iAddress - _iStartAddress] & ~(1 << nIndex));
                    break;
            }

            return true;
        }

        public bool ReadWord(int iAddress, out ushort sValue)
        {
            sValue = 0;
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int nStartAddress = (iAddress - _iStartAddress) / 0x10;
                        sValue = (ushort)_asRawMemory[nStartAddress];
                    }
                    break;
                case enumMemoryType.Word:
                    sValue = (ushort)_asRawMemory[iAddress - _iStartAddress];
                    break;
            }

            return true;
        }

        public bool ReadWord(int iAddress, out int iValue)
        {
            iValue = 0;
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int address = (iAddress - _iStartAddress) / 0x10;
                        iValue = (ushort)_asRawMemory[address] | (((ushort)_asRawMemory[address + 1]) << 16);
                    }
                    break;
                case enumMemoryType.Word:
                    {
                        int address = iAddress - _iStartAddress;
                        iValue = (ushort)_asRawMemory[address] | (((ushort)_asRawMemory[address + 1]) << 16);
                    }
                    break;
            }

            return true;
        }

        public bool ReadWord(int iAddress, out long lValue)
        {
            lValue = 0;
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int address = (iAddress - _iStartAddress) / 0x10;
                        lValue = (ushort)_asRawMemory[address] | (((ushort)_asRawMemory[address + 1]) << 16);
                        lValue = lValue | (((uint)_asRawMemory[address + 2]) << 32);
                        lValue = lValue | (((uint)_asRawMemory[address + 3]) << 48);
                    }
                    break;
                case enumMemoryType.Word:
                    {
                        int address = iAddress - _iStartAddress;
                        lValue = _asRawMemory[address];
                        lValue = (ushort)_asRawMemory[address] | (((ushort)_asRawMemory[address + 1]) << 16);
                        lValue = lValue | (((uint)_asRawMemory[address + 2]) << 32);
                        lValue = lValue | (((uint)_asRawMemory[address + 3]) << 48);
                    }
                    break;
            }

            return true;
        }

        public bool ReadWord(int iAddress, int nIndex, int nLength, out ushort sValue)
        {
            sValue = 0;
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int nStartAddress = ((iAddress - _iStartAddress) / 0x10) * 0x10;
                        int iTemp = _asRawMemory[nStartAddress / 0x10];
                        int iMask = 0;
                        for (int i = 0; i < nLength; i++)
                        {
                            iMask |= 1 << i;
                        }
                        iTemp = iTemp & (iMask << (iAddress - nStartAddress));
                        sValue = (ushort)(iTemp >> (iAddress - nStartAddress));
                    }
                    break;
                case enumMemoryType.Word:
                    {
                        int iTemp = _asRawMemory[iAddress - _iStartAddress];
                        int iMask = 0;
                        for (int i = 0; i < nLength; i++)
                        {
                            iMask |= 1 << i;
                        }
                        iTemp = iTemp & (iMask << nIndex);
                        sValue = (ushort)(iTemp >> nIndex);
                    }
                    break;
            }

            return true;
        }

        public bool WriteWord(int iAddress, ushort sData)
        {
            if (_iStartAddress > iAddress || _iEndAddress < iAddress) return false;

            switch (_MemoryType)
            {
                case enumMemoryType.Bit:
                    {
                        int nStartAddress = (iAddress - _iStartAddress) / 0x10;
                        _asRawMemory[nStartAddress] = (short)sData;
                    }
                    break;
                case enumMemoryType.Word:
                    _asRawMemory[iAddress - _iStartAddress] = (short)sData;
                    break;
            }

            return true;
        }

        public bool ReadWordA(int iAddress, int iLength, out string strValue)
        {
            if (_MemoryType != enumMemoryType.Word)
            {
                strValue = string.Empty;
                return false;
            }

            strValue = "";
            StringBuilder sb = new StringBuilder();
            int nMemoryLength = (iLength + 1) / 2;
            if (_iStartAddress > iAddress || _iEndAddress < (iAddress + nMemoryLength - 1)) return false;
            int nStartAddress = iAddress - _iStartAddress;

            for (int i = nStartAddress; i < nStartAddress + nMemoryLength; i++)
            {
                sb.Append(Convert.ToChar(_asRawMemory[i] & 0xFF));
                sb.Append(Convert.ToChar((_asRawMemory[i] >> 8) & 0xFF));
            }

            strValue = sb.ToString();
            strValue = strValue.Substring(0, iLength);
            strValue = strValue.Trim(" \0".ToCharArray());
            return true;
        }

        public bool WriteWordA(int iAddress, int iLength, string strData)
        {
            if (_MemoryType != enumMemoryType.Word)
            {
                return false;
            }

            int nMemoryLength = (iLength + 1) / 2;
            if (_iStartAddress > iAddress || _iEndAddress < (iAddress + nMemoryLength - 1)) return false;

            // WordLength 보다 큰 Data의 입력시 Length만큼 Data를 잘라냄
            if (strData.Length > iLength)
                strData = strData.Substring(0, iLength);
            strData = strData.PadRight(nMemoryLength * 2);

            int nStartAddress = iAddress - _iStartAddress;

            for (int i = nStartAddress; i < nStartAddress + nMemoryLength; i++)
            {
                _asRawMemory[i] = Convert.ToInt16(strData[i * 2]);
                _asRawMemory[i] = Convert.ToInt16((ushort)_asRawMemory[i] | (Convert.ToUInt16(strData[i * 2 + 1]) << 8));
            }

            return true;
        }
    }
    #endregion

    public sealed class CMelsecNetMemoryCollection : Dictionary<string, CMelsecNetMemory>
    {
    }
}