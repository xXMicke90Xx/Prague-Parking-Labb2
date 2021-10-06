using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace Prague_Parking
{
    class Program
    {

        public static string[] myVehicles = new string[100];
        public static int index = 0;
        public static int nextSpot = 0;

        static void Main(string[] args)
        {
            WindowSetup();

            FillNullSpaces();

            string input = "";
            while (input != "6")
            {
                PrintColumnsOfVehicles();
                MainMenu();
                input = GetResponse("Please enter a choice 1-4, or 5 to exit: ");
                MainMenyChoice(input);
                Console.Clear();
            }

            Console.ReadLine();
        }

        static void WindowSetup()
        {
            string setupWindowMessage = @"_________________________________
                                          |                                 |
                                          |                                 |
                                          |        Please put window        |
                                          |          in Fullscreen          |            
                                          |              (F11)              |
                                          |                                 |    
                                          |                                 |
                                          ___________________________________";
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 4);
            Console.WriteLine(setupWindowMessage);
            while (Console.WindowWidth < 200 && Console.WindowTop < 50)
            {


            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
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
                    MoveVehicle(index, nextSpot);
                    break;
                case "3":
                    CheckOut();
                    break;
                case "4":
                    Help();
                    break;
                case "5":
                    //Den ska bara vara break
                    break;
                case "6":
                    {
                        Console.WriteLine("Closing project");
                        Thread.Sleep(60);
                        Environment.Exit(0);
                        break;
                    }
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
                "|        [5] Reset Window                   |",
                "|        [6] Avsluta                        |",
                "|                                           |",
                "_____________________________________________"};


            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(menu[i].PadLeft(Console.WindowWidth / 2 + 22));
            }
        }
        //----------------------- Skriver ut kolummer med alla platser som finns i myCars arrayen---------------------------------------

        #region Print to the screen and color function
        static void PrintColumnsOfVehicles()
        {

            string frameForColumns = "";
            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth - 3, '_'));
            Console.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                Console.Write("          ");
                //Skriver ut 1-25                           
                ColorMatch(myVehicles[i]);
                Console.Write($"{(i < 9 ? "|" + (i + 1) + " " : "|" + (i + 1))} {myVehicles[i].PadRight((Console.WindowWidth / 3) - 19)}|");
                //Skriver ut 26 - 50
                ColorMatch(myVehicles[i + myVehicles.Length / 4]);
                Console.Write($"{i + myVehicles.Length / 4 + 1} {myVehicles[i + myVehicles.Length / 4].PadRight((Console.WindowWidth / 3) - 19)}|");
                //skriver ut 51- 75
                ColorMatch(myVehicles[i + myVehicles.Length / 2]);
                Console.Write($"{i + myVehicles.Length / 2 + 1} {myVehicles[i + myVehicles.Length / 2].PadRight((Console.WindowWidth / 3) - 19)}|");
                //Skriver ut 76 - 100
                ColorMatch(myVehicles[i + (myVehicles.Length / 4) * 3]);
                Console.Write($"{i + ((myVehicles.Length / 4) * 3) + 1}{(i == 24 ? "" : " ")} {myVehicles[i + (myVehicles.Length / 4) * 3]}|");

                Console.WriteLine();
                Console.ResetColor();
            }
            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth - 3, '_'));
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

        #endregion
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
        static int GetResponseAsNumber(string message, ref int index)
        {
            Console.Write($"{message} ");

            string choice = Console.ReadLine();
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2 - 1, Console.CursorTop);
            bool isInt = int.TryParse(choice, out int result);

            result--;
            //sätter resultatet till nuvarande plats om man försöker skriva utanför arrayen
            if (result < 0 || result > 99)
            {
                result = index;
            }
            return result;
        }
        // -------------------------Sak ta emot och lagra vart bilar på tillgänglig plats--------------------------


        #region CheckIn and helper functions to that
        static void CheckIn()
        {

            // Console.Clear();
            string vehicleType = "";
            string checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle ");
            //Sätter fordonstypen
            SelectVehicleType(ref vehicleType, ref checkingIn);

            string registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            registrationNumber = EnterRegistration(registrationNumber);
            DateTime timeCheckedIn = DateTime.Now;

            //flyttar fordon
            VehicleAtCorrectPosition(vehicleType, registrationNumber, timeCheckedIn);

        }

        private static void SelectVehicleType(ref string vehicleType, ref string checkingIn)
        {
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
        }

        private static void VehicleAtCorrectPosition(string vehicleType, string registrationNumber, DateTime timeCheckedIn)
        {
            string correctFormatedString = $"{ vehicleType}#{registrationNumber}#{timeCheckedIn.ToString("MMM-dd HH:mm")}";
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i] == "Ledig" && vehicleType == "CAR")
                {
                    myVehicles[i] = correctFormatedString;
                    break;
                }
                if (myVehicles[i] == "Ledig" && vehicleType == "MC ")
                {
                    myVehicles[i] = correctFormatedString;
                    break;
                }
                else if (myVehicles[i].Contains("MC ") && vehicleType == "MC ")
                {
                    bool isTrue = FoundTwoMatches(myVehicles[i]);
                    if (isTrue != true)
                    {
                        myVehicles[i] += "|";
                        myVehicles[i] += correctFormatedString;
                        break;
                    }
                }
            }
        }
        private static string EnterRegistration(string registrationNumber)
        {
            Regex matchAccents = new Regex(@"[a-zA-ZÀ-ÖØ-öø-ÿ0-9]");
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains(registrationNumber.ToUpper()))
                {
                    registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
                    for (int j = 0; j < registrationNumber.Length; j++)
                    {
                        if (matchAccents.IsMatch(registrationNumber[j].ToString().ToUpper()) != true || registrationNumber.Length > 10 || myVehicles[i].Contains(registrationNumber.ToUpper()))
                        {
                            registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
                        }
                    }
                }
            }
            return registrationNumber.ToUpper();
        }

        #endregion




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

        #region Move Vehicles(s) and helper functions
        static void MoveVehicle(int index, int nextSpot)
        {
            string searchForRegistration = GetResponse("Which registration number do you want to move?");
            searchForRegistration = searchForRegistration.ToUpper();
            //söker efter inmatat registreringsnummer
            bool isFound = SearchForRegistration(ref index, searchForRegistration);


            //TODO: testa && bryta ut i en funktion
            if (isFound == true)
            {
                nextSpot = InsertMovedVehicle(ref index, searchForRegistration);
            }
            else
            {
                Console.SetCursorPosition((Console.WindowWidth) / 2 - 1, Console.CursorTop);
                Console.WriteLine("No matches found");
            }


        }

        private static int InsertMovedVehicle(ref int index, string searchForRegistration)
        {
            int nextSpot;
            do
            {
                Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                Console.WriteLine("Använd ett tal mellan 1 och 100");
                Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                nextSpot = GetResponseAsNumber("Which spot do you want to move the vehicle to?", ref index);

            } while (nextSpot < 0 && nextSpot > 99 || nextSpot == index);

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
                    string[] tempHolder = myVehicles[index].Split("|");
                    if (tempHolder[0].Contains(searchForRegistration))
                    {
                        if (!myVehicles[nextSpot].Contains("CAR"))
                        {
                            myVehicles[nextSpot] = tempHolder[0];
                            myVehicles[index] = tempHolder[1];
                            break;
                        }
                    }
                    else if (tempHolder[1].Contains(searchForRegistration))
                    {
                        if (!myVehicles[nextSpot].Contains("CAR"))
                        {
                            myVehicles[nextSpot] = tempHolder[1];
                            myVehicles[index] = tempHolder[0];
                            break;
                        }
                    }
                }
                else if (!myVehicles[nextSpot].Contains("CAR") && myVehicles[index].Contains("MC ") && FoundTwoMatches(myVehicles[nextSpot]) == false)
                {
                    string[] tempHolder = myVehicles[index].Split("|");
                    if (tempHolder[0].Contains(searchForRegistration))
                    {
                        myVehicles[nextSpot] += "|";
                        myVehicles[nextSpot] += tempHolder[0];
                        myVehicles[index] = "Ledig";
                        break;
                    }
                    else if (tempHolder[1].Contains(searchForRegistration))
                    {
                        myVehicles[nextSpot] += "|";
                        myVehicles[nextSpot] += tempHolder[1];
                        myVehicles[index] = "Ledig";
                        break;
                    }
                }
            }

            return nextSpot;
        }

        private static bool SearchForRegistration(ref int index, string searchForRegistration)
        {
            bool isFound = false;

            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains(searchForRegistration))
                {
                    isFound = true;
                    index = i;
                }
            }
            return isFound;
        }

        #endregion

        static void CheckoutMessage(int index)
        {
            DateTime checkOutTime = DateTime.Now;
            string lol = myVehicles[index].Substring(myVehicles[index].Length - 6);
            string checkOutMessage = $@"___________________________________________
                                       |               Titel: Help                 |
                                       |                                           |
                                       |  The Vehicle is located at ParkingSpace   |
                                       |                 {index + 1}                 |
                                       |    It has been parked for a total of      |
                                       |               {checkOutTime.Hour}                            |
                                       |                                           |
                                       |___________________________________________|";



        }
        //---------------------------------Ska användas för att checka ut bil-----------------------------------------------------------
        static void CheckOut()
        {
            ConsoleKeyInfo cki;

            Console.Write("Please enter the registration number of the car you wish to check out: ");

            Console.WriteLine();
            string RegSearch = "";
            bool userDone = false;
            int savedIndex = -1;
            while (userDone == false)
            {
                cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            if (RegSearch.Length > 0 && savedIndex >= 0)
                            {
                                Console.WriteLine("Are you sure you want to remove the vehicle? Then press enter");
                                cki = Console.ReadKey(true);
                                if (cki.Key == ConsoleKey.Enter)
                                {
                                    CheckoutMessage(savedIndex);

                                    if ((FoundTwoMatches(myVehicles[savedIndex]) == true && myVehicles[savedIndex].Substring(0, myVehicles[savedIndex].IndexOf('|')).Contains(RegSearch.ToUpper())) ||
                                       (FoundTwoMatches(myVehicles[savedIndex]) == true && myVehicles[savedIndex].Substring(myVehicles[savedIndex].IndexOf("|"), 10).Contains(RegSearch.ToUpper())))

                                    {
                                        myVehicles[savedIndex] = ChoseMC(myVehicles, savedIndex, cki);
                                        userDone = true;
                                    }
                                    else if (myVehicles[savedIndex].Substring(0, 3) == "CAR")
                                    {
                                        myVehicles[savedIndex] = "Ledig";
                                        userDone = true;

                                    }
                                    else if (myVehicles[savedIndex].Substring(0, 3) == "MC ")
                                    {
                                        myVehicles[savedIndex] = "Ledig";
                                        userDone = true;
                                    }
                                    else
                                    {
                                        break;
                                    }


                                }
                                else
                                {
                                    var position = Console.CursorTop;
                                    Console.SetCursorPosition(0, position);
                                    CleanScreen(position);


                                    Console.WriteLine("\n\n");
                                    Console.WriteLine("Please enter the registration number of the car you wish to check out: ");




                                    Console.Write($"Registration number: {RegSearch}");


                                    Console.SetCursorPosition(0, position);
                                    break;
                                }

                                break;
                            }
                            else
                            {
                                break;
                            }


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
                                CleanScreen(position);

                                RegSearch = RegSearch.Remove(RegSearch.Length - 1);
                                Console.WriteLine("\n\n");
                                Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                                Console.Write($"Registration number: {RegSearch}");

                                savedIndex = PrintSearchResult(RegSearch.ToUpper());
                                Console.SetCursorPosition(0, position);
                                break;
                            }
                        }
                    default:
                        {
                            if (char.IsLetterOrDigit(cki.KeyChar))
                            {
                                var position = Console.CursorTop;

                                Console.SetCursorPosition(0, position);
                                CleanScreen(position);

                                Console.WriteLine("\n\n");
                                Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                                RegSearch += cki.KeyChar;
                                Console.Write($"Registration number: {RegSearch}");
                                savedIndex = PrintSearchResult(RegSearch.ToUpper());
                                Console.SetCursorPosition(0, position);
                            }
                            break;
                        }
                }
            }
            Console.Clear();
        }

        private static string ChoseMC(string[] removeOneVehicle, int index, ConsoleKeyInfo cki)
        {

            Console.WriteLine("Two Vehicles was found in the space, select one to remove");
            bool madeChoice = false;
            int choice = 0;
            string[] split = removeOneVehicle[index].Split("|");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{split[0]}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");
            Console.Write($"{split[1]}");
            while (madeChoice == false)
            {
                cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.RightArrow:
                        {
                            if (choice != 1)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop);
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"{split[0]} | ");
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.Cyan;

                                Console.Write($"{split[1]}");

                                choice = 1;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (choice != 0)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop);
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write($"{split[0]}");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" | {split[1]}");
                                choice = 0;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    case ConsoleKey.Enter:
                        {
                            madeChoice = true;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            Console.ResetColor();
            if (choice == 0)
            {
                return split[1];
            }
            else
            {
                return split[0];
            }
        }

        //-------------------------------Ska rensa sökningsfunktionen bara utan att röra resten----------------------------------
        static void CleanScreen(int y)
        {
            Console.SetCursorPosition(0, y - 3);
            for (int i = 0; i < 11; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, y - 4);

        }
        //-----------------------------------Söker Lista efter Regnummer---------------------------------------
        static int PrintSearchResult(string toCheck)
        {
            int x = (Console.WindowWidth / 4) * 3;
            int y = 43;
            bool firstMatch = false;
            int numberOfMatches = 0;
            int savedindex = 0;
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains(toCheck) && myVehicles[i] != "Ledig" && numberOfMatches < 10)
                {
                    if (firstMatch == false)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write(myVehicles[i], Console.ForegroundColor = ConsoleColor.Cyan);
                        Console.ResetColor();
                        y++;
                        firstMatch = true;
                        savedindex = i;

                    }
                    else
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(myVehicles[i]);
                        y++;
                        numberOfMatches++;

                    }


                }
            }
            if (firstMatch == true)
                return savedindex;
            else
                return -1;
        }

        //-------------------------------Fyller i alla platser med ett standardvärde--------------------------------
        static void FillNullSpaces()
        {
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i] == null)
                {
                    myVehicles[i] = "Ledig";
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
