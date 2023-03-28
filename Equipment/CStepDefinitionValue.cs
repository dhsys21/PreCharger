using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreCharger.Equipment
{
    class CStepDefinitionValue
    {
        private double _Voltage;
        private double _Current;
        private int _Time;

        public double Voltage { get => _Voltage; set => _Voltage = value; }
        public double Current { get => _Current; set => _Current = value; }
        public int Time { get => _Time; set => _Time = value; }
    }
}
