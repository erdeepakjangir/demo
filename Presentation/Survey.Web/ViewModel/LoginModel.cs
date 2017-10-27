using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Survey.Web.ViewModel
{
    public class LoginModel
    {
        [Required]        
        public string DomainName { get; set; }//added
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool IsAdmin{ get; set; }
    }
}