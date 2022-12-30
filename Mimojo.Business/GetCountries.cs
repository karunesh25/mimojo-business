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
using System.Collections.Generic;
using Mimojo.Business.Dal;

namespace Mimojo.Business
{
    public static class GetCountries
    {
        [FunctionName("GetCountries")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<CountryModel> response = new List<CountryModel>();
            try
            {
                log.LogInformation("GetCountries HTTP trigger function processed a request.");
                //var headers = req.Headers;
                //var key = headers.ToList()[0];

                var authorizationHeader = req.Headers?["Authorization"];
                string token = null;
                var parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
                if (parts.Length == 2 && parts[0].Equals("Bearer"))
                    token = parts[1];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ProfileRequestModel model = JsonConvert.DeserializeObject<ProfileRequestModel>(requestBody);
                response = SubscriptionDal.GetCountries();

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
