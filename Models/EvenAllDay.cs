using MVC_ASP_EVENT.Models.DTO;

namespace MVC_ASP_EVENT.Models
{
    public class EvenAllDay
    {

        public Event Event { get; set; }
        public List<EventTypeDay> EventTypeDay { get; set; }
    }
}
