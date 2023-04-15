using Microsoft.AspNetCore.Identity;
using OnlineStore.Library.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Library.UserManagementService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(128)")] public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(128)")] public string LastName { get; set; }

        public Address DefaultAddress { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}