using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Mimojo.Business.Dal;
using Mimojo.Business.Model;

namespace Mimojo.Business
{
    public static class Test
    {
        [FunctionName("Test")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string message = "{ \"Value\": \"{\\r\\n \\\"name\\\" : \\\"karun\\\",\\r\\n \\\"userId\\\" : \\\"choudhary\\\"\\r\\n}\", \"Formatters\": [], \"ContentTypes\": [], \"StatusCode\": 200 }";

            //dynamic d = JsonConvert.DeserializeObject<dynamic>(message);

            //string s = d.Value;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SubscriptionModel  model = JsonConvert.DeserializeObject<SubscriptionModel>(requestBody);

            //SubscriptionModel subs = new SubscriptionModel();
            //subs.ExpiryDate = DateTime.Now.AddDays(30);
            //subs.UserId = "1234";

            SubscriptionDal.Create(model);

            //PaymentModel payment = new PaymentModel();
            //payment.Amount = 10;
            //payment.Date = DateTime.Now;

            //PaymentDal.Create(payment);

            //RegistrationModel registration = new RegistrationModel();
            //registration.EmailId = "karunesh.choudhary@gmail.com";
            //registration.Password = "12345";

            //RegistrationDal.Create(registration);

            return new OkObjectResult("Ok");
        }
    }
}
