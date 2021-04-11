using System;
using System.Data;

namespace Api.Domain.Interfaces
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
