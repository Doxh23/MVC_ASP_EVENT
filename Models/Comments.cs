using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ASP_EVENT.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string PostDate { get; set; }

        public int EventId { get; set; }
    }
}
