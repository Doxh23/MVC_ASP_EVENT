using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace MVC_ASP_EVENT.tools
{
    static public class callAPI
    {

      static public  string getResult(HttpClient http, string api )
        {
           
            
            using (HttpResponseMessage response = http.GetAsync(api).Result)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        static public bool postData(HttpClient http, string api,dynamic data)
        {

            string jsonser = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonser,Encoding.UTF8,"application/json");
            using (HttpResponseMessage response = http.PostAsync(api,content).Result)
            {
                try
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    return JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

                }catch (Exception ex)
                {
                    return false;
                }
            }


        }

        static public string Login(HttpClient http, string api, dynamic data)
        {

            string jsonser = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonser, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = http.PostAsync(api, content).Result)
            {
                try
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    return response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }


        }

    }
}
