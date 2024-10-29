using Spectre.Console;
using Inlamning_3_Prague_Parking_V2.Classes;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;

string filepath = "../../../";
//int oldGarageSize;
var configValues = ReadConfigTxt();
ParkingGarage pragueParking = new ParkingGarage(configValues.mcPrize, configValues.carPrize, configValues.garageSize);

ParkingSpot[] parkeringsPlatser;
if (File.Exists(filepath + "ParkingArray.json"))
{
    string parkingJsonString = File.ReadAllText(filepath + "ParkingArray.json");
    parkeringsPlatser = JsonSerializer.Deserialize<ParkingSpot[]>(parkingJsonString);
}
else
{
    parkeringsPlatser = new ParkingSpot[pragueParking.GarageSize];
    for (int i = 0; i < parkeringsPlatser.Length; i++)
    {
        parkeringsPlatser[i] = new ParkingSpot(0);
    }
    SaveParkingSpots();
}
ReloadConfigFile();



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
            "Get Vehicle",       
            "Move Vehicle",        
            "Find Vehicle",        
            "Reload Config File",     
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
                GetVehicle();
                break;
            }
        case "Move Vehicle":
            {
                MoveVehicle();
                break;
            }
        case "Find Vehicle":
            {
                break;
            }
        case "Reload Config File":
            {
                ReloadConfigFile();
                break;
            }
        case "Show Parking Spaces":
            {
                ShowParkingSpaces();
                break;
            }
        case "Show Detailed Spaces":
            {
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
        var table1 = new Table();  
        table1.AddColumn("[yellow]Press enter to return to Main Menu.[/]");
        AnsiConsole.Write(table1);
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
            Console.WriteLine("\nInvalid, please try again.");
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
void GetVehicle()
{
    string regNumber;
    // Be om registreringsnummer
    do
    {
        Console.WriteLine("Enter Registration Number:  ");
        regNumber = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(regNumber))
        {
            var table2 = new Table();
            table2.AddColumn("[yellow]We cannot find a vehicle that registration number here. [/]");
            AnsiConsole.Write(table2);
            return;
        }
    } while (string.IsNullOrEmpty(regNumber));

    // Leta upp fordonet i parkeringsplatserna
    ParkingSpot currentSpot = null;
    Vehicle vehicleToRemove = null;
    int currentSpotIndex = -1;

    for (int i = 1; i < parkeringsPlatser.Length; i++)
    {
        var spot = parkeringsPlatser[i];
        vehicleToRemove = spot.parkingSpot.FirstOrDefault(v => v.RegNumber == regNumber);

        if (vehicleToRemove != null)
        {
            currentSpot = spot;
            currentSpotIndex = i;
            break;
        }
    }

    if (currentSpot == null || vehicleToRemove == null)
    {
        Console.WriteLine($"No vehicle with registration number {regNumber} found.");
        return;
    }

    // Beräkna parkeringstid och eventuella kostnader
    DateTime currentTime = DateTime.Now;
    TimeSpan parkingDuration = currentTime - vehicleToRemove.ParkingTime;

    // Anta att de första 10 minuterna är gratis
    double price = 0;
    if (parkingDuration.TotalMinutes > 10)
    {
        if (vehicleToRemove is Car)
        {
            price = (parkingDuration.TotalMinutes - 10) * pragueParking.CarPrize / 60;
        }
        else if (vehicleToRemove is Mc)
        {
            price = (parkingDuration.TotalMinutes - 10) * pragueParking.McPrize / 60;
        }
    }

    Console.WriteLine($"Parking duration: {parkingDuration.TotalMinutes:F1} minutes.");
    Console.WriteLine($"Parking cost: {price}CZK");

    // Bekräfta om användaren vill ta bort fordonet
    var confirm = AnsiConsole.Confirm("Do you want to retrieve and remove the vehicle?", true);
    if (confirm)
    {
        // Ta bort fordonet från nuvarande parkeringsplats
        currentSpot.parkingSpot.Remove(vehicleToRemove);
        currentSpot.CurrentSize -= vehicleToRemove.Size;

        Console.WriteLine($"Vehicle {regNumber} has been retrieved from spot {currentSpotIndex}.");

        // Spara uppdaterade parkeringsplatser till JSON-filen
        SaveParkingSpots();
    }
}
void MoveVehicle()

{
    string regNumber;

    do
    {
        Console.WriteLine("Enter Registration Number:  ");
        regNumber = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(regNumber))
        {
            
            var table2 = new Table();
            table2.AddColumn("[yellow]We cannot find a vehicle that registration number here. [/]");
            AnsiConsole.Write(table2);
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

        var table3 = new Table();
        table3.AddColumn($"[yellow]Vehicle with registration nummber {regNumber} not found.[/]");
        AnsiConsole.Write(table3);
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
void ReloadConfigFile()
{
    //oldGarageSize = parkeringsPlatser.Length;
    bool isEmpty = true;

    for (int i = 0; i < parkeringsPlatser.Length; i++)
    {
        if (parkeringsPlatser[i].CurrentSize > 0)
        {
            isEmpty = false;
            break;
        }
    }

    var newConfigValues = ReadConfigTxt();


    if ((newConfigValues.garageSize < parkeringsPlatser.Length) && (isEmpty == false))
    {
        ParkingGarage pragueParking = new ParkingGarage(newConfigValues.mcPrize, configValues.carPrize, parkeringsPlatser.Length);
        Console.WriteLine("Garage not empty, number of spots remains the same. \n" +
            "Please empty the garage before decreasing its size.");
    }
    else if((newConfigValues.garageSize < parkeringsPlatser.Length) && (isEmpty == true))
    {
        ParkingGarage newPragueParking = new ParkingGarage(newConfigValues.mcPrize, newConfigValues.carPrize, newConfigValues.garageSize);
        pragueParking = newPragueParking;
        parkeringsPlatser = new ParkingSpot[pragueParking.GarageSize];
        for (int i = 0; i < parkeringsPlatser.Length; i++)
        {
            parkeringsPlatser[i] = new ParkingSpot(0);
        }
    }
    else
    {
        ParkingGarage newPragueParking = new ParkingGarage(newConfigValues.mcPrize, newConfigValues.carPrize, newConfigValues.garageSize);
        pragueParking = newPragueParking;
        ParkingSpot[] newParkeringsPlatser = new ParkingSpot[pragueParking.GarageSize];

        Array.Copy(parkeringsPlatser, newParkeringsPlatser, parkeringsPlatser.Length);

        for (int i = parkeringsPlatser.Length; i < newParkeringsPlatser.Length; i++)
        {
            newParkeringsPlatser[i] = new ParkingSpot(0);
        }
        parkeringsPlatser = newParkeringsPlatser;

    }
    SaveParkingSpots();
}
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








