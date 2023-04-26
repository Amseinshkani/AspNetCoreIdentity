using System.ComponentModel.DataAnnotations;

namespace ISApplication.Models
{
    public class LoginViewModel
    {
        [Required, Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required, Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Display(Name = "مرا به خاطر بسپار")]
        public bool Rememberme { get; set; }

    }
}
