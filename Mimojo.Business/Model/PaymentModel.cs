using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Model
{
    public class PaymentModel
    {
        public string UserId { get; set; }
        public string CardBin { get; set; }
        public string CardLast4 { get; set; }
        public string Expiry { get; set; }
        public string CardName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
    }
}
