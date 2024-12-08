﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Addresso
    {
        public Addresso(string fName, string lName, string city, string street, string country)
        {
            FName = fName;
            LName = lName;
            City = city;
            Street = street;
            Country = country;
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
    }
}