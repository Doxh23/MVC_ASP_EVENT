using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP_EVENT.Models;
using MVC_ASP_EVENT.tools;
using Newtonsoft.Json;
using NuGet.Common;

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
        public IActionResult Inscription(int id)
        {
            List<EventTypeDay> result = callAPI.getResult(_httpClient, $"EventTypeDay/getByEvent/{id}", typeof(List<EventTypeDay>));
            return View(result);
        }
        public IActionResult InscriptionDay(int id,string date)
        {
            Console.WriteLine(date);
            Participate par = new Participate() { 
                Id = 2,
            EventId = id,
            Date = date,
            Presence = "ghhhhh"
            };

            try
            {
              callAPI.postData(_httpClient, "Participate", par);
            }catch(Exception ex)
            {
                
            }
            return RedirectToAction("Inscription", new {id = id});
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

                string token = callAPI.Login(_httpClient, "User/Login", u);
                if (!string.IsNullOrWhiteSpace(token))
                {
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
        public ActionResult disconnect()
        {
            return View();
        }



        public ActionResult Register()
        {

            ViewBag.Roles = callAPI.getResult(_httpClient, "Role", typeof(List<Role>));
               return View();

        }
        [HttpPost]
        public ActionResult Register(User u,string role = "User")
        {

            User user = u;
            List<Role> roles = callAPI.getResult(_httpClient, "Role", typeof(List<Role>));
            user.Role = roles.FirstOrDefault(x => x.Name == role).Id;
            try
            {
                callAPI.postData(_httpClient, "User/Register",user);
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
