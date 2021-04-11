using Api.Domain.Entities;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByLoginAsync(string email);
    }
}
