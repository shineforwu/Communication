using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Comon.Modbus.Rtu
{
    public interface IModbusRut_Interface
    {
        IModbusMaster CreateRtu();
        IModbusMaster CreateRtu(SerialPort port);
    }
}
