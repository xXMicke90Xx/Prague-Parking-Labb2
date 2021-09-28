using System;

namespace Prague_Parking
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] cars = new string[100];
            /*
             Programmeny:
             1. CAR#abc123#1988-2-4:05:55:33|           26.Ledig                               51.Ledig                               76. Ledig  
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
             
            
            MainMenu();
            string input = GetResponse("\tPlease enter a choice 1-4, or 5 to exit");
            MainMenyChoice(input);
            
             

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
           
            for (int i = 0; i < Console.WindowWidth; i++)
            {

            }
        }
        static string GetResponse(string message)
        {
            Console.WriteLine(message);
            string choice = Console.ReadLine();
            return choice;
        }

        static void CheckIn()
        {

        }
        static void MoveCar()
        {

        }
        static void CheckOut()
        {

        }
        static void Help()
        {
            string help = @"";
        }
    }
}
