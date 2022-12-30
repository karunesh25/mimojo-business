using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Mimojo.Business.Model;
using System.Linq;
using Mimojo.Business.Dal;
using Mimojo.Business.Security;
using System.Net;

namespace Mimojo.Business
{
    public static class GetProfile
    {
        [FunctionName("GetProfile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            ProfileResponseModel response = new ProfileResponseModel();
            try
            {
                log.LogInformation("GetProfile HTTP trigger function processed a request.");

                var authorizationHeader = req.Headers?["Authorization"];
                string token = null;
                var parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
                if (parts.Length == 2 && parts[0].Equals("bearer"))
                    token = parts[1];
                JWTService service = new JWTService("dGhpcyBpcyBteSBjdXN0b20gU2VjcmV0IGtleSBmb3IgYXV0aGVudGljYXRpb24gYmxha2pramtqa2pramtqa2pram5zbWJtbnM=");
                if (!service.IsTokenValid(token))
                {
                    return new ObjectResult("Error 403 Forbidden")
                    {
                        StatusCode = (int?)HttpStatusCode.Forbidden
                    };
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ProfileRequestModel model = JsonConvert.DeserializeObject<ProfileRequestModel>(requestBody);

                response.Registration = RegistrationDal.GetRegistrationDetails(model);
                response.Subscription = SubscriptionDal.GetSubscriptionDetails(model);
                response.Payment = PaymentDal.GetPaymentDetails(model);

                log.LogInformation("Response : " + JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message);
            }
            return new OkObjectResult(response);
        }
    }
}
