using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanRequestsController(
    LoanRequestService service,
    LoanRequestQuery query,
    PayLoanHandler handler) : ControllerBase
{
    [HttpPost("{id:int}/open/")]
    public async Task<IActionResult> Open([FromRoute] int id, [FromBody] AddLoanRequestDto dto)
    {
        await service.Open(id, dto);
        return Ok();
    }

    [HttpPatch("{id:int}/accept/")]
    public async Task<IActionResult> RegisterWithStatement([FromRoute] int id)
    {
        await service.Accept(id);
        return Ok();
    }

    [HttpPatch("reject/{id:int}/")]
    public async Task<IActionResult> Update([FromRoute] int id)
    {
        await service.Reject(id);
        return Ok();
    }

    [HttpPatch("activate/")]
    public async Task<IActionResult> Activate([FromBody] ActivateLoanRequestCommand dto)
    {
        await handler.Handle(dto);
        return Ok();
    }


    [HttpGet("{id:int}/")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var lr = await query.GetById(id);
        return Ok(lr);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lrs = await query.GetAll();
        return Ok(lrs);
    }

    [HttpGet("customer_{id:int}/")]
    public async Task<IActionResult> GetAllByCustomer([FromRoute] int id)
    {
        var lrs = await query.GetAllByCustomer(id);
        return Ok(lrs);
    }

    [HttpGet("accept/")]
    public async Task<IActionResult> GetAllAccept()
    {
        var lrs = await query.GetAllAcceptLoans();
        return Ok(lrs);
    }

    [HttpGet("active/")]
    public async Task<IActionResult> GetAllActive()
    {
        var lrs = await query.GetAllActiveLoans();
        return Ok(lrs);
    }

    [HttpGet("close/")]
    public async Task<IActionResult> GetAllClose()
    {
        var lrs = await query.GetAllClosedLoans();
        return Ok(lrs);
    }

    [HttpGet("delayed/")]
    public async Task<IActionResult> GetAllDelay()
    {
        var lrs = await query.GetAllDelayedLoans();
        return Ok(lrs);
    }

    [HttpGet("with_customer/")]
    public async Task<IActionResult> GetAllWithCustomer()
    {
        var lrs = await query.GetAllWithCustomer();
        return Ok(lrs);
    }

    [HttpGet("accept_with_customers/")]
    public async Task<IActionResult> GetAllAcceptWithCustomer()
    {
        var lrs = await query.GetAllAcceptLoansWithCustomer();
        return Ok(lrs);
    }

    [HttpGet("active_with_customers/")]
    public async Task<IActionResult> GetAllActiveLoansCustomer()
    {
        var lrs = await query.GetAllActiveLoansWithCustomer();
        return Ok(lrs);
    }

    [HttpGet("close_with_customers/")]
    public async Task<IActionResult> GetAllClosedWithCustomer()
    {
        var lrs = await query.GetAllClosedLoansWithCustomer();
        return Ok(lrs);
    }

    [HttpGet("delay_with_customers/")]
    public async Task<IActionResult> GetAllDelayedWithCustomer()
    {
        var lrs = await query.GetAllDelayedLoansWithCustomer();
        return Ok(lrs);
    }

    [HttpGet("remaining/")]
    public async Task<IActionResult> GetAllRemaining()
    {
        var lrs = await query.GetAllRemainingLoans();
        return Ok(lrs);
    }
}