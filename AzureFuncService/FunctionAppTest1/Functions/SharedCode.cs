using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppTest1.Functions
{
    public static class SharedCode
    {
        public static async Task<(string,string,string,string)> ParseRequest(HttpRequest req, ClaimsPrincipal principal)
        {
            string test = req.Query["test"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string UserId = string.Empty;
            string GivenName = string.Empty;
            string SurName = string.Empty;

            foreach (Claim claim in principal.Claims)
            {
                if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") UserId = claim.Value;
                if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname") GivenName = claim.Value;
                if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") SurName = claim.Value;
            }

            if (test?.ToLower() == "yes")
            {
                UserId = GlobalVariables.TestUserId.ToString();
                GivenName = "Test";
                SurName = "Testov";
            }
            else
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    throw new Exception("Acccess denied");
                }
            }
            return (requestBody, UserId, GivenName, SurName);
        }
    }
}
