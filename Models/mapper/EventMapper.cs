using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVC_ASP_EVENT.Models.DTO;

namespace MVC_ASP_EVENT.Models.mapper
{
   static public class EventMapper
    {

        public static EventDTO  toEventDTO(this Event e)
        {
            return new EventDTO()
            {
                Adress = e.Adress,
                EndDate = e.EndDate,
                location = e.location,
                Name = e.Name,
                StartDate = e.StartDate,
                status = e.status.Name
            };

        }

        public static List<EventDTO> toListEventDTO(this List<Event> e)
        {
            List<EventDTO> list = new List<EventDTO>();

            foreach (Event item in e)
            {
                list.Add(item.toEventDTO());
            }
            return list;
        }
    }
}
