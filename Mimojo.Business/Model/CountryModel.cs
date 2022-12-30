
using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
    }

    public class CountryUserModel
    {
        public string UserId { get; set; }
        public string Country { get; set; }
    }
}
