using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingDisconnectedMode.Core.Entities;

namespace TicketingDisconnectedMode.Core.Interfaces
{
    public interface IRepository<T>
    {
        List<Ticket> FetchAllFilter(Func<T, bool> filter = null);
        T GetById(int id);
        bool Add(T item);
        bool Update(T item);
        bool Delete(T item);
    }
}
