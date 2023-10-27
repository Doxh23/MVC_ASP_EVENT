using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ASP_EVENT.Models
{
    public class Participate
    {

        public int Id { get; set; }
        public int EventId { get; set; }

        public string Date { get; set; }

        public string Presence { get; set; }
    }
}
