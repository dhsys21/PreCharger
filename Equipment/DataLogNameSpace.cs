
using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// NameSpace that implements the parsing of the data:log? command
/// </summary>
namespace GgDataLogNamespace
{
    /// <summary>
    /// GgBinData holds one second of data from the GG data:log? query. This
    /// query always returns 256 data points for Voltage, Amps, CellId, and
    /// SequenceState. These 256 points represent 8 cards of 32 channels data.
    /// The first data point is card 1 channel 1. The 33 data point is card 2
    /// channel 1 etc...
    /// </summary>
    public class GgBinData
    {

        /// <summary>
        /// The number of milleseconds since GG bootup.
        /// </summary>
        public UInt64 TimeStamp { get { return _iTimeStamp; } }
        /// <summary>
        /// The private value of TimeStamp Property.
        /// </summary>
        private UInt64 _iTimeStamp;
        /// <summary>
        /// The VSense for the 256 channels.
        /// </summary>
        public List<float> VSense { get { return _oVSense; } }
        /// <summary>
        /// The private value of the VSense property.
        /// </summary>
        private List<float> _oVSense;
        /// <summary>
        /// The VLocal for the 256 channels
        /// </summary>
        public List<float> VLocal { get { return _oVLocal; } }
        /// <summary>
        /// The prvate value of the VLocal Property
        /// </summary>
        private List<float> _oVLocal;
        /// <summary>
        /// The IMon for the 256 channels
        /// </summary>
        public List<float> IMon { get { return _oIMon; } }
        /// <summary>
        /// The private value of Property IMon
        /// </summary>
        private List<float> _oIMon;


        /// <summary>
        /// The DCIR 1 for the 256 channels
        /// </summary>
        public List<float> Dcir1 { get { return _oDcir1; } }
        /// <summary>
        /// The private value of Property Dcir1
        /// </summary>
        private List<float> _oDcir1;
        /// <summary>
        /// The DCIR 2 for the 256 channels
        /// </summary>
        public List<float> Dcir2 { get { return _oDcir2; } }
        /// <summary>
        /// The private value of Property Dcir1
        /// </summary>
        private List<float> _oDcir2;


        /// <summary>
        /// Cell Id associated with the channel. GG currently only returns 0 for
        /// this data.
        /// </summary>
        public List<UInt16> CellId { get { return _oCellId; } }
        /// <summary>
        /// The private value of the property CellId
        /// </summary>
        private List<UInt16> _oCellId;
        /// <summary>
        /// Sequence State associated with the channel. The definitions of those states
        /// will be defined soon. GG Currently returns 0 for this data.
        /// </summary>
        public List<Int16> SequenceState { get { return _oSequenceState; } }
        /// <summary>
        /// The private value to help the property SequenceState
        /// </summary>
        private List<Int16> _oSequenceState;
        /// <summary>
        /// Contructor to create the data in GgBinData class
        /// </summary>
        /// <param name="iTimeStamp">Ms since GG bootup</param>
        /// <param name="oVSense">List of 256 VSense meas</param>
        /// <param name="oVLocal">List of 256 VLocal meas</param>
        /// <param name="oIMon">List of 256 IMon Measurements</param>
        /// <param name="oCellId">What Cell Id the channel associated with, 256 of these.</param>
        /// <param name="oSequenceState">What Sequqence State the channel is in, 256 of these</param>
        public GgBinData(
            UInt64 iTimeStamp,
            List<float> oVSense,
            List<float> oVLocal,
            List<float> oIMon,
            List<float> oDcir1,
            List<float> oDcir2,
            List<UInt16> oCellId,
            List<Int16> oSequenceState
            )
        {
            _iTimeStamp = iTimeStamp;
            _oVSense = oVSense;
            _oVLocal = oVLocal;
            _oIMon = oIMon;
            _oDcir1 = oDcir1;
            _oDcir2 = oDcir2;
            _oCellId = oCellId;
            _oSequenceState = oSequenceState;
        }


    }

