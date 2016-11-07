using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobSquad
{
    class Car
    {
        public int Id { get; }
        public string Brand { get; }
        public int PassengerCount { get; }
        public string GearBox { get; }
        public string Fuel { get; }
        public string Type { get; }
        public string RegistrationNumber { get; }
        public string NickName { get; }

        public Car(int id, string brand, int passengerCount, string gearBox, string fuel,
            string type, string registrationNumber, string nickName)
        {
            Id = id;
            Brand = brand;
            PassengerCount = passengerCount;
            GearBox = gearBox;
            Fuel = fuel;
            Type = type;
            RegistrationNumber = registrationNumber;
            NickName = nickName;
        }

    }
}
