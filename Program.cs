using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.Parking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.ReadKey();

            MainMenu menu = new MainMenu();
            menu.Menu(new Parking());
        }  
    }
}
