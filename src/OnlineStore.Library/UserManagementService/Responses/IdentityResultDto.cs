using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OnlineStore.Library.UserManagementService.Requests
{
    public class IdentityResultDto
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}