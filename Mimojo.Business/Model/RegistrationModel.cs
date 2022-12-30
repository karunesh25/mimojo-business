using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class RegistrationModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
