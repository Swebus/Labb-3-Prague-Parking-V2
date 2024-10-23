using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class Mc : Vehicle
    {
        public int Size { get; }

        public Mc(string regNumber, DateTime parkingTime)
            : base(regNumber, parkingTime)
        {
            Size = 1;
        }
    }
}
