using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FarmaShop.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime MemberSince { get; set; }
        
        public IEnumerable<Order> Orders { get; set; }
        
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }

        public int UserInfoId { get; set; }
        
        public ApplicationUserInfo UserInfo { get; set; }
    }
}