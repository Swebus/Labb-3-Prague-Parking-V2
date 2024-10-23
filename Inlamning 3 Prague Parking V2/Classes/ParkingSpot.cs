using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class ParkingSpot
    {
        public List<Vehicle> parkingSpot { get; set; } = new List<Vehicle>();
        public int MaxSize { get; } = 2;
        public int CurrentSize { get; set; }


        public ParkingSpot(int currentSize) 
        {
            CurrentSize = currentSize;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            parkingSpot.Add(vehicle);
            
        }
    }
}
