namespace JordiAragon.SharedKernel.Domain.Guards
{
    using System;
    using System.Runtime.CompilerServices;
    using Ardalis.GuardClauses;

    public static class GuardExtensions
    {
        public static void NotUtc(
            this IGuardClause guardClause,
            DateTime input,
            [CallerArgumentExpression(nameof(input))] string parameterName = null)
        {
            if (input.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"The provided {parameterName} must be in UTC format.", parameterName);
            }
        }
    }
}