using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class Mc : Vehicle
    {
        private int size = 2;

        public override int Size
        {
            get { return size; }
        }
        public Mc(string regNumber, DateTime parkingTime)
            : base(regNumber, parkingTime)
        {
           
        }
    }
}
