using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password has to be atleast 6 cahracters with upper and lower characters")]
        public string Passowrd { get; set; }
    }
}
