using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Repository;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly IDbContext _dbContext;

        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> FindByLoginAsync(string email)
        {
            //Metodo 1 (Não recomendado, já que o Dapper irá trazer todos os registros em memoria para na sequencia realizar o filtro)
            var item = (await _dbContext.Connection.GetAllAsync<UserEntity>()).Where(a => a.Email == email);

            item = (await _dbContext.Connection.QueryAsync<UserEntity>("SELECT * FROM [User] WHERE Email = @Email", new
            {
                Email = email
            })).ToList();

            if (item.Count() == 0)
                return null;
            else
                return item.FirstOrDefault();
        }
    }
}
