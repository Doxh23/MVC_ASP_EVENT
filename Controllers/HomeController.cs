using Microsoft.AspNetCore.Mvc;
using MVC_ASP_EVENT.Models;
using MVC_ASP_EVENT.Models.DTO;
using MVC_ASP_EVENT.Models.mapper;
using MVC_ASP_EVENT.tools;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace MVC_ASP_EVENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionManager _session;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger,SessionManager session, HttpClient httpClient)
        {
            _logger = logger;
            _session = session;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            if(_session.token is not null)
            {
                Console.WriteLine(_session.ConnectedUser.FirstName);
            }
            return View();
        }

        public IActionResult Events()
        {
            List<EvenAllDay> list = new List<EvenAllDay>();
            List<Models.Event> events = JsonConvert.DeserializeObject<List<Models.Event>>(callAPI.getResult(_httpClient, "Event"));
            foreach(Models.Event item in events)
            {
                List<EventTypeDay> eventsdays = JsonConvert.DeserializeObject<List<EventTypeDay>>(callAPI.getResult(_httpClient, $"EventTypeDay/getByEvent/{item.Id}"));
                list.Add(new EvenAllDay()
                {
                    Event = item,
                    EventTypeDay = eventsdays
                }) ;


            }
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}