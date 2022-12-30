using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Mimojo.Business.Dal;
using Mimojo.Business.Model;
using Newtonsoft.Json;

namespace Mimojo.Business
{
    public class Registration
    {
        [FunctionName("Registration")]
        public void Run([ServiceBusTrigger("registration-queue", Connection = "ConnectionStringRegistration")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"Registration ServiceBus queue trigger function processed message: {myQueueItem}");

                dynamic obj = JsonConvert.DeserializeObject<dynamic>(myQueueItem);
                string strValue = obj.Value;
                RegistrationModel model = JsonConvert.DeserializeObject<RegistrationModel>(strValue);
                RegistrationDal.Create(model);

                log.LogInformation($"Registration completed : {strValue}");
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message);
            }
        }
    }
}
