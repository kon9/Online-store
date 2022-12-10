using Microsoft.AspNetCore.Identity;
using OnlineStore.Library.Common.Models;
using System.Net.NetworkInformation;

namespace OnlineStore.Library.UserManagementService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address DefaultAddress { get; set; }
        public Address DeliveryAddress { get; set; }


    }
}
