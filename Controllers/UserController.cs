using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP_EVENT.Models;
using MVC_ASP_EVENT.tools;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace MVC_ASP_EVENT.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly SessionManager _sessionManager;
        public UserController(HttpClient http,SessionManager sess)
        {
            _httpClient = http;
            _sessionManager = sess;
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
      
        public IActionResult InscriptionDay(int id,string date)
        {
            Console.WriteLine(date);
            Participate par = new Participate() { 
                Id = _sessionManager.ConnectedUser.Id,
            EventId = id,
            Date = date,
            Presence = "ghhhhh"
            };

            try
            {
              callAPI.postData(_httpClient, "Participate", par, _sessionManager);
            }catch(Exception ex)
            {
                
            }
            return RedirectToAction("Event","Home", new {id = id});
        }
        public IActionResult desinscriptionDay(int id, string date)
        {
           //TODOOOOOOOOOOOOO
            return RedirectToAction("Event", "Home", new { id = id });
        }
        // GET: UserController/Details/5
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin u)
        {

            try
            {

                string token = callAPI.Login(_httpClient, "User/Login", u,_sessionManager);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    JwtSecurityToken jwt = new JwtSecurityToken(token);
                    _sessionManager.settingToken(token);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return RedirectToAction("Login");

                }


            }
            catch(Exception e)
            {
                return RedirectToAction("Login");
            }
        }
        // GET: UserController/Create
        public ActionResult Logout()
        {
            _sessionManager.Logout();
            return RedirectToAction("Index","Home");
        }



        public ActionResult Register()
        {

            ViewBag.Roles = callAPI.getResult(_httpClient, "Role", typeof(List<Role>), _sessionManager);
               return View();

        }
        [HttpPost]
        public ActionResult Register(User u,string role = "User")
        {

            User user = u;
            List<Role> roles = callAPI.getResult(_httpClient, "Role", typeof(List<Role>), _sessionManager);
            user.Role = roles.FirstOrDefault(x => x.Name == role).Id;
            try
            {
                callAPI.postData(_httpClient, "User/Register",user, _sessionManager );
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Register");

            }

        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
