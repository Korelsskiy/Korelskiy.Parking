using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.Parking
{
    [DataContract]
    public class Parking
    {
        
        [DataMember]
        public string Title { get; set; }
        public int NumberOfCars { get; set; } = 0;
        [DataMember]
        public int ParkingCapacity { get; set; }
        [DataMember]
        public List<ParkingPlace> ParkingPlaces { get; set; }

        public Parking() { }
        public Parking(string title, int capacity)
        {
            Title = title;
            ParkingCapacity = capacity;
            ParkingPlaces = CreateParkingPlaces(capacity);
        }

        private List<ParkingPlace> CreateParkingPlaces(int num)
        {
            List<ParkingPlace> parkingPlaces = new List<ParkingPlace>();

            for (int i = 0; i < num; i++)
            {
                parkingPlaces.Add(new ParkingPlace(i + 1));
            }

            return parkingPlaces;
        }

        private void AddNewCar(Parking parking)
        {
            Console.WriteLine("Введите название пребывающей машины");
            string carName = Console.ReadLine();
            if (carName.Length == 0)
            {
                Console.WriteLine("Введена пустая строка!");
                AddNewCar(parking);
            }

            Console.WriteLine("Введите номер пребывающей машины");
            string carNumber = Console.ReadLine();

            if (carNumber.Length == 0)
            {
                Console.WriteLine("Введена пустая строка!");
                AddNewCar(parking);
            }

            foreach (ParkingPlace place in parking.ParkingPlaces)
            {
                if (place.CarInPlace != null)
                {
                    if (place.CarInPlace.Number == carNumber)
                    {
                        Console.WriteLine("Машина с таким номером уже находится на вашей парковке!");
                        AddNewCar(parking);
                    }
                }

            }

            Console.WriteLine("Введите номер места для этой машины");
            int placeNumber = 0;
            bool valid = Int32.TryParse(Console.ReadLine(), out placeNumber);

            if (valid)
            {
                ParkingPlace place = null;
                foreach (ParkingPlace parkingPlace in parking.ParkingPlaces)
                {
                    if (parkingPlace.Id == placeNumber)
                    {
                        place = parkingPlace;
                    }
                }
                if (place != null)
                {
                    if (place.CarInPlace == null)
                    {
                        place.CarInPlace = new Car(carName, carNumber);
                        parking.NumberOfCars++;
                        Console.WriteLine($"Автомобиль {carName} занял свое место - {place.Id}.");
                        Console.ReadKey();
                        Menu(parking);
                    }
                    else
                    {
                        Console.WriteLine("Место уже занято!");
                        AddNewCar(parking);
                    }
                }
                else
                {
                    Console.WriteLine("Такого места нет!");
                    AddNewCar(parking);
                }
            }
            else
            {
                Console.WriteLine("Вы ввели не число!");
                AddNewCar(parking);
            }


        }

        private void DeleteCar(Parking parking)
        {
            if (parking.NumberOfCars == 0)
            {
                Console.WriteLine("Все места на парковке свободны, вы не можете удалить машину.");
                Console.ReadKey();
                Menu(parking);
            }

            Console.WriteLine("Введите номер освобождаемого места");
            int placeNumber = 0;
            bool valid = Int32.TryParse(Console.ReadLine(), out placeNumber);

            if (valid)
            {
                ParkingPlace place = null;
                foreach (ParkingPlace parkingPlace in parking.ParkingPlaces)
                {
                    if (parkingPlace.Id == placeNumber)
                    {
                        place = parkingPlace;
                    }

                }
                if (place != null)
                {
                    if (place.CarInPlace != null)
                    {
                        place.CarInPlace = null;
                        Console.WriteLine($"Место {place.Id} освобождено");
                        Console.ReadKey();
                        Menu(parking);
                    }
                    else
                    {
                        Console.WriteLine("Это место уже свободно!");
                        DeleteCar(parking);
                    }
                }
                else
                {
                    Console.WriteLine("Такого места нет!");
                    DeleteCar(parking);
                }
            }
            else
            {
                Console.WriteLine("Вы ввели не число!");
                DeleteCar(parking);
            }
        }
        

        public void CreateNewParking(Parking parking)
        {

            
            Console.WriteLine("Введите название вашей парковки:");
            string parkingTitle = Console.ReadLine();
            if (parkingTitle.Length == 0)
            {
                Console.WriteLine("Пустая строка!");
                CreateNewParking(parking);
            }

            Console.WriteLine("Введите вместимость вашей парковки(не более 10):");
            int parkingСapacity = 0;

            bool valid = Int32.TryParse(Console.ReadLine(), out parkingСapacity);

            if (valid)
            {
                if (parkingСapacity <= 10 && parkingСapacity > 0)
                {
                    var jsonFormatter = new DataContractJsonSerializer(typeof(List<Parking>));



                    Parking p = new Parking(parkingTitle, parkingСapacity);
                    List<Parking> parkings = new List<Parking>();

                    using (var file = new FileStream($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\parkings.json", FileMode.OpenOrCreate))
                    {
                        if (file.Length > 0)
                        {
                            parkings.AddRange(jsonFormatter.ReadObject(file) as List<Parking>);
                        }
                    }
                    parkings.Add(p);
                  


                    using (var file = new FileStream($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\parkings.json", FileMode.OpenOrCreate))
                    {
                       jsonFormatter.WriteObject(file, parkings);
                    }

                    Console.Clear();
                    Menu(parking);
                }
                else
                {
                    Console.WriteLine("Введено неверное число парковочных мест.");
                    CreateNewParking(parking);
                }

            }
            else
            {
                Console.WriteLine("Вы ввели не число!");
                CreateNewParking(parking);
            }
        }

        public void LoadParking(Parking parking)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<Parking>));
            List<Parking> parkings = new List<Parking>();
           

            using (var file = new FileStream($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\parkings.json", FileMode.OpenOrCreate))
            {
                if (file.Length > 0)
                {
                    parkings.AddRange(jsonFormatter.ReadObject(file) as List<Parking>);
                }
                else
                {
                    Console.WriteLine("Нет доступных парковок!");
                    Console.ReadKey();
                    MainMenu menu = new MainMenu();
                    file.Close();
                    menu.Menu(parking);
                }
            }

            Console.Clear();
            Parking p = new Parking();

            Console.WriteLine("Выберите парковку для загрузки:");

            if (parkings != null)
            {
                foreach (Parking park in parkings)
                {
                    Console.WriteLine(park.Title);
                }
            }


            string parkName = Console.ReadLine();

            p = parkings.Where(x => x.Title == parkName).FirstOrDefault();
            if (p != null)
            {
                Console.WriteLine($"Парковка {p.Title} загружена");
                Console.ReadKey();
                Menu(p);
            }
            else
            {
                Console.WriteLine("Такой парковки нет!");
                LoadParking(parking);
            }
        }
        private void Menu(Parking parking)
        {
            Console.Clear();

            switch (ShowMenu(parking.Title))
            {
                case "add":
                    parking.AddNewCar(parking);
                    break;
                case "del":
                    parking.DeleteCar(parking);
                    break;
                case "save":
                    parking.SaveParking(parking);
                    break;
                case "stat":
                    parking.DisplayParkingStatus(parking);
                    break;
                case "exit":
                    Console.Clear();
                    MainMenu menu = new MainMenu();
                    menu.Menu(parking);
                    break;
                default:
                    Console.Clear();
                    Menu(parking);
                    break;
            }
        }

        private void DisplayParkingStatus(Parking parking)
        {
            int emptyPlaces = 0;
            int allPlaces = parking.ParkingPlaces.Count;
            string emp = "";
            foreach (ParkingPlace place in parking.ParkingPlaces)
            {
                if (place.CarInPlace == null)
                {
                    emptyPlaces++;
                    emp += $"{place.Id}, ";
                }
            }

            Console.WriteLine($"На парковке сейчас {emptyPlaces} свободных мест из {allPlaces}. Это места: {emp}");
            Console.ReadKey();
            Menu(parking);
           
        }

        private void SaveParking(Parking parking)
        {
            List<Parking> parkings = new List<Parking>();

            var jsonFormatter = new DataContractJsonSerializer(typeof(List<Parking>));

            using (var file = new FileStream($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\parkings.json", FileMode.OpenOrCreate))
            {
                parkings.AddRange(jsonFormatter.ReadObject(file) as List<Parking>);
            }

            Parking parkingForDelete = parkings.Where(x => x.Title == parking.Title).FirstOrDefault();

            if (parkingForDelete != null)
            {
                parkings.Remove(parkingForDelete);
            }

            parkings.Add(parking);

            using (var file = new FileStream($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\parkings.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file, parkings);
            }

            Console.WriteLine("Ваша парковка сохранена");

            Console.ReadKey();

            Menu(parking);

        }
        private string ShowMenu(string parkName)
        {
            Console.WriteLine($"Добро пожаловать на парковку {parkName}. Чем займемся?");
            Console.WriteLine("Добавим машину(add)");
            Console.WriteLine("Удалим машину(del)");
            Console.WriteLine("Статистика по парковке(stat)");
            Console.WriteLine("Сохранить мою парковку(save)");
            Console.WriteLine("Выйти в главное меню(exit)");

            string response = Console.ReadLine();

            return response;
        }
    }
}
