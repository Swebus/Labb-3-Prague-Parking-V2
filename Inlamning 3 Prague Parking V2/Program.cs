﻿using Spectre.Console;
using Inlamning_3_Prague_Parking_V2.Classes;
using System.Text.Json;
using System.ComponentModel.Design;

string filepath = "../../../";



ParkingGarage pragueParking = new ParkingGarage(10, 20, 101);

ParkingSpot[] parkeringPlatser = new ParkingSpot[pragueParking.GarageSize];
for (int i = 0; i < parkeringPlatser.Length; i++)
{
    parkeringPlatser[i] = new ParkingSpot(0);
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
            .PageSize(8)
            .AddChoices(new[] {
            "Park Vehicle",
            "Get Vehicle",        //Sigrid
            "Move Vehicle",        //RObert
            "Find Vehicle",        //Omeed
            "Reload Config File",      //Sebastian
            "Show Parking Spaces",
            "Show Detailed Spaces",
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
        case "Move Vehicle":
            {
                MoveVehicle();
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
        Console.Write("\nPress a key to continue: . . . ");
        Console.ReadKey();
        Console.Clear();
    }
}



void ParkVehicle()
{
    int type = ChooseVehicleType();

    if (type == 1)      //type 1 = Car
    {
        string regNumber = GetRegNumber();
        DateTime parkingTime = DateTime.Now;
        Car newCar = new Car(regNumber, parkingTime);

        newCar.ParkVehicle(parkeringPlatser);
        SaveParkingSpots();
    }
    else if (type == 2)     //type 2 = Mc
    {
        string regNumber = GetRegNumber();
        DateTime parkingTime = DateTime.Now;
        Mc newMc = new Mc(regNumber, parkingTime);

        newMc.ParkVehicle(parkeringPlatser);
        SaveParkingSpots();
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
void MoveVehicle()
{
    string regNumber;
    
    do
    {
        Console.WriteLine("Enter Registration Number or exit for exit: ");
        regNumber = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(regNumber))
        {
            Console.WriteLine("Invalid, please try again or type exit: ");
        }
        else if (regNumber.ToLower() == "exit")
        {
            Console.WriteLine("Exit, back to Main Meny");
            return;
        }

    } while (string.IsNullOrEmpty(regNumber)); 
   
     
    ParkingSpot currentSpot = null;
    Vehicle vehicleToMove = null;
    int currentSpotIndex = -1;

    // loop igenom för match regNum
    for (int i = 1; i < parkeringPlatser.Length; i++)
    {
        var spot = parkeringPlatser[i];
        vehicleToMove = spot.parkingSpot.FirstOrDefault(Vehicle => Vehicle.RegNumber == regNumber);
        if (vehicleToMove != null)
        {
            currentSpot = spot;
            currentSpotIndex = i;
            break;
        }
    }

    if (currentSpot == null)
    {
        Console.WriteLine($"Vehicle with registration number {regNumber} not found.");
        return;
    }
    
    Console.WriteLine($"Current parking spot for {regNumber} is {currentSpotIndex}");
    int newSpotIndex;

    bool isValidtoCheckOut = true;
    do
    {

        Console.Write("Enter new parking spot number: ");

        if (int.TryParse(Console.ReadLine(), out newSpotIndex) && newSpotIndex > 0 && newSpotIndex < parkeringPlatser.Length)
        {
            var newSpot = parkeringPlatser[newSpotIndex];

            if (newSpot.CurrentSize + vehicleToMove.Size <= newSpot.MaxSize)
            {
                // To bort
                currentSpot.parkingSpot.Remove(vehicleToMove);
                currentSpot.CurrentSize -= vehicleToMove.Size;

                // Flytta
                newSpot.parkingSpot.Add(vehicleToMove);
                newSpot.CurrentSize += vehicleToMove.Size;
                Console.WriteLine($"Vehicle {regNumber} moved to spot {newSpotIndex}");

                // Spara och update
                SaveParkingSpots();
                isValidtoCheckOut = false;
            }
            else
            {
                Console.WriteLine("Not enought space.");
            }
        }
        else
        {
            Console.WriteLine("Invalid parking spot number. Please try again.");

        }
    } while (isValidtoCheckOut);
}
void ShowParkingSpaces()
{

    Console.WriteLine("==============================");

    for (int i = 1; i < parkeringPlatser.Length; i++)
    {
        if ((i - 1) % 10 == 0)
        {
            Console.WriteLine("");
        }
        if (parkeringPlatser[i].CurrentSize == parkeringPlatser[i].MaxSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if ((parkeringPlatser[i].CurrentSize < parkeringPlatser[i].MaxSize) && (parkeringPlatser[i].CurrentSize > 00))
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
void SaveParkingSpots()
{
    string updatedParkingArrayJsonString = JsonSerializer.Serialize(parkeringPlatser, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filepath + "ParkingArray.json", updatedParkingArrayJsonString);
}
//Console.WriteLine(newCar.RegNumber + "   " + vehicleList[1].ParkingTime);



// Metod som kontrollerar tillåtna recken

// Metod som kontrollerar om fordon redan finns  -- Tar emot inputsträng, gert tillbaka true eller false. 

// Metod som räknar ut pris för parkering.  -- första 10 minuterna är gratis












