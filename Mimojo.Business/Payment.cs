using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Mimojo.Business.Dal;
using Mimojo.Business.Model;
using Newtonsoft.Json;

namespace Mimojo.Business
{
    public class Payment
    {
        [FunctionName("Payment")]
        public void Run([ServiceBusTrigger("payment-queue", Connection = "ConnectionStringPayment")]string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"Payment ServiceBus queue trigger function processed message: {myQueueItem}");

                dynamic obj = JsonConvert.DeserializeObject<dynamic>(myQueueItem);
                string strValue = obj.Value;
                PaymentModel model = JsonConvert.DeserializeObject<PaymentModel>(strValue);
                PaymentDal.Create(model);

                log.LogInformation($"Payment completed : {strValue}");
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message + "stacktrace " + ex.StackTrace);
            }
        }
    }
}
