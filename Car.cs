using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.Parking
{
    [DataContract]
    public class Car
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Number { get; set; }

        public Car() { }
        public Car(string name, string number)
        {
            Name = name;
            Number = number;
        }
    }
}
