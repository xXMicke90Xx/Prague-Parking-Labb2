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
                input = GetResponse("Please enter a choice 1-4, or 5 to exit:");
                MainMenyChoice(input);
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
                "|        [3] Checka utfordon                |",
                "|        [4] Hjälp                          |",
                "|        [5] Avsluta                        |",
                "|                                           |",
                "|                                           |",
                "_____________________________________________"};
            
            
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(menu[i].PadLeft(Console.WindowWidth/2 + 22));
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
                Console.Write($"{(i < 9 ? "|" + (i + 1) + " " : "|" +(i + 1))} {cars[i].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + cars.Length / 4]);
                Console.Write($"{i + cars.Length / 4 + 1} {cars[i + cars.Length / 4].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + cars.Length / 2]);
                Console.Write($"{i + cars.Length / 2 + 1} {cars[i + cars.Length / 2].PadRight((Console.WindowWidth / 3) - 19)}|");

                ColorMatch(cars[i + (cars.Length / 4)*3]);
                Console.Write($"{i + (cars.Length / 4) * 3 + 1}{(i == 24? "": " ")} {cars[i + (cars.Length / 4) * 3]}|");

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
            Console.WriteLine(message);
            string choice = Console.ReadLine();
            bool isInt = int.TryParse(choice, out int result);
            return result;
        }
        // -------------------------Sak ta emot och lagra vart bilar på tillgänglig plats--------------------------
        static void CheckIn()
        {

            Console.Clear();
            string vehicleType = "";
            string checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle");
            while (checkingIn.Trim() != "1" && checkingIn.Trim() != "2")
            {
                Console.WriteLine("Please enter 1 or 2");
                checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle");
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
            //TODO:
            //kolla så inmatad sträng är giltig och med rätt tecken
            string registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            while (registrationNumber.Length > 10)
            {
                registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            }
            registrationNumber = registrationNumber.ToUpper();
            DateTime timeCheckedIn = DateTime.Now;


            //TODO bryt ut i en funktion
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
        //---------------------Bestämmer Konsoll färg -----------------------
        static void ColorMatch (string Vehicle)
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
            if (isFound == true)
                Console.WriteLine($"Vehicle found at index {index}");
            else
                Console.WriteLine("No matches found");


            //TODO kolla så man inte går utanför arrayen
            int nextSpot = GetResponseAsNumber("Which spot do you want to move the vehicle to?");
            //minskar för användaren
            nextSpot--;
            //TODE testa
            while(nextSpot< 0 && nextSpot > 99)
            {
                Console.WriteLine("Använd ett tal mellan 1 och 100");
                nextSpot = GetResponseAsNumber("Which spot do you want to move the vehicle to?");
                nextSpot--;
            }
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[nextSpot].Contains("Ledig"))
                {
                    myVehicles[nextSpot] = myVehicles[index];
                    myVehicles[index] = "Ledig";
                }
            }
        }
        static void CheckOut(string [] vehicles)
        {
            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);
            Console.Write("Please enter the registration number of the car you wish to check out: ");
            Console.WriteLine();
            string isValidCaracter = "";
            while (true)
            {
                cki = Console.ReadKey(true);
                if (char.IsLetterOrDigit(cki.KeyChar))
                {
                    isValidCaracter += cki.KeyChar;
                }



                Console.Write(isValidCaracter);


            }

            


        }
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
        }
    }
}
