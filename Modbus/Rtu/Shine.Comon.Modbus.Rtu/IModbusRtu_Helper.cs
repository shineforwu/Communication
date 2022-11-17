using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;

namespace Shine.Comon.Modbus.Rtu
{
    public abstract class IModbusRtu_Helper:IModbusRut_Interface
    {
        //参数(分别为从站地址,起始地址,长度)
         private byte slaveAddress;
         private ushort startAddress;
         private ushort numberOfPoints;
        public SerialPort MyPort{get;set;}
        protected IModbusMaster master { get; private set; }

        public virtual IModbusMaster CreateRtu()
        {
            return CreateRtu(this.MyPort);
        }

        public virtual IModbusMaster CreateRtu(SerialPort port)
        {
            master = ModbusSerialMaster.CreateRtu(port);
            return master;
        }
    }
}
