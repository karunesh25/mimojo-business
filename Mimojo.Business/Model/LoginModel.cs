using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class LoginModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public bool IsValidUser { get; set; }
    }
}
