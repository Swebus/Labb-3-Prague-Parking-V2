using Spectre.Console;
using Inlamning_3_Prague_Parking_V2.Classes;

string filepath = "../../../";

int garageSize = 101;

Vehicle[,] vehicleList = new Vehicle[garageSize, 2];

ParkingGarage pragueParking = new ParkingGarage(10, 20, 101);

ParkingSpot[] parkeringPlatser = new ParkingSpot[pragueParking.GarageSize];
for (int i = 0; i < parkeringPlatser.Length; i++)
{
    ParkingSpot spot = new ParkingSpot(0);
}







//for (int i = 1;  i < parkingSpots.Length; i++)
//{
//    ParkingSpot parkingSpot = new ParkingSpot(0);
//}
// List<Vehicle> vehicleList = new List<Vehicle>(new Vehicle[garageSize]);






bool exit = false;
while (!exit)
{
    AnsiConsole.Write(
    new FigletText("Prague Parking")
        .Centered()
        .Color(Color.Red));
    Console.WriteLine("\n\n\n\n\n");






    //for (int i = 1; i < parkingSpots.Length; i++)
    //{
    //    int currentSize;
    //    currentSize = parkingSpots[i].CurrentSize;
    //    Console.Write(currentSize);
    //}






    // Main menu selections
    var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .PageSize(4)
            .AddChoices(new[] {
            "Park Vehicle",
            "Get Vehicle",
            "Show Parking Spaces",
            "Close Program",
            }));
    

    // Selection switch
    switch (selection)
    {
        case "Park Vehicle":
            {
                ParkVehicle();
                break;
            }
        case "Get Vehicle":
            {
               // GetVehicle();
                break;
            }
        case "Show Parking Spaces":
            {
                ShowParkingSpaces();
                break;
            }
        case "Close Program":
            {
                exit = true;
                break;
            }
    }
    if (!exit)
    {
        Console.Write("Press a key to continue: . . . ");
        Console.ReadKey();
        Console.Clear();
    }
}





void ParkVehicle()
{
    int type = ChooseVehicleType();

    if (type == 1)      //type 1 = Car
    {
        for (int i = 1; i < parkeringPlatser.Length; i++)
        {
            int check = parkeringPlatser[i].CurrentSize;
            if (check == 0)
            {
                string regNumber = GetRegNumber();
                DateTime parkingTime = DateTime.Now;
                Car newCar = new Car(regNumber, parkingTime);

                Console.WriteLine(newCar.RegNumber);

                parkeringPlatser[i].AddVehicle(newCar);
                //if (newCar is Car car)
                //{
                //    parkingSpots[i].CurrentSize = Car.Size
                //}
                Console.WriteLine(parkeringPlatser[i].parkingSpot[1].RegNumber);
                Console.Write("Press a key to continue: . . . ");
                Console.ReadKey();
                //vehicleList[i, 0] = newCar;
                break;
            }
        }
    }
    else if (type == 2)     //type 2 = Mc
    {
        for (int i = 1; i <= vehicleList.GetLength(0); i++)

        {
            
            if (vehicleList[i, 0] == null)
            {
                string regNumber = GetRegNumber();
                DateTime parkingTime = DateTime.Now;
                Mc newMc = new Mc(regNumber, parkingTime);

                vehicleList[i, 0] = newMc;
                break;
            }
            else if ((vehicleList[i, 0] != null) && (vehicleList[i,0] is Mc) && (vehicleList[i, 1] == null) )
            {
                string regNumber = GetRegNumber();
                DateTime parkingTime = DateTime.Now;
                Mc newMc = new Mc(regNumber, parkingTime);

                vehicleList[i, 1] = newMc;
                break;
            }
        }
    }
}
int ChooseVehicleType()
{
    int type = 0;
    var typeChoice = AnsiConsole.Prompt(
  new SelectionPrompt<string>()
      .PageSize(4)
      .AddChoices(new[] {
                 "Car",
                 "Mc",
      }));
    if (typeChoice == "Car")
    {
        type = 1;
    }
    else if (typeChoice == "Mc")
    {
        type = 2;
    }

    return type;
}
string GetRegNumber()
{
    Console.Write("Enter vehicle registration number: ");
    string regNumber = Console.ReadLine();
    //**************************************

    //input check!

    //**************************************

    return regNumber;
}

void GetVehicle()
{
    Console.Write("Enter the registration number of the vehicle to retrieve: ");
    string regNumber = Console.ReadLine().ToUpper().Trim(); // Normalize input for comparison

    bool found = false;

    for (int i = 1; i < vehicleList.GetLength(0); i++)
    {
        if (vehicleList[i, 0]?.RegNumber == regNumber)
        {
            Console.WriteLine($"Vehicle {regNumber} found in spot {i}. Retrieving...");
            vehicleList[i, 0] = null; // Remove vehicle from the first slot
            found = true;
            break;
        }
        if (vehicleList[i, 1]?.RegNumber == regNumber)
        {
            Console.WriteLine($"Vehicle {regNumber} found in spot {i}. Retrieving...");
            vehicleList[i, 1] = null; // Remove vehicle from the second slot
            found = true;
            break;
        }
    }

    if (!found)
    {
        Console.WriteLine("Vehicle not found.");
    }
    else
    {
        Console.WriteLine("Vehicle retrieved successfully.");
    }

    Console.Write("Press a key to continue...");
    Console.ReadKey();
}

void ShowParkingSpaces()
{

    Console.WriteLine("==============================");

    for (int i = 1; i < vehicleList.GetLength(0); i++)
    {
        if ((i - 1) % 10 == 0)
        {
            Console.WriteLine("");
        }
        if ((vehicleList[i, 0] is Car) | (vehicleList[i, 1] != null))
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if ((vehicleList[i, 0] is Mc) && (vehicleList[i, 1] == null))
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }


        Console.Write(" X ");

        Console.ResetColor();
    
    
    }





    Console.WriteLine("\n\n==============================");


    //for (int i = 1; i < vehicleList.GetLength(0); i++)
    //{
    //    if (vehicleList[i, 0] != null) 
    //    {
    //        Console.Write(vehicleList[i, 0].RegNumber);
    //        if (vehicleList[i, 1 !] != null)
    //        {
    //            Console.WriteLine(vehicleList[i, 1].RegNumber + "      ");
    //        }
    //        else
    //        {
    //            Console.WriteLine("");
    //        }
    //    }
    //    else
    //    { 
    //        Console.WriteLine("Empty"); 
    //    }
        
    //}
}



//Console.WriteLine(newCar.RegNumber + "   " + vehicleList[1].ParkingTime);












