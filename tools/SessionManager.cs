using MVC_ASP_EVENT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MVC_ASP_EVENT.Models.DTO;

namespace MVC_ASP_EVENT.tools
{
    public class SessionManager
    {

        private readonly ISession _session;
        private readonly HttpClient _httpClient;
        public SessionManager(IHttpContextAccessor httpContext, HttpClient http)
        {
            _session = httpContext.HttpContext.Session;
            _httpClient = http;
        }

        public void settingToken(string token)
        {
            this.token = token;

            UserDTO user = null;
            int result = int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First(c => c.Type == ClaimTypes.Sid).Value);
            Console.WriteLine($"id : -{result}");
            using (HttpResponseMessage response = _httpClient.GetAsync($"User/{result}").Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    user = JsonConvert.DeserializeObject<UserDTO>(response.Content.ReadAsStringAsync().Result);
                }
            }
            ConnectedUser = user;


        }
        public void Logout(){
            _session.Clear();
            }

        public string? token
        {
            get
            {
                return(_session.GetString("Token") is null ?
                  null : _session.GetString("Token"));
            }
            set
            {
                _session.SetString("Token",value);
            }
        }
        


        public UserDTO? ConnectedUser
        {
            get
            {
                return
                    (token is null ?
                   null : JsonConvert.DeserializeObject<UserDTO>(_session.GetString("ConnectedUser")));

            }
            set
            {
                _session.SetString("ConnectedUser", JsonConvert.SerializeObject(value));
            }

        }

       

    }
}
