using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class Car : Vehicle
    {
        public static int Size { get; } = 2;

        public Car(string regNumber, DateTime parkingTime)
            : base(regNumber, parkingTime)
        {
            
        }
    }
}
