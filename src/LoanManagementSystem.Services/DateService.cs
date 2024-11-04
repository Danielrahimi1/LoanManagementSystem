using System;

namespace LoanManagementSystem.Services;

public interface DateService
{
    public DateOnly UtcNow { get; set; }
}