// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

/* Realizzare una Console app che acceda al database Ticketing 
 * utilizzando il Disconnected Mode di ADO.NET e che implementi 
 * le stesse operazioni:
Stampi la lista dei Ticket in ordine cronologico (dal più recente al più vecchio)
Permetta l'inserimento di nuovi ticket (i dati devono essere inseriti dall'utente)
Permetta la cancellazione di un Ticket (utilizzare l'ID univoco per identificarlo) */

using TicketingDisconnectedMode.ConsoleApp;

Console.WriteLine("Ado Ticketing App");
Menu.Start();