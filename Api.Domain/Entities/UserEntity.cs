using Dapper.Contrib.Extensions;

namespace Api.Domain.Entities
{
    [Table("[User]")]
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
