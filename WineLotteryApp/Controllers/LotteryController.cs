using Microsoft.AspNetCore.Mvc;
using WineLotteryApp.Domain.Models;

namespace WineLotteryApp.Controllers;

[ApiController]
[Route("[controller]")]
public class LotteryController : ControllerBase
{
    [HttpGet(Name = "GetLottery")]
    public ActionResult GetLottery()
    {
        return Ok("Get Lottery");
    }

    [HttpPost(Name = "AddEntrant")]
    public ActionResult AddEntrant([FromBody] Entrant entrant)
    {

        return Ok(entrant.Name);
    }
}
