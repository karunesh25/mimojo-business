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
using Mimojo.Business.Security;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Mimojo.Business
{
    public static class Login
    {
        [FunctionName("Login")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                log.LogInformation("Login HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                LoginModel model = JsonConvert.DeserializeObject<LoginModel>(requestBody);

                response = RegistrationDal.GetLoginDetails(model);
                if (response.IsValidUser)
                {
                    AuthTokenContainerModel jwt = new AuthTokenContainerModel()
                    {
                        ExpireMinutes = 60,
                        SecretKey = "GpoamhqaGpoIiwibmJmIjoxNjcyMjQ3MzM3LCJleHAiOjE2NzIyNDc5MzcsImlhdCI6MTY3MjI0NzMzN30.ttHNDv9mN442unfLd18Evy6J9QAAheXl-470E2wu_eA",
                        SecurityAlgorithm = SecurityAlgorithms.HmacSha256Signature,
                        Claims = new Claim[]
                {
                    new Claim("TransactionId", response.UserId)
                }
                    };
                    JWTService service = new JWTService("dGhpcyBpcyBteSBjdXN0b20gU2VjcmV0IGtleSBmb3IgYXV0aGVudGljYXRpb24gYmxha2pramtqa2pramtqa2pram5zbWJtbnM=");
                    response.AccessToken = service.GenerateToken(jwt);
                }
                else
                {
                    return new ObjectResult("Error 403 Forbidden")
                    {
                        StatusCode = (int?)HttpStatusCode.Forbidden
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message);
            }
            return new OkObjectResult(response);
        }


    }
}
