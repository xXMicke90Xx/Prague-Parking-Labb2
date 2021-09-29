using System;
using System.Text;
using System.Threading;

namespace Prague_Parking
{
    class Program
    {
        public string[] myCars = new string[100];
        static void Main(string[] args)
        {

            Console.WindowWidth = 240;

            Console.WriteLine(Console.WindowWidth + " | " + Console.WindowHeight);
            //Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,5}|{3,5}|", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla", "CAR#0123456789#datumblalbla"));

            string[] cars = { "123456789012345678901234567890", "1234567890123456", "Car#abs1234542", "123456789012345678901234567890", "e", "f", "g", "h", "i", "o" };



            /*
             Programmeny:
             1. CAR#1234567890#2-4:05:55|CAR#1234567890#2-4:05:55         26.Ledig                               51.Ledig                               76. Ledig  
             2. Ledig   
             3. 1 plats för motorcykel ledig

             ____________________________________________
            |       Titel: Prague Parking               |
            |                                           |
            |        [1] Incheckning av fordon          |
            |        [2] Flytta fordon                  |
            |        [3] Ta bort fordon                 |
            |        [4] Hjälp                          |
            |        [5] Avsluta                        |
            |                                           |
            |                                           |
            |                                           |
            |___________________________________________|
                   


             */

            string input = "";
            while (input != "5")
            {
                MainMenu();
                input = GetResponse("\tPlease enter a choice 1-4, or 5 to exit");
                MainMenyChoice(input);
            }



            Console.ReadLine();
        }
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

        static void CarVisualize(string[] cars)
        {
            for (int i = 0; i < 25; i++)
            {
                Console.Write("         |");
                for (int j = 0; i < 1; j++)
                {

                    Console.Write($"{cars[0].PadRight((Console.WindowWidth / 3) - 19)}|{cars[1].PadRight((Console.WindowWidth / 3) - 19)}|{cars[2].PadRight((Console.WindowWidth / 3) - 19)}|{cars[3]}|");
                }


            }  //Ska skriva ut listan i 4 kolummer, så med andra ord  car --> tomt ---> ledig---->tomt
        }
        static string GetResponse(string message)
        {
            Console.WriteLine(message);
            string choice = Console.ReadLine();
            return choice;
        }

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
            timeCheckedIn.ToString("yyyy-MM-dd:HH:mm");

            string final = $"{ vehicleType}#{registrationNumber}#{timeCheckedIn}";
            //skriver ut teststräng
            Console.WriteLine(final);
            //final = cars

            //TODO sökfunktion för att söka efter en ledig plats
            



        }
        static void MoveCar()
        {

        }
        static void CheckOut()
        {

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
