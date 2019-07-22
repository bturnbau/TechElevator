using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using ParkReservation;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            IParkDAO parkDAO = new ParkDAO(connectionString);
            ICampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationDAO(connectionString);
            ISiteDAO siteDAO = new SiteDAO(connectionString);

            ParkCLI parkCLI = new ParkCLI(parkDAO, campgroundDAO, reservationDAO);
            parkCLI.RunCLI();
        }
    }
}
