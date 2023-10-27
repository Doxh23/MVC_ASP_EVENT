using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MVC_ASP_EVENT.tools
{
    static public class callAPI
    {

        static public dynamic getResult(HttpClient http, string api, Type type, SessionManager sess)
        {

            if(sess.token != null)
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme,sess.token);
            }
            else
            {
                http.DefaultRequestHeaders.Authorization = null;

            }
            using (HttpResponseMessage response = http.GetAsync(api).Result)
            {
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, type);
            }
        }

        static public bool postData(HttpClient http, string api, dynamic data, SessionManager sess)
        {
            if (sess.token != null)
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, sess.token);
            }
            else
            {
                http.DefaultRequestHeaders.Authorization = null;

            }
            string jsonser = JsonConvert.SerializeObject(data);
         
            var content = new StringContent(jsonser, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = http.PostAsync(api, content).Result)
            {
                try
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    return JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

                }
                catch (Exception ex)
                {
                    return false;
                }
            }


        }

        static public string Login(HttpClient http, string api, dynamic data, SessionManager sess)
        {
            if (sess.token != null)
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, sess.token);
            }
            else
            {
                http.DefaultRequestHeaders.Authorization = null;

            }
            string jsonser = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonser, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = http.PostAsync(api, content).Result)
            {
                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                        return response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }


        }

    }
}
