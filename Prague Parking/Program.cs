using System;
using System.Text;
using System.Threading;

namespace Prague_Parking
{
    class Program
    {

        public static string[] myCars = new string[100];
        static void Main(string[] args)
        {
            Console.SetWindowPosition(0, 0);
            Console.WindowWidth = 240;

            Console.WriteLine(Console.WindowWidth + " | " + Console.WindowHeight);
            //Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,5}|{3,5}|", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla"));





            
            EmptySpace(myCars);
            CarVisualize(myCars);
            string input = "";
            while (input != "5")
            {

                MainMenu();
                input = GetResponse("\tPlease enter a choice 1-4, or 5 to exit");
                MainMenyChoice(input);
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
                    CheckOut();
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
            string mainMenu = @"
            _____________________________________________
            |       Titel: Prague Parking               |
            |                                           |
            |        [1] Incheckning av fordon          |
            |        [2] Flytta fordon                  |
            |        [3] Checka utfordon                |
            |        [4] Hjälp                          |
            |        [5] Avsluta                        |
            |                                           |
            |                                           |
            |                                           |
            |___________________________________________|";
            Console.WriteLine(mainMenu);
        }
        //----------------------- Skriver ut kolummer med alla platser som finns i myCars arrayen---------------------------------------
        static void CarVisualize(string[] cars)
        {

            string lidAndBottom = "";
            Console.WriteLine(lidAndBottom.PadRight(Console.WindowWidth-1, '_'));
            Console.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                Console.Write("         |");

                Console.Write($"{i+1} {cars[i].PadRight((Console.WindowWidth / 3) - 19)}|");
                Console.Write($"{i + cars.Length / 4 + 1} {cars[i + cars.Length / 4].PadRight((Console.WindowWidth / 3) - 19)}|");
                Console.Write($"{i + cars.Length / 2 + 1} {cars[i + cars.Length / 2].PadRight((Console.WindowWidth / 3) - 19)}|");
                Console.Write($"{i + (cars.Length / 4)*3 + 1} {cars[i + (cars.Length / 4) * 3]}|");

                Console.WriteLine();


            }
            Console.WriteLine(lidAndBottom.PadRight(Console.WindowWidth - 1, '_'));


        }
        //-----------------------------Tar emot menyval-------------------------------------------------------
        static string GetResponse(string message)
        {
            Console.WriteLine(message);
            string choice = Console.ReadLine();
            return choice;
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
            string registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            while (registrationNumber.Length > 10)
            {
                registrationNumber = GetResponse("Enter your registration number, max 10 characters long: ");
            }
            //TODO: snygga till
            DateTime timeCheckedIn = DateTime.Now;
            

            string final = $"{ vehicleType}#{registrationNumber}#{timeCheckedIn.ToString("MMM-dd HH:mm")}";
            //skriver ut teststräng
            Console.WriteLine(final);
            //final = cars

            //TODO sökfunktion för att söka efter en ledig plats

            /*
             Om type == CAR && index i listan == tom => lägg till strängen där.
             Annars sök på nästa plats ==> om alla platser är fyllda => skriv ut det till konsolen

            //TODO
            //Kolla så att man inte försöker lägga till en tredje mc
             
             Om type == "MC " && index i listan == tom ==> lägg till strängen där.
             Eller om FoundMatch == false => lägg till i slutet av strängen
             Annars sök efter ny plats. Om ingen plats hittas => skriv ut det till konsolen



             */

            for (int i = 0; i < myCars.Length; i++)
            {
                if(myCars[i] == "Ledig" && vehicleType == "CAR")
                {
                    myCars[i] = final;
                    break;
                }
                if(myCars[i] == "Ledig" && vehicleType == "MC ")
                {
                    myCars[i] = final;
                    break;
                }
            }
            CarVisualize(myCars);


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

        }
        static void CheckOut()
        {

        }
        static void EmptySpace(string[] cars)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] == null)
                {
                    cars[i] = "Ledig";
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
