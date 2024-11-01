using System;

namespace LoanManagementSystem.Services;

public interface DateService
{
    public DateOnly NowUtc { get; set; }
}