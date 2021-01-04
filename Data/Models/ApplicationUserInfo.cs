using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FarmaShop.Data.Models
{
    public class ApplicationUserInfo {

        public int Id { get; set; }
        
        public byte[] Image { get; set; }
        
        public bool IsActive { get; set; }
        
        public double Balance { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }

        public string UserId { get; set; }
        //One to One
        public ApplicationUser User { get; set; }

    }
}