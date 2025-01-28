using WineLotteryApp.Models;

namespace WineLotteryApp.Services;

public class LotteryService : ILotteryService
{
    public List<Entrant> Entrants { get; private set; } = new();
    public List<Ticket> Tickets { get; private set; } = new();
    public List<Wine> Wines { get; private set; } = new();

    public List<string> WineNames = new()
    {
        "Fjord Vin",
    "Nordlys Nectar",
    "Viking Vintner",
    "Skål Skjønnhet",
    "Oslo Opulence",
    "Bergen Bliss",
    "Arctic Ambrosia",
    "Midnight Sun Merlot",
    "Fjell Chardonnay"
    };

    private int numberOfTickets = 101;
    private int numberOfWines = 3;
    private int totalPriceOfWines = 1000;


    public Entrant AddEntrant(string name)
    {
        var entrant = new Entrant { Name = name };
        Entrants.Add(entrant);
        return entrant;
    }

    public List<Entrant> GetEntrants()
    {
        return Entrants;
    }

    public void GenerateTickets()
    {
        Tickets.Clear();
        for (int i = 1; i < numberOfTickets; i++)
            {
                Tickets.Add(new Ticket { Number = i });
            }
    }

    public List<Ticket> GetTickets()
    {
        return Tickets;
    }

    public List<Wine> GenerateWines()
    {
        Random random = new Random();
        Wines.Clear();
        int remainingPrice = totalPriceOfWines;

        for (int i = 0; i < numberOfWines - 1; i++)
        {
            int maxPrice = Math.Min(remainingPrice - (numberOfWines - i - 1) * 100, 700); // max price 700
            int price = random.Next(100, maxPrice + 1); // min price 100
            int nameIndex = random.Next(WineNames.Count);

            Wines.Add(new Wine { Name = WineNames[nameIndex], Price = price });
            WineNames.RemoveAt(nameIndex);
            remainingPrice -= price;
        }

        int lastNameIndex = random.Next(WineNames.Count); // Set name of last wine after previous names have been removed
        Wines.Add(new Wine { Name = WineNames[lastNameIndex], Price = remainingPrice }); // Set price of last wine to remaining price

        Wines = Wines.OrderBy(w => w.Price).ToList(); // Sort wines by price in ascending order
        return Wines;
    }

    public List<Wine> GetWines()
    {
        return Wines;
    }

    public Entrant BuyTicket(string entrantName, List<int> ticketNumbers)
    {
        var entrant = Entrants.FirstOrDefault(e => e.Name == entrantName);
        if (entrant == null)
        {
            throw new Exception("Entrant not found");
        }
        if (entrant.Money < ticketNumbers.Count * 10)
        {
            throw new Exception("Not enough money");
        }

        foreach (var ticketNumber in ticketNumbers)
        {
            var ticket = Tickets.FirstOrDefault(t => t.Number == ticketNumber);
            if (ticket == null)
            {
                throw new Exception($"Ticket number {ticketNumber} not found");
            }
            entrant.Money -= ticket.Price;
            entrant.Tickets.Add(ticket);
            Tickets.Remove(ticket);
        }

        return entrant;
    }

    public List<Entrant> RunLottery()
    {
        Random random = new Random();
        List<Entrant> winners = new List<Entrant>();
        List<int> winningTicketList = new List<int>();
        string winnerString = $"The winning numbers are {winningTicketList[0]}, {winningTicketList[1]} and {winningTicketList[2]}. Congratulations to our lucky winner(s)!";

        while (Wines.Count > 0)
        {
            int winningTicketNumber = random.Next(1, 101);
            winningTicketList.Add(winningTicketNumber);
            var winner = Entrants.FirstOrDefault(e => e.Tickets.Any(t => t.Number == winningTicketNumber));
            if (winner != null)
            {
                winner.Wines.Add(Wines[0]);
                Wines.RemoveAt(0);
                if (!winners.Contains(winner))
                {
                    winners.Add(winner);
                }
            }
        }

        return winners;
    }

    public List<Entrant> TestRun()
    {
        AddEntrant("Alice");
        AddEntrant("Bob");
        AddEntrant("Charlie");
        GenerateTickets();
        GenerateWines();
        BuyTicket("Alice", new List<int> { 1, 2, 3 });
        BuyTicket("Bob", new List<int> { 4, 5, 6 });
        BuyTicket("Charlie", new List<int> { 7, 8, 9 });
        return RunLottery();
    }
}
