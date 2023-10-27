using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP_EVENT.Models;
using MVC_ASP_EVENT.Models.DTO;
using MVC_ASP_EVENT.Models.mapper;
using MVC_ASP_EVENT.tools;
using Newtonsoft.Json;
using System.Reflection;

namespace MVC_ASP_EVENT.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly HttpClient _httpClient;
        private readonly SessionManager _sessionManager;
        public AdminController(HttpClient httpClient, SessionManager sessionManager)
        {

            _httpClient = httpClient;
            _sessionManager = sessionManager;
        }
        public ActionResult Index()
        {
            return View();
        }
       public IActionResult CreateEvent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateEvent(Event e)
        {
           

            try
            {
                List<Status> list = new List<Status>();
                list = callAPI.getResult(_httpClient, "Status",typeof(List<Status>),_sessionManager);
                Status status = list.First(x => x.Name == "not started");
                e.status = status;
                bool success = callAPI.postData(_httpClient, "Event", e,_sessionManager);
                if (success)
                {
                   List<Event> events = callAPI.getResult(_httpClient, "Event", typeof(List<Event>),_sessionManager);
                    Event newEvent = events.Last();
                    return RedirectToAction("CreateDay",newEvent);
                }
                else
                {
                    return View("CreateEvent","something wrong");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        


        public IActionResult CreateDay(Event newEvent)
        {
            ViewBag.Days = ((DateTime.Parse(newEvent.EndDate) - DateTime.Parse(newEvent.StartDate)).Days);
            ViewBag.Type = callAPI.getResult(_httpClient, "EventType", typeof(List<Models.Type>), _sessionManager);


            return View(newEvent);
        }
        [HttpPost]
        public IActionResult CreateDay(List<string> type)
        {
            List<Models.Type> types = callAPI.getResult(_httpClient, "EventType", typeof(List<Models.Type>), _sessionManager);
            for (int i=1; i<= type.Count; i++)
            {
                Models.Type result = types.First(x => x.Name == type[i-1]);
                List<Event> list = callAPI.getResult(_httpClient, "Event", typeof(List<Event>), _sessionManager);
                Models.Event createdEvent = list.Last();
                EventTypeDay eventDay = new EventTypeDay()
                {
                    date = $"{DateTime.Parse(createdEvent.StartDate).AddDays(i)}",
                    Type = result,
                    EventId = createdEvent.Id,
                    
                };

                callAPI.postData(_httpClient, "EventTypeDay", eventDay, _sessionManager);
            }

            return RedirectToAction("Index","Home");
        }

    }
}
