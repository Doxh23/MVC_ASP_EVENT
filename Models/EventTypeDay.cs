using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ASP_EVENT.Models
{
    public class EventTypeDay
    {

        public int Id { get; set; }
        public Models.Type Type { get; set; }
        public int EventId { get; set; }

        public string date { get; set; }
    }
}