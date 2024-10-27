namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;

    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid accountId, string firstname, string lastname);
    }
}