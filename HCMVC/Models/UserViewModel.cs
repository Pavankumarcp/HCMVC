using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCMVC.Models
{
    public class UserViewModel
    {

        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public int Id { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
       
    public class RolesRegisterVM 
    { 
        public string SelectedValue { get; set; } 
        public IEnumerable<SelectListItem> Values { get; set; } 
        public LoginModel RegisterRoles { get; set; } 
    }
}
