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
using Mimojo.Business.Dal;

namespace Mimojo.Business
{
    public static class AddUserCountry
    {
        [FunctionName("AddUserCountry")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            CountryUserModel model = new CountryUserModel();
            try
            {
                log.LogInformation("GetProfile HTTP trigger function processed a request.");
                //var headers = req.Headers;
                //var key = headers.ToList()[0];

                var authorizationHeader = req.Headers?["Authorization"];
                string token = null;
                var parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
                if (parts.Length == 2 && parts[0].Equals("Bearer"))
                    token = parts[1];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                model = JsonConvert.DeserializeObject<CountryUserModel>(requestBody);

                SubscriptionDal.CreateUserCountry(model);

                log.LogInformation($"User Country completed : {requestBody}");
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message);
            }
            return new OkObjectResult(model);
        }
    }
}
