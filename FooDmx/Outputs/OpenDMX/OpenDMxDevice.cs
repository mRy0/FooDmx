using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FooDmx.Outputs.OpenDMX
{
    public class OpenDMxDevice
    {
        private string _portName;
        private FTD2XX_NET.FTDI _FTDI;

        public OpenDMxDevice(string portName)
        {
            _portName = portName;
        }
        public byte[] buffer = new byte[513];
        public bool done = false;
        public uint bytesWritten = 0;
        public FTD2XX_NET.FTDI.FT_STATUS status;

        public const byte BITS_8 = 8;
        public const byte STOP_BITS_2 = 2;
        public const byte PARITY_NONE = 0;
        public const UInt16 FLOW_NONE = 0;
        public const byte PURGE_RX = 1;
        public const byte PURGE_TX = 2;


        public void Start()
        {
            status = _FTDI.OpenBySerialNumber(_portName);
            Thread thread = new Thread(new ThreadStart(writeData));
            thread.Start();
            setDmxValue(0, 0);  //Set DMX Start Code
        }
        public void setDmxValue(int channel, byte value)
        {
            if (buffer != null)
            {
                buffer[channel + 1] = value;
            }
        }

        public void writeData()
        {
            while (!done)
            {
                initOpenDMX();
                _FTDI.SetBreak(true);
                _FTDI.SetBreak(false);
                _FTDI.Write(buffer, buffer.Length, ref bytesWritten);
                Thread.Sleep(20);
            }

        }

        public int write(uint handle, byte[] data, int length)
        {
            uint bytesWritten = 0;
           
            status = _FTDI.Write(data, (uint)length, ref bytesWritten);
            return (int)bytesWritten;
        }
        
        public void initOpenDMX()
        {
            status = _FTDI.ResetDevice();
            status = _FTDI.SetBaudRate(250000);  // set baud rate
            status = _FTDI.SetDataCharacteristics(BITS_8, STOP_BITS_2, PARITY_NONE);
            status = _FTDI.SetFlowControl(FLOW_NONE, 0,0);
            status = _FTDI.SetRTS(false);
            status = _FTDI.Purge(PURGE_TX);
            status = _FTDI.Purge(PURGE_RX);
        }
    }
}
