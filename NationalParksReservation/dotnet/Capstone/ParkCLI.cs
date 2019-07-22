using Capstone.DAL;
using Capstone.Models;
using ParkReservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{   
    public class ParkCLI
    {
        #region Variables
        private const string Command_AllParks = "1";
        private const string Command_Quit = "q";
        private const string Command_ViewAllCampgrounds = "1";
        private const string Command_ReturntoPreviousScreen = "2";
        private const string Command_30DaysReservations = "3";
        private const string Command_Reservation = "1";
        private const string Command_PreviousMenu = "2";
        private const int Command_ExitCode0 = 0;
        private const string errorMssg100 = "     This date in the past. You will need to enter a future date.";
        private const string errorMssg101 = "     This date in the past or prior to your arrival. You will need to enter a future date.";
        private string menuMessage = "We thank you for choosing to stay with our parks.";
        private const string menuChoice = "Please choose from the following parks to learn more:";
        #endregion

        #region Initialize
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private IReservationDAO reservationDAO;
        private CLIHelper cliHelper= new CLIHelper();
        public ParkCLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.reservationDAO = reservationDAO;
 
        }
        #endregion

        public void RunCLI()
        {
            PrintHeader();
            PrintMenu();

            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_AllParks:
                        ViewAllParks();
                        break;

                    case Command_Quit:
                        RVASCII();
                        Console.WriteLine();
                        Console.WriteLine("     Thank you for stopping by the National Park Campsite Reservation System!");
                        Console.ReadKey();
                        System.Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("     The command provided was not a valid command, please press any key to return to the main menu.");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        PrintHeader();
                        PrintMenu();
                        break;
                }

                //PrintMenu();
            }
        }
        private void PrintMenu()
        {
            Console.WriteLine();
            string textToEnter = "MAIN MENU";
            string textToEnter1 = "Please enter your choice:";
            string textToEnter2 = "1. Show all Parks";
            string textToEnter3 = "Q. Quit";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (textToEnter.Length / 2)) + "}", textToEnter));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (textToEnter1.Length / 2)) + "}", textToEnter1));
            Console.WriteLine(String.Format("{0," + (((Console.WindowWidth / 2) + 4) + (textToEnter.Length / 2)) + "}", textToEnter2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2 - 5) + (textToEnter3.Length / 2)) + "}", textToEnter3));
            Console.WriteLine();
        }
        private void ViewAllParks()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            MountainASCII();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;           
            Console.WriteLine(menuMessage.PadLeft(63));
            Console.WriteLine(menuChoice.PadLeft(65));
            Console.ResetColor();
            Console.WriteLine();

            IList<Park> Parks = parkDAO.ViewAllParks();
            foreach (Park park in Parks)
            {
                Console.WriteLine($"                      {park.park_id}. {park.name}");
            }
            Console.WriteLine("                      Press Any Other Key to Return to Main Menu");

            Console.SetCursorPosition(22, 18);

            //Returns 0 if not a park ID, else the park ID
            int UserInput = cliHelper.ParkInputCheck(Console.ReadLine());
            if (UserInput != Command_ExitCode0)
            {
                Console.Clear();
                ShowParkDetails(UserInput);
            }
            PrintHeader();
            PrintMenu();
            RunCLI();
           
        }
        private void ShowParkDetails(int key)
        {
            Park park = parkDAO.ShowParkDetails(key);
            MountainASCII();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"                Thank you for selecting {park.name} National Park.");
            Console.WriteLine();
            Console.WriteLine($"     Below you'll see facts about {park.name} as well as a list of campgrounds:");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"        Location:        {park.location}");
            Console.WriteLine($"        Established:     {Convert.ToString(park.establish_date.ToShortDateString())}");
            Console.WriteLine($"        Area:            {park.area} sq km");
            Console.WriteLine($"        Annual Visitors: {park.visitors}");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"{park.description}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            try
            {
                Console.WriteLine("     Would you like to: ");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("        1. Search for an availabile reservation");
                Console.WriteLine("        2. Return to the previous menu");
                Console.WriteLine("        3. See all reservations in the next 30 days");
                Console.WriteLine("        Q. Return to the main menu");
                Console.SetCursorPosition(8, 31);
                bool exit = false;
                while (!exit)
                {
                    string choice = Console.ReadLine().ToLower();
                    if (choice == Command_Reservation)
                    {
                        Console.Clear();
                        ViewAllCampgrounds(park.park_id);
                    }
                    else if (choice == Command_PreviousMenu)
                    {
                        Console.Clear();
                        ViewAllParks();
                    }
                    else if (choice == Command_30DaysReservations)
                    {
                        Console.Clear();
                        ViewReservationsNext30();
                    }
                    else if (choice == Command_Quit)
                    {
                        RunCLI();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("        That is not a valid option. Please press any key to try again.");
                        Console.ReadKey();
                        Console.ResetColor();
                        Console.Clear();
                        ShowParkDetails(key);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("     That is not a valid option. Please press any key to return to the previous menu.");
                Console.ReadKey();
                ViewAllParks();
            }
        }
        private void ViewReservationsNext30()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string res30days = string.Format("{0, -7} {1, -13} {2, -28} {3, -18} {4,-12} {5,-17}\n ", "  ID", "Site ID", "Res Name", "From Date", "To Date", "Date of Transaction");
            Console.WriteLine(res30days);
            Console.ResetColor();
            foreach (Reservation reservation in reservationDAO.shownext30())
            {
                Console.WriteLine("{0, -7} {1, -9} {2, -31} {3, -17} {4,-17} {5,-17} ", $"  {reservation.reservation_id}",  $" {reservation.site_id}", $" {reservation.reservation_name}", $" {reservation.from_date.ToShortDateString()}", $" {reservation.to_date.ToShortDateString()}", $" {reservation.booking_created.ToShortDateString()}");
            }
        }
        private void ViewAllCampgrounds(int key)
        {
            string newStr = string.Format("{0, -8}  {1, -17}  {2, -15}  {3, -15}  {4,-5}\n", "  ID", "Name", "Open Month", "Close Month", "Price Per Day");

            Console.ForegroundColor = ConsoleColor.Gray;
            MountainASCII();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("                Campground Information:");
            Console.WriteLine(newStr);
            Console.ResetColor();

            foreach (Campground campground in campgroundDAO.ViewAllCampgrounds(key))
            {
                Console.WriteLine(String.Format("{0, -4}  {1, -25}  {2, -15}  {3, -15}  {4, -5}", $"  {campground.campground_id}.", $"{campground.name}", $"{campground.open_from_mm}", $"{campground.open_to_mm}", $"{campground.daily_fee:C2}"));
            }
            MakeReservationMenu();
        }
        private void MakeReservationMenu()
        {
            try
            {
                int campgroundID_input;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("     Please enter the ID of the campground you would like to reserve.");
                Console.WriteLine("             (You may enter Q to return to the Main Menu)");
                Console.ResetColor();

                bool customerInput = int.TryParse(Console.ReadLine(), out campgroundID_input);
                if (customerInput == true)
                {
                    makeReservationErrors(campgroundID_input);
                }
                incorrectKeyError();
                bool secondTry = (int.TryParse(Console.ReadLine().ToLower(), out campgroundID_input));
                if (secondTry == true)
                {
                    makeReservationErrors(campgroundID_input);
                }
                    PrintHeader();
                    PrintMenu();
                    RunCLI();
            }
            catch (Exception)
            {
                Console.WriteLine("     This is not a valid campground. Please enter any key to return to try again.");
                Console.ReadKey();
                Console.Clear();
                ViewAllParks();
            }
        }
        private void AdvancedSearchMenu(int campgroundID_input, DateTime arrivalDate, DateTime departDate)
        {
            int maxOccupancy;
            bool handicapAccess = false;
            bool utilities = false;
            int maxRVLength;
            MountainASCII();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     How many people will be camping? ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out maxOccupancy);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     Do you need the site to have Handicap Accessibility? (Y/N)");
            Console.ResetColor();
            if (Console.ReadLine().ToLower() == "y")
            {
                handicapAccess = true;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     Do you need utility hookup? (Y/N)");
            Console.ResetColor();
            if (Console.ReadLine().ToLower() == "y")
            {
                utilities = true;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     If you are bringing an RV, what is it's length? (If no RV, enter 0)");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out maxRVLength);

            AdvancedAvailCampsites(campgroundID_input, arrivalDate, departDate, maxOccupancy, handicapAccess, utilities, maxRVLength);
        }
        private void AdvancedAvailCampsites(int campgroundID_input, DateTime arrivalDate, DateTime departDate, int maxOccupancy, bool handicapAccess, bool utilities, int maxRVLength)
        {
            foreach (Site site in cliHelper.DoCampSiteSearch(this.campgroundDAO, campgroundID_input, arrivalDate, departDate))
            {
                if (site.accessible == handicapAccess && site.max_occupancy >= maxOccupancy && site.max_rv_length >= maxRVLength && site.utilities == utilities)
                {
                    Console.WriteLine("Here is a site that matched your search criteria!");
                    Console.WriteLine(String.Format("{0, -12} {1, -15} {2, -13} {3, -13} {4, -18} {5,-14}", $"    {site.site_id}.", $"{site.site_number}", $"{site.max_occupancy}", $"{site.AccessibleMssg}", $"{site.UtilitiesMssg}", $"{site.max_rv_length}"));
                    Console.ReadLine();
                }
                else
                {
                    //need to put in some functionality here
                    Console.WriteLine("Sorry... There are no campsites that meet your seach criteria. ");
                    Console.WriteLine("Would you like to see all sites available? (Y/N) ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        AvailableCampsites(campgroundID_input, arrivalDate, departDate);
                    }
                    else System.Environment.Exit(0);

                }
            }

        }
        private void AvailableCampsites(int campgroundID_input, DateTime arrivalDate, DateTime departDate)
        {

            IList<Site> AvailableSites = cliHelper.DoCampSiteSearch(this.campgroundDAO, campgroundID_input, arrivalDate, departDate);
            Campground campground = campgroundDAO.FindCampground(campgroundID_input);

            if (AvailableSites.Count == 0)
            {
                //need functionality here to direct the user back up to dates menue
                Console.WriteLine("Sorry... There are no campsites for these dates. ");
                Console.WriteLine("Would you like to choose new Dates? (Y/N)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    //send them to date menu
                }

            }
            int totalDays = (departDate - arrivalDate).Days;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     Thank you for that information. Here are the sites that are available with your information:");
            Console.ResetColor();
            Console.WriteLine();
            string campInfo = string.Format("{0, -7} {1, -13} {2, -15} {3, -17} {4,-12} {5,-17} {6, -12}\n ", "   ID", "Site Number", "Max Occupancy", "Handicap Access", "Utilities", "Max RV Length", "Total Price");
            Console.WriteLine(campInfo);
            for (int i = 0; i < 5; i++)
            {
               Console.WriteLine(String.Format("{0, -12} {1, -15} {2, -13} {3, -13} {4, -18} {5,-14} {6, -10} ", $"    {AvailableSites[i].site_id}.", $"{AvailableSites[i].site_number}", $"{AvailableSites[i].max_occupancy}", $"{AvailableSites[i].AccessibleMssg}", $"{AvailableSites[i].UtilitiesMssg}", $"{AvailableSites[i].max_rv_length}", $"{(totalDays * campground.daily_fee):c2}"));
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     Which site should would you like to reserve? (Please enter the ID number listed)");
            Console.ResetColor();

            int siteNum;
            int.TryParse(Console.ReadLine(), out siteNum);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     What name should we enter for the reservation?");
            Console.ResetColor();

            string resName = Console.ReadLine();
            Reservation reservation = cliHelper.ReserveSite(AvailableSites, arrivalDate, departDate, siteNum, resName);
          
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine($"     Congratulations! You have placed your reservation. " +
                                $"\n     Your confirmation number is: {reservationDAO.InsertNewBooking(reservation)}. " +
                                "\n     Please hold onto this number for future reference.");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("    You can enter 'M' to return to the main menu, or any other key to quit.");

            string finalTransaction = Console.ReadLine();
            if (finalTransaction == "m")
            {
                PrintHeader();
                PrintMenu();
                RunCLI();
            }
            System.Environment.Exit(0);

        }
        private void defaultMenu()
        {
            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_Quit:
                        RVASCII();
                        Console.WriteLine();
                        Console.WriteLine("     Thank you for stopping by the National Park Campsite Reservation System!");
                        Console.ReadKey();
                        System.Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("     The command provided was not a valid command, please press any key to return to the main menu.");
                        Console.ResetColor();
                        Console.ReadKey();
                        PrintHeader();
                        PrintMenu();
                        break;
                }
                PrintMenu();
            }
        }
        private static void MountainASCII()
        {
            Console.WriteLine("           /\\									" +
                                "\n         /**\\								" +
                                "\n        /****\\   /\\								" +
                                "\n       /      \\ /**\\								" +
                                "\n      /  /\\    /    \\        /\\    /\\  /\\      /\\            /\\/\\/\\  /\\	" +
                                "\n     /  /  \\  /      \\      /  \\/\\/  \\/  \\  /\\/  \\/\\  /\\  /\\/ / /  \\/  \\	" +
                                "\n    /  /    \\/ /\\     \\    /    \\ \\  /    \\/ /   /  \\/  \\/  \\  /    \\   \\	" +
                                "\n   /  /      \\/  \\/\\   \\  /      \\    /   /    \\				" +
                                "\n__/__/_______/___/__\\___\\__________________________________________________  ");
            Console.WriteLine();
        }
        private static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("                                             .				" +
                                "\n                                              .         ;  		" +
                                "\n                 .              .              ;%     ;;   		" +
                                "\n                   ,           ,                :;%  %;   		" +
                                "\n                   :         ;                   :;%;'     .,   	" +
                                "\n           ,.        %;     %;            ;        %;'    ,;		" +
                                "\n             ;       ;%;  %%;        ,     %;    ;%;    ,%'		" +
                                "\n              %;       %;%;      ,  ;       %;  ;%;   ,%;' 		" +
                                "\n               ;%;      %;        ;%;        % ;%;  ,%;'		    " +
                                "\n                `%;.     ;%;     %;'         `;%%;.%;'		    " +
                                "\n                 `:;%.    ;%%. %@;        %; ;@%;%'			    " +
                                "\n                    `:%;.  :;bd%;          %;@%;'			    " +
                                "\n                      `@%:.  :;%.         ;@@%;'   			    " +
                                "\n                        `@%.  `;@%.      ;@@%;         		    " +
                                "\n                          `@%%. `@%%    ;@@%;        		    " +
                                "\n                            ;@%. :@%%  %@@%;       			    " +
                                "\n                              %@bd%%%bd%%:;     		     Welcome to the	    " +
                                "\n                                #@%%%%%:;;			   NATIONAL PARK CAMP " +
                                "\n                                %@@%%%::;			   RESERVATION SYSTEM " +
                                "\n                                %@@@%(o);  . '         		    " +
                                "\n                                %@@@o%;:(.,'         		    " +
                                "\n                            `.. %@@@o%::;         		        " +
                                "\n                               `)@@@o%::;         			    " +
                                "\n                                %@@(o)::;        			    " +
                                "\n                               .%@@@@%::;         			    " +
                                "\n                               ;%@@@@%::;.          			    " +
                                "\n                              ;%@@@@%%:;;;. 				        " +
                                "\n                          ...;%@@@@@%%:;;;;,.. 			        ");
            Console.ResetColor();
        }
        private void DisplayInvalidOption()
        {
            Console.WriteLine(" Press any key to return to main menu");
            Console.ReadLine();
        }
        private void makeReservationErrors(int campgroundID_input)
        {
            DateTime arrivalDate;           
            string msg = "";
            do {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("     What date would you like to arrive? (MM/DD/YYYY) ");
                Console.ResetColor();
                arrivalDate = Convert.ToDateTime(Console.ReadLine());
                msg = cliHelper.CheckArrivalDates(DateTime.Today, arrivalDate);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                if(msg == errorMssg100)
                {
                    Console.WriteLine(msg);
                }               
            }
            while (msg == errorMssg100);
    
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     What date will you be leaving? (MM/DD/YYYY) ");
            Console.ResetColor();
            DateTime departDate = Convert.ToDateTime(Console.ReadLine());
           
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();          
             if (cliHelper.CheckDepartDates(DateTime.Today, arrivalDate, departDate) == errorMssg101)
            {
                Console.WriteLine(errorMssg101);
                Console.ResetColor();
                MakeReservationMenu();
            }

            if (cliHelper.CheckParkOpenMonths(campgroundDAO.FindCampground(campgroundID_input), arrivalDate, departDate))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("     Would you like to search for additional criteria? (Y/N)");
                Console.ResetColor();
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Clear();
                    AdvancedSearchMenu(campgroundID_input, arrivalDate, departDate);
                    //if no campsites meet criteria, say something and dont continue to next method
                }
                Console.ResetColor();
                Console.Clear();
                MountainASCII();
                AvailableCampsites(campgroundID_input, arrivalDate, departDate);

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("              Unfortunately, this Park is not open for the dates you entered. " +
                                  "\n     You may enter 'Y' to enter new dates, or any other key to return to the main menu.");
                string newDates = Console.ReadLine().ToLower();
                if (newDates == "y")
                {
                    MakeReservationMenu();
                }
                else
                {
                    Console.Clear();
                    PrintHeader();
                    PrintMenu();
                    RunCLI();
                }
            }
        }
        private static void RVASCII()
        {

            Console.WriteLine("          ___________            ___________            ___________	 " +
                                "\n         [__         |          [__         |          [__         |   	 " +
                                "\n        __/_| [] []  |  (')    __/_| [] []  |  (')    __/_| [] []  |  (')  " +
                                "\n       [_,________,__|==   )  [_,________,__|==   )  [_,________,__|==   ) " +
                                "\n         O        O     (-)     O        O     (-)     O        O     (-)  ");
        }
        private static void incorrectKeyError()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("             Are you sure you want to return to the main menu? ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("           If this was an error, you may reenter the camp number. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("             Otherwise press enter to return to the main menu.");
            Console.ResetColor();
        }
    }
}

