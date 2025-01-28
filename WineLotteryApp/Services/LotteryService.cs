using WineLotteryApp.Models;

namespace WineLotteryApp.Services;

public class LotteryService : ILotteryService
{
    public List<Entrant> Entrants { get; private set; } = new();
    public List<Ticket> Tickets { get; private set; } = new();
    public List<Wine> Wines { get; private set; } = new();

    private List<string> WineNames = new()
    {
        "Rødmusset Rørosing",
        "Brusende Bergenser",
        "Haldens Hvite",
        "Råde Rød",
        "Skogsbær Syrah",
        "Isbre Isvin",
        "Fjordens Favoritt",
        "Nordisk Nektar",
        "Trollskog Tempranillo",
        "Vinter Vin",
        "Sommer Sauvignon"
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
            throw new InvalidOperationException("Entrant not found");
        }
        if (entrant.Money < ticketNumbers.Count * 10)
        {
            throw new InvalidOperationException("Not enough money");
        }

        foreach (var ticketNumber in ticketNumbers)
        {
            var ticket = Tickets.FirstOrDefault(t => t.Number == ticketNumber);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket number {ticketNumber} not found");
            }
            entrant.Money -= ticket.Price;
            entrant.Tickets.Add(ticket);
            Tickets.Remove(ticket);
        }

        return entrant;
    }

    public List<Entrant> RunLottery()
    {
        if (Entrants == null || !Entrants.Any())
            throw new InvalidOperationException("No entrants available");

        if (Wines == null || !Wines.Any())
            throw new InvalidOperationException("No wines available");

        if (Tickets == null || !Tickets.Any())
            throw new InvalidOperationException("No tickets available");


        Random random = new Random();
        List<Entrant> winners = new List<Entrant>();
        List<int> ticketsInPlay = GetBoughtTickets(); // Get all tickets that have been bought, no need to draw tickets that have not been bought

        if (!ticketsInPlay.Any())
            throw new InvalidOperationException("No tickets have been bought.");

        while (Wines.Count > 0) // Draw wines until all wines are drawn
        {
            int winningTicketNumber = random.Next(ticketsInPlay.Count);

            var winner = Entrants.FirstOrDefault(e => e.Tickets.Any(t => t.Number == ticketsInPlay[winningTicketNumber]));
            if (winner != null)
            {
                winner.Wines.Add(Wines[0]);
                Wines.RemoveAt(0);
                if (!winners.Contains(winner)) // No need to add the same winner multiple times
                {
                    winners.Add(winner);
                }
            }
            else
            {
                throw new InvalidOperationException("Winner not found for the winning ticket.");
            }
        }

        foreach (var winner in winners) // Clear tickets of winners, looks better
        {
            winner.Tickets.Clear();
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
        BuyTicket("Alice", new List<int> { 1, 2, 3, 4, 5, 6, 7});
        BuyTicket("Bob", new List<int> { 13, 15, 17, 19, 21});
        BuyTicket("Charlie", new List<int> { 33, 44, 55, 66, 77});
        return RunLottery();
    }

    public void ResetLottery()
    {
        Entrants.Clear();
        Tickets.Clear();
        Wines.Clear();
    }

    private List<int> GetBoughtTickets()
    {
        return Entrants.SelectMany(e => e.Tickets).Select(t => t.Number).ToList();
    }
}
