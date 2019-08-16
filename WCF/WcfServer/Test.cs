using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCommon;

namespace WcfServer
{
    public class Test:ITest
    {
        public void Fun1(string data)
        {
            System.Console.WriteLine(data);
        }

    }
}
