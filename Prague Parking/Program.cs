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
        public static string input = "";
        static void Main(string[] args)
        {
            WindowSetup();
            FillNullSpaces();
            Run();
            Console.ReadLine();
        }

        private static void Run()
        {
            while (input != "5")
            {
                PrintColumnsOfVehicles();
                MainMenu();
                input = GetResponse("Please enter a choice 1-4, or 5 to exit: ");
                MainMenyChoice(input);
                Console.Clear();
            }
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
        //test
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
                    //Den ska bara vara break
                    break;
                case "5":
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
                "|        Prague Parking System              |",
                "|                                           |",
                "|        [1] Check In Vehicle               |",
                "|        [2] Move Vehicle                   |",
                "|        [3] Check Out Vehicle              |",
                "|        [4] Reset Window                   |",
                "|        [5] Exit Application               |",
                "|                                           |",
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
                Console.Write($"{i + ((myVehicles.Length / 4) * 3) + 1}{(i == 24 ? "" : " ")} {myVehicles[i + (myVehicles.Length / 4) * 3]}");

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
        static void ColorMatch(byte colorChange)
        {
            if (colorChange == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (colorChange == 1)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
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
        // -------------------------Ska ta emot och lagra vart bilar på tillgänglig plats--------------------------


        #region CheckIn and helper functions to that
        static void CheckIn()
        {
            bool isNoMatch = false;
            // Console.Clear();
            string vehicleType = "";
            string checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle or [3] to exit ");
            //Sätter fordonstypen
            SelectVehicleType(ref vehicleType, ref checkingIn);

            string registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");

            registrationNumber = EnterRegistration(registrationNumber);


            DateTime timeCheckedIn = DateTime.Now;

            //flyttar fordon
            CheckInCorrectPosition(vehicleType, registrationNumber, timeCheckedIn);

        }

        private static void SelectVehicleType(ref string vehicleType, ref string checkingIn)
        {
            while (checkingIn.Trim() != "1" && checkingIn.Trim() != "2" && checkingIn != "3")
            {
                Console.SetCursorPosition((Console.WindowWidth - checkingIn.Length) / 2 - 1, Console.CursorTop);
                Console.WriteLine("Please enter 1 or 2");
                checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle or [3] to exit ");
            }
            switch (checkingIn)
            {
                case "1":
                    vehicleType = "CAR";
                    break;
                case "2":
                    vehicleType = "MC ";
                    break;
                case "3":
                    Console.Clear();
                    Run();
                    break;
            }
        }

        private static void CheckInCorrectPosition(string vehicleType, string registrationNumber, DateTime timeCheckedIn)
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
            Regex matchAccents = new Regex(@"[a-zA-ZÀ-ÖØ-öø-ÿ0-9]{1,10}");
            for (int i = 0; i < registrationNumber.Length; i++)
            {
                while (!matchAccents.IsMatch(registrationNumber[i].ToString().ToUpper()))
                {
                    registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
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
            InsertAtCorrectPosition(index, searchForRegistration, nextSpot);

            return nextSpot;
        }

        private static void InsertAtCorrectPosition(int index, string searchForRegistration, int nextSpot)
        {
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
                //beroende på vart i strängen MCn finns
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
        }

        private static bool SearchForRegistration(ref int index, string searchForRegistration)
        {
            bool isFound = false;

            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains("CAR") || myVehicles[i].Contains("MC ") && FoundTwoMatches(myVehicles[i]) == false)
                {
                    string[] splitIfOne = myVehicles[i].Split('#');
                    if (splitIfOne[1] == searchForRegistration)
                    {
                        isFound = true;
                        index = i;
                    }
                }
                else if (myVehicles[i].Contains("MC ") && FoundTwoMatches(myVehicles[i]) == true)
                {
                    string[] splitIfTwo = myVehicles[i].Split('#', '|');
                    if (splitIfTwo[1] == searchForRegistration || splitIfTwo[4] == searchForRegistration)
                    {
                        isFound = true;
                        index = i;
                    }
                }
            }
            return isFound;
        }

        #endregion
        public static TimeSpan TotalTimeParked(string vehicle)
        {
            
            DateTime checkOutTime = DateTime.Now;
            DateTime checkInTime = Convert.ToDateTime(vehicle.Substring(vehicle.Length - 6));
            
            TimeSpan diff = checkOutTime.Subtract(checkInTime);
            
            
            return diff ;

        }
        static void CheckoutMessage(int index)
        {
            Console.Clear();
            TimeSpan checkOutTime = TotalTimeParked(myVehicles[index-1]);
            
            

            string[] checkOutMessage = new string[11] {
                "_____________________________________________",
                "|                  CheckOut                 |",
                "|                                           |",
                "|  The Vehicle Is Located At Parkingspace   |",
                "|                                           |",
                "|                                           |",
                "|       The total time parked is            |",
                "|                    Minutes                |",
                "|                                           |",
                "|                                           |",
                "_____________________________________________"};

            
            for (int i = 0; i < checkOutMessage.Length; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - checkOutMessage[0].Length/2  , (Console.WindowHeight / 2) - checkOutMessage.Length/2 + i);
                Console.WriteLine(checkOutMessage[i]);
                
            }
            Console.SetCursorPosition((Console.WindowWidth / 2), (Console.WindowHeight / 2) - 1);
            Console.Write(index);
            Console.SetCursorPosition((Console.WindowWidth / 2) - 5, (Console.WindowHeight / 2) + 2);
            Console.Write(Math.Round(checkOutTime.TotalMinutes, 0));
            Console.ReadLine();
        }
        //------------------------Ska användas för att checka ut bil, varje knapptryck registreras-----------------------------------------------------------
        static void CheckOut()
        {
            ConsoleKeyInfo cki;

            

            Console.WriteLine();
            string RegSearch = "";
            bool userDone = false;
            int savedIndex = -1;
            while (userDone == false)
            {
                if (RegSearch == "")
                {
                    var cursorHeight = Console.CursorTop;
                    Console.SetCursorPosition(0, cursorHeight);
                    CleanScreen(cursorHeight);


                    Console.WriteLine("\n\n");
                    Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                    Console.Write($"Registration number: {RegSearch}");
                }
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


                                    if ((FoundTwoMatches(myVehicles[savedIndex]) == true && myVehicles[savedIndex].Substring(0, myVehicles[savedIndex].IndexOf('|')).Contains(RegSearch.ToUpper())) ||
                                       (FoundTwoMatches(myVehicles[savedIndex]) == true && myVehicles[savedIndex].Substring(myVehicles[savedIndex].IndexOf("|"), 10).Contains(RegSearch.ToUpper())))

                                    {
                                        
                                        Console.WriteLine("Two Vehicles was found in the space, select one to remove");
                                        myVehicles[savedIndex] = ChoseMC(savedIndex, cki);
                                        userDone = true;
                                        CheckoutMessage(savedIndex+1);
                                    }
                                    else if (myVehicles[savedIndex].Substring(0, 3) == "CAR")
                                    {
                                        CheckoutMessage(savedIndex+1);
                                        myVehicles[savedIndex] = "Ledig";
                                        userDone = true;

                                    }
                                    else if (myVehicles[savedIndex].Substring(0, 3) == "MC ")
                                    {
                                        CheckoutMessage(savedIndex+1);
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
                                    var cursorHeight = Console.CursorTop;
                                    Console.SetCursorPosition(0, cursorHeight);
                                    CleanScreen(cursorHeight);


                                    Console.WriteLine("\n\n");
                                    Console.WriteLine("Please enter the registration number of the car you wish to check out: ");
                                    Console.Write($"Registration number: {RegSearch}");


                                    Console.SetCursorPosition(0, cursorHeight);
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

                                savedIndex = SearchResult(RegSearch.ToUpper());
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
                                savedIndex = SearchResult(RegSearch.ToUpper());
                                Console.SetCursorPosition(0, position);
                            }
                            break;
                        }
                }
            }
            Console.Clear();
        }

        private static string ChoseMC(int index, ConsoleKeyInfo cki)
        {

            // ColorMatch är en överlagring på en metod "0" och "1" säger vilket färgval man vill ha
            bool madeChoice = false;
            byte choice = 0;
            string[] split = myVehicles[index].Split("|");

            ColorMatch(0);
            Console.Write($"{split[0]}");
            ColorMatch(1);
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
                                ColorMatch(1);
                                Console.Write($"{split[0]} | ");
                                ColorMatch(0);

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
                                ColorMatch(0);
                                Console.Write($"{split[0]}");
                                ColorMatch(1);
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
        static void CleanScreen(int windowHeight)
        {
            Console.SetCursorPosition(0, windowHeight - 3);
            for (int i = 0; i < 11; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, windowHeight - 4);

        }
        //-----------------------------------Söker Lista efter Regnummer---------------------------------------
        static int SearchResult(string toCheck)
        {
            bool firstMatch = false;
            int WindowHeightSetting = 43;
            
            int numberOfMatches = 0;
            int savedindex = 0;
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i].Contains(toCheck) && myVehicles[i] != "Ledig" && numberOfMatches < 10)
                {
                    if (firstMatch == false)
                    {

                        PrintSearchResult(i, firstMatch, WindowHeightSetting);
                        firstMatch = true;
                        savedindex = i;
                        numberOfMatches++;
                        WindowHeightSetting++;
                    }
                    else
                    {
                        PrintSearchResult(i, firstMatch, WindowHeightSetting);
                        numberOfMatches++;
                        WindowHeightSetting++;
                    }


                }
            }
            if (firstMatch == true)
                return savedindex;
            else
                return -1;
        }
        //-------------------------------Skriver ut sökresultat på en specifik plats----------------------------
        static void PrintSearchResult(int index, bool firstMatch, int WindowHeightSetting)
        {
             
            int WindowWidthSetting = (Console.WindowWidth / 4) * 3;
            
            
                    if (firstMatch == false)
                    {
                          
                        Console.SetCursorPosition(WindowWidthSetting, WindowHeightSetting);
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(myVehicles[index]);
                        Console.ResetColor();
                        

                    }
                    else
                    {
                        Console.SetCursorPosition(WindowWidthSetting, WindowHeightSetting);
                        Console.Write(myVehicles[index]);
                        
                         

                    }  
            
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
    }
}
