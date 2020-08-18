using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.DTOs.User
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
           
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
