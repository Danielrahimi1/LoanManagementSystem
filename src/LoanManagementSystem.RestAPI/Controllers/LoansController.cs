using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoansController(LoanService service, LoanQuery query) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var loan = await query.GetById(id);
        return Ok(loan);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await query.GetAll();
        return Ok(loans);
    }

    [HttpGet("short/")]
    public async Task<IActionResult> GetAllShortPeriod()
    {
        var loans = await query.GetAllShortPeriod();
        return Ok(loans);
    }

    [HttpGet("long/")]
    public async Task<IActionResult> GetAllLongPeriod()
    {
        var loans = await query.GetAllLongPeriod();
        return Ok(loans);
    }

    [HttpPost("create/")]
    public async Task<IActionResult> Create([FromBody] AddLoanDto dto)
    {
        await service.Create(dto);
        return Ok();
    }

    [HttpPatch("update/{id:int}/")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLoanDto dto)
    {
        await service.Update(id, dto);
        return Ok();
    }

    [HttpDelete("delete/{id:int}/")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await service.Delete(id);
        return Ok();
    }
}