using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class SubscriptionModel
    {
        public string UserId { get; set; }
        public string Subscription { get; set; }
        public string Description { get; set; }
        public string  Country { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
