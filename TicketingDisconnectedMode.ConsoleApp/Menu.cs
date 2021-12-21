using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingDisconnectedMode.Core.BusinessLayer;
using TicketingDisconnectedMode.Core.Entities;
using TicketingDisconnectedMode.Core.Interfaces;
using TicketingDisconnectedMode.DB.Repository;

namespace TicketingDisconnectedMode.ConsoleApp
{
    public class Menu
    {
        private static readonly MainBusinessLayer mainBL =
            new MainBusinessLayer(new TicketDB());

        public static void Start()
        {
            Console.WriteLine("Benvenuto nell'app Tickets!");
            char choice;

            do
            {
                Console.WriteLine("Scegli 1 per stampare l'elenco dei ticket in ordine cronologico." +
                    "\nScegli 2 per inserire un nuovo ticket." +
                    "\nScegli 3 per cancellare un ticket." +
                    "\nScegli Q per uscire.");

                choice = Console.ReadKey().KeyChar;

                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        StampaTicketOrdineCronologico();
                        break;
                    case '2':
                        InsertTicket();
                        break;
                    case '3':
                        DeleteTicket();
                        break;
                    case 'Q':
                        Console.WriteLine("Arrivederci");
                        break;
                    default:
                        Console.WriteLine("Scelta non valida");
                        break;
                }

            } while (choice != 'Q');
        }

        private static void DeleteTicket()
        {
            Console.WriteLine("Lista dei ticket in ordine cronologico.");
            int id;
            Ticket t;
            StampaTicketOrdineCronologico();
            Console.WriteLine("\nDigita l'Id del ticket da eliminare.");

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Inserisci un id numerico!");
            }

            do
            {
                t = mainBL.FindTicketById(id);
                if (t == null)
                    Console.WriteLine("Nessun ticket corrisponde all'id inserito. Controlla meglio!");
            }
            while (t == null);

            bool deleted = mainBL.DeleteTicket(t);

            if (deleted == true)
                Console.WriteLine("Il ticket è stato cancellato correttamente");
        }

        private static void InsertTicket()
        {
            string descrizione = null;
            DateTime data;
            string utente;
            string stato;

            Console.WriteLine("Inserisci dati del ticket.");

            Console.WriteLine("Descrizione: ");
            descrizione = Console.ReadLine();

            Console.WriteLine("Data: ");
            while (!DateTime.TryParse(Console.ReadLine(), out data))
            {
                Console.WriteLine("Inserisci un formato corretto di data!");
            }

            Console.WriteLine("Utente: ");
            utente = Console.ReadLine();

            Console.WriteLine("Stato: ");
            stato = Console.ReadLine();

            Ticket ticket = new Ticket()
            {
                Descrizione = descrizione,
                Data = data,
                Utente = utente,
                Stato = stato
            };

            bool added = mainBL.InsertTicket(ticket);
            if (added)
                Console.WriteLine("Ticket aggiunto correttamente.");
            else Console.WriteLine("Qualcosa è andato storto.");
        }

        private static void StampaTicketOrdineCronologico()
        {
            List<Ticket> tickets = mainBL.StampaTicketOrdineCronologico();
            foreach (Ticket t in tickets)
                Console.WriteLine(t);
        }
    }
}
