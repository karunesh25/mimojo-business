using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Mimojo.Business.Dal;
using Mimojo.Business.Model;
using Newtonsoft.Json;

namespace Mimojo.Business
{
    public class Subscription
    {
        [FunctionName("Subscription")]
        public void Run([ServiceBusTrigger("subscription-queue", Connection = "ConnectionStringSubscription")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"Subscription ServiceBus queue trigger function processed message: {myQueueItem}");

                dynamic obj = JsonConvert.DeserializeObject<dynamic>(myQueueItem);
                string strValue = obj.Value;
                SubscriptionModel model = JsonConvert.DeserializeObject<SubscriptionModel>(strValue);
                SubscriptionDal.Create(model);
                CountryUserModel userModel = new CountryUserModel();
                userModel.UserId = model.UserId;
                userModel.Country = model.Country;
                SubscriptionDal.CreateUserCountry(userModel);

                log.LogInformation($"Subscription completed : {strValue}");
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message + "stacktrace " + ex.StackTrace);
            }
        }
    }
}
