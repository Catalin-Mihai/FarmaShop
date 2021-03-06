﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FarmaShop.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public string ZipCode { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public double OrderTotal { get; set; }

        public DateTime OrderPlaced { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}