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
        public AdminController(HttpClient httpClient)
        {

            _httpClient = httpClient;
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
                list = JsonConvert.DeserializeObject<List<Status>>(callAPI.getResult(_httpClient, "Status"));
                Status status = list.First(x => x.Name == "not started");
                e.status = status;
                bool success = callAPI.postData(_httpClient, "Event", e);
                if (success)
                {
                    Event newEvent = JsonConvert.DeserializeObject<List<Event>>(callAPI.getResult(_httpClient, "Event")).Last();
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
            ViewBag.Type = JsonConvert.DeserializeObject<List<Models.Type>>(callAPI.getResult(_httpClient, "EventType"));


            return View(newEvent);
        }
        [HttpPost]
        public IActionResult CreateDay(List<string> type)
        {
            List<Models.Type> types = JsonConvert.DeserializeObject<List<Models.Type>>(callAPI.getResult(_httpClient, "EventType"));
            for (int i=1; i<= type.Count; i++)
            {
                Models.Type result = types.First(x => x.Name == type[i-1]);
                Models.Event createdEvent = JsonConvert.DeserializeObject<List<Event>>(callAPI.getResult(_httpClient, "Event")).Last();
                EventTypeDay eventDay = new EventTypeDay()
                {
                    date = $"{DateTime.Parse(createdEvent.StartDate).AddDays(i)}",
                    Type = result,
                    EventId = createdEvent.Id,
                    
                };

                callAPI.postData(_httpClient, "EventTypeDay", eventDay);
            }

            return RedirectToAction("Index","Home");
        }

    }
}
