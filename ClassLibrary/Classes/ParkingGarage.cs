using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class ParkingGarage
    {

        public int McPrize;
        public int CarPrize;
        public int GarageSize;

        public ParkingGarage(int mcPrize, int carPrize, int garageSize)
        {
            McPrize = mcPrize;
            CarPrize = carPrize;
            GarageSize = garageSize;
        }

    } 
}
