using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.Parking
{
    [DataContract]
    public class ParkingPlace
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Car CarInPlace { get; set; }
        public ParkingPlace() { }
        public ParkingPlace(int id)
        {
            Id = id;
        }
    }
}
