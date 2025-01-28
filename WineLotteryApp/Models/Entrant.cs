namespace WineLotteryApp.Models;

public class Entrant
{
    public string Name { get; set; } = "";
    public List<Ticket> Tickets { get; set; } = new();
    public List<Wine> Wines { get; set; } = new();
    public int Money { get; set; } = 100;
}
