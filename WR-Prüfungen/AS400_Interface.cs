using ElmMQDNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WR_Prüfungen
{
    class AS400_Interface
    {
       public AS400_Interface(string s) {

            int reasonCode = 0;
            int compCode = 0;
            ElmMQDNet ty = new ElmMQDNet("MQConfig.xml");
            ty.Put(false, "Put00", "Test message", "", "USR", ref compCode, ref reasonCode);
            ty.Close();
        }
    }
}
