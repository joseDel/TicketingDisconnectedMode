using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingDisconnectedMode.Core.Entities;
using TicketingDisconnectedMode.Core.Interfaces;

namespace TicketingDisconnectedMode.DB.Repository
{
    public class TicketDB : ITicket
    {

        // stringa di connessione
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool Add(Ticket item)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Siamo connessi al DB.");
                else
                    Console.WriteLine("Non connessi al DB.");

                // inizializziamo dataset e data adapter
                var ticketAdapter = InitDataSetAndAdapter(ticketDS, connessione);
                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                // da qui lavoro in modalità disconnected -> offline
                DataRow newRow = ticketDS.Tables["Tickets"].NewRow();
                newRow["Descrizione"] = item.Descrizione;
                newRow["Data"] = item.Data;
                newRow["Utente"] = item.Utente;
                newRow["Stato"] = item.Stato;

                ticketDS.Tables["Tickets"].Rows.Add(newRow);

                // qui avviene la riconciliazione col DB e quindi il vero salvataggio lato DB
                ticketAdapter.Update(ticketDS, "Tickets");
                return true;    
                Console.WriteLine("Database correttamente aggiornato");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return false;   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return false;
            }
            finally
            {
                connessione.Close();
            }
        }

        public bool Delete(Ticket item)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);

            try
            {
                connessione.Open();
                /* if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Siamo connessi al DB.");
                else
                    Console.WriteLine("Non connessi al DB."); */

                // inizializziamo dataset e data adapter
                var ticketAdapter = InitDataSetAndAdapter(ticketDS, connessione);
                connessione.Close();
                // Console.WriteLine("Connessione chiusa");
                // da qui lavoro in modalità disconnected -> offline

                DataRow rowToDelete = ticketDS.Tables["Tickets"].Rows.Find(item.Id);
                if (rowToDelete != null)
                {
                    rowToDelete.Delete();
                }

                // qui avviene la riconciliazione col DB e quindi il vero salvataggio lato DB
                ticketAdapter.Update(ticketDS, "Tickets");
                Console.WriteLine("Database correttamente aggiornato");
                return true;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return false;
            }
            finally
            {
                connessione.Close();
            }
        }

        public List<Ticket> FetchAllFilter(Func<Ticket, bool> filter = null)
        {
            DataSet ticketDS = new DataSet();
            List<Ticket> ticketList = new List<Ticket>();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Siamo connessi al DB.");
                else
                    Console.WriteLine("Non connessi al DB.");

                // inizializziamo dataset e data adapter
                InitDataSetAndAdapter(ticketDS, connessione);
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
                // da qui lavoro in modalità disconnected -> offline

                foreach (DataRow row in ticketDS.Tables["Tickets"].Rows)
                {
                    Ticket ticket = new Ticket
                    {
                        Id = (int)row["Id"], 
                        Descrizione = (string)row["Descrizione"],
                        Data = (DateTime)row["Data"],
                        Utente = (string)row["Utente"],
                        Stato = (string)row["Stato"]
                    };

                    ticketList.Add(ticket);
                }
                return ticketList;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
                return null;
            }
            finally
            {
                connessione.Close();
            }
        }

        private SqlDataAdapter InitDataSetAndAdapter(DataSet ticketDS, SqlConnection connessione)
        {
            SqlDataAdapter ticketAdapter = new SqlDataAdapter();

            // Fill
            ticketAdapter.SelectCommand = new SqlCommand("select * from Tickets", connessione);
            ticketAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey; // evita di dover definire a mano le PK nelle tabelle
            ticketAdapter.Fill(ticketDS, "Tickets");

            // Insert
            ticketAdapter.InsertCommand = GenerateInsertCommand(connessione);

            // Delete
            ticketAdapter.DeleteCommand = GenerateDeleteCommand(connessione);

            return ticketAdapter;
        }

        private SqlCommand GenerateDeleteCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandText = "delete from tickets where Id=@id";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));
            return cmd;
        }

        private SqlCommand GenerateInsertCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandText = "insert into tickets values (@descrizione, @data, @utente, @stato)";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 0, "Data"));
            cmd.Parameters.Add(new SqlParameter("@descrizione", SqlDbType.NVarChar, 500, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.NVarChar, 100, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@stato", SqlDbType.NVarChar, 100, "Stato"));

            return cmd;
        }

        public Ticket GetById(int id)
        {
            List<Ticket> tickets = FetchAllFilter();
            return tickets.SingleOrDefault(t => t.Id == id);
        }

        public bool Update(Ticket item)
        {
            throw new NotImplementedException();
        }
    }
}