    /// <summary>
    /// The DataLogQueryClass contains the static method dataLogQuery which is the workhorse
    /// for getting binary data out of GG using the data:log? command. The query returns between
    /// 0 and 10 records(i.e. seconds) of data and each record of data is represented as an element
    /// in a List of GgBinData class instances.
    /// </summary>
    public class DataLogQueryClass
    {
        /// <summary>
        /// Static method that executes a data:log? command and then builds a list of
        /// GGBinData's. One class for every second of data. For errors this method will
        /// throw Exception but that is currently TBD. 
        /// </summary>
        /// <param name="oStream">Stream to read and write the data from</param>
        /// <param name="bSendQuery">Flag that indicates whether or not the query needs to be send</param>
        /// <returns>Between 0 and 6 records of binary data from the data:log? command</returns>
        public static List<GgBinData> dataLogQuery(Stream oStream, bool bSendQuery)
        {
            // Let's first send the query data:log?
            String sLogQuery = "data:log? ";
            byte[] bLogQuery = System.Text.Encoding.ASCII.GetBytes(sLogQuery + "\n");
            if (bSendQuery)
                oStream.Write(bLogQuery, 0, bLogQuery.Length);
            return dataLogQuery(oStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oStream">Stream to read the data lof query block from</param>
        /// <returns>Between 0 and 6 records of binary data from the data:log? command</returns>
        public static List<GgBinData> dataLogQuery(Stream oStream)
        {
            byte[] bBlock = readBinaryBlock(oStream);
            return dataLogQuery(bBlock);
        }

        /// <summary>
        /// Does a parsing of the data for data:log? but the byte array does number contain the SCPI header.
        /// </summary>
        /// <param name="bBlock"></param>
        /// <returns></returns>
        public static List<GgBinData> dataLogQuery(Byte[] bBlock)
        {

            List<GgBinData> oGgLogDataList = new List<GgBinData>();
            // Read the number of Records
            int iOffset = 0;

            Int32 iNumberOfRecords = toInt32(bBlock, iOffset, 1)[0];
            iOffset += sizeof(Int32); ;

            for (Int32 i = 0; i < iNumberOfRecords; i++)
            {

                // Read the Timestamp
                UInt64 iMs = toUInt64(bBlock, iOffset, 1).ToArray()[0];
                iOffset += sizeof(UInt64);

                // Read 256 IMon
                List<float> oIMon = toFloat(bBlock, iOffset, 256);
                iOffset += sizeof(float) * 256;

                // Read 256 VSense
                List<float> oVSense = toFloat(bBlock, iOffset, 256);
                iOffset += sizeof(float) * 256;

                // Read 256 VLocal
                List<float> oVLocal = toFloat(bBlock, iOffset, 256);
                iOffset += sizeof(float) * 256;

                // Read 256 DCIR 1 values
                List<float> oDcir1 = toFloat(bBlock, iOffset, 256);
                iOffset += sizeof(float) * 256;

                // Read 256 DCIR 2 values
                List<float> oDcir2 = toFloat(bBlock, iOffset, 256);
                iOffset += sizeof(float) * 256;

                // Read the 256 CellId's
                List<UInt16> oCellId = toUInt16(bBlock, iOffset, 256);
                iOffset += sizeof(UInt16) * 256;

                // Read the 256 States
                List<Int16> oSequenceState = toInt16(bBlock, iOffset, 256);
                iOffset += sizeof(Int16) * 256;

                oGgLogDataList.Add(new GgBinData(iMs, oVSense, oVLocal, oIMon, oDcir1, oDcir2, oCellId, oSequenceState));

                //if (iMs == 0)
                //{
                //    mike++;
                //    myCount = 0;
                //}
            }

            return oGgLogDataList;
        }

        /// <summary>
        /// Converts a byte array to UInt16 values.
        /// </summary>
        /// <param name="bByte">The byte Array</param>
        /// <param name="iIndex">The index to start in the byte array</param>
        /// <param name="iElements">The number of values to get</param>
        /// <returns>A list of values from the byte array</returns>
        private static List<UInt16> toUInt16(byte[] bByte, int iIndex, int iElements)
        {
            List<UInt16> iValues = new List<UInt16>();
            for (int i = 0; i < iElements; i++)
            {
                UInt16 iValue = System.BitConverter.ToUInt16(bByte, iIndex);
                iIndex += sizeof(UInt16);
                iValues.Add(iValue);
            }
            return iValues;
        }

        /// <summary>
        /// Converts a byte array to Int16 values.
        /// </summary>
        /// <param name="bByte">The byte Array</param>
        /// <param name="iIndex">The index to start in the byte array</param>
        /// <param name="iElements">The number of values to get</param>
        /// <returns>A list of values from the byte array</returns>
        private static List<Int16> toInt16(byte[] bByte, int iIndex, int iElements)
        {
            List<Int16> iValues = new List<Int16>();
            for (int i = 0; i < iElements; i++)
            {
                Int16 iValue = System.BitConverter.ToInt16(bByte, iIndex);
                iIndex += sizeof(Int16);
                iValues.Add(iValue);
            }
            return iValues;
        }

        /// <summary>
        /// Converts a byte array to Int32 values.
        /// </summary>
        /// <param name="bByte">The byte Array</param>
        /// <param name="iIndex">The index to start in the byte array</param>
        /// <param name="iElements">The number of values to get</param>
        /// <returns>A list of values from the byte array</returns>
        private static List<Int32> toInt32(byte[] bByte, int iIndex, int iElements)
        {
            List<Int32> iValues = new List<Int32>();
            for (int i = 0; i < iElements; i++)
            {
                Int32 iValue = System.BitConverter.ToInt32(bByte, iIndex);
                iIndex += sizeof(Int32);
                iValues.Add(iValue);
            }
            return iValues;
        }

        /// <summary>
        /// Converts a byte array to float values.
        /// </summary>
        /// <param name="bByte">The byte Array</param>
        /// <param name="iIndex">The index to start in the byte array</param>
        /// <param name="iElements">The number of values to get</param>
        /// <returns>A list of values from the byte array</returns>
        private static List<float> toFloat(byte[] bByte, int iIndex, int iElements)
        {
            List<float> iValues = new List<float>();
            for (int i = 0; i < iElements; i++)
            {
                float iValue = System.BitConverter.ToSingle(bByte, iIndex);
                iIndex += sizeof(float);
                iValues.Add(iValue);
            }
            return iValues;
        }
        /// <summary>
        /// Converts a byte array to UInt64 values.
        /// </summary>
        /// <param name="bByte">The byte Array</param>
        /// <param name="iIndex">The index to start in the byte array</param>
        /// <param name="iElements">The number of values to get</param>
        /// <returns>A list of values from the byte array</returns>
        private static List<UInt64> toUInt64(byte[] bByte, int iIndex, int iElements)
        {
            List<UInt64> iValues = new List<UInt64>();

            for (int i = 0; i < iElements; i++)
            {
                UInt64 iValue = System.BitConverter.ToUInt64(bByte, iIndex);
                iIndex += sizeof(UInt64);
                iValues.Add(iValue);
            }
            return iValues;
        }

        /// <summary>
        /// Read a definite binary block into a Byte array.
        /// </summary>
        /// <param name="oStream">Stream to read and write the data from</param>
        /// <returns>The definite binary block</returns>
        private static byte[] readBinaryBlock(Stream oStream)
        {
            // wait for the the # char.
            int iPoundChar = 0;
            do
            {
                iPoundChar = getByte(oStream);
            }
            while (iPoundChar != 35);


            // Now read the number of digits that indicate the size
            int iDigitsForBlockSize = readAsInt(oStream, 1);

            // Now read the size of the block
            int iBlockSize = readAsInt(oStream, iDigitsForBlockSize);

            // Read the Block with all the data
            byte[] bBlock = readBytes(oStream, iBlockSize);
            byte[] bThrowAway = readBytes(oStream, 1);
            return bBlock;
        }

        /// <summary>
        /// Gets the next Byte and waits if it is not there.
        /// </summary>
        /// <param name="oStream">Stream to read and write the data from</param>
        /// <returns>The Byte as an integer</returns>
        private static int getByte(Stream oStream)
        {
            int iByte = 0;
            do
            {
                iByte = oStream.ReadByte();
                if (iByte == -1)
                    System.Threading.Thread.Sleep(1);
            }
            while (iByte == -1);
            return iByte;
        }

        /// <summary>
        /// Reads iSize bytes from a Network stream
        /// </summary>
        /// <param name="oStream">Stream to read and write the data from</param>
        /// <param name="iSize">Number of bytes to read</param>
        /// <returns></returns>
        private static byte[] readBytes(Stream oStream, int iSize)
        {
            byte[] bBytes = new Byte[iSize];
            int idx = 0;
            do
            {
                int iBytesRead = oStream.Read(bBytes, idx, iSize);
                idx += iBytesRead;
                iSize -= iBytesRead;
            }
            while (iSize > 0);

            return bBytes;
        }

        /// <summary>
        /// Reads n Bytes as an int
        /// </summary>
        /// <param name="oStream">Stream to read and write the data from</param>
        /// <param name="iSize">Number of Byte digits to convert to int</param>
        /// <returns>The integer value</returns>
        private static int readAsInt(Stream oNetworkStream, int iSize)
        {
            byte[] bDigits = readBytes(oNetworkStream, iSize);
            System.Text.ASCIIEncoding oAsciiEncoding = new System.Text.ASCIIEncoding();
            String sDigits = oAsciiEncoding.GetString(bDigits);
            int iDigits = Int32.Parse(sDigits);
            return iDigits;
        }
    }
}