using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string houseNumber, string street, string ward, string district, string city, int zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            HouseNumber = houseNumber;
            Street = street;
            Ward = ward;
            District = district;
            City = city;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
    }
}
