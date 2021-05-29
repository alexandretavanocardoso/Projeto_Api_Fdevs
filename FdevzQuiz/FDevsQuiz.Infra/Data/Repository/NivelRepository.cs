using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class NivelRepository : CrudRepository<long, EnqNivel>, INivelRepository
    {
        public NivelRepository(IDbContext context) : base(context)
        {
        }
    }
}
