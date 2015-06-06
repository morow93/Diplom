using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputSignUpModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        [StringLength(maximumLength: 20, MinimumLength = 2, ErrorMessage="Имя пользователя должно содержать от 2 до 20 символов")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия пользователя")]
        [StringLength(maximumLength: 20, MinimumLength = 2, ErrorMessage = "Фамилия пользователя должна содержать от 2 до 20 символов")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="Недопустимый email адрес")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Пароль должен содержать от 6 до 20 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение должны совпадать")]
        public string ConfirmPassword { get; set; }
    }
}
