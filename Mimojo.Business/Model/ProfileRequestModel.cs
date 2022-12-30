using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class ProfileRequestModel
    {
        public string UserId { get; set; }
    }
    public class ProfileResponseModel
    {
        public RegistrationModel Registration { get; set; }
        public SubscriptionModel Subscription { get; set; }
        public PaymentModel Payment { get; set; }
    }
}
