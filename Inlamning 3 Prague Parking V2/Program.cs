﻿using Spectre.Console;
using Inlamning_3_Prague_Parking_V2.Classes;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;

string filepath = "../../../";

var configValues = ReadConfigTxt();

ParkingGarage pragueParking = new ParkingGarage(configValues.mcPrize, configValues.carPrize, configValues.garageSize);

string parkingJsonString = File.ReadAllText(filepath + "ParkingArray.json");

ParkingSpot[] parkeringsPlatser = JsonSerializer.Deserialize<ParkingSpot[]>(parkingJsonString);

//ParkingSpot[] parkeringPlatser = new ParkingSpot[pragueParking.GarageSize];
//for (int i = 0; i < parkeringPlatser.Length; i++)
//{
//    parkeringPlatser[i] = new ParkingSpot(0);
//}



bool exit = false;
while (!exit)
{
    AnsiConsole.Write(
    new FigletText("Prague Parking")
        .Centered()
        .Color(Color.Red));
    Console.WriteLine("\n\n\n\n\n");




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

        newCar.ParkVehicle(parkeringsPlatser);
        SaveParkingSpots();
    }
    else if (type == 2)     //type 2 = Mc
    {
        string regNumber = GetRegNumber();
        DateTime parkingTime = DateTime.Now;
        Mc newMc = new Mc(regNumber, parkingTime);

        newMc.ParkVehicle(parkeringsPlatser);
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
    
    while (true)
    {
        Console.Write("Enter vehicle registration number: ");
        string regNumber = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(regNumber) | (regNumber.Length < 1) | (regNumber.Length > 10 | ContainsSpecialCharacters(regNumber)))
        {
            Console.WriteLine("Invalid, please try again.)");
            continue;
        }
        bool regNumberExists = parkeringsPlatser.Any(spot => spot.ContainsVehicle(regNumber));

        if (regNumberExists)
        {
            Console.WriteLine("This registration number is already parked. Please enter a new one.");
        }
        else
        {
            return regNumber;
        } 
    }


} 
bool ContainsSpecialCharacters(string regNumber)
{
    return Regex.IsMatch(regNumber, @"[^\p{L}\p{N}]");
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
    for (int i = 1; i < parkeringsPlatser.Length; i++)
    {
        var spot = parkeringsPlatser[i];
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

        if (int.TryParse(Console.ReadLine(), out newSpotIndex) && newSpotIndex > 0 && newSpotIndex < parkeringsPlatser.Length)
        {
            var newSpot = parkeringsPlatser[newSpotIndex];

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

    for (int i = 1; i < parkeringsPlatser.Length; i++)
    {
        if ((i - 1) % 10 == 0)
        {
            Console.WriteLine("");
        }
        if (parkeringsPlatser[i].CurrentSize == parkeringsPlatser[i].MaxSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if ((parkeringsPlatser[i].CurrentSize < parkeringsPlatser[i].MaxSize) && (parkeringsPlatser[i].CurrentSize > 00))
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }


        Console.Write(" X ");

        Console.ResetColor();
    
    
    }





    Console.WriteLine("\n\n==============================");

}
void SaveParkingSpots()
{
    string updatedParkingArrayJsonString = JsonSerializer.Serialize(parkeringsPlatser, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filepath + "ParkingArray.json", updatedParkingArrayJsonString);
}


// Metod som räknar ut pris för parkering.  -- första 10 minuterna är gratis



(int mcPrize, int carPrize, int garageSize) ReadConfigTxt()
{
    var configValues = new Dictionary<string, int>();

    foreach (var line in File.ReadLines(filepath + "Config.txt"))
    {
        if (string.IsNullOrEmpty(line) || line.TrimStart().StartsWith("#")) continue;

        var parts = line.Split(new[] { '=' }, 2);
        if (parts.Length == 2)
        {
            string key = parts[0].Trim();
            string value = parts[1].Trim().Split('#')[0].Trim();
            configValues[key] = int.Parse(value);
        }
    }

    configValues.TryGetValue("McPrize", out int mcPrize);
    configValues.TryGetValue("CarPrize", out int carPrize);
    configValues.TryGetValue("GarageSize", out int garageSize);

    return (mcPrize, carPrize, garageSize);
}








