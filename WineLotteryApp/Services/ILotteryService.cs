using WineLotteryApp.Models;

namespace WineLotteryApp.Services
{
    public interface ILotteryService
    {
        //Properties
        List<Entrant> Entrants { get; }
        List<Ticket> Tickets { get; }
        List<Wine> Wines { get; }

        //Functions
        Entrant AddEntrant(string name);
        List<Entrant> GetEntrants();
        void GenerateTickets();
        List<Ticket> GetTickets();
        List<Wine> GenerateWines();
        List<Wine> GetWines();
        Entrant BuyTicket(string entrantName, List<int> ticketNumbers);
        List<Entrant> RunLottery();
        List<Entrant> TestRun();
    }
}