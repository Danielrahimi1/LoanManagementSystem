using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstallmentsController(InstallmentQuery query, PayInstallmentHandler handler) : ControllerBase
{
    [HttpGet("{id:int}/")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var ins = await query.GetById(id);
        return Ok(ins);
    }

    [HttpGet("customer_{id:int}/")]
    public async Task<IActionResult> GetByCustomer([FromRoute] int id)
    {
        var lrs = await query.GetAllByCustomer(id);
        return Ok(lrs);
    }

    [HttpGet("loan_{id:int}/")]
    public async Task<IActionResult> GetByLoan([FromRoute] int id)
    {
        var lrs = await query.GetAllByLoan(id);
        return Ok(lrs);
    }

    [HttpGet("loan_request_{id:int}/")]
    public async Task<IActionResult> GetByLoanRequest([FromRoute] int id)
    {
        var lrs = await query.GetAllByLoanRequest(id);
        return Ok(lrs);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lrs = await query.GetAll();
        return Ok(lrs);
    }

    [HttpGet("closed/")]
    public async Task<IActionResult> GetAllClose()
    {
        var lrs = await query.GetAllClosed();
        return Ok(lrs);
    }

    [HttpGet("delayed/")]
    public async Task<IActionResult> GetDelay()
    {
        var lrs = await query.GetAllDelayed();
        return Ok(lrs);
    }

    [HttpGet("incomes/")]
    public async Task<IActionResult> GetIncome()
    {
        var lrs = await query.GetAllIncome();
        return Ok(lrs);
    }
    
    [HttpPatch("pay/")]
    public async Task<IActionResult> Pay([FromBody] PayInstallmentCommand dto)
    {
        await handler.Handle(dto);
        return Ok();
    }
}