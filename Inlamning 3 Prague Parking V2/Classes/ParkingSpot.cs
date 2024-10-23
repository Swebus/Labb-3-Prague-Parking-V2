using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class ParkingSpot
    {
        public List<Vehicle> parkingSpot { get; set; }
        public int MaxSize { get; }
        public int CurrentSize { get; set; }


        public ParkingSpot(int currentSize) 
        {
            MaxSize = 2;
            parkingSpot = new List<Vehicle>();
            CurrentSize = currentSize;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            parkingSpot.Add(vehicle);
        }
    }
}
