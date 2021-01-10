using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.Parking
{
    public class MainMenu
    {
        public string Display()
        {
            Console.WriteLine("############ParkingGod v1.2############");
            Console.WriteLine("Новая парковка(new)");
            Console.WriteLine("Загрузить парковку(load)");
            Console.WriteLine("Выйти из программы(exit)");

            string response = Console.ReadLine();
            return response;
        }

        public void Menu(Parking parking)
        {
            Console.Clear();
            switch (Display())
            {
                case "new":
                    parking.CreateNewParking(parking);
                    break;
                case "load":
                    parking.LoadParking(parking);
                    break;
                case "exit":
                    Environment.Exit(1);
                    break;
                default:
                    Console.Clear();
                    Menu(parking);
                    break;
            }
        }
    }
}
