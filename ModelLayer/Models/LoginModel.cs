using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[A-Za-z0-9#'\$%&`\*\+\-/=\?\^_\{\\]{5,64}@[A-Z0-9a-z\-\.]{3,253}\.[a-z]{3}$")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\*\.!@\$%\^&\(\)\{\}\[\]:;<>,\.\?\/~_\+\-=|\\]).{8,32}$")]
        public string Password { get; set; }
    }
}
