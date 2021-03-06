using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace Prague_Parking
{
    class Program
    {
        // Projekt med Mohammed Karimi, Patrik Beijar Odh och Mikael Nilsson.
        public static string[] myVehicles = new string[100];
        public static int index = 0;
        public static int nextSpot = 0;
        public static string input = "";
        static void Main(string[] args)
        {
            WindowSetup();
            FillNullSpaces();
            Run();
        }

        private static void Run()
        {
            while (input != "6")
            {
                PrintColumnsOfVehicles();
                MainMenu();
                input = GetResponse("Please enter a choice 1-5, or 6 to exit: ");
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
                                           |_________________________________|";
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
                    CheckInVehicle();
                    break;
                case "2":
                    MoveVehicle();
                    break;
                case "3":
                    ActiveSearch("CheckOut", ref index);
                    break;

                case "4":
                    //Den ska bara vara break
                    break;
                case "5":
                    MainHelpMenu();
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
                "|        Prague Parking System              |",
                "|                                           |",
                "|        [1] Check In Vehicle               |",
                "|        [2] Move Vehicle                   |",
                "|        [3] Check Out Vehicle              |",
                "|        [4] Reset Window                   |",
                "|        [5] Help                           |",
                "|        [6] Exit Application               |",
                "|                                           |",
                "|___________________________________________|"};


            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(menu[i].PadLeft(Console.WindowWidth / 2 + 22));
            }
        }
        //----------------------- Skriver ut kolummer med alla platser som finns i myCars arrayen---------------------------------------

        #region Print to the screen and color function
        static void PrintColumnsOfVehicles()
        {
            int toLong = Console.WindowWidth - (((Console.WindowWidth / 4) * 3) + 24);
            string frameForColumns = "";
            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth, '_'));
            Console.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                Console.Write("|     ");
                //Skriver ut 1-25                           
                ColorMatch(myVehicles[i]);
                Console.Write($"{(i < 9 ? "|" + (i + 1) + " " : "|" + (i + 1))} {myVehicles[i].PadRight((Console.WindowWidth / 4))}|");
                //Skriver ut 26 - 50
                ColorMatch(myVehicles[i + myVehicles.Length / 4]);
                Console.Write($"{i + myVehicles.Length / 4 + 1} {myVehicles[i + myVehicles.Length / 4].PadRight((Console.WindowWidth / 4))}|");
                //skriver ut 51- 75
                ColorMatch(myVehicles[i + myVehicles.Length / 2]);
                Console.Write($"{i + myVehicles.Length / 2 + 1} {myVehicles[i + myVehicles.Length / 2].PadRight((Console.WindowWidth / 4))}|");
                //Skriver ut 76 - 100
                ColorMatch(myVehicles[i + (myVehicles.Length / 4) * 3]);
                bool twoMC = FoundTwoMatches(myVehicles[i + myVehicles.Length / 4 * 3]);

                Console.Write($"{i + ((myVehicles.Length / 4) * 3) + 1}{(i == 24 ? "" : " ")} {(twoMC == true && myVehicles[i + myVehicles.Length / 4 * 3].Length >= toLong ? ShortenMatch(myVehicles[i + myVehicles.Length / 4 * 3]) : myVehicles[i + (myVehicles.Length / 4) * 3])}");
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.CursorTop);
                Console.ResetColor();
                Console.Write("|");

            }
            Console.WriteLine(frameForColumns.PadRight(Console.WindowWidth, '_'));
        }
        //------------------------Kortar ner strängen så den inte går utanför från skärmen--------------------------
        static string ShortenMatch(string vehicles)
        {
            string[] split = new string[2];
            split = vehicles.Split("|");
            split[0] = split[0].Substring(0, split[0].LastIndexOf("#"));
            split[1] = split[1].Substring(0, split[1].LastIndexOf("#"));

            return $@"{split[0]}|{split[1]}";
        }

        //---------------------Bestämmer Konsoll färg ---------------------------------
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
        static void ColoredChoice(string choice)
        {
            if (choice == "FirstChoice")
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if (choice == "SecondChoice")
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
                string newMessage = "Invalid move, minimum range is 1 and maximum is 100";
                Console.SetCursorPosition((Console.WindowWidth - newMessage.Length) / 2 - 1, Console.CursorTop);
                Console.WriteLine(newMessage);
                Console.ReadKey();
            }
            return result;
        }
        // -------------------------Ska ta emot och lagra bilar på tillgänglig plats--------------------------

        #region CheckIn and helper functions to that
        static void CheckInVehicle()
        {
            bool isNoMatch = false;
            string vehicleType = "";
            string checkingIn = GetResponse("[1] check in a Car or [2] check in motorcykle or [3] to exit ");
            //Sätter fordonstypen
            SelectVehicleType(ref vehicleType, ref checkingIn);
            string registrationNumber = "";

            //skriver in regnummer + kollar så inga dubletter finns
            do
            {
                registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
                registrationNumber = EnterRegistration(registrationNumber);
                isNoMatch = SearchForRegistration(ref index, registrationNumber);
                
            } while (isNoMatch == true || string.IsNullOrEmpty(registrationNumber) || registrationNumber.Contains("CAR") || registrationNumber.Contains("MC"));
            DateTime timeCheckedIn = DateTime.Now;

            //flyttar fordon
            CheckInCorrectPosition(vehicleType, registrationNumber, timeCheckedIn);
        }

        //------------------------Användaren får välja ett fordon att checka in----------------------------------
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
        //------------------------------------Checkar in fordon, skriver dom till sträng--------------------------------------
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
        //--------------------------Kontrollerar så regnummret är godkänt----------------------------------
        private static string EnterRegistration(string registrationNumber)
        {
            Regex matchAccents = new Regex(@"[a-zA-ZÀ-ÖØ-öø-ÿ0-9]");
            for (int i = 0; i < registrationNumber.Length; i++)
            {
                //byt till isnullorempty
                while (!matchAccents.IsMatch(registrationNumber[i].ToString().ToUpper()) && !Char.IsLetterOrDigit(registrationNumber[i]) || registrationNumber.Length > 10)
                {
                    registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
                    i = 0;
                }
            }
            return registrationNumber.ToUpper();
        }

        #endregion
        //------------------------Kollar ifall det redan finns en MC på positionen eller inte----------------------------------------------------
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
        static void MoveVehicle()
        {
            string searchForRegistration = "";
            searchForRegistration = ActiveSearch("Move", ref index);
            if (!string.IsNullOrWhiteSpace(searchForRegistration))
            {
                InsertMovedVehicle(ref index, searchForRegistration);
            }
        }
        //-----------------------------------------------------------------------------------------------
        private static int InsertMovedVehicle(ref int index, string searchForRegistration)
        {
            int nextSpot;
            do
            {
                Console.SetCursorPosition((Console.WindowWidth) / 2 - 13, Console.CursorTop);
                Console.WriteLine("Use a number between 1-100");
                Console.SetCursorPosition((Console.WindowWidth) / 2 - 23, Console.CursorTop);
                nextSpot = GetResponseAsNumber("Which spot do you want to move the vehicle to?", ref index);

            } while (nextSpot < 0 && nextSpot > 99 || nextSpot == index);
            InsertAtCorrectPosition(index, searchForRegistration, ref nextSpot);

            return nextSpot;
        }
        //----------------------------------Sätter in fordon på en korrekt position--------------------------------
        private static void InsertAtCorrectPosition(int index, string searchForRegistration, ref int nextSpot)
        {
            
                //sätter in en bil på rätt plats
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
                //om två motorcyklar finns 
                else if (myVehicles[nextSpot].Contains("Ledig") && myVehicles[index].Contains("MC ") && FoundTwoMatches(myVehicles[index]) == true)
                {
                    //om första platsen innehåller rätt regnummer
                    string[] tempHolder = myVehicles[index].Split("|");
                    if (tempHolder[0].Contains(searchForRegistration))
                    {
                        if (!myVehicles[nextSpot].Contains("CAR")   )
                        {
                            myVehicles[nextSpot] += "|";
                            myVehicles[nextSpot] = tempHolder[0];
                            myVehicles[index] = tempHolder[1];
                        }
                    }
                    //om andra platsen innehåller rätt regnummer
                    else if (tempHolder[1].Contains(searchForRegistration))
                    {
                        if (!myVehicles[nextSpot].Contains("CAR"))
                        {
                            myVehicles[nextSpot] += "|";
                            myVehicles[nextSpot] = tempHolder[1];
                            myVehicles[index] = tempHolder[0];
                        }
                    }
                }
                //beroende på vart i strängen MCn finns
                else if (myVehicles[index].Contains("MC ") && FoundTwoMatches(myVehicles[nextSpot]) == false && FoundTwoMatches(myVehicles[index]) == false && !myVehicles[nextSpot].Contains("CAR"))
                {
                        myVehicles[nextSpot] += "|";
                        myVehicles[nextSpot] += myVehicles[index];
                        myVehicles[index] = "Ledig";
                        
                }
                else if(FoundTwoMatches(myVehicles[index]) == true && FoundTwoMatches(myVehicles[nextSpot]) == false && !myVehicles[nextSpot].Contains("CAR"))
                {
                    string[] tempHolder = myVehicles[index].Split("|");
                    if (tempHolder[0].Contains(searchForRegistration))
                    {
                        myVehicles[nextSpot] += "|";
                        myVehicles[nextSpot] += tempHolder[0];
                        myVehicles[index] = tempHolder[1];
                    }
                    else if (tempHolder[1].Contains(searchForRegistration))
                    {
                        myVehicles[nextSpot] += "|";
                        myVehicles[nextSpot] += tempHolder[1];
                        myVehicles[index] = tempHolder[0];
                    }

                }
                else
                {
                    // skickar felmeddelanden:

                    if (myVehicles[index].Contains("CAR") && myVehicles[nextSpot].Contains("CAR") || myVehicles[nextSpot].Contains("CAR") && myVehicles[index].Contains("CAR"))
                    {
                        ContainsCarAndCar();
                        
                    }
                    else if (myVehicles[index].Contains("CAR") && myVehicles[nextSpot].Contains("MC "))
                    {
                        ContainsCarAndMC();
                        
                    }
                    else if (myVehicles[index].Contains("MC ") && myVehicles[nextSpot].Contains("CAR"))
                    {
                        ContainsMcAndCar();
                        
                    }
                    else if (FoundTwoMatches(myVehicles[nextSpot]) == true)
                    {
                        ContainsTwoMc();
                    }
                }
        }
       

        private static void ContainsTwoMc()
        {
            string newMessage = "A motorcykle can't move to a place containing two motorcykles!";
            Console.SetCursorPosition((Console.WindowWidth - newMessage.Length) / 2 - 1, Console.CursorTop);
            Console.WriteLine(newMessage);
            Console.ReadLine();
        }

        private static void ContainsMcAndCar()
        {
            string newMessage = "A motorcykle can't move to a place containing a car!";
            Console.SetCursorPosition((Console.WindowWidth - newMessage.Length) / 2 - 1, Console.CursorTop);
            Console.WriteLine(newMessage);
            Console.ReadLine();
        }
        private static void ContainsCarAndMC()
        {
            string newMessage = "A car can't move to a place containing a mc!";
            Console.SetCursorPosition((Console.WindowWidth - newMessage.Length) / 2 - 1, Console.CursorTop);
            Console.WriteLine(newMessage);
            Console.ReadLine();
        }
        private static void ContainsCarAndCar()
        {
            string newMessage = "A car can't move to a place containing a car!";
            Console.SetCursorPosition((Console.WindowWidth - newMessage.Length) / 2 - 1, Console.CursorTop);
            Console.WriteLine(newMessage);
            Console.ReadLine();
        }
        //Används för att kontrollera så att samma regnummer inte existerar vid incheckning av nytt fordon
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
                        break;
                    }
                }
                else if (myVehicles[i].Contains("MC ") && FoundTwoMatches(myVehicles[i]) == true)
                {
                    string[] splitIfTwo = myVehicles[i].Split('#', '|');
                    if (splitIfTwo[1] == searchForRegistration)
                    {
                        isFound = true;
                        index = i;
                        break;
                    }
                    else if (splitIfTwo[4] == searchForRegistration)
                    {
                        isFound = true;
                        index = i;
                        break;
                    }
                }
            }

            return isFound;
        }
        #endregion
        //---------Beräknar tiden från incheckning till utcheckning--------------------
        public static TimeSpan TotalTimeParked(string vehicle)
        {
            DateTime checkOutTime = DateTime.Now;
            DateTime checkInTime = Convert.ToDateTime(vehicle.Substring(vehicle.Length - 6));
            TimeSpan diff = checkOutTime.Subtract(checkInTime);

            return diff;
        }
        //------------------------------Skriver ut ruta för men information för användare om vart fordon finns och hur länge den har stått där-------------------------------------------
        static void CheckoutMessage(int index)
        {
            Console.Clear();
            TimeSpan checkOutTime = TotalTimeParked(myVehicles[index - 1]);



            string[] checkOutMessage = new string[11] {
                "_____________________________________________",
                "|                  CheckOut                 |",
                "|                                           |",
                "|  The Vehicle Is Located At Parkingspace   |",
                "|                                           |",
                "|                                           |",
                "|         The Total Time Parked Is          |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|___________________________________________|"};


            for (int i = 0; i < checkOutMessage.Length; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - checkOutMessage[0].Length / 2, (Console.WindowHeight / 2) - checkOutMessage.Length / 2 + i);
                Console.WriteLine(checkOutMessage[i]);

            }
            Console.SetCursorPosition((Console.WindowWidth / 2), (Console.WindowHeight / 2) - 1);
            Console.Write(index);
            Console.SetCursorPosition((Console.WindowWidth / 2) - 10, (Console.WindowHeight / 2) + 3);
            Console.Write(Math.Round((double)checkOutTime.Days, 0) + " " + "Days " + Math.Round((double)checkOutTime.Hours, 0).ToString() + " Hours " + Math.Round(checkOutTime.TotalMinutes, 0) + " Minutes");
            Console.ReadLine();
        }

        //----------------------------Skriver ut text beroende på vilket menyval som har gjorts--------------------------------
        public static void MoveOrCheckOutMessage(string RegSearch, string checkOrMove)
        {

            Console.WriteLine("\n\n");
            Console.WriteLine($"Please enter the registration number of the car you wish to {checkOrMove}.");
            Console.Write($"Registration number: {RegSearch}");


        }
        //------------------------Ska användas för att definera vilket regnummer man vill flytta/tabort, varje knapptryck registreras-----------------------------------------------------------
        static string ActiveSearch(string checkOrMove, ref int index)
        {
            ConsoleKeyInfo cki;

            int cursorHeight = 44; // Standardvärdet för utskriftshöjd
            Console.WriteLine();
            string regSearch = ""; // Regnummret att söka efter
            bool userDone = false;
            index = -1;
            Instructions(0, cursorHeight);  // Skriver ut en ruta med lite instruktioner
            Console.SetCursorPosition(0, cursorHeight);
            while (userDone == false)
            {
                if (regSearch == "")
                {
                    SearchTextClearer(); //Kommer användas upprepande för att endast rensa tecken (alla rader) under menyn
                    MoveOrCheckOutMessage(regSearch, checkOrMove);
                    Box();
                }

                cki = Console.ReadKey(true); // Basen för den aktiva sökningen

                switch (cki.Key)
                {
                    case ConsoleKey.Enter: //Rätt regnummer är upplyst pch användaren vill använda det
                        {
                            if (regSearch.Length > 0 && index >= 0)
                            {
                                userDone = true;
                                return UserHasChosen(checkOrMove, ref index, cki, regSearch);  // Retunerar regnummer till MoveVehicle om tillkallad därifrån  
                            }
                            else
                            {
                                SearchTextClearer(); //Rensar undre del av skärm
                                MoveOrCheckOutMessage(regSearch, checkOrMove); //Skriver ut standardtext beroende på vartifrån metoden har kallats
                                Console.SetCursorPosition(0, cursorHeight);
                                break;
                            }
                        }
                    case ConsoleKey.Escape: //Avbryter sökningen och går till huvudmenyn
                        {
                            userDone = true;
                            break;
                        }
                    case ConsoleKey.Backspace: // Tar bort sökt(a) tecken, detta sker med ett tryck- ett tecken - rensa - skriv ut nuvarande sökning
                        {
                            if (regSearch.Length < 1)
                                break;
                            else
                            {
                                SearchTextClearer(); //Rensar undre del av skärm
                                regSearch = regSearch.Remove(regSearch.Length - 1);
                                MoveOrCheckOutMessage(regSearch, checkOrMove); //Skriver ut standardtext beroende på vartifrån metoden har kallats
                                Box(); //Snygg box för att skilja aktuellt val mot resten som finns
                                index = SearchResult(regSearch.ToUpper(), checkOrMove); // Används för att uppdatera index för myVehicle mot sökning baserat på lägsta index -> flest matchande tecken
                                Console.SetCursorPosition(0, cursorHeight);
                                break;
                            }
                        }
                    default: // Lägger till tecken, detta sker med ett tryck- ett tecken - rensa - skriv ut nuvarande sökning
                        {
                            if (char.IsLetterOrDigit(cki.KeyChar))
                            {
                                SearchTextClearer(); //Rensar undre del av skärm
                                regSearch += cki.KeyChar;
                                MoveOrCheckOutMessage(regSearch, checkOrMove); //Skriver ut standardtext beroende på vartifrån metoden har kallats
                                Box(); //Snygg box för att skilja aktuellt val mot resten som finns
                                index = SearchResult(regSearch.ToUpper(), checkOrMove); // Används för att uppdatera index för myVehicle mot sökning baserat på lägsta index -> flest matchande tecken
                                Console.SetCursorPosition(0, cursorHeight);
                            }
                           
                            break;
                        }
                }
            }
            Console.Clear();
            return "";
        }
        //----------------------------Ska, beroende på om move eller checkout är aktivt, skicka tillbaka en sträng med regnummer eller ta bort ett fordon-------------------------
        public static string UserHasChosen(string checkOrMove, ref int index, ConsoleKeyInfo cki, string regSearch)
        {           //Denna if sats kollar helt enkelt ifall det finns 2 mc på platsen samt om regSearch finns i någon av fordonens regplåt
            if ((FoundTwoMatches(myVehicles[index]) == true && myVehicles[index].Substring(0, myVehicles[index].IndexOf('|')).Contains(regSearch.ToUpper())) ||
                (FoundTwoMatches(myVehicles[index]) == true && myVehicles[index].Substring(myVehicles[index].IndexOf("|"), 14).Contains(regSearch.ToUpper())))
            {
                Console.WriteLine("Two Vehicles was found in the space, please select one");  // Beror på ifall "move" eller "Checkout" är aktivt
                if (checkOrMove == "Move")
                {
                    return OneMCRemove(ref index, cki, checkOrMove);
                }
                else
                {
                    myVehicles[index] = OneMCRemove(ref index, cki, checkOrMove);
                    CheckoutMessage(index + 1);
                }

            }
            else if (myVehicles[index].Substring(0, 3) == "CAR")
            {
                if (checkOrMove == "Move")
                {
                    return myVehicles[index];
                }
                else
                {
                    CheckoutMessage(index + 1);
                    myVehicles[index] = "Ledig";
                }
            }
            else if (myVehicles[index].Substring(0, 3) == "MC ")
            {
                if (checkOrMove == "Move")
                {
                    return myVehicles[index];
                }
                else
                {
                    CheckoutMessage(index + 1);
                    myVehicles[index] = "Ledig";
                }
            }
            return "";
        }
        //----------------Rensar Text på specifik position-----------------
        public static void SearchTextClearer()
        {
            var position = Console.CursorTop;
            CleanScreen(position);
        }
        //--------------------------------------Vid sökning och två fordon finns på samma plats, låter användaren välja ett fordon------------------------------------------------------- 
        private static string OneMCRemove(ref int index, ConsoleKeyInfo cki, string checkOrMove)
        {
            // Ska lysa upp bakgrunden på ett av 2 regnummer när 2 mc återfinns på samma plats och en av dom ska väljas
            //
            bool madeChoice = false;
            byte choice = 0;
            string[] split = myVehicles[index].Split("|");

            ColoredChoice("FirstChoice"); //
            Console.Write($"{split[0]}");
            ColoredChoice("SecondChoice");
            Console.Write(" | ");
            Console.Write($"{split[1]}");

            while (madeChoice == false)
            {
                cki = Console.ReadKey(true);

                switch (cki.Key)
                {
                    case ConsoleKey.RightArrow: //Högra sidan ska ha vit bakgrund och svarta tecken
                        {
                            if (choice != 1)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop);
                                ColoredChoice("SecondChoice");
                                Console.Write($"{split[0]} | ");
                                ColoredChoice("FirstChoice");
                                Console.Write($"{split[1]}");

                                choice = 1;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    case ConsoleKey.LeftArrow: //Vänstra sidan ska ha vit bakgrund och svarta tecken
                        {
                            if (choice != 0)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop);
                                ColoredChoice("FirstChoice");
                                Console.Write($"{split[0]}");
                                ColoredChoice("SecondChoice");
                                Console.Write($" | {split[1]}");
                                choice = 0;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    case ConsoleKey.Enter: //Användaren har valt fordon
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
            // ska, beroende på om det ska flyttas ett fordon eller tas bort ett fordon, retunera registreringsnummret, move retunerar det sökta, remove det osökta(för att skrivas ensam på det indexet)
            if (choice == 0 && checkOrMove == "Move")
            {
                return split[0].Substring(split[0].IndexOf('#') + 1, split[0].LastIndexOf('#') - 4);
            }
            if (choice == 0 && checkOrMove == "CheckOut")
            {
                return split[1];
            }
            else if (choice == 1 && checkOrMove == "Move")
            {
                return split[1].Substring(split[1].IndexOf('#') + 1, split[1].LastIndexOf('#') - 4);
            }
            else
            {
                return split[0];
            }
        }
        //-------------------------------------Skriver ut instruktionsruta vid sökning av fordon------------------------------------
        static void Instructions(int cursorX, int cursorY)
        {
            string[] instructionsToPrint = new string[7] {
                "_________________________________",
                "|                               |",
                "|    Press Enter To Choose      |",
                "|                               |",
                "|   Press ESC To Go To Abort    |",
                "|                               |",
                "|_______________________________|"};

            Console.SetCursorPosition(cursorX, cursorY - instructionsToPrint.Length - 3);
            for (int i = 0; i < instructionsToPrint.Length; i++)
            {
                Console.WriteLine(instructionsToPrint[i].PadLeft(10));
            }
        }
        //-------------------------------Ska rensa sökningsfunktionen bara utan att röra resten----------------------------------
        static void CleanScreen(int windowHeight)
        {

            Console.SetCursorPosition(0, windowHeight - 3);
            for (int i = 0; i < 15; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, windowHeight - 4);

        }
        static void CleanScreen(int windowWidth, int windowHeight)
        {
            Console.SetCursorPosition(windowWidth, windowHeight);
            for (int i = 0; i < 15; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, windowHeight - 4);
        }
        //-----------------------------------Söker Lista efter Regnummer---------------------------------------
        static int SearchResult(string toCheck, string checkOrMove)
        {
            bool exactMatch = false;
            bool firstMatch = false;
            int WindowHeightSetting = 43;
            string tempReg = ""; 
            int numberOfMatches = 0;
            int savedindex = 0;
            for (int i = 0; i < myVehicles.Length; i++)
            {
                if (myVehicles[i] != "Ledig" && FoundTwoMatches(myVehicles[i]) != true) // inte ledig och ej 2 parkerade MCs
                {
                    tempReg = myVehicles[i].Substring(myVehicles[i].IndexOf('#') + 1, myVehicles[i].LastIndexOf('#') - 4);
                }
                else if (FoundTwoMatches(myVehicles[i]) == true)  // Om två MCs återfanns
                {
                    string[] split = myVehicles[i].Split("|");
                    if (split[1].Substring(split[1].IndexOf('#') + 1, split[1].LastIndexOf('#') - 4).Contains(toCheck)) //Första regnummret en match? 
                    {
                        tempReg = split[1].Substring(split[1].IndexOf('#') + 1, split[1].LastIndexOf('#') - 4);
                    }
                    else if (split[0].Substring(split[0].IndexOf('#') + 1, split[0].LastIndexOf('#') - 4).Contains(toCheck)) //Andra regnummret en match? 
                    {
                        tempReg = split[0].Substring(split[0].IndexOf('#') + 1, split[0].LastIndexOf('#') - 4);
                    }
                }
                //  Själva spar och utskrifsfunktionen. firstmatch = det som användaren kommer få välja, resten blir bara utskrivet (up till totalt 10 regnummer)
                
                if (tempReg == toCheck)
                {

                    WindowHeightSetting = 43;
                    PrintSearchResult(i, firstMatch, WindowHeightSetting, tempReg, toCheck);
                    firstMatch = true;
                    exactMatch = true;
                    savedindex = i;
                    numberOfMatches++;
                    WindowHeightSetting++;
                    i = myVehicles.Length;
                }
                
                else if (tempReg.Contains(toCheck) && numberOfMatches < 10 && myVehicles[i] != "Ledig")
                {
                    if (firstMatch == false && exactMatch == false)
                    {

                        PrintSearchResult(i, firstMatch, WindowHeightSetting, tempReg, toCheck);
                        firstMatch = true;
                        savedindex = i;
                        numberOfMatches++;
                        WindowHeightSetting++;
                    }
                    else
                    {
                        PrintSearchResult(i, firstMatch, WindowHeightSetting, tempReg, toCheck);
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
        static void PrintSearchResult(int index, bool firstMatch, int WindowHeightSetting, string tempReg, string toSearch)
        {

            int WindowWidthSetting = (Console.WindowWidth / 4) * 3;
            if (tempReg == toSearch)
            {

                Box();

                Console.SetCursorPosition(WindowWidthSetting, WindowHeightSetting);
                ColoredChoice("FirstChoice");
                Console.Write(myVehicles[index]);
                Console.ResetColor();
            }

            else if (firstMatch == false)
            {
                Console.SetCursorPosition(WindowWidthSetting, WindowHeightSetting);
                ColoredChoice("FirstChoice");
                Console.Write(myVehicles[index]);
                Console.ResetColor();
            }
            else
            {
                Console.SetCursorPosition(WindowWidthSetting, WindowHeightSetting + 1);
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
        //--------------------Skriver ut en box---------------------------------------------------------
        public static void Box()
        {
            string[] box = new string[4]

            {   "                      Current Choice                        ",
                "------------------------------------------------------------",
                "|                                                          |",
                "------------------------------------------------------------"
            };

            for (int i = 0; i < box.Length; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 4 * 3 - 1, 41 + i);
                Console.Write(box[i]);
            }
        }
        //---------------------------------------------Hjälp Menyn---------------------------------------
        static void MainHelpMenu()
        {
            string userInput = "";
            string SecondInput = "";
            Console.Clear();
            PrintColumnsOfVehicles();
            string[] menu = new string[11] {
                "_____________________________________________",
                "|        Titel: Help                        |",
                "|                                           |",
                "|        [1] How to check in Car/MC         |",
                "|        [2] How to move Car/MC             |",
                "|        [3] How to remove Car/MC           |",
                "|        [4] Exit                           |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|___________________________________________|"};

            //Centrerar menyn.
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine(menu[i].PadLeft(Console.WindowWidth / 2 + menu[0].Length / 2));
            }
            //Om personen inte anger ett tal mellan 1-4 så kommer datorn fråga efter ett nummber mellan 1-4.
            do
            {
                userInput = GetResponse("Please enter a number between 1-4: ");
            } while (userInput != "1" && userInput != "2" && userInput != "3" && userInput != "4");

            do
            {
                switch (userInput)
                {
                    case "1":
                        CheckInMenu();
                        SecondInput = GetResponse("Please press X to go back. ");
                        if (SecondInput == "X" || SecondInput == "x")
                        {
                            MainHelpMenu();
                        }
                        break;
                    case "2":
                        MoveVehicleMenu();
                        SecondInput = GetResponse("Please press X to go back. ");
                        if (SecondInput == "X" || SecondInput == "x")
                        {
                            MainHelpMenu();
                        }
                        break;
                    case "3":
                        RemoveVehicleMenu();
                        SecondInput = GetResponse("Please press X to go back. ");
                        if (SecondInput == "X" || SecondInput == "x")
                        {
                            MainHelpMenu();
                        }
                        break;
                    case "4":
                        Console.Clear();
                        Run();
                        break;
                    default:
                        break;
                }

            } while (userInput != "X" || userInput != "x");
        }
        static void CheckInMenu()
        {
            //Clearar konsolen, skriver ut garaget och sedan info menyn.
            Console.Clear();
            PrintColumnsOfVehicles();
            string[] CheckInMenu = new string[11] {
                "_____________________________________________",
                "|           How to check in Car/MC          |",
                "|                                           |",
                "|  *To check in the car, first you need to  |",
                "|   choose what type of vehicle you want to |",
                "|   park, and then enter its register-      |",
                "|   -number and the computer will find a    |",
                "|   empty space                             |",
                "|                                           |",
                "|                                           |",
                "|___________________________________________|"};
            //Centrerar menyn.
            for (int i = 0; i < CheckInMenu.Length; i++)
            {
                Console.WriteLine(CheckInMenu[i].PadLeft(Console.WindowWidth / 2 + CheckInMenu[0].Length / 2));
            }
        }
        static void MoveVehicleMenu()
        {
            //Clearar konsolen, skriver ut garaget och sedan info menyn.
            Console.Clear();
            PrintColumnsOfVehicles();
            string[] MoveVehicle = new string[11] {
                "_____________________________________________",
                "|            How to move Car/MC             |",
                "|                                           |",
                "|  *To move the car, enter its register-    |",
                "|   number and what parking plot you want   |",
                "|   to move it to.                          |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|___________________________________________|"};
            //Centrerar menyn.
            for (int i = 0; i < MoveVehicle.Length; i++)
            {
                Console.WriteLine(MoveVehicle[i].PadLeft(Console.WindowWidth / 2 + MoveVehicle[0].Length / 2));
            }
        }
        static void RemoveVehicleMenu()
        {
            //Clearar konsolen, skriver ut garaget och sedan info menyn.
            Console.Clear();
            PrintColumnsOfVehicles();
            string[] RemoveVehicle = new string[11] {
                "_____________________________________________",
                "|           How to remove Car/MC            |",
                "|                                           |",
                "|  *To remove the vehicle, enter its        |",
                "|   register number, and you will find the  |",
                "|   parking lot the vehicle is parked in    |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|                                           |",
                "|___________________________________________|"};
            //Centrerar menyn.
            for (int i = 0; i < RemoveVehicle.Length; i++)
            {
                Console.WriteLine(RemoveVehicle[i].PadLeft(Console.WindowWidth / 2 + RemoveVehicle[0].Length / 2));
            }
        }
    }
}
