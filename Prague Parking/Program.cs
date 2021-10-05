using System;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Prague_Parking
{
    class Program
    {

        public static string[] myVehicles = new string[100];


        static void Main(string[] args)
        {
            
            Console.WindowWidth = 240;
            Console.WindowHeight = 63;
            FillNullSpaces(myVehicles);
            PrintColumnsOfVehicles(myVehicles);
            string input = "";
            while (input != "5")
            {

                MainMenu();
                input = GetResponse("Please enter a choice 1-4, or 5 to exit: ");
                MainMenyChoice(input);
                Console.Clear();
                PrintColumnsOfVehicles(myVehicles);
            }

            Console.ReadLine();
        }
        //-----------------------Huvudmeny--------------------------------------
        static void MainMenyChoice(string input)
        {
            switch (input)
            {
                case "1":
                    CheckIn();
                    break;
                case "2":
                    MoveCar();
                    break;
                case "3":
                    CheckOut(myVehicles);
                    break;
                case "4":
                    Help();
                    break;
                case "5":
                    Console.WriteLine("Closing project");
                    Thread.Sleep(60);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter a correct choice");
                    break;
            }
        }

        //--------------------Skriver ut Huvudmenyn -------------------------------
        static void MainMenu()
        {

            string[] menu = new string[11] {
                "_____________________________________________",
                "|       Titel: Prague Parking               |",
                "|                                           |",
                "|        [1] Incheckning av fordon          |",
                "|        [2] Flytta fordon                  |",
                "|        [3] Checka ut fordon               |",
                "|        [4] Hjälp                          |",
                "|        [5] Avsluta                        |",
                "|                                           |",
                "|                                           |",
                "_____________________________________________"};




            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(menu[i].PadLeft(Console.WindowWidth / 2 + 22));
            }
        }
        //----------------------- Skriver ut kolummer med alla platser som finns i myCars arrayen---------------------------------------
        static void PrintColumnsOfVehicles(string[] cars)
        {


            string frameForColumns = "";
            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth - 3, '_'));
            Console.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                Console.Write("          ");

                ColorMatch(cars[i]);
                Console.Write($"{(i < 9 ? "|" + (i + 1) + " " : "|" + (i + 1))} {cars[i].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + cars.Length / 4]);
                Console.Write($"{i + cars.Length / 4 + 1} {cars[i + cars.Length / 4].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + cars.Length / 2]);
                Console.Write($"{i + cars.Length / 2 + 1} {cars[i + cars.Length / 2].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + (cars.Length / 4) * 3]);
                Console.Write($"{i + (cars.Length / 4) * 3 + 1}{(i == 24 ? "" : " ")} {cars[i + (cars.Length / 4) * 3]}|");

                Console.WriteLine();


                Console.ResetColor();
            }

            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth - 3, '_'));


        }
        //-----------------------------Tar emot menyval-------------------------------------------------------
        static string GetResponse(string message)
        {
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2 - 1, Console.CursorTop);
            Console.Write(message);
            string choice = Console.ReadLine();
            Console.WriteLine();
            return choice;
        }
        static int GetResponseAsNumber(string message)
        {
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2 - 1, Console.CursorTop);
            Console.WriteLine(message);
            string choice = Console.ReadLine();
            bool isInt = int.TryParse(choice, out int result);

            result--;
            if (result < 0)
                result = 0;
            //TODO: behåll samma index
            else if (result > 99)
                result = 0;
            return result;
        }
        // -------------------------Sak ta emot och lagra vart bilar på tillgänglig plats--------------------------


        //TODO: kolla så regnummer inte redan finns i listan
        //TODO: kolla så regnummer inte matas in med fel tecken
        static void CheckIn()
        {

            // Console.Clear();
            string vehicleType = "";
            string checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle ");
            while (checkingIn.Trim() != "1" && checkingIn.Trim() != "2")
            {
                Console.SetCursorPosition((Console.WindowWidth - checkingIn.Length) / 2 - 1, Console.CursorTop);
                Console.WriteLine("Please enter 1 or 2");
                checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle ");
            }
            switch (checkingIn)
            {
                case "1":
                    vehicleType = "CAR";
                    break;
                case "2":
                    vehicleType = "MC ";
                    break;
            }

            string registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            //kollar så att det inmatade registreringsnummret är max 10 tecken lång & inte redan finns
            registrationNumber = EnterRegistration(registrationNumber);

            DateTime timeCheckedIn = DateTime.Now;


            //flyttar fordon
            VehicleAtCorrectPosition(vehicleType, registrationNumber, timeCheckedIn);

        }

        private static void VehicleAtCorrectPosition(string vehicleType, string registrationNumber, DateTime timeCheckedIn)
        {
            string final = $"{ vehicleType}#{registrationNumber}#{timeCheckedIn.ToString("MMM-dd HH:mm")}";
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i] == "Ledig" && vehicleType == "CAR")
                {
                    myVehicles[i] = final;
                    break;
                }
                if (myVehicles[i] == "Ledig" && vehicleType == "MC ")
                {
                    myVehicles[i] = final;
                    break;
                }
                else if (myVehicles[i].Contains("MC ") && vehicleType == "MC ")
                {
                    bool isTrue = FoundTwoMatches(myVehicles[i]);
                    if (isTrue != true)
                    {
                        myVehicles[i] += "|";
                        myVehicles[i] += final;
                        break;
                    }
                }
            }
        }

        //TODO fixa logiken
        private static string EnterRegistration(string registrationNumber)
        {
            Regex matchAccents = new Regex(@"[a-zA-ZÀ-ÖØ-öø-ÿ0-9]{1,10}");

            for (int i = 0; i < myVehicles.Length; i++)
            {

                while (myVehicles[i].Contains(registrationNumber.ToUpper()) || matchAccents.IsMatch(registrationNumber.ToUpper()) != true)
                {
                    registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
                }
            }
            return registrationNumber.ToUpper();
        }

        //---------------------Bestämmer Konsoll färg -----------------------
        static void ColorMatch(string Vehicle)
        {
            if (Vehicle.Substring(0, 3) == "CAR")
            {
                Console.ForegroundColor = ConsoleColor.Red;

            }
            else if (Vehicle.Substring(0, 3) == "MC " && FoundTwoMatches(Vehicle.ToString()) == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (Vehicle.Substring(0, 3) == "MC " && FoundTwoMatches(Vehicle.ToString()))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (Vehicle == "Ledig")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

        }

        static bool FoundTwoMatches(string finalString)
        {
            bool isFound = false;
            int firstMatch = finalString.IndexOf("MC ");
            int secondMatch = finalString.IndexOf("MC ", firstMatch + 1);
            if (secondMatch != -1)
                isFound = true;
            else
                isFound = false;
            return isFound;
        }
        static void MoveCar()
        {


            string searchForRegistration = GetResponse("Which registration number do you want to move?");
            searchForRegistration = searchForRegistration.ToUpper();

            //TODO bryta ut till en sökfunktion
            bool isFound = false;
            int index = 0;
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains(searchForRegistration))
                {
                    isFound = true;
                    index = i;
                }
            }




            int nextSpot = 0;
            //TODO: testa && bryta ut i en funktion
            if (isFound == true)
            {
                do
                {
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine($"Vehicle found at index {index}");
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("Använd ett tal mellan 1 och 100");
                    nextSpot = GetResponseAsNumber("Which spot do you want to move the vehicle to?");

                } while (nextSpot < 0 && nextSpot >= 99);

            }
            else
            {
                Console.SetCursorPosition((Console.WindowWidth) / 2 - 1, Console.CursorTop);
                Console.WriteLine("No matches found");
            }


            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[nextSpot].Contains("Ledig") && myVehicles[index].Contains("CAR"))
                {
                    myVehicles[nextSpot] = myVehicles[index];
                    myVehicles[index] = "Ledig";
                }
                //om en motorcykel finns
                else if (myVehicles[nextSpot].Contains("Ledig") && myVehicles[index].Contains("MC ") && FoundTwoMatches(myVehicles[index]) == false)
                {
                    myVehicles[nextSpot] = myVehicles[index];
                    myVehicles[index] = "Ledig";
                }
                //om två motorcyklar finns finns
                else if (myVehicles[nextSpot].Contains("Ledig") && myVehicles[index].Contains("MC ") && FoundTwoMatches(myVehicles[index]) == true)
                {
                    //myVehicles[nextSpot] = myVehicles[index];
                    //myVehicles[index] = "Ledig";

                }
            }
        }
        //---------------------------------Ska användas för att checka ut bil-----------------------------------------------------------
        static void CheckOut(string[] vehicles)
        {
            ConsoleKeyInfo cki;

            Console.Write("Please enter the registration number of the car you wish to check out: ");
            Console.WriteLine();
            string RegSearch = "";
            bool userDone = false;
            while (userDone == false)
            {

                cki = Console.ReadKey(true);


                switch (cki.Key)
                {


                    case ConsoleKey.Enter:
                        {
                            userDone = true;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            userDone = true;
                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            if (RegSearch.Length < 1)
                                break;
                            else
                            {
                                var position = Console.CursorTop;
                                Console.SetCursorPosition(0, position);
                                CleanScreen(0, position);
                               
                                RegSearch = RegSearch.Remove(RegSearch.Length - 1);
                                Console.WriteLine("\n\n");
                                Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                               
                               
                                
                                
                                Console.Write($"Registration number: {RegSearch}");

                                PrintSearchResult(RegSearch.ToUpper(), myVehicles);
                                break;
                            }
                        }
                    case ConsoleKey.UpArrow:
                        {
                            break;
                        }

                    case ConsoleKey.DownArrow:
                        {
                            break;
                        }
                    default:
                        {
                            if (char.IsLetterOrDigit(cki.KeyChar))
                            {
                                var position = Console.CursorTop;
                                Console.SetCursorPosition(0, position);
                                CleanScreen(0, position);
                                
                                Console.WriteLine("\n\n");
                                Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                                RegSearch += cki.KeyChar;
                                Console.Write($"Registration number: {RegSearch}");
                                PrintSearchResult(RegSearch.ToUpper(), myVehicles);

                            }
                            break;

                        }
                }

            }
            Console.Clear();

        }
        //-------------------------------Ska rensa sökningsfunktionen bara utan att röra resten----------------------------------
        static void CleanScreen(int x, int y)
        {
            Console.SetCursorPosition(0, y-3);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(new string (' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, y - 4);
            
        }
        //-----------------------------------Söker Lista efter Regnummer---------------------------------------
        static void PrintSearchResult(string toCheck, string[] listOfVehicles)
        {
            int x = 200;
            int y = 43;
            string[] foundVehicles = new string[listOfVehicles.Length];
           
            for (int i = 0; i < listOfVehicles.Length; i++)
            {
                if (listOfVehicles[i].Contains(toCheck) && listOfVehicles[i] != "Ledig")
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(listOfVehicles[i]);
                    y++;
                    
                }
            }
        }

        //-------------------------------Fyller i alla platser med ett standardvärde--------------------------------
        static void FillNullSpaces(string[] Vehicles)
        {
            for (int i = 0; i < Vehicles.Length; i++)
            {
                if (Vehicles[i] == null)
                {
                    Vehicles[i] = "Ledig";
                }
            }
        }
        static void Help()
        {

            //string help = @"___________________________________________
            //               |               Titel: Help                 |
            //               |                                           |
            //               | [1] How to check in Car/MC                |
            //               |    -> Enter a valid register number and   |
            //               |       what type of vehicle you want to    |
            //               |       park.                               |
            //               | [2] How to move Car/MC                    |
            //               |    -> Enter the register number of the    |
            //               |       vehicle and find a new parking lot. |
            //               | [3] How to remove Car/MC                  |
            //               |    -> Find the vehicle you want to remove |
            //               |       and enter it's registernumber.      |
            //               | [4] Exit                                  |
            //               |___________________________________________|";

            string help = @"___________________________________________
                           |               Titel: Help                 |
                           |                                           |
                           | [1] How to check in Car/MC                |
                           | [2] How to move Car/MC                    |
                           | [3] How to remove Car/MC                  |
                           | [4] Exit                                  |
                           |                                           |
                           |___________________________________________|";
            Console.WriteLine(help);
        }
    }
}
