using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingDisconnectedMode.Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; } 
        public string Descrizione { get; set; }
        public DateTime Data { get; set; }
        public string Utente { get; set; }
        public string Stato { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Descrizione} - {Data} - {Utente} - {Stato}";
        }
    }
}
