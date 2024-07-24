using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace FooDmx.Outputs.OpenDMX
{
    public class OpenDMXOutput : IOutput
    {

        private FTD2XX_NET.FTDI _FTDI;

        public string Name => "opendmx";

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


        public bool IsActive => true;

        public byte[] Addresses { get; private set; }

        public OpenDMXOutput()
        {
            Addresses = new byte[513];
        }

        public UserControl GetOptionsPage()
        {
            return null;
        }

        private SemaphoreSlim _locker = new SemaphoreSlim(1, 1);

        public event Action<byte[]> Updated;

        //private byte[] _addresses = new byte[512];

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _FTDI = new FTD2XX_NET.FTDI();

            uint deviceNum = 0; 
            _FTDI.GetNumberOfDevices(ref deviceNum);
            var devices = new FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[deviceNum];

            _FTDI.GetDeviceList(devices);

            var first = devices.FirstOrDefault(); 

            status = _FTDI.OpenBySerialNumber(first.SerialNumber);

            //Thread thread = new Thread(new ThreadStart(writeData));
            //thread.Start();
            buffer[0] = 0;
            //setDmxValue(0, 0);  //Set DMX Start Code

            while (!cancellationToken.IsCancellationRequested)
            {
                await _locker.WaitAsync();
                try
                {
                    writeData();
                }
                finally { _locker.Release(); }
                await Task.Delay(1000 / 40, cancellationToken);
            }

        }
        //public void setDmxValue(int channel, byte value)
        //{
        //    if (buffer != null)
        //    {
        //        buffer[channel + 1] = value;
        //    }
        //}

        public void writeData()
        {
            initOpenDMX();
            _FTDI.SetBreak(true);
            _FTDI.SetBreak(false);
            _FTDI.Write(buffer, buffer.Length, ref bytesWritten);

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
            status = _FTDI.SetFlowControl(FLOW_NONE, 0, 0);
            status = _FTDI.SetRTS(false);
            status = _FTDI.Purge(PURGE_TX);
            status = _FTDI.Purge(PURGE_RX);
        }
        public void SetAddresses(byte[] addresses)
        {
            _locker.Wait();
            try
            {
                Array.Copy(addresses, 0, buffer, 1, addresses.Length);
                Addresses = addresses;
            }
            finally { _locker.Release(); }

            Updated?.Invoke(Addresses);

        }
    }
}
