using Microsoft.AspNetCore.Mvc;
using MVC_ASP_EVENT.Models;
using MVC_ASP_EVENT.Models.DTO;
using MVC_ASP_EVENT.Models.mapper;
using MVC_ASP_EVENT.tools;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;

namespace MVC_ASP_EVENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionManager _session;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, SessionManager session, HttpClient httpClient)
        {
            _logger = logger;
            _session = session;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            if (_session.token is not null)
            {
                Console.WriteLine(_session.ConnectedUser.FirstName);
            }
            return View();
        }

        public IActionResult Events()
        {
            List<EvenAllDay> list = new List<EvenAllDay>();
            List<Models.Event> events = callAPI.getResult(_httpClient, "Event", typeof(List<Event>));
            foreach (Models.Event item in events)
            {
                List<EventTypeDay> eventsdays = callAPI.getResult(_httpClient, $"EventTypeDay/getByEvent/{item.Id}", typeof(List<EventTypeDay>));
                list.Add(new EvenAllDay()
                {
                    Event = item,
                    EventTypeDay = eventsdays
                });


            }
            return View(list);
        }


        public IActionResult Event(int id)
        {

            Event ev = callAPI.getResult(_httpClient, $"Event/{id}", typeof(Event));

            List<EventTypeDay> eventsdays = callAPI.getResult(_httpClient, $"EventTypeDay/getByEvent/{id}", typeof(List<EventTypeDay>));
            ViewBag.Comments = callAPI.getResult(_httpClient, $"Comments/event/{id}", typeof(List<Comments>));



            return View(new EvenAllDay()
            {
                Event = ev,
                EventTypeDay = eventsdays
            });
        }
        [HttpPost]
        public IActionResult addComments(int EventId , string comment)
        {
            try
            {

            Comments comm = new Comments()
            {
                EventId = EventId,
                UserId = _session.ConnectedUser.Id,
                Content = comment,
                PostDate = DateTime.Now.ToString()
            };
            bool sucess = callAPI.postData(_httpClient, "Comments", comm);
                if (sucess)
                {
                    return RedirectToAction("Event", new { id = EventId });
                }else {
                    return RedirectToAction("Error");
                }
            }catch (Exception ex)
            {
                return RedirectToAction("error", ex);
            }

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}