using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCommon
{
    public abstract partial class ISerialPortHelper:IDisposable
    {
        
        public SerialPort MySerialPort { get; set; }=new SerialPort();
        public bool IsHeartbeatOK { get; set; } = true;

        public virtual void Dispose()
        {
            if(MySerialPort != null)
            {
                MySerialPort.Close();
                MySerialPort.Dispose();
            }
        }

        public virtual bool SendCmd(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
