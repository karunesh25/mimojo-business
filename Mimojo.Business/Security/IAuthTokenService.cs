using System.Security.Claims;
using System.Collections.Generic;

namespace Mimojo.Business.Security
{
    public interface IAuthTokenService
    {
        string SecretKey { get; set; }
        bool IsTokenValid(string token);
        string GenerateToken(AuthTokenContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }

    public class AuthTokenContainerModel
    {
        #region Members
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public int ExpireMinutes { get; set; }

        public Claim[] Claims { get; set; }
        #endregion
    }
}
