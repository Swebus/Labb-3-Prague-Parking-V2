﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Prague_Parking_V2.Classes
{
    public class Vehicle
    {
        public string RegNumber { get; set; }
        public DateTime ParkingTime { get; set; }
        //public string Index { get; set; }

        public Vehicle(string regNumber, DateTime parkingTime)
        {
            RegNumber = regNumber;
            ParkingTime = parkingTime;


        }
    }
}