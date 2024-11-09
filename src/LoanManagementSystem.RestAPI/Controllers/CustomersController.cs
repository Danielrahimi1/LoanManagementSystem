using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(CustomerService service, CustomerQuery query) : ControllerBase
{
    [HttpPost("register/")]
    public async Task<IActionResult> Register([FromBody] AddCustomerDto dto)
    {
        await service.Register(dto);
        return Ok();
    }

    [HttpPost("register_statement/")]
    public async Task<IActionResult> RegisterWithStatement([FromBody] AddCustomerWithStatementDto dto)
    {
        await service.RegisterWithStatement(dto);
        return Ok();
    }

    [HttpPatch("update/{id:int}/")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerDto dto)
    {
        await service.Update(id, dto);
        return Ok();
    }

    [HttpPatch("charge/{id:int}/")]
    public async Task<IActionResult> Charge([FromRoute] int id, [FromBody] UpdateBalanceDto dto)
    {
        await service.Charge(id, dto);
        return Ok();
    }

    [HttpPatch("verify_manually/{id:int}/")]
    public async Task<IActionResult> VerifyManually([FromRoute] int id)
    {
        await service.VerifyManually(id);
        return Ok();
    }

    [HttpDelete("delete/{id:int}/")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await service.Delete(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var customer = await query.GetById(id);
        return Ok(customer);
    }

    [HttpGet("statement/{id:int}")]
    public async Task<IActionResult> GetByIdWithStatement([FromRoute] int id)
    {
        var customer = await query.GetByIdWithStatement(id);
        return Ok(customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await query.GetAll();
        return Ok(customers);
    }

    [HttpGet("statements/")]
    public async Task<IActionResult> GetAllWithStatement()
    {
        var customers = await query.GetAllWithStatement();
        return Ok(customers);
    }

    [HttpGet("risky/")]
    public async Task<IActionResult> GetRiskyCustomers()
    {
        var customers = await query.GetRiskyCustomers();
        return Ok(customers);
    }

    [HttpGet("riskyStatements/")]
    public async Task<IActionResult> GetRiskyCustomersWithStatement()
    {
        var customers = await query.GetRiskyCustomersWithStatement();
        return Ok(customers);
    }
}