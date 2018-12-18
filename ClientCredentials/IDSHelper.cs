using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
namespace ClientCredentials
{
    public class IDSHelper
    {
        public static async Task MainAsync()
        {
            try
            {
                DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

                TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, "Client", "secret");
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);


                var response = await client.GetAsync("http://localhost:5000/api/values/");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task Post()
        {
            try
            {
                DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

                TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, "Client", "secret");
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);

                Client c1 = new Client
                {
                    ClientId = "superAdmin",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" },
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                };
                string strJson = JsonConvert.SerializeObject(c1.ToEntity());
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> response = client.PostAsync("http://localhost:5000/api/values/", content);

                if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Result.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task Admin()
        {
            try
            {
                DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

                TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, "adminClient", "secret");
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);

                Client c1 = new Client
                {
                    ClientId = "test2",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                    //,Claims = new List<Claim>
                    //{
                    //    new Claim(JwtClaimTypes.Role, "admin")
                    //},
                    //ClientClaimsPrefix = "" //把client_ 前缀去掉
                };
                string strJson = JsonConvert.SerializeObject(c1.ToEntity());
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> response = client.PostAsync("http://localhost:5000/api/values/", content);

                if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Result.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
