using Dapper.Contrib.Extensions;
using System;

namespace Api.Domain.Entities
{
    public abstract class BaseEntity
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
