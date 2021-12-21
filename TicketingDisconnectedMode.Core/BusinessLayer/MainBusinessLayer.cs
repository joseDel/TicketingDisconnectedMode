using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingDisconnectedMode.Core.Entities;
using TicketingDisconnectedMode.Core.Interfaces;

namespace TicketingDisconnectedMode.Core.BusinessLayer
{
    public class MainBusinessLayer : IBusinessLayer
    {
        private readonly ITicket _repositoryTicket;

        public MainBusinessLayer(ITicket repoTicket)
        {
            _repositoryTicket = repoTicket;
        }

        public List<Ticket> StampaTicketOrdineCronologico()
        {
            List<Ticket> tickets = _repositoryTicket.FetchAllFilter();
            return tickets.OrderByDescending(x => x.Data).ToList();
        }

        public bool InsertTicket(Ticket ticket)
        {
            if (ticket == null)
                return false;
            return _repositoryTicket.Add(ticket);
        }

        public Ticket FindTicketById(int id)
        {
            if (id == null)
                return null;
            return _repositoryTicket.GetById(id);
        }

        public bool DeleteTicket(Ticket ticket)
        {
            return _repositoryTicket.Delete(ticket);
        }
    }
}
