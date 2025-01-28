using Microsoft.AspNetCore.Mvc;
using WineLotteryApp.Models;
using WineLotteryApp.Services;

namespace WineLotteryApp.Controllers;

[ApiController]
[Route("/")]
public class LotteryController : ControllerBase
{
    private readonly ILotteryService _lotteryService;

    public LotteryController(ILotteryService lotteryService)
    {
        _lotteryService = lotteryService;
    }

    [HttpPost("AddEntrant")]
    public Entrant AddEntrant(string name)
    {
        return _lotteryService.AddEntrant(name);
    }

    [HttpGet("GetEntrants")]
    public List<Entrant> GetEntrants()
    {
        return _lotteryService.GetEntrants();
    }

    [HttpPost("GenerateTickets")]
    public ActionResult GenerateTickets()
    {
        _lotteryService.GenerateTickets();
        return Ok();
    }

    [HttpGet("GetTickets")]
    public List<Ticket> GetTickets()
    {
        return _lotteryService.GetTickets();
    }

    [HttpPost("GenerateWines")]
    public List<Wine> GenerateWines()
    {
        return _lotteryService.GenerateWines();
    }

    [HttpGet("GetWines")]
    public List<Wine> GetWines()
    {
        return _lotteryService.GetWines();
    }

    [HttpPost("BuyTickets")]
    public Entrant BuyTicket(string entrantName, [FromBody] List<int> ticketNumbers)
    {
        return _lotteryService.BuyTicket(entrantName, ticketNumbers);
    }

    [HttpPost("RunLottery")]
    public List<Entrant> RunLottery()
    {
        return _lotteryService.RunLottery();
    }

    [HttpPost("TestRun")]
    public List<Entrant> TestRun()
    {
        return _lotteryService.TestRun();
    }
}
